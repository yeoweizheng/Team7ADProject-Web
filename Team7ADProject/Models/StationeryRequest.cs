using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationeryRequest
    {
        public int StationeryRequestId { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<StationeryQuantity> StationeryQuantities { get; set; }
        public StationeryRequest() { }
        public StationeryRequest(string date,string remarks)
        {
            this.StationeryQuantities = new List<StationeryQuantity>();
            this.Date = date;
            this.Remarks = remarks;
            this.Status = "Pending";
        }
    }
}

