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
    public class StoreController : Controller
    {
        private static List<SidenavItem> clerkSideNavItems;
        private static List<SidenavItem> supSideNavItems;
        private static Team7ADProjectDbContext db;
        private static UserService userService;
        private static StationeryService stationeryService;
        private static RequestService requestService;
        private static OrderService orderService;
        private static NotificationService notificationService;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
            stationeryService = new StationeryService();
            requestService = new RequestService();
            orderService = new OrderService();
            notificationService = new NotificationService();
            clerkSideNavItems = new List<SidenavItem>();
            clerkSideNavItems.Add(new SidenavItem("Department Requests", "/Store/DepartmentRequests"));
            clerkSideNavItems.Add(new SidenavItem("Stationery Retrieval List", "/Store/RetrievalList"));
            clerkSideNavItems.Add(new SidenavItem("Disbursement List", "/Store/DisbursementList"));
            clerkSideNavItems.Add(new SidenavItem("Stock List", "/Store/StockList"));
            clerkSideNavItems.Add(new SidenavItem("Adjustment Vouchers", "/Store/ClerkAdjustmentVouchers"));
            clerkSideNavItems.Add(new SidenavItem("Orders", "/Store/Orders"));
            clerkSideNavItems.Add(new SidenavItem("Notifications", "/Store/Notifications"));
            clerkSideNavItems.Add(new SidenavItem("Scheduled Jobs", "/Store/ScheduledJobs"));
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
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["departmentRequests"] = requestService.GetDepartmentRequests();
            return View();
        }
        [Route("Store/DepartmentRequestDetail/{DepartmentRequestId}")]
        public ActionResult DepartmentRequestDetail(int departmentRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["departmentRequest"] = requestService.GetDepartmentRequestById(departmentRequestId);
            ViewData["stationeryQuantities"] = requestService.GetStationeryQuantitiesByDepartment(departmentRequestId);
            return View();   
        }
        public ActionResult AddToRetrieval(int departmentRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            requestService.AddToRetrieval(user.UserId, departmentRequestId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult RemoveFromRetrieval(int departmentRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            requestService.RemoveFromRetrieval(user.UserId, departmentRequestId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult MarkAsRetrieved()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            requestService.MarkAsRetrieved(user.UserId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult RetrievalList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            RetrievalList retrievalList = requestService.GetRetrievalListByStoreClerk(user.UserId);
            ViewData["retrievalList"] = retrievalList;
            ViewData["stationeryQuantities"] = requestService.GetStationeryQuantitiesFromRetrieval(retrievalList.RetrievalListId);
            return View();
        }

        public ActionResult AddToDisbursement(int departmentRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            requestService.AddToDisbursement(user.UserId, departmentRequestId);
            return new HttpStatusCodeResult(200);
        }

        public ActionResult RemoveDisbursement(int departmentRequestId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            requestService.RemoveDisbursement(user.UserId, departmentRequestId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult MarkAsDisbursed()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            requestService.MarkAsDisbursed(user.UserId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult DisbursementList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            DisbursementList disbursementList = requestService.GetDisbursementListByStoreClerk(user.UserId);
            ViewData["disbursementList"] = disbursementList;
            ViewData["stationeryQuantities"] = requestService.GetStationeryQuantitiesFromDisbursement(disbursementList.DisbursementListId);
            return View();
        }
        public ActionResult StockList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
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
            ViewData["user"] = user;
            ViewData["sidenavItems"] = user.UserType == "storeClerk"? clerkSideNavItems : supSideNavItems;
            ViewData["stationery"] = stationeryService.GetStationeryById(stationeryId);
            return View();
        }
        public ActionResult ClerkAdjustmentVouchers()
        {   User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["adjustmentVouchers"] = stationeryService.GetAdjustmentVouchers();
            return View();
        }
        
        public ActionResult AddAdjustmentVoucher(string stationeryIdStr, string quantityStr, string reason)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            List<Stationery> stationeries = db.Stationery.ToList();
            ViewData["stationeries"] = stationeries;
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            if (HttpContext.Request.HttpMethod == "POST")
            {
                int stationeryId = Convert.ToInt32(stationeryIdStr);
                int quantity = Convert.ToInt32(quantityStr);
                stationeryService.AddAdjustmentVoucher(stationeryId, quantity, reason);
                return RedirectToAction("ClerkAdjustmentVouchers");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Orders()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            ViewData["orders"] = orderService.GetOrders();
            return View();
        }
        [Route("Store/OrderDetail/{orderId}")]
        public ActionResult OrderDetail (int orderId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            Order order = orderService.GetOrderById(orderId);
            ViewData["order"] = order;
            return View();
        }
        public ActionResult AddOrder()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            ViewData["stationeries"] = stationeryService.GetStationeries();
            return View();
        }
        public ActionResult Notifications()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["notificationStatuses"] = notificationService.GetNotificationStatusesFromUser(user.UserId);
            return View();
        }
       [Route("Store/NotificationDetail/{notificationStatusId}")]
        public ActionResult NotificationDetail(int notificationStatusId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");      
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            NotificationStatus notificationStatus = notificationService.GetNotificationStatusById(notificationStatusId);           
            ViewData["notification"] = notificationStatus.Notification;
            notificationService.MarkAsRead(notificationStatusId);
            return View();
        }
        
        public ActionResult SupAdjustmentVouchers()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = supSideNavItems;
            ViewData["adjustmentVouchers"] = stationeryService.GetAdjustmentVouchers();
            return View();
        }
        public ActionResult AddStationery(string itemNumber, string categoryIdStr, string description,
            string unitOfMeasureIdStr, string quantityInStockStr, string reorderLevelStr)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["user"] = user;
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["categories"] = stationeryService.GetCategories();
            ViewData["unitOfMeasures"] = stationeryService.GetUnitOfMeasure();
            if(HttpContext.Request.HttpMethod=="POST")
            {
                int categoryId = Convert.ToInt32(categoryIdStr);
                int unitOfMeasureId = Convert.ToInt32(unitOfMeasureIdStr);
                int quantityInStock = Convert.ToInt32(quantityInStockStr);
                int reorderLevel = Convert.ToInt32(reorderLevelStr);
                stationeryService.AddStationery(itemNumber, categoryId, description, unitOfMeasureId, quantityInStock, reorderLevel);
                return RedirectToAction("StockList");
            }
            else
            {
                return View();
            }
        }
        public ActionResult ApproveAdjustmentVoucher(int adjustmentVoucherId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            stationeryService.ApproveAdjustmentVoucher(adjustmentVoucherId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult RejectAdjustmentVoucher(int adjustmentVoucherId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            stationeryService.RejectAdjustmentVoucher(adjustmentVoucherId);
            return new HttpStatusCodeResult(200);
        }
        [Route("Store/EditStockDetail/{stationeryId}")]
        public ActionResult EditStockDetail(int stationeryId, string description)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            ViewData["stationery"] = stationeryService.GetStationeryById(stationeryId);
            if (HttpContext.Request.HttpMethod == "POST")
            {
                stationeryService.EditStockDetail(stationeryId, description);
                return RedirectToAction("StockList");
            }
            else
            {
                return View();
            }
        }
        public ActionResult GetSupplierPrices()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            return Content(RestController.JSONStringify(orderService.GetSupplierPrices()));
        }
        public ActionResult SubmitOrders(String allOrdersJSON)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return new HttpStatusCodeResult(403);
            orderService.AddOrders(allOrdersJSON);
            return new HttpStatusCodeResult(200);
        }
        [Route("Store/UpdateOrder/{orderId}")]
        public ActionResult UpdateOrder(int orderId, String quantitiesReceivedJSON)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return new HttpStatusCodeResult(403);
            orderService.UpdateOrder(orderId, quantitiesReceivedJSON);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult ScheduledJobs()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = clerkSideNavItems;
            return View();
        }
        public ActionResult GenerateDepartmentRequests()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return new HttpStatusCodeResult(403);
            if (user.UserType != "storeClerk") return new HttpStatusCodeResult(403);
            requestService.GenerateDepartmentRequests();
            return new HttpStatusCodeResult(200);
        }
    }
}