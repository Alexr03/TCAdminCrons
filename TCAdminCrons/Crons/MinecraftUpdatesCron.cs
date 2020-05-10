using System;
using System.Threading.Tasks;
using Quartz;
using TCAdminCrons.Configuration;

namespace TCAdminCrons.Crons
{
    public class MinecraftUpdatesCron : TcAdminCronJob
    {
        public MinecraftUpdatesCron()
        {
            this.CronConfiguration = new CronConfiguration(7_200_000);
        }

        public override async Task DoAction(IJobExecutionContext context)
        {
            Console.WriteLine("DOING MINECRAFT UPDATE CRON JOB.");
            await base.DoAction(context);
        }
    }
}