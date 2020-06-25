namespace TCAdminCrons.Configuration
{
    public class MinecraftCronConfiguration : CronConfiguration
    {
        public int GameId { get; set; } = 0;
        
        public int GetLastUpdates { get; set; } = 15;

        public static MinecraftCronConfiguration GetConfiguration()
        {
            return ConfigurationHelper.GetConfiguration<MinecraftCronConfiguration>("MinecraftUpdates.json");
        }
    }
}