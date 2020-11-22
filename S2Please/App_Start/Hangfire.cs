using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using S2Please.Jobs;

namespace S2Please
{
    public static class Hangfire
    {
        private static string _connection = ConfigurationManager.AppSettings["DBConnectionHangfire"];

        public static IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(_connection, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }
        public static void _Execute()
        {
            HangfireAspNet.Use(GetHangfireServers);
            // Let's also create a sample background job
            //BackgroundJob.Schedule(() => MailJob._Execute(),MailJob.Time);
            RecurringJob.AddOrUpdate<MailJob>(MailJob.RecurringJob, x=>x._Execute(), MailJob.CronExpression);


        }
    }
}