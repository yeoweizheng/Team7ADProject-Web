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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            Object response = new { user = user };
            return Content(JSONStringify(response));
        }
        public ActionResult Login(string requestBody)
        {
            dynamic loginDetails = JsonConvert.DeserializeObject(requestBody);
            User user = userService.Login(loginDetails.username.ToString(), loginDetails.password.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
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
        public ActionResult StockDetail(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int stationeryId = request.stationeryId;
            Stationery stationery = stationeryService.GetStationeryById(stationeryId);
            Object response = new
            {
                stationeryId = stationery.StationeryId,
                itemNumber = stationery.ItemNumber,
                category = stationery.Category.Name,
                description = stationery.Description,
                unitOfMeasure = stationery.UnitOfMeasure.Name,
                quantityInStock = stationery.QuantityInStock
            };
            return Content(JSONStringify(response));
        }
        public ActionResult AddStationeryRequest(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            String stationeryQuantitiesJSON = JSONStringify(request.stationeryRequests);
            requestService.AddStationeryRequest(user.UserId, stationeryQuantitiesJSON);
            return Json(new { result = "success" });
        }
        public ActionResult GenerateDepartmentRequests(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            requestService.GenerateDepartmentRequests();
            return Json(new { result = "success" });
        }
        public ActionResult DepartmentRequestDetail(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            DepartmentRequest departmentRequest = requestService.GetDepartmentRequestById((int)request.departmentRequestId);
            List<Object> stationeryQuantities = new List<Object>();
            foreach(var stationeryQuantity in departmentRequest.StationeryQuantities)
            {
                stationeryQuantities.Add(new
                {
                    stationeryId = stationeryQuantity.Stationery.StationeryId,
                    description = stationeryQuantity.Stationery.Description,
                    quantityRequested = stationeryQuantity.QuantityRequested,
                    quantityRetrieved = stationeryQuantity.QuantityRetrieved,
                    quantityDisbursed = stationeryQuantity.QuantityDisbursed,
                });
            }
            Object response = new
            {
                id = departmentRequest.DepartmentRequestId,
                date = departmentRequest.Date,
                remarks = departmentRequest.Remarks,
                status = departmentRequest.Status,
                stationeryQuantities = stationeryQuantities
            };
            return Content(JSONStringify(response));
        }
        public ActionResult AcceptDepartmentRequest(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            requestService.AcceptDepartmentRequest(departmentRequestId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult RejectDepartmentRequest(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            requestService.RejectDepartmentRequest(departmentRequestId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult AddToRetrieval(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            requestService.AddToRetrieval(user.UserId, departmentRequestId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult RemoveFromRetrieval(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            requestService.RemoveFromRetrieval(user.UserId, departmentRequestId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult AddToDisbursement(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            requestService.AddToDisbursement(user.UserId, departmentRequestId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult RemoveFromDisbursement(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            requestService.RemoveDisbursement(user.UserId, departmentRequestId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult StationeryRetrievalList(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>) requestService.GetRetrievalListByStoreClerk(user.UserId).DepartmentRequests;
            List<Object> departmentRequestsObj = new List<Object>();
            foreach(var departmentRequest in departmentRequests)
            {
                departmentRequestsObj.Add(new
                {
                    id = departmentRequest.DepartmentRequestId,
                    date = departmentRequest.Date,
                    department = departmentRequest.Department.Name
                });
            }
            Object response = new
            {
                departmentRequests = departmentRequestsObj
            };
            return Content(JSONStringify(response));
        }
        public ActionResult StationeryRetrievalStationeryQuantities(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            StoreClerk storeClerk = (StoreClerk)user;
            List<StationeryQuantity> stationeryQuantities = (List<StationeryQuantity>)requestService.GetStationeryQuantitiesFromRetrieval(storeClerk.RetrievalList.RetrievalListId);
            List<Object> stationeryQuantitesObj = new List<Object>();
            foreach(var stationeryQuantity in stationeryQuantities)
            {
                stationeryQuantitesObj.Add(new
                {
                    id = stationeryQuantity.Stationery.StationeryId,
                    itemNumber = stationeryQuantity.Stationery.ItemNumber,
                    description = stationeryQuantity.Stationery.Description,
                    quantityRequested = stationeryQuantity.QuantityRequested
                });
            }
            Object response = new
            {
                stationeryQuantities = stationeryQuantitesObj
            };
            return Content(JSONStringify(response));
        }
        public ActionResult UpdateRetrieval(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            String stationeryQuantitiesJSON = JSONStringify(request.stationeryQuantities);
            bool success = requestService.UpdateRetrieval(user.UserId, stationeryQuantitiesJSON);
            if (success)
            {
                return Json(new { result = "success" });
            } else
            {
                return Json(new { result = "failed" });
            }
        }
        public ActionResult StockLevel(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            List<Stationery> stationeries = stationeryService.GetStationeries();
            List<Object> stationeryQuantitiesObj = new List<Object>();
            foreach(var stationery in stationeries)
            {
                stationeryQuantitiesObj.Add(new
                {
                    stationeryId = stationery.StationeryId,
                    quantity = stationery.QuantityInStock
                });
            }
            return Content(JSONStringify(new { stationeryQuantities = stationeryQuantitiesObj }));
        }
        public ActionResult DisbursementList(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            List<DepartmentRequest> departmentRequests = (List<DepartmentRequest>) requestService.GetDisbursementListByStoreClerk(user.UserId).DepartmentRequests;
            List<Object> departmentRequestsObj = new List<Object>();
            foreach(var departmentRequest in departmentRequests)
            {
                departmentRequestsObj.Add(new
                {
                    id = departmentRequest.DepartmentRequestId,
                    date = departmentRequest.Date,
                    department = departmentRequest.Department.Name
                });
            }
            Object response = new
            {
                departmentRequests = departmentRequestsObj
            };
            return Content(JSONStringify(response));
        }
        public ActionResult DisbursementStationeryQuantities(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            StoreClerk storeClerk = (StoreClerk)user;
            List<StationeryQuantity> stationeryQuantities = (List<StationeryQuantity>)requestService.GetStationeryQuantitiesFromDisbursement(storeClerk.DisbursementList.DisbursementListId);
            List<Object> stationeryQuantitesObj = new List<Object>();
            foreach(var stationeryQuantity in stationeryQuantities)
            {
                stationeryQuantitesObj.Add(new
                {
                    id = stationeryQuantity.Stationery.StationeryId,
                    itemNumber = stationeryQuantity.Stationery.ItemNumber,
                    description = stationeryQuantity.Stationery.Description,
                    quantityRequested = stationeryQuantity.QuantityRequested,
                    quantityRetrieved = stationeryQuantity.QuantityRetrieved
                });
            }
            Object response = new
            {
                stationeryQuantities = stationeryQuantitesObj
            };
            return Content(JSONStringify(response));
        }
        public ActionResult UpdateDisbursement(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int departmentRequestId = request.departmentRequestId;
            String stationeryQuantitiesJSON = JSONStringify(request.stationeryQuantities);
            requestService.UpdateDisbursement(departmentRequestId, stationeryQuantitiesJSON);
            return Json(new { result = "success" });
        }
        public ActionResult AdjustmentVouchers(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            List<AdjustmentVoucher> adjustmentVouchers = stationeryService.GetAdjustmentVouchers();
            List<Object> adjustmentVouchersObj = new List<Object>();
            foreach(var adjustmentVoucher in adjustmentVouchers)
            {
                adjustmentVouchersObj.Add(new
                {
                    id = adjustmentVoucher.AdjustmentVoucherId,
                    item = adjustmentVoucher.Stationery.Description,
                    quantity = adjustmentVoucher.Quantity,
                    status = adjustmentVoucher.Status
                });
            }
            return Content(JSONStringify(adjustmentVouchersObj));
        }
        public ActionResult RaiseAdjustmentVoucher(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int stationeryId = request.stationeryId;
            int quantity = request.quantity;
            string reason = request.reason;
            stationeryService.AddAdjustmentVoucher(stationeryId, quantity, reason, user.UserId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult AdjustmentVoucherDetail(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int adjustmentVoucherId = request.adjustmentVoucherId;
            AdjustmentVoucher adjustmentVoucher = stationeryService.GetAdjustmentVoucherById(adjustmentVoucherId);
            Object response = new
            {
                adjustmentVoucherId = adjustmentVoucher.AdjustmentVoucherId,
                itemNumber = adjustmentVoucher.Stationery.ItemNumber,
                item = adjustmentVoucher.Stationery.Description,
                quantity = adjustmentVoucher.Quantity,
                reason = adjustmentVoucher.Reason,
                status = adjustmentVoucher.Status
            };
            return Content(JSONStringify(response));
        }
        public ActionResult ApproveAdjustmentVoucher(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int adjustmentVoucherId = request.adjustmentVoucherId;
            stationeryService.ApproveAdjustmentVoucher(adjustmentVoucherId);
            return Content(JSONStringify(new { result = "success" }));
        }
        public ActionResult RejectAdjustmentVoucher(string requestBody)
        {
            dynamic request = JsonConvert.DeserializeObject(requestBody);
            User user = userService.GetUserFromSession(request.sessionId.ToString());
            if (user == null) return Content(JSONStringify(new { result = "forbidden" }));
            int adjustmentVoucherId = request.adjustmentVoucherId;
            stationeryService.RejectAdjustmentVoucher(adjustmentVoucherId);
            return Content(JSONStringify(new { result = "success" }));
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