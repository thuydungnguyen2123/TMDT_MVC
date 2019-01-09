using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebStore.Models;

namespace WebStore.Areas.admin.Controllers
{
    public class All_ProductsController : Controller
    {
        private WebStoreEntities db = new WebStoreEntities();

        // GET: admin/All_Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Type);
            return View(products.ToList());
        }

        // GET: admin/All_Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: admin/All_Products/Create
        public ActionResult Create()
        {
            ViewBag.Ma_Brand = new SelectList(db.Brands, "Ma_Brand", "Ten_Brand");
            ViewBag.Ma_Type = new SelectList(db.Types, "Ma_Type", "Ten_Type");
            return View();
        }

        // POST: admin/All_Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Ma_Product,Ma_Brand,Ma_Type,Ten_Product,Gia_Product,Info_Product,Available,Image_Product,Detail_Product,Describe_Product,Ten_Alias")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Ten_Alias = MyTools.ToURLFriendly(product.Ma_Product, product.Ten_Product);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ma_Brand = new SelectList(db.Brands, "Ma_Brand", "Ten_Brand", product.Ma_Brand);
            ViewBag.Ma_Type = new SelectList(db.Types, "Ma_Type", "Ten_Type", product.Ma_Type);
            return View(product);
        }

        // GET: admin/All_Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_Brand = new SelectList(db.Brands, "Ma_Brand", "Ten_Brand", product.Ma_Brand);
            ViewBag.Ma_Type = new SelectList(db.Types, "Ma_Type", "Ten_Type", product.Ma_Type);
            return View(product);
        }

        // POST: admin/All_Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Ma_Product,Ma_Brand,Ma_Type,Ten_Product,Gia_Product,Info_Product,Available,Image_Product,Detail_Product,Describe_Product,Ten_Alias")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.Ten_Alias = MyTools.ToURLFriendly(product.Ma_Product, product.Ten_Product);
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ma_Brand = new SelectList(db.Brands, "Ma_Brand", "Ten_Brand", product.Ma_Brand);
            ViewBag.Ma_Type = new SelectList(db.Types, "Ma_Type", "Ten_Type", product.Ma_Type);
            return View(product);
        }

        // GET: admin/All_Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: admin/All_Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult SEOUrl(string tenHHSEO)
        {
            var id = int.Parse(tenHHSEO.Substring(0, tenHHSEO.IndexOf("-")));
            var sp = db.Products.SingleOrDefault(p => p.Ma_Product == id);
            return View("Details", sp);
        }

        /// <summary>
        /// Covert correct data
        /// </summary>
        /// <returns></returns>
        public string ConvertUrlFriendly()
        {
            foreach (Product sp in db.Products)
            {
                sp.Ten_Alias = MyTools.ToURLFriendly(sp.Ma_Product, sp.Ten_Product);
            }
            foreach (Brand sp in db.Brands)
            {
                sp.Ten_Alias = MyTools.ToURLFriendly(sp.Ma_Brand, sp.Ten_Brand);
            }
            foreach (Models.Type sp in db.Types)
            {
                sp.Ten_Alias = MyTools.ToURLFriendly(sp.Ma_Type, sp.Ten_Type);
            }
            db.SaveChanges();
            return "OK";
        }
    }
}
