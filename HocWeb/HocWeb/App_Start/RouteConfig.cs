using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HocWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
          name: "Search",
          url: "tim-kiem",
          defaults: new { controller = "ChiTiet", action = "Search", id = UrlParameter.Optional },
          namespaces: new[] { "HocWeb.Controllers" }
        );

            routes.MapRoute(
         name: "Complete",
         url: "hoan-thanh",
         defaults: new { controller = "GioHang", action = "Complete", id = UrlParameter.Optional },
         namespaces: new[] { "HocWeb.Controllers" }
       );

            routes.MapRoute(
          name: "Buy",
          url: "thanh-toan",
          defaults: new { controller = "GioHang", action = "Buy", id = UrlParameter.Optional },
          namespaces: new[] { "HocWeb.Controllers" }
        );

            routes.MapRoute(
            name: "GioHang",
            url: "gio-hang",
            defaults: new { controller = "GioHang", action = "Index", id = UrlParameter.Optional },
            namespaces: new[] { "HocWeb.Controllers" }
        );
          
            routes.MapRoute(
               name: "Add Cart",
               url: "them-gio-hang",
               defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
               namespaces: new[] { "HocWeb.Controllers" }
        );
            //routes.MapRoute(
            //           name: "TrangChu",
            //           url: "trang-chu",
            //           defaults: new { controller = "TrangChu", action = "Index", id = UrlParameter.Optional },
            //           namespaces: new[] { "HocWeb.Controllers" }
            //       );


            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Register",
            //    url: "dang-ky",
            //    defaults: new { controller = "Register", action = "Register", id = UrlParameter.Optional },
            //    namespaces: new[] { "HocWeb.Controllers" }
            //);
          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TrangChu", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
