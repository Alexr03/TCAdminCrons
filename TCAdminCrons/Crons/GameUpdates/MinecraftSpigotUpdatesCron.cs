using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models.Spigot;

namespace TCAdminCrons.Crons.GameUpdates
{
    public class MinecraftSpigotUpdatesCron : TcAdminCronJob
    {
        private readonly MinecraftCronConfiguration _minecraftCronConfiguration =
            MinecraftCronConfiguration.GetConfiguration();

        public override async Task DoAction()
        {
            if (!_minecraftCronConfiguration.SpigotSettings.Enabled)
            {
                Log.Information("[Minecraft Spigot Update Cron] - Disabled in Configuration.");
                return;
            }
            try
            {
                Log.Information("[Minecraft Spigot Update Cron] Running...");
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
            var spigotUpdates = SpigotVersionManifest.GetManifests().Version;

            foreach (var version in spigotUpdates.Take(_minecraftCronConfiguration.SpigotSettings.GetLastReleaseUpdates))
            {
                var gameUpdate = version.GetGameUpdate();
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Log.Information($"[Minecraft Spigot Update Cron] Saved Game Update for {version.Version}");
                }
                else
                {
                    Log.Information("[Minecraft Spigot Update Cron] Game Update already exists for " + version.Version);
                }
            }
        }
    }
}