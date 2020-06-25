using System;
using System.Net;
using System.Reflection;
using FluentScheduler;
using Serilog;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminCrons.Configuration;
using TCAdminCrons.Crons.GameUpdates;
using TCAdminWrapper;

namespace TCAdminCrons
{
    public class Program
    {
        public static Registry CronRegistry;
        
        public static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            Console.Title = "TCAdmin Crons - By Alexr03";

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("TCAdmin Crons - By Alexr03 | Version: " + Assembly.GetExecutingAssembly().GetName().Version);
            CronRegistry = new Registry();
            
            RegisterToTcAdmin();
            RegisterCrons();
            
            while (true)
            {
                Console.ReadLine();
            }
        }

        public static void RegisterCrons()
        {
            Log.Information("Initializing Cron Registry");

            CronRegistry.NonReentrantAsDefault();
            CronRegistry.Schedule<MinecraftVanillaUpdatesCron>().AndThen<MinecraftPaperUpdatesCron>().ToRunNow().AndEvery(1)
                .Hours();

            JobManager.Initialize(CronRegistry);
            
            foreach (var schedule in JobManager.AllSchedules)
            {
                Log.Information($"{schedule.Name} has been scheduled to run at {schedule.NextRun:F}");
            }
        }

        public static void RegisterToTcAdmin()
        {
            var config = TcAdminConfiguration.GetConfiguration();
            var tcAdminClientConfiguration =
                new TCAdminClientConfiguration(config.ConnectionString, config.Encrypted, "TCAdminCrons", new TCAdminSettings(false));
            _ = new TcAdminClient(tcAdminClientConfiguration);
            try
            {
                new Server(1).Find();
                Log.Information("Registered to TCAdmin successfully.");
            }
            catch (Exception e)
            {
                Log.Error(e, e.Message);
            }
        }
    }
}