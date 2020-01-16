using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["showSidebar"] = false;
            return View();
        }
        public ActionResult Login()
        {
            ViewData["showSidebar"] = false;
            return View();
        }

    }
}