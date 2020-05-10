using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;
using TCAdminCrons.Configuration;
using TCAdminWrapper;

namespace TCAdminCrons
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            Console.WriteLine("TCAdmin Crons.");
            RegisterToTcAdmin();

            RegisterCrons().GetAwaiter().GetResult();
            Console.ReadLine();
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