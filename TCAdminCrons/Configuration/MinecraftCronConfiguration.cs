namespace TCAdminCrons.Configuration
{
    public class MinecraftCronConfiguration : CronConfiguration
    {
        public int GameId { get; set; } = 0;
        
        public int GetLastUpdates { get; set; } = 15;

        public VanillaSettings VanillaSettings { get; set; } = new VanillaSettings();
        
        public PaperSettings PaperSettings { get; set; } = new PaperSettings();
        
        public static MinecraftCronConfiguration GetConfiguration()
        {
            return ConfigurationHelper.GetConfiguration<MinecraftCronConfiguration>("MinecraftUpdates.json");
        }
    }

    public class VanillaSettings
    {
        public bool Enabled { get; set; } = true;
        
        public string NameTemplate { get; set; } = "{Id}";

        public string Group { get; set; } = "Vanilla Release";
        
        public string SnapshotGroup { get; set; } = "Vanilla Snapshot";
        
        public bool UseVersionAsViewOrder { get; set; } = true;
    }
    
    public class PaperSettings
    {
        public bool Enabled { get; set; } = true;

        public string Group { get; set; } = "Paper";
        
        public string NameTemplate { get; set; } = "{Id}";
        
        public bool UseVersionAsViewOrder { get; set; } = true;
    }
}