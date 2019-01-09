using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "ProductDetails",
                 url: "chi-tiet-san-pham/{tenHHSEO}",
                 defaults: new
                 {
                     controller = "Product",
                     action = "SEOUrl"
                 }
            );

            routes.MapRoute(
                 name: "ProductSortBy",
                 url: "danh-sach-san-pham/{tenHHSEO}",
                 defaults: new
                 {
                     controller = "Product",
                     action = "SortBy_SEOUrl"
                 }
            );

            routes.MapRoute(
                 name: "AllProduct",
                 url: "danh-sach-san-pham",
                 defaults: new
                 {
                     controller = "Product",
                     action = "ProductListAll"
                 }
            );

            routes.MapRoute(
                 name: "Cart",
                 url: "gio-hang",
                 defaults: new
                 {
                     controller = "Cart",
                     action = "Index"
                 }
            );

            routes.MapRoute(
                name: "Add_Cart",
                url: "them-gio-hang",
                defaults: new
                {
                    controller = "Cart",
                    action = "AddItem",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new
                {
                    controller = "MainHome",
                    action = "Contact",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "PlaceOrder",
                url: "dat-hang",
                defaults: new
                {
                    controller = "Cart",
                    action = "PlaceOrder",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Checkout",
                url: "thanh-toan",
                defaults: new
                {
                    controller = "Cart",
                    action = "Checkout",
                    id = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "MainHome", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
