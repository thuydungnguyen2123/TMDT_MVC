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
    public class DS_OrdersController : Controller
    {
        private WebStoreEntities db = new WebStoreEntities();

        // GET: admin/DS_Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Member);
            return View(orders.ToList());
        }

        // GET: admin/DS_Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            ViewBag.info = "Mã đơn hàng: " + order.Ma_Order.ToString();
            var dto = db.DetailOrders.Where(p => p.Ma_Order == id).ToList();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(dto);
        }

        // GET: admin/DS_Orders/Create
        public ActionResult Create()
        {
            ViewBag.Ma_Mem = new SelectList(db.Members, "Ma_Mem", "Ten_Mem");
            return View();
        }

        // POST: admin/DS_Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ma_Order,Ma_Mem,Total,Created,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Ma_Mem = new SelectList(db.Members, "Ma_Mem", "Ten_Mem", order.Ma_Mem);
            return View(order);
        }

        // GET: admin/DS_Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ma_Mem = new SelectList(db.Members, "Ma_Mem", "Ten_Mem", order.Ma_Mem);
            return View(order);
        }

        // POST: admin/DS_Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ma_Order,Ma_Mem,Total,Created,Note")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ma_Mem = new SelectList(db.Members, "Ma_Mem", "Ten_Mem", order.Ma_Mem);
            return View(order);
        }

        // GET: admin/DS_Orders/Detail_Delete/5
        public ActionResult Detail_Delete(int? id,int idO)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Order order = db.Orders.Find(id);
            DetailOrder dto = db.DetailOrders.Find(id);
            ViewBag.idOrder = idO;
            if (dto == null)
            {
                return HttpNotFound();
            }
            return View(dto);
        }

        // POST: admin/DS_Orders/Detail_Delete/5
        [HttpPost, ActionName("Detail_Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Detail_DeleteConfirmed(int id, int idO)
        {
            Order order = db.Orders.Find(idO);
            DetailOrder dto = db.DetailOrders.Find(id);

            double tien = dto.Price;
            order.Total -= tien;

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
            }
            db.DetailOrders.Remove(dto);
            db.SaveChanges();


            return RedirectToAction("Details/"+ idO.ToString());
        }

        // GET: admin/DS_Orders/Detail_Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: admin/DS_Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            var tran = db.Transactions.SingleOrDefault(p => p.Ma_Order == id);
            var lst = db.DetailOrders.Where(p => p.Ma_Order == id).ToList();
            for(int i = 0; i < lst.Count(); i++)
            {
                DetailOrder dto = db.DetailOrders.Find(lst[i].Ma_DeOr);
                db.DetailOrders.Remove(dto);
                db.SaveChanges();
            }

            db.Transactions.Remove(tran);
            db.Orders.Remove(order);
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
    }
}
