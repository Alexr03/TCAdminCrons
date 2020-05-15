using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models.Paper;

namespace TCAdminCrons.Crons
{
    public class MinecraftPaperUpdatesCron : TcAdminCronJob
    {
        private readonly MinecraftCronConfiguration _minecraftCronConfiguration =
            MinecraftCronConfiguration.GetConfiguration();

        public MinecraftPaperUpdatesCron()
        {
            this.CronConfiguration = new CronConfiguration(_minecraftCronConfiguration.RepeatEveryMilliseconds);
        }

        public override async Task DoAction(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("[Minecraft Paper Update Cron] Running...");
                AddUpdatesForMcTemp();
                // RemoveAllGameUpdates();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void AddUpdatesForMcTemp()
        {
            var gameUpdates = GameUpdate.GetUpdates(_minecraftCronConfiguration.GameId).Cast<GameUpdate>().ToList();
            var paperUpdates = PaperManifest.GetManifest();

            foreach (var version in paperUpdates.Versions)
            {
                var gameUpdate = PaperManifest.GetGameUpdate(version);
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Console.WriteLine($"[Minecraft Paper Update Cron] Saved Game Update for {version}");
                }
                else
                {
                    Console.WriteLine("[Minecraft Paper Update Cron] Game Update already exists for " + version);
                }
            }
        }
    }
}