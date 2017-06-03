using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
               name: "Checkout",
               url: "thanh-toan",
               defaults: new { controller = "CartItem", action = "Checkout" },
               namespaces: new string[] { "Web.Controllers" }
           );

            routes.MapRoute(
               name: "CartItem",
               url: "gio-hang",
               defaults: new { controller = "CartItem", action = "Index" },
               namespaces: new string[] { "Web.Controllers" }
           );

            routes.MapRoute(
               name: "Login",
               url: "dang-nhap",
               defaults: new { controller = "Account", action = "Login" },
               namespaces: new string[] { "Web.Controllers" }
           );

            routes.MapRoute(
               name: "Register",
               url: "dang-ky",
               defaults: new { controller = "Account", action = "Register" },
               namespaces: new string[] { "Web.Controllers" }
           );

            routes.MapRoute(
                name: "GetListProductByTag",
                url: "tag/{tagId}",
                defaults: new { controller = "Product", action = "GetListByTag", tagId = UrlParameter.Optional },
                namespaces: new string[] { "Web.Controllers" }
            );

            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "Product", action = "Search" },
                namespaces: new string[] { "Web.Controllers" }
            );

            routes.MapRoute(
                name: "Detail",
                url: "chi-tiet/{alias}-{productId}",
                defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
                namespaces: new string[] { "Web.Controllers" }
            );

            routes.MapRoute(
                name: "Category",
                url: "{alias}-{categoryId}",
                defaults: new { controller = "Product", action = "Category", categoryId = UrlParameter.Optional },
                namespaces: new string[] { "Web.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new string[] { "Web.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Web.Controllers" }
            );
        }
    }
}
