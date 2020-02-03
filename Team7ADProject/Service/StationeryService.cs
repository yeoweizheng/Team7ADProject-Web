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
        public void AddAdjustmentVoucher(int stationeryId, int quantity, string reason)
        {
            db = new Team7ADProjectDbContext();
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            db.AdjustmentVoucher.Add(new AdjustmentVoucher(stationery, quantity, reason));
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
            //notificationService.SendNotificationToUser(4, DateTime.Today.ToString("dd-MMM-yy"), "Store Supervisor", "Adjustment Voucher Approved", "The Adjustment Voucher you submitted has been Approved");
            db.SaveChanges();
        }
        public void RejectAdjustmentVoucher(int adjustmentVoucherId)
        {
            db = new Team7ADProjectDbContext();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Rejected";
            //notificationService.SendNotificationToUser(4, DateTime.Today.ToString("dd-MMM-yy"), "Store Supervisor", "Adjusment Voucher Rejected", "The Adjustment Vocuher you submitted has been Rejected. Kindly review it again.");
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
    }
}