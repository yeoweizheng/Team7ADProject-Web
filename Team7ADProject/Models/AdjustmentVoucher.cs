using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class AdjustmentVoucher
    {
        public int AdjustmentVoucherId { get; set; }
        //public int StationeryId { get; set; }
        public virtual Stationery Stationery { get; set; }

        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public AdjustmentVoucher() { }
        public AdjustmentVoucher(Stationery stationery, int quantity, string status)
        {
            this.Stationery = stationery;
            this.Quantity = quantity;
            this.Status = status;
        }
    }
}