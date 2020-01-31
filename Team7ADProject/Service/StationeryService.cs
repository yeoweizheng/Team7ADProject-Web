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
        public StationeryService()
        {
            this.db = new Team7ADProjectDbContext();
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
        public void ApproveAdjustmentVoucher(int storeSupervisorId, int adjustmentVoucherId)
        {
            StoreSupervisor storeSupervisor = (StoreSupervisor)db.User.Where(x => x.UserId == storeSupervisorId).FirstOrDefault();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Approve";
            db.SaveChanges();
        }
        public void RejectAdjustmentVoucher(int storeSupervisorId, int adjustmentVoucherId)
        {
            StoreSupervisor storeSupervisor = (StoreSupervisor)db.User.Where(x => x.UserId == storeSupervisorId).FirstOrDefault();
            AdjustmentVoucher adjustmentVoucher = db.AdjustmentVoucher.Where(x => x.AdjustmentVoucherId == adjustmentVoucherId).FirstOrDefault();
            adjustmentVoucher.Status = "Reject";
            db.SaveChanges();
        }
        public void EditStockDetail(int stationeryId, string description)
        {
            Stationery stationery = db.Stationery.Where(x => x.StationeryId == stationeryId).FirstOrDefault();
            stationery.Description = description;
            db.SaveChanges();
        }
    }
}