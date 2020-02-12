using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationeryQuantity
    {
        public int StationeryQuantityId { get; set; }
        public int QuantityRequested { get; set; }
        public int QuantityRetrieved { get; set; }
        public int QuantityDisbursed { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityForecast { get; set; }
        public double Price { get; set; }
        public double Subtotal { get; set; }
        public virtual Stationery Stationery { get; set; }
        public virtual StationeryRequest StationeryRequest { get; set; }
        public StationeryQuantity() { }
        public StationeryQuantity(Stationery stationery)
        {
            this.Stationery = stationery;
        }
    }

}