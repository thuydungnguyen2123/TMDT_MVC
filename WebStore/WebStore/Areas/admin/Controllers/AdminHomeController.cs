using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebStore.Areas.admin.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: admin/AdminHome
        public ActionResult Index()
        {
            return View();
        }
    }
}