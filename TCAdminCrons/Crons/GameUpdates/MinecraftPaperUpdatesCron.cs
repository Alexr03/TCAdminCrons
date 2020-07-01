using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models.Paper;

namespace TCAdminCrons.Crons.GameUpdates
{
    public class MinecraftPaperUpdatesCron : TcAdminCronJob
    {
        private readonly MinecraftCronConfiguration _minecraftCronConfiguration =
            MinecraftCronConfiguration.GetConfiguration();

        public override async Task DoAction()
        {
            if (!_minecraftCronConfiguration.EnableCron || !_minecraftCronConfiguration.PaperSettings.Enabled)
            {
                Log.Information("[Minecraft Paper Update Cron] - Disabled in Configuration.");
                return;
            }
            try
            {
                Log.Information("[Minecraft Paper Update Cron] Running...");
                AddUpdatesForMcTemp();
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
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
                    Log.Information($"[Minecraft Paper Update Cron] Saved Game Update for {version}");
                }
                else
                {
                    Log.Information("[Minecraft Paper Update Cron] Game Update already exists for " + version);
                }
            }
        }
    }
}