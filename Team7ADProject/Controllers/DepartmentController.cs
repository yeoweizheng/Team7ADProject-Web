using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Models;
using Team7ADProject.Database;
using Team7ADProject.Service;
using Newtonsoft.Json;

namespace Team7ADProject.Controllers
{
    public class DepartmentController : Controller
    {
        private static List<SidenavItem> staffSidenavItems;
        private static List<SidenavItem> headSidenavItems;
        private static UserService userService;
        private static Team7ADProjectDbContext db;
        private static StationeryService stationeryService;
        private static NotificationService notificationService;
        private static RequestService requestService;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
            stationeryService = new StationeryService();
            notificationService = new NotificationService();
            requestService = new RequestService();
            staffSidenavItems = new List<SidenavItem>();
            staffSidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/StaffStationeryRequests"));
            staffSidenavItems.Add(new SidenavItem("Department Requests", "/Department/DepartmentRequests"));
            staffSidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
            headSidenavItems = new List<SidenavItem>();
            headSidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/HeadStationeryRequests"));
            headSidenavItems.Add(new SidenavItem("Authorize Staff", "/Department/AuthorizeStaff"));
            headSidenavItems.Add(new SidenavItem("Assign Representative", "/Department/AssignRepresentative"));
            headSidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
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
            ViewData["user"] = user;
            ViewData["sidenavItems"] = staffSidenavItems;
            ViewData["stationeryRequests"] = requestService.GetStationeryRequestsByStaffId(user.UserId);
            return View();
        }
        public ActionResult AddStationeryRequest(string stationeryQuantitiesJSON)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = staffSidenavItems;
            ViewData["stationeries"] = stationeryService.GetStationeries();
            if (HttpContext.Request.HttpMethod == "POST")
            {
                System.Diagnostics.Debug.WriteLine(user.ToString());
                requestService.AddStationeryRequest(user.UserId, stationeryQuantitiesJSON);
                return new HttpStatusCodeResult(200);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Notifications()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = user.UserType == "departmentStaff" ? staffSidenavItems : headSidenavItems;
            ViewData["notificationStatuses"] = notificationService.GetNotificationStatusesFromUser(user.UserId);
            return View();
        }

        [Route("Department/NotificationDetail/{notificationStatusId}")]
        public ActionResult NotificationDetail(int notificationStatusId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = user.UserType == "departmentStaff" ? staffSidenavItems : headSidenavItems;
            NotificationStatus notificationStatus = notificationService.GetNotificationStatusById(notificationStatusId);
            ViewData["notification"] = notificationStatus.Notification;
            ViewData["notificationStatuses"] = notificationService.GetNotificationStatusesFromUser(user.UserId);
            notificationService.MarkAsRead(notificationStatusId);
            return View();
        }
        [Route("Department/StationeryRequestDetail/{stationeryRequestId}")]
        public ActionResult StationeryRequestDetail(int stationeryRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff" && user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = user.UserType == "departmentStaff" ? staffSidenavItems : headSidenavItems;
            ViewData["stationeryRequest"] = requestService.GetStationeryRequestById(stationeryRequestId);
            return View();
        }
        public ActionResult HeadStationeryRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = headSidenavItems;
            ViewData["stationeryRequests"] = requestService.GetStationeryRequestsByDepartment(((DepartmentHead)user).Department.DepartmentId);
            return View();
        }
        public ActionResult ApproveStationeryRequest(int stationeryRequestId, string remarks)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            requestService.ApproveStationeryRequest(stationeryRequestId, remarks);
            ViewData["notificationStatuses"] = notificationService.GetNotificationStatusesFromUser(user.UserId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult RejectStationeryRequest(int stationeryRequestId, string remarks)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            requestService.RejectStationeryRequest(stationeryRequestId, remarks);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult AuthorizeStaff(string departmentStaffIdStr)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = headSidenavItems;
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["authorizeForms"] = requestService.GetAuthorizeFormsByDepartment(departmentId);
            return View();
        }
        public ActionResult AddAuthorizeStaff(string departmentStaffIdStr, string startDate, string endDate)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = headSidenavItems;
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["departmentStaffs"] = userService.GetDepartmentStaffsByDepartment(departmentId);
            if (HttpContext.Request.HttpMethod == "POST")
            {
                int departmentStaffId = Convert.ToInt32(departmentStaffIdStr);
                requestService.AddAuthorizeStaff(departmentStaffId, startDate, endDate);
                return RedirectToAction("AuthorizeStaff");
            }
            else
            {
                return View();
            }
        }
        public ActionResult CancelAuthorizeStaff(int authorizeFormId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            requestService.CancelAuthorizeStaff(user.UserId, authorizeFormId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult AssignRepresentative(string departmentStaffIdStr)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = headSidenavItems;
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["assignForms"] = requestService.GetAssignFormsByDepartment(departmentId);
            return View();
        }
        public ActionResult AddAssignRepresentative(string departmentStaffIdStr, string startDate, string endDate)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = headSidenavItems;
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["departmentStaffs"] = userService.GetDepartmentStaffsByDepartment(departmentId);
            if (HttpContext.Request.HttpMethod == "POST")
            {
                int departmentStaffId = Convert.ToInt32(departmentStaffIdStr);
                requestService.AddAssignRepresentative(departmentStaffId, startDate, endDate);
                return RedirectToAction("AssignRepresentative");
            }
            else
            {
                return View();
            }
        }
        public ActionResult CancelAssignRepresentative(int assignFormId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            requestService.CancelAssignRepresentative(user.UserId, assignFormId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult DepartmentRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = user.UserType == "departmentStaff" ? staffSidenavItems : headSidenavItems;
            ViewData["departmentRequests"] = requestService.GetDepartmentRequestsByDepartment(((DepartmentStaff)user).Department.DepartmentId);
            return View();
        }
    }
}