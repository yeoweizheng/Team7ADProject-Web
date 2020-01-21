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
        private static List<SidenavItem> supSideNavItems;
        private static Team7ADProjectDbContext db;
        private static UserService userService;
        private static StationeryService stationeryService;
        private static RequestService requestService;

        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
            stationeryService = new StationeryService();
            requestService = new RequestService();
            clerkSideNavItems = new List<SidenavItem>();
            clerkSideNavItems.Add(new SidenavItem("Department Requests", "/Store/DepartmentRequests"));
            clerkSideNavItems.Add(new SidenavItem("Stationery Retrieval List", "/Store/RetrievalList"));
            clerkSideNavItems.Add(new SidenavItem("Disbursement List", "/Store/DisbursementList"));
            clerkSideNavItems.Add(new SidenavItem("Stock List", "/Store/StockList"));
            clerkSideNavItems.Add(new SidenavItem("Adjustment Vouchers", "/Store/ClerkAdjustmentVouchers"));
            clerkSideNavItems.Add(new SidenavItem("Orders", "/Store/Orders"));
            clerkSideNavItems.Add(new SidenavItem("Notifications", "/Store/Notifications"));
            supSideNavItems = new List<SidenavItem>();
            supSideNavItems.Add(new SidenavItem("Stock List", "/Store/StockList"));
            supSideNavItems.Add(new SidenavItem("Adjustment Vouchers", "/Store/SupAdjustmentVouchers"));
            supSideNavItems.Add(new SidenavItem("Orders", "/Store/Orders"));
            supSideNavItems.Add(new SidenavItem("Reports", "/Store/Reports"));
            supSideNavItems.Add(new SidenavItem("Notifications", "/Store/Notifications"));
        }

        public ActionResult Index()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            switch (user.UserType)
            {
                case "storeClerk":
                    return RedirectToAction("DepartmentRequests");
                case "storeSupervisor":
                    return RedirectToAction("StockList");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DepartmentRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["departmentRequests"] = requestService.GetDepartmentRequests();
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
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            ViewData["stationeries"] = stationeryService.GetStationeries();
            return View();
        }
        [Route("Store/StockDetail/{stationeryId}")]
        public ActionResult StockDetail(int stationeryId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["stationery"] = stationeryService.GetStationeryById(stationeryId);
            return View();
        }

        public ActionResult ClerkAdjustmentVouchers()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["adjustmentVouchers"] = stationeryService.GetAdjustmentVouchers();
            return View();
        }
       
        public ActionResult AddAdjustmentVoucher()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            List<Stationery> stationeries = db.Stationery.ToList();
            ViewData["stationeries"] = stationeries;
            ViewData["sidenavItems"] = clerkSideNavItems;
            return View();
        }
        public ActionResult Orders()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
           
            return View();
        }
        public ActionResult Notifications()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            ViewData["user"] = user;
            return View();
        }
        public ActionResult SupAdjustmentVouchers()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = supSideNavItems;
            ViewData["adjustmentVouchers"] = stationeryService.GetAdjustmentVouchers();
            return View();
        }

    }
}