using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models;

namespace TCAdminCrons.Crons
{
    public class MinecraftUpdatesCron : TcAdminCronJob
    {
        public MinecraftUpdatesCron()
        {
            this.CronConfiguration = new CronConfiguration(7_200_000);
        }

        public override async Task DoAction(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("DOING MINECRAFT UPDATE CRON JOB.");
                AddUpdatesForMcTemp();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void AddUpdatesForMcTemp()
        {
            var snapshots = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "snapshot").Take(5);

            var releases = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "release").Take(5);

            foreach (var metaData in snapshots.Select(version => version.GetMetadata()))
            {
                Console.WriteLine($"Adding update for: {metaData.Id}");

                metaData.CreateGameUpdate().Save();
                Console.WriteLine($"Saved Game Update for {metaData.Id}");
            }

            foreach (var metaData in releases.Select(version => version.GetMetadata()))
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
    }
}