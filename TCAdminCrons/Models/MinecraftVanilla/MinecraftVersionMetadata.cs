using System;
using Newtonsoft.Json;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;

namespace TCAdminCrons.Models.MinecraftVanilla
{
    public class Server
    {

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Downloads
    {
        [JsonProperty("server")]
        public Server Server { get; set; }
    }

    public class MinecraftVersionMetadata
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("downloads")]
        public Downloads Downloads { get; set; }

        [JsonProperty("releaseTime")]
        public DateTime ReleaseTime { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public GameMod CreateGameMod()
        {
            var config = MinecraftCronConfiguration.GetConfiguration();
            
            var gameMod = new GameMod
            {
                Name = this.Id,
                GroupName = this.Type,
                WindowsFileName = $"{this.Downloads.Server.Url} minecraft_server.jar",
                LinuxFileName = "",
                ExtractPath = "/",
                InstallType = InstallType.Silent,
                DefaultInstall = false,
                GameId = config.GameId,
                Comments = $"Release Date: {this.ReleaseTime} | Added by TCAdminCrons",
                UserAccess = true,
                SubAdminAccess = true,
                ResellerAccess = true
            };
            
            gameMod.GenerateKey();
            return gameMod;
        }

        public GameUpdate CreateGameUpdate()
        {
            var config = MinecraftCronConfiguration.GetConfiguration();
            
            var gameUpdate = new GameUpdate
            {
                Name = this.Id,
                GroupName = this.Type,
                WindowsFileName = $"{this.Downloads.Server.Url} minecraft_server.jar",
                LinuxFileName = "",
                ExtractPath = "/",
                Reinstallable = true,
                DefaultInstall = false,
                GameId = config.GameId,
                Comments = $"Release Date: {this.ReleaseTime} | Added by TCAdminCrons",
                UserAccess = true,
                SubAdminAccess = true,
                ResellerAccess = true,
            };

            gameUpdate.GenerateKey();
            return gameUpdate;
        }
    }
}