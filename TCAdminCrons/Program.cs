using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models;
using TCAdminWrapper;

namespace TCAdminCrons
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("TCAdmin Crons.");
            RegisterToTcAdmin();

            // AddUpdatesForMcTemp();
            RemoveAllGameUpdates(146);
            // RemoveAllGameMods(146);

            Task.Run(async () => { await RegisterCrons(); });

            Console.ReadLine();
        }

        public static void AddUpdatesForMcTemp()
        {
            var snapshots = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "snapshot").Take(5);

            var releases = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "release").Take(5);
            
            foreach (var metaData in snapshots.Reverse().ToList().Select(version => version.GetMetadata()))
            {
                Console.WriteLine($"Adding update for: {metaData.Id}");

                metaData.CreateGameUpdate().Save();
                Console.WriteLine($"Saved Game Update for {metaData.Id}");
            }
            
            foreach (var metaData in releases.Reverse().ToList().Select(version => version.GetMetadata()))
            {
                Console.WriteLine($"Adding update for: {metaData.Id}");

                metaData.CreateGameUpdate().Save();
                Console.WriteLine($"Saved Game Update for {metaData.Id}");
            }
        }

        public static void RemoveAllGameMods(int gameId)
        {
            Console.WriteLine("Deleting all from game id " + gameId);
            var objectList = GameMod.GetMods(gameId);
            objectList.DeleteAll();
            Console.WriteLine("Done");
        }

        public static void RemoveAllGameUpdates(int gameId)
        {
            Console.WriteLine("Deleting all from game id " + gameId);
            var objectList = GameUpdate.GetUpdates(gameId);
            objectList.DeleteAll();
            Console.WriteLine("Done");
        }

        public static async Task RegisterCrons()
        {
            var exporters = typeof(TcAdminCronJob)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(TcAdminCronJob)) && !t.IsAbstract)
                .Select(t => (TcAdminCronJob) Activator.CreateInstance(t)).ToList();

            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Clear();
            await scheduler.Start();

            foreach (var scheduledTask in exporters)
            {
                var type = scheduledTask.GetType();
                var build = JobBuilder.Create(type).WithIdentity(type.Name + "_" + new Random().Next(1000)).Build();
                var schedule = TriggerBuilder.Create().WithIdentity(type.Name + "_" + new Random().Next(1000))
                    .StartNow().WithSimpleSchedule(x =>
                        x.WithInterval(
                                TimeSpan.FromMilliseconds(scheduledTask.CronConfiguration.ExecuteEveryMilliseconds))
                            .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(build, schedule);
                Console.WriteLine(
                    $"[Cron Scheduler] Scheduled {type.Name} to fire every {scheduledTask.CronConfiguration.ExecuteEveryMilliseconds}ms");
            }
        }

        public static void RegisterToTcAdmin()
        {
            var config = TcAdminConfiguration.GetConfiguration();
            var tcAdminClientConfiguration =
                new TCAdminClientConfiguration(config.ConnectionString, config.Encrypted, "TCAdminCrons");
            _ = new TcAdminClient(tcAdminClientConfiguration);
            Console.WriteLine("Registered to TCAdmin successfully.");
        }
    }
}