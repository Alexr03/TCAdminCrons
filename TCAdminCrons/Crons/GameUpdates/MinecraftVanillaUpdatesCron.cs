using System;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Models.MinecraftVanilla;

namespace TCAdminCrons.Crons.GameUpdates
{
    public class MinecraftVanillaUpdatesCron : TcAdminCronJob
    {
        private readonly MinecraftCronConfiguration _minecraftCronConfiguration = MinecraftCronConfiguration.GetConfiguration();

        public override async Task DoAction()
        {
            if (!_minecraftCronConfiguration.Enabled)
            {
                Log.Information("Skipping Minecraft Vanilla Cron Job - Disabled in Configuration.");
                return;
            }
            try
            {
                Log.Information("[Minecraft Update Cron] Running...");
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
            var snapshots = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "snapshot").Take(_minecraftCronConfiguration.GetLastUpdates);

            var releases = MinecraftVersionManifest.GetManifests().Versions
                .Where(x => x.Type.ToLower() == "release").Take(_minecraftCronConfiguration.GetLastUpdates);

            foreach (var metaData in snapshots.Select(version => version.GetMetadata()))
            {
                var gameUpdate = metaData.CreateGameUpdate();
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Log.Information($"[Minecraft Update Cron] Saved Game Update for {metaData.Id}");
                }
                else
                {
                    Log.Information("[Minecraft Update Cron] Game Update already exists for " + metaData.Id);
                }
            }

            foreach (var metaData in releases.Select(version => version.GetMetadata()))
            {
                var gameUpdate = metaData.CreateGameUpdate();
                if (!gameUpdates.Any(x => x.Name == gameUpdate.Name && x.GroupName == gameUpdate.GroupName))
                {
                    gameUpdate.Save();
                    Log.Information($"[Minecraft Update Cron] Saved Game Update for {metaData.Id}");
                }
                else
                {
                    Log.Information("[Minecraft Update Cron] Game Update already exists for " + metaData.Id);
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