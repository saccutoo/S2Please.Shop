using Newtonsoft.Json;
using S2Please.Helper;
using SHOP.COMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using S2Please.Database;
using System.Web.Security;
using System.Data.SqlClient;
using System.Configuration;

namespace S2Please
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterDependencies.Register();
            //Hangfire._Execute();
            //SqlDependency.Start(ConfigurationManager.AppSettings["DBConnection"]);
        }

        protected void Application_BeginRequest()
        {
            try
            {
                Security.CheckCookieLanguage();
                Security.CheckCookieTimeVersion();
            }
            catch (Exception)
            {
                FormsAuthentication.SignOut();
            }

        }
        //protected void Application_EndRequest()
        //{
        //    Dictionary<string, DateTime> loggedInUsers = new Dictionary<string, DateTime>();

        //    loggedInUsers = (Dictionary<string, DateTime>)HttpRuntime.Cache["LoggedInUsers"];
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var userName = User.Identity.Name;
        //        if (loggedInUsers != null)
        //        {
        //            loggedInUsers[userName] = DateTime.Now;
        //            HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
        //        }
        //    }

        //    if (loggedInUsers != null)
        //    {
        //        foreach (var item in loggedInUsers.ToList())
        //        {
        //            if (item.Value < DateTime.Now.AddMinutes(-10))
        //            {
        //                loggedInUsers.Remove(item.Key);
        //            }
        //        }
        //        HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
        //    }

        //}
    }
}
