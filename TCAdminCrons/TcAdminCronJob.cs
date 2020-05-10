using System.Threading.Tasks;
using Quartz;
using TCAdminCrons.Configuration;

namespace TCAdminCrons
{
    public class TcAdminCronJob : IJob
    {
        public CronConfiguration CronConfiguration;
        
        public Task Execute(IJobExecutionContext context)
        {
            return DoAction(context);
        }

        public virtual async Task DoAction(IJobExecutionContext context)
        {
            return;
        }
    }
}