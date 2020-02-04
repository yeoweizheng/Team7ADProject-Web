using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Database;
using Team7ADProject.Models;

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
            notificationService.SendNotificationToUser(adjustmentVoucher.RaisedBy.UserId, DateTime.Today.ToString("dd-MMM-yy"), "Store Supervisor", "Adjustment Voucher Approved", "Adjustment voucher approved.", db);
            db.SaveChanges();
        }
        public void RejectAdjustmentVoucher(int adjustmentVoucherId)
        {
            db = new Team7ADProjectDbContext();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Rejected";
            notificationService.SendNotificationToUser(adjustmentVoucher.RaisedBy.UserId, DateTime.Today.ToString("dd-MMM-yy"), "Store Supervisor", "Adjusment Voucher Rejected", "Adjustment voucher rejected.", db);
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
    }
}