using System.Web.Mvc;

namespace S2Please.Areas.WEB_SHOP
{
    public class WEB_SHOPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WEB_SHOP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              name: "Home-Page",
              url: "Home-Page",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              name: "Login",
              url: "Login",
              defaults: new { controller = "Authen", action = "Login", id = UrlParameter.Optional }
          );
            context.MapRoute(
             name: "ForgotPassword",
             url: "ForgotPassword",
             defaults: new { controller = "Authen", action = "ForgotPassword", id = UrlParameter.Optional }
         );
            context.MapRoute(
             name: "Product-Detail",
             url: "Product-Detail/{toDecrypt}",
             defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional }
         );
            context.MapRoute(
             name: "Products",
             url: "Products/{toDecryptProductType}/{toDecryptGroup}",
             defaults: new { controller = "Product", action = "Products", id = UrlParameter.Optional }
         );
            context.MapRoute(
            name: "Cart",
            url: "Cart",
            defaults: new { controller = "Cart", action = "Cart", id = UrlParameter.Optional }
        );
            context.MapRoute(
            name: "checkout",
            url: "checkout",
            defaults: new { controller = "Cart", action = "CheckOut", id = UrlParameter.Optional }
        );
            context.MapRoute(
          name: "order_information",
          url: "order_information/{toDecrypt}",
          defaults: new { controller = "Cart", action = "OrderInformation", id = UrlParameter.Optional }
      );
            context.MapRoute(
          name: "Customer",
          url: "Customer",
          defaults: new { controller = "User", action = "Customer", id = UrlParameter.Optional }
      );
            context.MapRoute(
         name: "customer-update-order",
         url: "customer-update-order/{toDecrypt}",
         defaults: new { controller = "User", action = "FormUpdateOrder", id = UrlParameter.Optional }
     );
            context.MapRoute(
                "WEB_SHOP_default",
                "WEB_SHOP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}