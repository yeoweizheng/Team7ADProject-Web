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
        private Team7ADProjectDbContext db;
        private NotificationService notificationService;      
        public StationeryService()
        {
            this.db = new Team7ADProjectDbContext();
            this.notificationService = new NotificationService();
        }
        public List<Stationery> GetStationeries()
        {
            return db.Stationery.ToList();
        }
        public Stationery GetStationeryById(int stationeryId)
        {
            return db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
        }
        public List<AdjustmentVoucher> GetAdjustmentVouchers()
        {
            return db.AdjustmentVoucher.ToList();
        }
        public void AddAdjustmentVoucher(int stationeryId, int quantity, string reason)
        {
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            db.AdjustmentVoucher.Add(new AdjustmentVoucher(stationery, quantity, reason));
        }
        public List<Category> GetCategories()
        {
            return db.Category.ToList();
        }
        public List<UnitOfMeasure> GetUnitOfMeasure()
        {
            return db.UnitOfMeasure.ToList();
        }
        public void AddStationery(string itemNumber, int categoryId, string description,
            int unitOfMeasureId, int quantityInStock, int reorderLevel)
        {
            Category category = db.Category.Where(x => x.CategoryId == categoryId).FirstOrDefault();
            UnitOfMeasure unitOfMeasure = db.UnitOfMeasure.Where(x => x.UnitOfMeasureId == unitOfMeasureId).FirstOrDefault();
            Stationery stationery = new Stationery(itemNumber, category, description, unitOfMeasure, quantityInStock, reorderLevel);
            db.Stationery.Add(stationery);
            db.SaveChanges();
        }
        public void ApproveAdjustmentVoucher(int adjustmentVoucherId)
        {
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Approved";
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == adjustmentVoucher.Stationery.StationeryId).FirstOrDefault();
            stationery.QuantityInStock += adjustmentVoucher.Quantity;
            //notificationService.SendNotificationToUser(4, DateTime.Today.ToString("dd-MMM-yy"), "Store Supervisor", "Adjustment Voucher Approved", "The Adjustment Voucher you submitted has been Approved");
            db.SaveChanges();
        }
        public void RejectAdjustmentVoucher(int adjustmentVoucherId)
        {
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Rejected";
            //notificationService.SendNotificationToUser(4, DateTime.Today.ToString("dd-MMM-yy"), "Store Supervisor", "Adjusment Voucher Rejected", "The Adjustment Vocuher you submitted has been Rejected. Kindly review it again.");
            db.SaveChanges();
        }
    }
}