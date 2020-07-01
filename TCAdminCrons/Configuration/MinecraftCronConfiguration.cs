namespace TCAdminCrons.Configuration
{
    public class MinecraftCronConfiguration
    {
        public int GameId { get; set; } = 0;
        
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

        public string Description { get; set; } =
            "This is a vanilla server snapshot of Minecraft: Java Edition | Release Date: {Update.ReleaseTime} | Added by TCAdminCrons";

        public bool UseVersionAsViewOrder { get; set; } = true;
        
        public int GetLastReleaseUpdates { get; set; } = 15;
        
        public int GetLastSnapshotUpdates { get; set; } = 15;
    }
    
    public class PaperSettings
    {
        public bool Enabled { get; set; } = true;

        public string Group { get; set; } = "Paper";
        
        public string NameTemplate { get; set; } = "{Id}";
        
        public string Description { get; set; } = "Paper is the next generation of Minecraft server, compatible with Spigot plugins and offering uncompromising performance. | Added by TCAdminCrons";

        public bool UseVersionAsViewOrder { get; set; } = true;
        
        public int GetLastReleaseUpdates { get; set; } = 15;
    }
}