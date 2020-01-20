using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Models;
using Team7ADProject.Database;
using Team7ADProject.Service;

namespace Team7ADProject.Controllers
{
    public class StoreController : Controller
    {
        private static List<SidenavItem> clerkSideNavItems;
        private static Team7ADProjectDbContext db;
        private static UserService userService;
        private static StationeryService stationeryService;

        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
            stationeryService = new StationeryService();
            clerkSideNavItems = new List<SidenavItem>();
            clerkSideNavItems.Add(new SidenavItem("Stationery Requests", "/Store/StationeryRequests"));
            clerkSideNavItems.Add(new SidenavItem("Stationery Retrieval List", "/Store/RetrievalList"));
            clerkSideNavItems.Add(new SidenavItem("Disbursement List", "/Store/DisbursementList"));
            clerkSideNavItems.Add(new SidenavItem("Stock List", "/Store/StockList"));
            clerkSideNavItems.Add(new SidenavItem("Adjustment Vouchers", "/Store/AdjustmentVouchers"));
            clerkSideNavItems.Add(new SidenavItem("Orders", "/Store/Orders"));
            clerkSideNavItems.Add(new SidenavItem("Notifications", "/Store/Notifications"));
        }

        public ActionResult Index()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            return RedirectToAction("StationeryRequests");
        }
        public ActionResult StationeryRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            List<DepartmentRequest> departmentRequests = db.DepartmentRequest.ToList();
            ViewData["departmentRequests"] = departmentRequests;
            return View();
        }
        public ActionResult RetrievalList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            return View();
        }
        public ActionResult DisbursementList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            return View();
        }
        public ActionResult StockList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["stationeries"] = stationeryService.GetStationeries();
            return View();
        }
        [Route("Store/StockDetail/{stationeryId}")]
        public ActionResult StockDetail(int stationeryId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["stationery"] = stationeryService.GetStationeryById(stationeryId);
            return View();
        }

        public ActionResult AdjustmentVouchers()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            List<AdjustmentVoucher> adjustmentVouchers = db.AdjustmentVoucher.ToList();
            ViewData["adjustmentVouchers"] = adjustmentVouchers;
            return View();
        }
        public ActionResult Orders()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            return View();
        }
        public ActionResult Notifications()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            return View();
        }

    }
}