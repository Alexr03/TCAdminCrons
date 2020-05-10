using System;
using Newtonsoft.Json;

namespace TCAdminCrons.Configuration
{
    public static class ConfigurationHelper
    {
        public static T GetConfiguration<T>(string configName)
        {
            var configLocation = $"./Config/{configName}";
            if (!System.IO.File.Exists(configLocation))
            {
                throw new ArgumentException("Config not found", nameof(configName));
            }
            
            var configText = System.IO.File.ReadAllText(configLocation);
            return JsonConvert.DeserializeObject<T>(configText);
        }
    }
}