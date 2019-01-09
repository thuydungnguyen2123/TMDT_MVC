using System.Web.Mvc;

namespace WebStore.Areas.admin
{
    public class adminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                 name: "ChiTietSanPham",
                 url: "admin/chi-tiet-san-pham/{tenHHSEO}",
                 defaults: new
                 {
                     controller = "All_Products",
                     action = "SEOUrl"
                 }
            );

            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}