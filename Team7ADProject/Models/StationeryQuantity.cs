using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationeryQuantity
    {
        public int StationeryQuantityId { get; set; }
        public int Quantity { get; set; }
        public virtual Stationery Stationery { get; set; }
        public virtual StationeryRequest StationeryRequest { get; set; }
        public StationeryQuantity() { }
        public StationeryQuantity(int quantity, Stationery stationery)
        {
            this.Quantity = quantity;
            this.Stationery = stationery;
        }
    }

}