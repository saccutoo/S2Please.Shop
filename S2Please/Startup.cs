using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(S2Please.Startup))]
namespace S2Please
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}

//using Owin;
//using Microsoft.Owin;
//[assembly: OwinStartup(typeof(SignalRChat.Startup))]
//namespace SignalRChat
//{
//    public class Startup
//    {
//        public void Configuration(IAppBuilder app)
//        {

//            app.MapSignalR();
//        }
//    }
//}