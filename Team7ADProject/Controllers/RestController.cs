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
        public static void Init()
        {
            userService = new UserService();
            requestService = new RequestService();
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
            dynamic response = new { user = user };
            if (user != null) return Content(JSONStringify(response));
            return Json(new { result = "failed" });
        }
        public ActionResult Login(string requestBody)
        {
            dynamic loginDetails = JsonConvert.DeserializeObject(requestBody);
            User user = userService.Login(loginDetails.username.ToString(), loginDetails.password.ToString());
            String sessionId = userService.CreateSession(user);
            dynamic response = new { user = user, sessionId = sessionId };
            if (user != null) return Content(JSONStringify(response));
            return Json(new { result = "failed" });
        }
        public ActionResult StaffStationeryRequests(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Json(new { result = "failed" });
            List<StationeryRequest> stationeryRequests = requestService.GetStationeryRequestsByStaffId(user.UserId);
            List<Object> response = new List<Object>();
            foreach(var stationeryRequest in stationeryRequests)
            {
                response.Add(new
                {
                    id = stationeryRequest.StationeryRequestId,
                    date = stationeryRequest.Date,
                    status = stationeryRequest.Status,
                    remarks = stationeryRequest.Remarks
                });
            }
            return Content(JSONStringify(response));
        }
        [NonAction]
        private String JSONStringify(Object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            System.Diagnostics.Debug.WriteLine("\n" + json);
            return json;
        }
    }
}