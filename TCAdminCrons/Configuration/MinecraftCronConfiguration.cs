namespace TCAdminCrons.Configuration
{
    public class MinecraftCronConfiguration
    {
        public int GameId { get; set; } = 0;

        public static MinecraftCronConfiguration GetConfiguration()
        {
            return ConfigurationHelper.GetConfiguration<MinecraftCronConfiguration>("MinecraftUpdates.json");
        }
    }
}