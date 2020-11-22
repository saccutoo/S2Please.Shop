using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Hangfire;
using S2Please.Jobs;
using System.Configuration;
using Hangfire.SqlServer;

[assembly: OwinStartup(typeof(S2Please.Startup))]
namespace S2Please
{
    public partial class Startup
    {
        private  string _connection = ConfigurationManager.AppSettings["DBConnectionHangfire"];
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            //GlobalConfiguration.Configuration.UseSqlServerStorage(_connection);
            //RecurringJob.AddOrUpdate<MailJob>(MailJob.RecurringJob, x => x._Execute(), MailJob.CronExpression);
            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}

//using Owin;
//using Microsoft.Owin;
//using System.Web.Http;
//using Hangfire;
//[assembly: OwinStartup(typeof(SignalRChat.Startup))]

//namespace SignalRChat
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {
//            app.MapSignalR();
//            //app.UseHang
//        }
//    }
//}
