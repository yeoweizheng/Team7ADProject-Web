using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public string DateCreated { get; set; }
        public string Status { get; set; }
        public virtual ICollection<StationeryQuantity> StationeryQuantities { get; set; }
        public Order() { }
        public Order (Supplier supplier, string dateCreated)
        {
            this.StationeryQuantities = new List<StationeryQuantity>();
            this.Supplier = supplier;
            this.DateCreated = dateCreated;
            this.Status = "Created";
        }
    }
}