﻿using System;
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
        private static OrderService orderService;
        public static void Init()
        {
            db = new Team7ADProjectDbContext();
            userService = new UserService();
            stationeryService = new StationeryService();
            requestService = new RequestService();
            orderService = new OrderService();
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
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
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
            ViewData["sidenavItems"] = clerkSideNavItems;
            ViewData["departmentRequest"] = requestService.GetDepartmentRequestById(departmentRequestId);
            List<StationeryRequest> stationeryRequests = db.StationeryRequest.ToList();
            ViewData["stationeryRequest"] = stationeryRequests;
            List<StationeryQuantity> stationeryQuantities = db.StationeryQuantity.ToList();
            ViewData["stationeryQuantities"] = stationeryQuantities;
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
        public ActionResult DisbursementList()
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
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
            ViewData["sidenavItems"] = user.UserType == "storeClerk"? clerkSideNavItems : supSideNavItems;
            ViewData["stationery"] = stationeryService.GetStationeryById(stationeryId);
            return View();
        }

        public ActionResult ClerkAdjustmentVouchers()
        {   User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk") return RedirectToAction("Index", "Home");
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
            ViewData["stationeryQuantities"] = order.StationeryQuantities;
            return View();
        }
        public ActionResult AddOrder()
        {
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
            if (user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
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
            requestService.ApproveAdjustmentVoucher(user.UserId, adjustmentVoucherId);
            return new HttpStatusCodeResult(200);
        }
        public ActionResult RejectAdjustmentVoucher(int adjustmentVoucherId)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            requestService.RejectAdjustmentVoucher(user.UserId, adjustmentVoucherId);
            return new HttpStatusCodeResult(200);
        }
        [Route("Store/EditStockDetail/{stationeryId}")]
        public ActionResult EditStockDetail(int stationeryId, string description, string quantityInStockStr, string reorderLevelStr)
        {
            User user = userService.GetUserFromCookie(Request.Cookies["Team7ADProject"]);
            if (user == null) return RedirectToAction("Index", "Home");
            if (user.UserType != "storeClerk" && user.UserType != "storeSupervisor") return RedirectToAction("Index", "Home");
            ViewData["sidenavItems"] = user.UserType == "storeClerk" ? clerkSideNavItems : supSideNavItems;
            ViewData["stationery"] = stationeryService.GetStationeryById(stationeryId);
            
            if (HttpContext.Request.HttpMethod == "POST")
            {
                int quantityInStock = Convert.ToInt32(quantityInStockStr);
                int reorderLevel = Convert.ToInt32(reorderLevelStr);
                requestService.EditStockDetail(user.UserId, stationeryId, description, quantityInStock, reorderLevel);
                return RedirectToAction("StockList");
            }
            else
            {
                return View();
            }
        }
    }
}