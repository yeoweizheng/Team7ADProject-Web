using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7ADProject.Controllers
{
    public class RestController : Controller
    {
        public JsonResult TestConnection(string header, string body)
        {
            System.Diagnostics.Debug.WriteLine("msg");
            System.Diagnostics.Debug.WriteLine(header);
            return Json(new { result = "success" });
        }
    }
}