namespace TCAdminCrons.Configuration
{
    public class TcAdminConfiguration
    {
        public string ConnectionString { get; set; } = "";
        public bool Encrypted { get; set; } = true;

        public static TcAdminConfiguration GetConfiguration()
        {
            return ConfigurationHelper.GetConfiguration<TcAdminConfiguration>("TcAdminConfig.json");
        }
    }
}