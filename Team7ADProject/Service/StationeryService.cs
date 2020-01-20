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
    }
}