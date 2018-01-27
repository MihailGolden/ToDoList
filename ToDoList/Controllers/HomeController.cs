using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int C1 = 0;
            int branch1 = 0;
            int B1 = 0;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}