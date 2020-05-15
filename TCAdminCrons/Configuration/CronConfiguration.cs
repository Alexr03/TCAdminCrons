namespace TCAdminCrons.Configuration
{
    public class CronConfiguration
    {
        public bool Enabled { get; set; } = false;
        
        public readonly long ExecuteEveryMilliseconds;

        public CronConfiguration(long executeEveryMilliseconds)
        {
            ExecuteEveryMilliseconds = executeEveryMilliseconds;
        }
    }
}