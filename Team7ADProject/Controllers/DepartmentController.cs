using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Models;
using Team7ADProject.Database;

namespace Team7ADProject.Controllers
{
    public class DepartmentController : Controller
    {
        private static List<SidenavItem> staffSidenavItems;
        private static List<SidenavItem> headSidenavItems;
        private static Team7ADProjectDbContext db;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
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