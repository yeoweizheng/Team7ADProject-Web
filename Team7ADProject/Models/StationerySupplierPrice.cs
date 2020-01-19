using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationerySupplierPrice
    {
        public int StationerySupplierPriceId { get; set; }
        public virtual Stationery Stationery { get; set; }
        public virtual Supplier Supplier { get; set; }
        public double Price { get; set; }
        public StationerySupplierPrice() { }
        public StationerySupplierPrice(Stationery stationery, Supplier supplier, double price) 
        {
            this.Stationery = stationery;
            this.Supplier = supplier;
            this.Price = price;
        }
    }
}