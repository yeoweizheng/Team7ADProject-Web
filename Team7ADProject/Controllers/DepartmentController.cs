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
        }
        public List<SidenavItem> GetSidenavItems(int userId)
        {
            List<SidenavItem> sidenavItems = new List<SidenavItem>();
            User user = userService.GetUserById(userId);
            if(user.UserType == "departmentHead")
            {
                DepartmentHead departmentHead = (DepartmentHead)user;
                if (!userService.IsAuthorityDelegated(departmentHead.Department.DepartmentId))
                {
                    sidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/HeadStationeryRequests"));
                }
                sidenavItems.Add(new SidenavItem("Authorize Staff", "/Department/AuthorizeStaff"));
                sidenavItems.Add(new SidenavItem("Assign Representative", "/Department/AssignRepresentative"));
                sidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
                return sidenavItems;
            } else
            {
                DepartmentStaff staff = (DepartmentStaff)user;
                sidenavItems.Add(new SidenavItem("Stationery Requests", "/Department/StaffStationeryRequests"));
                if (userService.IsStaffAuthorized(userId))
                {
                    sidenavItems.Add(new SidenavItem("Approve Stationery Requests", "/Department/HeadStationeryRequests"));
                }
                if (staff.Representative)
                {
                    sidenavItems.Add(new SidenavItem("Department Requests", "/Department/DepartmentRequests"));
                }
                sidenavItems.Add(new SidenavItem("Notifications", "/Department/Notifications"));
                return sidenavItems;
            }
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
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            ViewData["stationeryRequests"] = requestService.GetStationeryRequestsByStaffId(user.UserId);
            return View();
        }
        public ActionResult AddStationeryRequest(string stationeryQuantitiesJSON)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
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
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
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
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
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
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            ViewData["stationeryRequest"] = requestService.GetStationeryRequestById(stationeryRequestId);
            return View();
        }
        public ActionResult HeadStationeryRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead" && user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            if(user.UserType == "departmentHead")
            {
                ViewData["stationeryRequests"] = requestService.GetStationeryRequestsByDepartment(((DepartmentHead)user).Department.DepartmentId);
            } else
            {
                ViewData["stationeryRequests"] = requestService.GetStationeryRequestsByDepartment(((DepartmentStaff)user).Department.DepartmentId);
            }
            return View();
        }
        public ActionResult ApproveStationeryRequest(int stationeryRequestId, string remarks)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "departmentHead" && user.UserType != "departmentStaff") return new HttpStatusCodeResult(403);
            requestService.ApproveStationeryRequest(stationeryRequestId, remarks);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult RejectStationeryRequest(int stationeryRequestId, string remarks)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "departmentHead" && user.UserType != "departmentStaff") return new HttpStatusCodeResult(403);
            requestService.RejectStationeryRequest(stationeryRequestId, remarks);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult AuthorizeStaff(string departmentStaffIdStr)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["authorizeForms"] = userService.GetAuthorizeFormsByDepartment(departmentId);
            return View();
        }
        public ActionResult AddAuthorizeStaff(string departmentStaffIdStr, string startDate, string endDate)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["departmentStaffs"] = userService.GetDepartmentStaffsByDepartment(departmentId);
            ViewData["todayDate"] = DateService.GetTodayDate();
            return View();
        }
        public ActionResult SubmitAuthorizeStaff(int departmentStaffId, string startDate, string endDate)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "departmentHead") new HttpStatusCodeResult(403);
            if(!DateService.IsValidStartEnd(startDate, endDate))
            {
                return Content(RestController.JSONStringify(new { result = "invalidStartEnd" }));
            }
            bool success = userService.AddAuthorizeStaff(departmentStaffId, startDate, endDate);
            if (success)
            {
                return Content(RestController.JSONStringify(new { result = "success" }));
            } else
            {
                return Content(RestController.JSONStringify(new { result = "failed" }));
            }
        }
        public ActionResult CancelAuthorizeStaff(int authorizeFormId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "departmentHead") return new HttpStatusCodeResult(403);
            userService.CancelAuthorizeStaff(authorizeFormId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult AssignRepresentative(string departmentStaffIdStr)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentHead") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            int departmentId = ((DepartmentHead)user).Department.DepartmentId;
            ViewData["currentRep"] = userService.GetAssignedRepresentative(departmentId);
            ViewData["staffList"] = userService.GetDepartmentStaffsByDepartment(departmentId);
            ViewData["departmentId"] = departmentId;
            return View();
        }
        public ActionResult AssignNewRepresentative(int departmentId, int staffId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "departmentHead") return new HttpStatusCodeResult(403);
            userService.AssignNewRepresentative(departmentId, staffId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult DepartmentRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            ViewData["departmentRequests"] = requestService.GetDepartmentRequestsByDepartment(((DepartmentStaff)user).Department.DepartmentId);
            return View();
        }
        [Route("Department/DepartmentRequestDetail/{DepartmentRequestId}")]
        public ActionResult DepartmentRequestDetail(int departmentRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "departmentStaff") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = GetSidenavItems(user.UserId);
            ViewData["departmentRequest"] = requestService.GetDepartmentRequestById(departmentRequestId);
            ViewData["stationeryQuantities"] = requestService.GetStationeryQuantitiesByDepartmentRequest(departmentRequestId);
            return View();   
        }
        public ActionResult UpdateDepartmentRequest(int departmentRequestId, string action)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "departmentStaff") return new HttpStatusCodeResult(403);
            if(action == "accept")
            {
                requestService.AcceptDepartmentRequest(departmentRequestId);
            } else
            {
                requestService.RejectDepartmentRequest(departmentRequestId);
            }
            return new HttpStatusCodeResult(200);
        }
    }
}