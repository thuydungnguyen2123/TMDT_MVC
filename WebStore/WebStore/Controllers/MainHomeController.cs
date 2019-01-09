using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class MainHomeController : Controller
    {
        private WebStoreEntities db = new WebStoreEntities();
        private const string CartSession = "CartSession";

        // GET: MainHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Send(string name, string mobile, string email, string content)
        {
            var feedback = new Feedback();
            feedback.Ten = name;
            feedback.SDT = mobile;
            feedback.Mail = email;
            feedback.Content = content;
            feedback.Created = DateTime.Now;
            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            int id = 0;
            id = feedback.Ma_FB;
            if (id == 0)
            {
                //sendMail cho quản trị ở đây
                //string conMail = System.IO.File.ReadAllText(Server.MapPath("~/Content/Mail/newfeedback.html"));
                WebClient wb = new WebClient();
                wb.Encoding = ASCIIEncoding.UTF8;
                string conMail = wb.DownloadString(Server.MapPath("~/Content/Mail/newfeedback.html"));
                conMail = conMail.Replace("{{CustomerName}}", name);
                conMail = conMail.Replace("{{Phone}}", mobile);
                conMail = conMail.Replace("{{Email}}", email);
                conMail = conMail.Replace("{{Content}}", content);
                //lấy toEmail từ quản trị trong webconfig
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                new MailHelper().SendMail(toEmail, "Feedback của khách hàng", conMail, "Feedback");

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}