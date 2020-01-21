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
            staffSidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/StaffStationeryRequests"));
            staffSidenavItems.Add(new SidenavItem("Disbursement Lists", "/Department/DisbursementLists"));
            staffSidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
        }
        public ActionResult Index()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            switch (user.UserType)
            {
                case "departmentStaff":
                    return RedirectToAction("StaffStationeryRequests");
                case "departmentHead":
                    return RedirectToAction("HeadStationeryRequests");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult StaffStationeryRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
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
            ViewData["user"] = user;
            return View();
        }
        [Route("Staff/StationeryRequestDetail/{stationeryRequestId}")]
        public ActionResult StationeryRequestDetail(int stationeryRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = staffSidenavItems;
            StationeryRequest stationeryRequest = db.StationeryRequest.Where(x => x.StationeryRequestId == stationeryRequestId).FirstOrDefault();
            ViewData["stationeryRequest"] = stationeryRequest;
            List<StationeryQuantity> stationeryQuantities = db.StationeryQuantity.ToList();
            ViewData["stationeryQuantities"] = stationeryQuantities;
            System.Diagnostics.Debug.WriteLine(stationeryRequestId + "");
            return View();
        }
    }
}