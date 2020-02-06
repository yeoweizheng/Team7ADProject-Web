using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7ADProject.Service;
using Team7ADProject.Models;

namespace Team7ADProject.Controllers
{
    public class RestController : Controller
    {
        private static UserService userService;
        private static RequestService requestService;
        private static StationeryService stationeryService;
        private static NotificationService notificationService;
        public static void Init()
        {
            userService = new UserService();
            requestService = new RequestService();
            stationeryService = new StationeryService();
            notificationService = new NotificationService();
        }
        public ActionResult TestConnection(string requestBody)
        {
            System.Diagnostics.Debug.WriteLine(requestBody);
            return Json(new { result = "success" });
        }
        public ActionResult LoginWithSession(string requestBody)
        {
            dynamic sessionDetails = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(sessionDetails.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            Object response = new { user = user };
            return Content(JSONStringify(response));
        }
        public ActionResult Login(string requestBody)
        {
            dynamic loginDetails = JsonConvert.DeserializeObject(requestBody);
            User user = userService.Login(loginDetails.username.ToString(), loginDetails.password.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            String sessionId = userService.CreateSession(user.UserId);
            Object response = new { user = user, sessionId = sessionId };
            return Content(JSONStringify(response));
        }
        public ActionResult Logout(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            userService.LogoutWithSessionId(request.sessionId.ToString());
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult StaffStationeryRequests(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            List<StationeryRequest> stationeryRequests = requestService.GetStationeryRequestsByStaffId(user.UserId);
            List<Object> response = new List<Object>();
            foreach(var stationeryRequest in stationeryRequests)
            {
                response.Add(new
                {
                    id = stationeryRequest.StationeryRequestId,
                    date = stationeryRequest.Date,
                    status = stationeryRequest.Status,
                });
            }
            return Content(JSONStringify(response));
        }
        public ActionResult StaffDepartmentRequests(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            DepartmentStaff departmentStaff = (DepartmentStaff)user;
            List<DepartmentRequest> departmentRequests = requestService.GetDepartmentRequestsByDepartment(departmentStaff.Department.DepartmentId);
            List<Object> response = new List<Object>();
            foreach(var departmentRequest in departmentRequests)
            {
                response.Add(new
                {
                    id = departmentRequest.DepartmentRequestId,
                    date = departmentRequest.Date,
                    status = departmentRequest.Status,
                });
            }
            return Content(JSONStringify(response));
        }
        public ActionResult StoreDepartmentRequests(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            List<DepartmentRequest> departmentRequests = requestService.GetDepartmentRequests();
            List<Object> response = new List<Object>();
            foreach(var departmentRequest in departmentRequests)
            {
                response.Add(new
                {
                    id = departmentRequest.DepartmentRequestId,
                    department = departmentRequest.Department.Name,
                    date = departmentRequest.Date,
                    status = departmentRequest.Status,
                });
            }
            return Content(JSONStringify(response));
        }
        public ActionResult Notifications(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            List<NotificationStatus> notificationStatuses = notificationService.GetNotificationStatusesFromUser(user.UserId);
            List<Object> response = new List<Object>();
            foreach(var notificationStatus in notificationStatuses)
            {
                response.Add(new
                {
                    date = notificationStatus.Notification.Date,
                    subject = notificationStatus.Notification.Subject
                }) ;
            }
            return Content(JSONStringify(response));
        }
        public ActionResult StationeryRequestDetail(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            StationeryRequest stationeryRequest = requestService.GetStationeryRequestById((int)request.stationeryRequestId);
            List<Object> stationeryQuantities = new List<Object>();
            foreach(var stationeryQuantity in stationeryRequest.StationeryQuantities)
            {
                stationeryQuantities.Add(new
                {
                    description = stationeryQuantity.Stationery.Description,
                    quantityRequested = stationeryQuantity.QuantityRequested,
                    unitOfMeasure = stationeryQuantity.Stationery.UnitOfMeasure.Name
                });
            }
            Object response = new
            {
                id = stationeryRequest.StationeryRequestId,
                date = stationeryRequest.Date,
                remarks = stationeryRequest.Remarks,
                status = stationeryRequest.Status,
                stationeryQuantities = stationeryQuantities
            };
            return Content(JSONStringify(response));
        }
        public ActionResult GetStationeries(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            List<Stationery> stationeries = stationeryService.GetStationeries();
            List<Object> response = new List<Object>();
            foreach(var stationery in stationeries)
            {
                response.Add(new
                {
                    id = stationery.StationeryId,
                    itemNo = stationery.ItemNumber,
                    category = stationery.Category.Name,
                    description = stationery.Description,
                    unitOfMeasure = stationery.UnitOfMeasure.Name,
                });
            }
            return Content(JSONStringify(response));
        }
        public ActionResult AddStationeryRequest(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            String stationeryQuantitiesJSON = JSONStringify(request.stationeryRequests);
            requestService.AddStationeryRequest(user.UserId, stationeryQuantitiesJSON);
            return Json(new { result = "success" });
        }
        public ActionResult GenerateDepartmentRequests(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "failed" }));
            requestService.GenerateDepartmentRequests();
            return Json(new { result = "success" });
        }
        [NonAction]
        public static String JSONStringify(Object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            //System.Diagnostics.Debug.WriteLine(json);
            return json;
        }
    }
}