using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Models;

namespace Team7ADProject.Controllers
{
    public class DepartmentController : Controller
    {
        static List<SidenavItem> staffSidenavItems;
        static List<SidenavItem> headSidenavItems;
        public static void Init()
        {
            staffSidenavItems = new List<SidenavItem>();
            staffSidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/StationeryRequests"));
            staffSidenavItems.Add(new SidenavItem("Disbursement Lists", "/Department/DisbursementLists"));
            staffSidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
        }
        public ActionResult Index()
        {
            return RedirectToAction("StationeryRequests");
        }
        public ActionResult StationeryRequests()
        {
            ViewData["showSidebar"] = true;
            ViewData["sidenavItems"] = staffSidenavItems;
            return View();
        }
    }
}