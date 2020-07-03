using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;

namespace TCAdminCrons.Models.Spigot
{
    public class SpigotResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("built")]
        public int Built { get; set; }
        
        public GameUpdate GetGameUpdate()
        {
            var config = MinecraftCronConfiguration.GetConfiguration();
            
            var newId = Regex.Replace(this.Version, "[^0-9]", "");
            int.TryParse(newId, out var parsedId);
            
            var variables = new Dictionary<string, object>
            {
                {"Update", this}
            };
            
            var gameUpdate = new GameUpdate
            {
                Name = config.SpigotSettings.NameTemplate.ReplaceWithVariables(variables),
                GroupName = config.SpigotSettings.Group,
                WindowsFileName = $"{GetDownloadUrl(Version)} minecraft_server.jar",
                LinuxFileName = $"{GetDownloadUrl(Version)} minecraft_server.jar",
                ExtractPath = "/",
                Reinstallable = true,
                DefaultInstall = false,
                GameId = config.GameId,
                Comments = config.SpigotSettings.Description.ReplaceWithVariables(variables),
                UserAccess = true,
                SubAdminAccess = true,
                ResellerAccess = true,
                ViewOrder = config.SpigotSettings.UseVersionAsViewOrder ? parsedId : 0
            };

            gameUpdate.GenerateKey();
            return gameUpdate;
        }
        
        private static string GetDownloadUrl(string version)
        {
            return $"https://serverjars.com/api/fetchJar/bukkit/{version}";
        }
    }
    public class SpigotVersionManifest
    {
        [JsonProperty("response")] public IList<SpigotResponse> Version { get; set; }

        public static SpigotVersionManifest GetManifests()
        {
            using (var wc = new WebClient())
            {
                return JsonConvert.DeserializeObject<SpigotVersionManifest>(
                    wc.DownloadString("https://serverjars.com/api/fetchAll/spigot"));
            }
        }
    }
}