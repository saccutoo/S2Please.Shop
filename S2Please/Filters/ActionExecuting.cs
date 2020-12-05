using S2Please.Helper;
using S2Please.Models;
using SHOP.COMMON;
using SHOP.COMMON.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2Please.Filters
{
    public class ActionExecuting : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rawUrl =HttpContext.Current.Request.RawUrl;
            if (HttpContext.Current.Request.FilePath.ToString().ToLower().IndexOf("admin") > -1)
            {
                if (filterContext.ActionDescriptor.ActionName.ToLower() != "login")
                {
                    if (CurrentUser.UserAdmin == null || CurrentUser.UserAdmin.USER_ID == 0)
                    {
                        if (filterContext.ActionDescriptor.ActionName.ToLower().Contains("sendrequestchangepass") || filterContext.ActionDescriptor.ActionName.ToLower().Contains("forgotpassword"))
                        {
                            goto Finish;
                        }
                        filterContext.Result = new RedirectToRouteResult(new
             System.Web.Routing.RouteValueDictionary(new { controller = "Authen", action = "Login", RedirectUrl = rawUrl }));
                    }

                    else
                    {
                        //var user = MapperHelper.Map<SHOP.COMMON.Entity.User,UserModel>(CurrentUser.UserAdmin);
                        //Security.UserSignInAdmin(user, HttpContext.Current);

                        Dictionary<string, DateTime> loggedInUsers = new Dictionary<string, DateTime>();

                        loggedInUsers = (Dictionary<string, DateTime>)HttpRuntime.Cache["LoggedInUsers"];

                        var userName = CurrentUser.UserAdmin.USER_NAME;
                        if (loggedInUsers != null)
                        {
                            loggedInUsers[userName] = DateTime.Now;
                            HttpRuntime.Cache["LoggedInUsers"] = loggedInUsers;
                        }

                        goto Finish;
                    }
                }
                else
                {
                    if (CurrentUser.UserAdmin == null || CurrentUser.UserAdmin.USER_ID == 0)
                    {
                        goto Finish;
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
            System.Web.Routing.RouteValueDictionary(new { controller = "Dashboard", action = "Index", RedirectUrl = rawUrl }));
                    }
                }
            }

            Finish:
            base.OnActionExecuting(filterContext);
        }

    }
}