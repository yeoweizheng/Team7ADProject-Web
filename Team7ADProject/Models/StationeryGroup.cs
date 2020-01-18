using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationeryGroup
    {
        public int StationeryGroupId { get; set; }
        public int Quantity { get; set; }
        public int StationeryId { get; set; }
        public virtual Stationery Stationery { get; set; }
        public int StationeryRequestId { get; set; }
        public virtual StationeryRequest StationeryRequest { get; set; }
        public StationeryGroup() { }
        public StationeryGroup(int quantity, Stationery stationery)
        {
            this.Quantity = quantity;
            this.Stationery = stationery;
        }
    }
}