using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;

namespace TCAdminCrons.Models.Paper
{
    public class PaperManifest
    {
        [JsonProperty("project")]
        public string Project { get; set; }

        [JsonProperty("versions")]
        public IList<string> Versions { get; set; }

        public static PaperManifest GetManifest()
        {
            using (var wc = new WebClient())
            {
                return JsonConvert.DeserializeObject<PaperManifest>(
                    wc.DownloadString("https://papermc.io/api/v1/paper/"));
            }
        }

        public static GameUpdate GetGameUpdate(string version)
        {
            var config = MinecraftCronConfiguration.GetConfiguration();
            
            var gameUpdate = new GameUpdate
            {
                Name = version,
                GroupName = "Paper",
                WindowsFileName = $"{GetDownloadUrl(version)} minecraft_server.jar",
                LinuxFileName = "",
                ExtractPath = "/",
                Reinstallable = true,
                DefaultInstall = false,
                GameId = config.GameId,
                Comments = $"Paper is the next generation of Minecraft server, compatible with Spigot plugins and offering uncompromising performance. | Added by TCAdminCrons",
                UserAccess = true,
                SubAdminAccess = true,
                ResellerAccess = true,
            };

            gameUpdate.GenerateKey();
            return gameUpdate;
        }

        private static string GetDownloadUrl(string version)
        {
            return $"https://papermc.io/api/v1/paper/{version}/latest/download";
        }
    }
}