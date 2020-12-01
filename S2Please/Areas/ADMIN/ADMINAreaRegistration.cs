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
             name: "admin/order-save/{id}",
             url: "admin/order-save/{id}",
             defaults: new { controller = "Order", action = "OrderSave", id = UrlParameter.Optional }
         );
            context.MapRoute(
            name: "admin/update-order/{id}",
            url: "admin/update-order/{id}",
            defaults: new { controller = "Order", action = "UpdateOrder", id = UrlParameter.Optional }
        );

           context.MapRoute(
          name: "admin/product-type",
          url: "admin/product-type",
          defaults: new { controller = "ProductType", action = "Index", id = UrlParameter.Optional }
      );
            context.MapRoute(
        name: "admin/product-group",
        url: "admin/product-group",
        defaults: new { controller = "ProductGroup", action = "Index", id = UrlParameter.Optional }
    );
            context.MapRoute(
        name: "admin/menu",
        url: "admin/menu",
        defaults: new { controller = "Menu", action = "Index", id = UrlParameter.Optional }
    );
            context.MapRoute(
        name: "admin/mail-queue",
        url: "admin/mail-queue",
        defaults: new { controller = "MailQueue", action = "Index", id = UrlParameter.Optional }
    );
            context.MapRoute(
       name: "admin/menu-admin",
       url: "admin/menu-admin",
       defaults: new { controller = "MenuAdmin", action = "Index", id = UrlParameter.Optional }
   );
            context.MapRoute(
      name: "admin/permission-role",
      url: "admin/permission-role",
      defaults: new { controller = "Permission", action = "PermissionRole", id = UrlParameter.Optional }
  );
            context.MapRoute(
      name: "admin/permission-user",
      url: "admin/permission-user",
      defaults: new { controller = "Permission", action = "PermissionUser", id = UrlParameter.Optional }
  );
            context.MapRoute(
                "ADMIN_default",
                "ADMIN/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}