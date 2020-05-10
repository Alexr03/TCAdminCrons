using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace TCAdminCrons.Models
{
    public class Version
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("time")] public DateTime Time { get; set; }

        [JsonProperty("releaseTime")] public DateTime ReleaseTime { get; set; }

        public MinecraftVersionMetadata GetMetadata()
        {
            using (var wc = new WebClient())
            {
                return JsonConvert.DeserializeObject<MinecraftVersionMetadata>(
                    wc.DownloadString(Url));
            }
        }
    }

    public class MinecraftVersionManifest
    {
        [JsonProperty("versions")] public IList<Version> Versions { get; set; }

        public static MinecraftVersionManifest GetManifests()
        {
            using (var wc = new WebClient())
            {
                return JsonConvert.DeserializeObject<MinecraftVersionManifest>(
                    wc.DownloadString("https://launchermeta.mojang.com/mc/game/version_manifest.json"));
            }
        }
    }
}