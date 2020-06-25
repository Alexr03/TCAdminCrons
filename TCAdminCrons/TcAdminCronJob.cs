using System.Threading.Tasks;
using FluentScheduler;

namespace TCAdminCrons
{
    public abstract class TcAdminCronJob : IJob
    {
        public abstract Task DoAction();

        public void Execute()
        {
            Task.Run(async () => await DoAction()).Wait();
        }
    }
}