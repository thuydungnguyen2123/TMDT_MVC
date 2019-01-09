using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class ProductController : Controller
    {
        private WebStoreEntities db = new WebStoreEntities();

        // GET: Product
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult ProductListAll()
        {
            var products = db.Products.Include(p => p.Brand).ToList();
            return View(products);
        }

        public ActionResult ProductListByBrand(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lst = db.Products.Where(p => p.Brand.Ma_Brand.ToString() == id || p.Brand.Ten_Brand == id).ToList();
            return View("~/Views/Product/ProductListAll.cshtml",lst);
        }

        public ActionResult ProductListByType(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lst = db.Products.Where(p => p.Brand.Ma_Brand.ToString() == id || p.Brand.Ten_Brand == id).ToList();
            return View("~/Views/Product/ProductListAll.cshtml", lst);
        }

        public ActionResult ProductDetail(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            ViewBag.idpro = id;
            var lst = db.Products.Where(p => p.Ma_Brand == product.Ma_Brand/* && p.Ma_Product != product.Ma_Product*/).ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            if (product.Available > 0)
            {
                ViewBag.avai = "Còn hàng";
            }
            else {
                ViewBag.avai = "Cháy hàng";
            }
            return View(lst);           
        }

        public ActionResult SEOUrl(string tenHHSEO)
        {
            var id = int.Parse(tenHHSEO.Substring(0, tenHHSEO.IndexOf("-")));
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.idpro = id;
            var lst = db.Products.Where(p => p.Ma_Brand == product.Ma_Brand/* && p.Ma_Product != product.Ma_Product*/).ToList();
            int dem = lst.Count();
            ViewBag.relate = lst;
            if (product.Available > 0)
            {
                ViewBag.avai = "Còn hàng";
            }
            else {
                ViewBag.avai = "Cháy hàng";
            }
            return View("ProductDetail", product);
        }

        public ActionResult SortBy_SEOUrl(string tenHHSEO)
        {
            string a = tenHHSEO.Substring(0, tenHHSEO.IndexOf("-"));
            string b = tenHHSEO.Substring(a.Length + 1);

            var lst = db.Products.Where(p => p.Brand.Ten_Brand.Contains(b) || p.Type.Ten_Type.Contains(b)).ToList();           
            return View("~/Views/Product/ProductListAll.cshtml", lst);
        }
    }
}