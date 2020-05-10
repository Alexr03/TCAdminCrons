namespace TCAdminCrons.Configuration
{
    public class CronConfiguration
    {
        public readonly long ExecuteEveryMilliseconds;

        public CronConfiguration(long executeEveryMilliseconds)
        {
            ExecuteEveryMilliseconds = executeEveryMilliseconds;
        }
    }
}