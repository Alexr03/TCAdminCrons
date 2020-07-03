namespace TCAdminCrons.Configuration
{
    public class MinecraftCronConfiguration
    {
        public int GameId { get; set; } = 0;
        public VanillaSettings VanillaSettings { get; set; } = new VanillaSettings();
        public PaperSettings PaperSettings { get; set; } = new PaperSettings();
        public SpigotSettings SpigotSettings { get; set; } = new SpigotSettings();
        public BukkitSettings BukkitSettings { get; set; } = new BukkitSettings();
        public static MinecraftCronConfiguration GetConfiguration()
        {
            return ConfigurationHelper.GetConfiguration<MinecraftCronConfiguration>("MinecraftUpdates.json");
        }
    }

    public class VanillaSettings
    {
        public bool Enabled { get; set; } = true;
        public string NameTemplate { get; set; } = "{Update.Id}";
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
        public string NameTemplate { get; set; } = "{Version}";
        public string Description { get; set; } = "Paper is the next generation of Minecraft server, compatible with Spigot plugins and offering uncompromising performance. | Added by TCAdminCrons";
        public bool UseVersionAsViewOrder { get; set; } = true;
        public int GetLastReleaseUpdates { get; set; } = 15;
    }
    
    public class SpigotSettings
    {
        public bool Enabled { get; set; } = true;
        public string Group { get; set; } = "Spigot";
        public string NameTemplate { get; set; } = "{Update.Version}";
        public string Description { get; set; } = "Spigot is a modified version of CraftBukkit with hundreds of improvements and optimizations that can only make CraftBukkit shrink in shame! | Added by TCAdminCrons";
        public bool UseVersionAsViewOrder { get; set; } = true;
        public int GetLastReleaseUpdates { get; set; } = 15;
    }
    
    public class BukkitSettings
    {
        public bool Enabled { get; set; } = true;
        public string Group { get; set; } = "Bukkit";
        public string NameTemplate { get; set; } = "{Update.Version}";
        public string Description { get; set; } = "CraftBukkit is lightly modified version of the Vanilla software allowing it to be able to run Bukkit plugins | Added by TCAdminCrons";
        public bool UseVersionAsViewOrder { get; set; } = true;
        public int GetLastReleaseUpdates { get; set; } = 15;
    }
}