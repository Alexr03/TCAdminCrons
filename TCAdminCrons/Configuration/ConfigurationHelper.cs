using System.IO;
using Newtonsoft.Json;

namespace TCAdminCrons.Configuration
{
    public static class ConfigurationHelper
    {
        public static T GetConfiguration<T>(string configName)
        {
            var configLocation = $"./Config/{configName}";
            if (!File.Exists(configLocation))
            {
                File.WriteAllText(configLocation, JsonConvert.SerializeObject(default(T), Formatting.Indented));
            }
            
            var configText = File.ReadAllText(configLocation);
            return JsonConvert.DeserializeObject<T>(configText);
        }
    }
}