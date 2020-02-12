using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class Stationery
    {
        public int StationeryId { get; set; }
        public string ItemNumber { get; set; }
        public virtual Category Category { get; set; }
        public string Description { get; set; }
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public String MonthlyDemand { get; set; }
        public String DemandStartDate { get; set; }
        public Stationery() 
        {
            this.DemandStartDate = "28-Jan-10";
        }
        public Stationery(string itemNumber, Category category, string description, UnitOfMeasure unitOfMeasure, int quantityInStock, int reorderLevel)
        {
            this.ItemNumber = itemNumber;
            this.Category = category;
            this.Description = description;
            this.UnitOfMeasure = unitOfMeasure;
            this.QuantityInStock = quantityInStock;
            this.ReorderLevel = reorderLevel;
            this.DemandStartDate = "28-Jan-10";
        }
    }

}