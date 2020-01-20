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
    public class DepartmentController : Controller
    {
        private static List<SidenavItem> staffSidenavItems;
        private static List<SidenavItem> headSidenavItems;
        private static Team7ADProjectDbContext db;
        private static UserService userService;
        private static NotificationService notificationService;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
            notificationService = new NotificationService();
            staffSidenavItems = new List<SidenavItem>();
            staffSidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/StationeryRequests"));
            staffSidenavItems.Add(new SidenavItem("Disbursement Lists", "/Department/DisbursementLists"));
            staffSidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
        }
        public ActionResult Index()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            return RedirectToAction("StationeryRequests");
        }
        public ActionResult StationeryRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = staffSidenavItems;
            List<StationeryRequest> stationeryRequests = db.StationeryRequest.ToList();
            ViewData["stationeryRequests"] = stationeryRequests;
            return View();
        }
        public ActionResult Notifications()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "departmentStaff"? staffSidenavItems : headSidenavItems;
            return View();
        }
        [Route("Staff/StationeryRequestDetails/{stationeryRequestId}")]
        public ActionResult StationeryRequestDetails(int stationeryRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = staffSidenavItems;

            System.Diagnostics.Debug.WriteLine(stationeryRequestId + "");
            return View();
        }
    }
}