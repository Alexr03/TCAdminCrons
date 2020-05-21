using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl.Matchers;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models.MinecraftVanilla;

namespace TCAdminCrons.Crons.GameUpdates
{
    [DisallowConcurrentExecution]
    public class MinecraftVanillaUpdatesCron : TcAdminCronJob
    {
        private readonly MinecraftCronConfiguration _minecraftCronConfiguration = MinecraftCronConfiguration.GetConfiguration();

        public MinecraftVanillaUpdatesCron()
        {
            this.CronConfiguration = new CronConfiguration(_minecraftCronConfiguration.RepeatEveryMilliseconds);
        }

        public override async Task DoAction(IJobExecutionContext context)
        {
            await context.Scheduler.PauseJobs(GroupMatcher<JobKey>.AnyGroup(), CancellationToken.None);
            try
            {
                Console.WriteLine("[Minecraft Update Cron] Running...");
                AddUpdatesForMcTemp();
                // RemoveAllGameUpdates();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                await context.Scheduler.ResumeJobs(GroupMatcher<JobKey>.AnyGroup(), CancellationToken.None);
            }
        }

        public void AddUpdatesForMcTemp()
        {
            var gameUpdates = GameUpdate.GetUpdates(_minecraftCronConfiguration.GameId).Cast<GameUpdate>().ToList();
            var snapshots = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "snapshot");

            var releases = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "release");

            foreach (var metaData in snapshots.Select(version => version.GetMetadata()))
            {
                var gameUpdate = metaData.CreateGameUpdate();
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Console.WriteLine($"[Minecraft Update Cron] Saved Game Update for {metaData.Id}");
                }
                else
                {
                    Console.WriteLine("[Minecraft Update Cron] Game Update already exists for " + metaData.Id);
                }
            }

            foreach (var metaData in releases.Select(version => version.GetMetadata()))
            {
                var gameUpdate = metaData.CreateGameUpdate();
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Console.WriteLine($"[Minecraft Update Cron] Saved Game Update for {metaData.Id}");
                }
                else
                {
                    Console.WriteLine("[Minecraft Update Cron] Game Update already exists for " + metaData.Id);
                }
            }
        }

        public void RemoveAllGameMods()
        {
            Console.WriteLine("[Minecraft Update Cron] Deleting all from game id " + _minecraftCronConfiguration.GameId);
            var objectList = GameMod.GetMods(_minecraftCronConfiguration.GameId);
            objectList.DeleteAll();
            Console.WriteLine("Done");
        }

        public void RemoveAllGameUpdates()
        {
            Console.WriteLine("[Minecraft Update Cron] Deleting all from game id " + _minecraftCronConfiguration.GameId);
            var objectList = GameUpdate.GetUpdates(_minecraftCronConfiguration.GameId);
            objectList.DeleteAll();
            Console.WriteLine("Done");
        }
    }
}