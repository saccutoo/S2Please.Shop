using Microsoft.Owin;
using Owin;
using Hangfire;
using S2Please.Filters;

[assembly: OwinStartup(typeof(S2Please.Startup))]
namespace S2Please
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            //app.UseHangfireDashboard();
            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
            //{
            //    Authorization = new[] { new UseHangfireDashboard() }
            //});
            //app.UseHangfireServer();

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
//            //ConfigureAuth(app);
//            app.MapSignalR();
//            app.UseHangfireDashboard();
//            //app.UseHangfireDashboard("/hangfire", new DashboardOptions
//            //{
//            //    Authorization = new[] { new UseHangfireDashboard() }
//            //});
//            app.UseHangfireServer();
//        }
//    }
//}
