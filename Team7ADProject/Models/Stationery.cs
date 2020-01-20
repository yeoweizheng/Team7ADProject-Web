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
        public string Category { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public Stationery() { }
        public Stationery(string itemNumber, string category, string description, string unitOfMeasure, int quantityInStock, int reorderLevel)
        {
            
            this.ItemNumber = itemNumber;
            this.Category = category;
            this.Description = description;
            this.UnitOfMeasure = unitOfMeasure;
            this.QuantityInStock = quantityInStock;
            this.ReorderLevel = reorderLevel;
        }
    }

}