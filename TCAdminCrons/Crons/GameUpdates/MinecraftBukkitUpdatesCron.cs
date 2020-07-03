using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models.Bukkit;

namespace TCAdminCrons.Crons.GameUpdates
{
    public class MinecraftBukkitUpdatesCron : TcAdminCronJob
    {
        private readonly MinecraftCronConfiguration _minecraftCronConfiguration =
            MinecraftCronConfiguration.GetConfiguration();

        public override async Task DoAction()
        {
            if (!_minecraftCronConfiguration.BukkitSettings.Enabled)
            {
                Log.Information("[Minecraft Bukkit Update Cron] - Disabled in Configuration.");
                return;
            }
            try
            {
                Log.Information("[Minecraft Bukkit Update Cron] Running...");
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
            var bukkitUpdates = BukkitVersionManifest.GetManifests().Version;

            foreach (var version in bukkitUpdates.Take(_minecraftCronConfiguration.BukkitSettings.GetLastReleaseUpdates))
            {
                var gameUpdate = version.GetGameUpdate();
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Log.Information($"[Minecraft Bukkit Update Cron] Saved Game Update for {version.Version}");
                }
                else
                {
                    Log.Information("[Minecraft Bukkit Update Cron] Game Update already exists for " + version.Version);
                }
            }
        }
    }
}