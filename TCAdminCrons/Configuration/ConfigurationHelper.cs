using System;
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
                File.WriteAllText(configLocation, JsonConvert.SerializeObject((T)Activator.CreateInstance(typeof(T)), Formatting.Indented, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Populate
                }));
            }
            
            var configText = File.ReadAllText(configLocation);
            return JsonConvert.DeserializeObject<T>(configText);
        }
    }
}