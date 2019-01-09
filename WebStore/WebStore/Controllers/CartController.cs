using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private WebStoreEntities db = new WebStoreEntities();

        //can cai key de luu session
        private const string CartSession = "CartSession";

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var lst = new List<CartItem>();
            if (cart != null)
            {
                lst = (List<CartItem>)cart;
            }
            return View(lst);
        }

        public ActionResult AddItem(int id, int qty)
        {
            Product sp = db.Products.Find(id);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var lst = (List<CartItem>)cart;
                if (lst.Exists(x => x.product.Ma_Product == sp.Ma_Product))
                {
                    var item = lst.Single(p => p.product.Ma_Product == sp.Ma_Product);
                    item.Quantity++;
                }
                else
                {
                    var item = new CartItem();
                    item.product = sp;
                    item.Quantity = qty;
                    lst.Add(item);
                    //gan vao session
                    Session[CartSession] = lst;
                    ViewBag.dem = lst.Count();
                }
            }
            else
            {
                //tao moi doi tuong cartitem 
                var item = new CartItem();
                item.product = sp;
                item.Quantity = qty;
                var lst = new List<CartItem>();
                lst.Add(item);
                //gan vao session
                Session[CartSession] = lst;
                ViewBag.dem = lst.Count();
            }
            return RedirectToAction("Index");//return cai View nay de hien thi gio hang
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
            if (sessionCart != null)
            {
                foreach (var item in sessionCart)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.product.Ma_Product == item.product.Ma_Product);
                    if (jsonItem != null)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult ClearAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(int id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.product.Ma_Product == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult PlaceOrder(int ship)
        {
            int shipFee = ship;
            decimal? total = 0;
            var cart = Session[CartSession];
            //ViewBag.Cart = cart;
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            foreach (var item in list)
            {
                total = total + (item.product.Gia_Product * item.Quantity);
            }
            ViewBag.TongTien = total;
            ViewBag.TienShip = shipFee;
            ViewBag.Tong = total + shipFee;
            return View(list);
        }

        //[HttpPost]
        //public ActionResult Checkout(string ShipName, string mobile, string address, string email)
        //{
        //    try
        //    {
        //        var dao = new UserDao();
        //        var order = new Order();
        //        order.CreatedDate = DateTime.Now;
        //        order.ShipMobile = mobile;
        //        order.ShipAddress = address;
        //        order.ShipEmail = email;
        //        var id = new OrderDao().Insert(order);
        //        var cart = (List<CartItem>)Session[CartSession];
        //        var detailDao = new OrderDetailDao();
        //        decimal total = 0;
        //        foreach (var item in cart)
        //        {
        //            var orderDetail = new OrderDetail();
        //            orderDetail.ProductID = item.Product.ID;
        //            orderDetail.OrderID = id;
        //            orderDetail.Price = item.Product.Price;
        //            orderDetail.Quantity = item.Quantity;
        //            detailDao.Insert(orderDetail);
        //            total += item.Product.Price.GetValueOrDefault(0) * item.Quantity;
        //        }
        //        var bill = new Bill();
        //        bill.OrderID = id;
        //        bill.TotalPay = total;
        //        bill.CreateDate = DateTime.Now;
        //        bill.Status = "Chua thanh toan";
        //        var billTotalPay = new BillDao().Insert(bill);
        //        string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/Client/template/neworder.html"));
        //        content = content.Replace("{{CustomerName}}", ShipName);
        //        content = content.Replace("{{Phone}}", mobile);
        //        content = content.Replace("{{Email}}", email);
        //        content = content.Replace("{{Address}}", address);
        //        content = content.Replace("{{Total}}", total.ToString("N0"));
        //        //lấy toEmail từ quản trị trong webconfig
        //        var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

        //        new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
        //        new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Redirect("/loi-thanh-toan");
        //    }
        //}
    }
}