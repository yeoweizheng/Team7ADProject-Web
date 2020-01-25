using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    public class RestController : Controller
    {
        public ActionResult TestConnection(string header, string body)
        {
            System.Diagnostics.Debug.WriteLine(header);
            System.Diagnostics.Debug.WriteLine(body);
            return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
        }
    }
}