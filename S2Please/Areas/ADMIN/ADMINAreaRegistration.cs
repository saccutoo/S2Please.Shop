using System.Web.Mvc;

namespace S2Please.Areas.ADMIN
{
    public class ADMINAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ADMIN";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
           name: "admin/product",
           url: "admin/product",
           defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
          name: "admin/product-save/{id}",
          url: "admin/product-save/{id}",
          defaults: new { controller = "Product", action = "ProductSave", id = UrlParameter.Optional }
      );
            context.MapRoute(
             name: "admin/dashboard",
             url: "admin/dashboard",
             defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
         );
            context.MapRoute(
            name: "admin/white-page",
            url: "admin/white-page",
            defaults: new { controller = "Dashboard", action = "WhitePage", id = UrlParameter.Optional }
        );
            context.MapRoute(
            name: "admin/login",
            url: "admin/login",
            defaults: new { controller = "Authen", action = "Login", id = UrlParameter.Optional }
        );
           
            context.MapRoute(
            name: "admin/send-request",
            url: "admin/send-request",
            defaults: new { controller = "Authen", action = "SendRequestChangePass", id = UrlParameter.Optional }
        );
            context.MapRoute(
           name: "admin/logout",
           url: "admin/logout",
           defaults: new { controller = "Authen", action = "Logout", id = UrlParameter.Optional }
       );
        context.MapRoute(
           name: "admin/order",
           url: "admin/order",
           defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
       );

            context.MapRoute(
                "ADMIN_default",
                "ADMIN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}