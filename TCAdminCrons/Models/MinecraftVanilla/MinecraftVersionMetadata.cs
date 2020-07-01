using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public GameUpdate CreateGameUpdate()
        {
            var config = MinecraftCronConfiguration.GetConfiguration();
            
            var newId = Regex.Replace(this.Id, "[^0-9]", "");
            int.TryParse(newId, out var parsedId);

            var variables = new Dictionary<string, object>
            {
                {"Update", this}
            };

            var gameUpdate = new GameUpdate
            {
                Name = config.VanillaSettings.NameTemplate.ReplaceWithVariables(variables),
                GroupName = this.Type == "release" ? config.VanillaSettings.Group : config.VanillaSettings.SnapshotGroup,
                WindowsFileName = $"{this.Downloads.Server.Url} minecraft_server.jar",
                LinuxFileName = $"{this.Downloads.Server.Url} minecraft_server.jar",
                ExtractPath = "/",
                Reinstallable = true,
                DefaultInstall = false,
                GameId = config.GameId,
                Comments = config.VanillaSettings.Description.ReplaceWithVariables(variables),
                UserAccess = true,
                SubAdminAccess = true,
                ResellerAccess = true,
                ViewOrder = config.VanillaSettings.UseVersionAsViewOrder ? parsedId : 0
            };

            gameUpdate.GenerateKey();
            return gameUpdate;
        }
    }
}