using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Database;
using Team7ADProject.Models;
using Team7ADProject.Controllers;
using RestSharp;

namespace Team7ADProject.Service
{
    public class StationeryService
    {
        private NotificationService notificationService;
        private Team7ADProjectDbContext db;
        public StationeryService()
        {
            this.notificationService = new NotificationService();
        }
        public List<Stationery> GetStationeries()
        {
            db = new Team7ADProjectDbContext();
            return db.Stationery.ToList();
        }
        public Stationery GetStationeryById(int stationeryId)
        {
            db = new Team7ADProjectDbContext();
            return db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
        }
        public List<AdjustmentVoucher> GetAdjustmentVouchers()
        {
            db = new Team7ADProjectDbContext();
            return db.AdjustmentVoucher.ToList();
        }
        public List<AdjustmentVoucher> GetAdjustmentVouchersByClerk(int clerkId)
        {
            db = new Team7ADProjectDbContext();
            List<AdjustmentVoucher> allAdjustmentVouchers = db.AdjustmentVoucher.ToList();
            List<AdjustmentVoucher> adjustmentVouchers = new List<AdjustmentVoucher>();
            foreach(var adjustmentVoucher in allAdjustmentVouchers)
            {
                if (adjustmentVoucher.RaisedBy.UserId == clerkId) adjustmentVouchers.Add(adjustmentVoucher);
            }
            return adjustmentVouchers;
        }
        public void AddAdjustmentVoucher(int stationeryId, int quantity, string reason, int raisedById)
        {
            db = new Team7ADProjectDbContext();
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            User raisedBy = db.User.Where(x => x.UserId == raisedById).FirstOrDefault();
            db.AdjustmentVoucher.Add(new AdjustmentVoucher(stationery, quantity, reason, (StoreClerk) raisedBy));
            db.SaveChanges();
        }
        public List<Category> GetCategories()
        {
            db = new Team7ADProjectDbContext();
            return db.Category.ToList();
        }
        public List<UnitOfMeasure> GetUnitOfMeasure()
        {
            db = new Team7ADProjectDbContext();
            return db.UnitOfMeasure.ToList();
        }
        public void AddStationery(string itemNumber, int categoryId, string description,
            int unitOfMeasureId, int quantityInStock, int reorderLevel)
        {
            db = new Team7ADProjectDbContext();
            Category category = db.Category.Where(x => x.CategoryId == categoryId).FirstOrDefault();
            UnitOfMeasure unitOfMeasure = db.UnitOfMeasure.Where(x => x.UnitOfMeasureId == unitOfMeasureId).FirstOrDefault();
            Stationery stationery = new Stationery(itemNumber, category, description, unitOfMeasure, quantityInStock, reorderLevel);
            db.Stationery.Add(stationery);
            db.SaveChanges();
        }
        public void ApproveAdjustmentVoucher(int adjustmentVoucherId)
        {
            db = new Team7ADProjectDbContext();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Approved";
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == adjustmentVoucher.Stationery.StationeryId).FirstOrDefault();
            stationery.QuantityInStock += adjustmentVoucher.Quantity;
            notificationService.SendNotificationToUser(adjustmentVoucher.RaisedBy.UserId, DateService.GetTodayDate(), "Store Supervisor", "Adjustment Voucher Approved", "Adjustment voucher approved.", db);
            db.SaveChanges();
        }
        public void RejectAdjustmentVoucher(int adjustmentVoucherId)
        {
            db = new Team7ADProjectDbContext();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Rejected";
            notificationService.SendNotificationToUser(adjustmentVoucher.RaisedBy.UserId, DateService.GetTodayDate(), "Store Supervisor", "Adjusment Voucher Rejected", "Adjustment voucher rejected.", db);
            db.SaveChanges();
        }
        public void EditStockDetail(int stationeryId, string description)
        {
            db = new Team7ADProjectDbContext();
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            stationery.Description = description;
            db.SaveChanges();
        }
        public void ChangeStockLevel(int stationeryId, int diff)
        {
            db = new Team7ADProjectDbContext();
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            stationery.QuantityInStock += diff;
            db.SaveChanges();
        }
        public Dictionary<int, int> GetStationeryStockLevels()
        {
            db = new Team7ADProjectDbContext();
            Dictionary<int, int> stationeryStock = db.Stationery.ToDictionary(x => x.StationeryId, x => x.QuantityInStock);
            return stationeryStock;
        }
        public AdjustmentVoucher GetAdjustmentVoucherById(int adjustmentVoucherId)
        {
            db = new Team7ADProjectDbContext();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            return adjustmentVoucher;
        }
        public int GetDemandForMonth(int stationeryId, string dateStr)
        {
            db = new Team7ADProjectDbContext();
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            String startDate = stationery.DemandStartDate;
            int monthsElapsed = -1;
            while (DateService.IsValidStartEnd(startDate, dateStr))
            {
                startDate = DateService.AddMonth(startDate);
                monthsElapsed++;
            }
            dynamic demand = JsonConvert.DeserializeObject(stationery.MonthlyDemand);
            List<int> demandList = demand.ToObject<List<int>>();
            return demandList[monthsElapsed];
        }
        public int GetNoOfMonthsForDemand(int stationeryId)
        {
            db = new Team7ADProjectDbContext();
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            dynamic demand = JsonConvert.DeserializeObject(stationery.MonthlyDemand);
            List<int> demandList = demand.ToObject<List<int>>();
            return demandList.Count;
        }
        public int GetCategoryDemandForMonth(int categoryId, string dateStr)
        {
            db = new Team7ADProjectDbContext();
            List<Stationery> stationeries = db.Stationery.ToList();
            String startDate = stationeries[0].DemandStartDate;
            int monthsElapsed = -1;
            while (DateService.IsValidStartEnd(startDate, dateStr))
            {
                startDate = DateService.AddMonth(startDate);
                monthsElapsed++;
            }
            int totalDemand = 0;
            foreach(var stationery in stationeries)
            {
                if (stationery.Category.CategoryId != categoryId) continue;
                dynamic demand = JsonConvert.DeserializeObject(stationery.MonthlyDemand);
                List<int> demandList = demand.ToObject<List<int>>();
                totalDemand += demandList[monthsElapsed];
            }
            return totalDemand;
        }
        public List<StationeryQuantity> GetPredictedDemand(string todayDateStr, Team7ADProjectDbContext db)
        {
            if(db == null) db = new Team7ADProjectDbContext();
            string dictId = DateService.GetDictIdFromTodayDate(todayDateStr);
            var client = new RestClient("http://localhost:5000/get");
            var request = new RestRequest();
            request.AddParameter("id", dictId);
            string response = client.Get(request).Content;
            dynamic responseObj = JsonConvert.DeserializeObject(response);
            string result = responseObj.result;
            List<Stationery> stationeries = db.Stationery.ToList();
            List<StationeryQuantity> stationeryQuantities = new List<StationeryQuantity>();
            if(result == "success")
            {
                string predictions = responseObj.predictions;
                dynamic predictionsObj = JsonConvert.DeserializeObject(predictions);
                foreach(var stationery in stationeries)
                {
                    StationeryQuantity stationeryQuantity = new StationeryQuantity(stationery);
                    stationeryQuantity.QuantityForecast = predictionsObj[stationery.ItemNumber];
                    stationeryQuantities.Add(stationeryQuantity);
                }
            }
            return stationeryQuantities;
        }
        public List<StationeryQuantity> GetRecommendedQuantities(string date)
        {
            db = new Team7ADProjectDbContext();
            List<StationeryQuantity> recommendedQuantities = GetPredictedDemand(date, db);
            List<Order> orders = db.Order.ToList();
            String lastMonthEndDate = DateService.GetLastMonthEndDate(date);
            foreach(var order in orders)
            {
                if(DateService.IsEqualOrAfter(order.DateCreated, lastMonthEndDate)){
                    foreach(var orderSQ in order.StationeryQuantities)
                    {
                        foreach(var recSQ in recommendedQuantities)
                        {
                            if(recSQ.Stationery.StationeryId == orderSQ.Stationery.StationeryId)
                            {
                                recSQ.QuantityForecast -= orderSQ.QuantityOrdered;
                            }
                        }
                    }
                }
            }
            return recommendedQuantities;
        }
        public void UploadDemandData(string date)
        {
            db = new Team7ADProjectDbContext();
            String lastMonthEndDate = DateService.GetLastMonthEndDate(date);
            List<StationeryRequest> stationeryRequests = db.StationeryRequest.ToList();
            List<StationeryQuantity> demandSQs = new List<StationeryQuantity>();
            List<Stationery> stationeries = db.Stationery.ToList();
            foreach(var stationery in stationeries)
            {
                demandSQs.Add(new StationeryQuantity(stationery));
            }
            foreach(var stationeryRequest in stationeryRequests)
            {
                if(DateService.IsEqualOrAfter(stationeryRequest.Date, lastMonthEndDate))
                {
                    foreach(var srSQ in stationeryRequest.StationeryQuantities)
                    {
                        foreach(var dSQ in demandSQs)
                        {
                            if(srSQ.Stationery.StationeryId == dSQ.Stationery.StationeryId)
                            {
                                dSQ.QuantityDemanded += srSQ.QuantityRequested;
                            }
                        }
                    }
                }
            }
            Dictionary<string, float> demandDict = new Dictionary<string, float>();
            foreach(var dSQ in demandSQs)
            {
                demandDict.Add(dSQ.Stationery.ItemNumber, dSQ.QuantityDemanded);
            }
            Object upload = new { 
                id = DateService.GetDictIdForNextMonth(date),
                data = demandDict
            };
            var client = new RestClient();
            var request = new RestRequest("http://localhost:5000/put", Method.PUT);
            request.AddParameter("id", DateService.GetDictIdForNextMonth(date), ParameterType.QueryStringWithoutEncode);
            request.AddParameter("data", RestController.JSONStringify(demandDict), ParameterType.QueryStringWithoutEncode);
            client.Execute(request);
            //client.Put(request);
        }
    }
}