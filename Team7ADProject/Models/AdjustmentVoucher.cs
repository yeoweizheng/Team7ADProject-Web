using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class AdjustmentVoucher
    {
        public int AdjustmentVoucherId { get; set; }
        public virtual Stationery Stationery { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public virtual StoreClerk RaisedBy { get; set; }
        public AdjustmentVoucher() { }
        public AdjustmentVoucher(Stationery stationery, int quantity, string reason, StoreClerk raisedBy)
        {
            this.Stationery = stationery;
            this.Quantity = quantity;
            this.Reason = reason;
            this.Status = "Pending";
            this.RaisedBy = raisedBy;
        }
    }
}