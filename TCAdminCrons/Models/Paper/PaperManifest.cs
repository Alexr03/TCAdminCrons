using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
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
            
            var newId = Regex.Replace(version, "[^0-9]", "");
            int.TryParse(newId, out var parsedId);
            
            var variables = new Dictionary<string, object>
            {
                {"Version", version}
            };
            
            var gameUpdate = new GameUpdate
            {
                Name = config.PaperSettings.NameTemplate.ReplaceWithVariables(variables),
                GroupName = config.PaperSettings.Group,
                WindowsFileName = $"{GetDownloadUrl(version)} minecraft_server.jar",
                LinuxFileName = $"{GetDownloadUrl(version)} minecraft_server.jar",
                ExtractPath = "/",
                Reinstallable = true,
                DefaultInstall = false,
                GameId = config.GameId,
                Comments = config.VanillaSettings.Description.ReplaceWithVariables(variables),
                UserAccess = true,
                SubAdminAccess = true,
                ResellerAccess = true,
                ViewOrder = config.PaperSettings.UseVersionAsViewOrder ? parsedId : 0
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