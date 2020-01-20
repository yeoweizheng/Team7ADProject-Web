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
        public StationeryRequest(string date)
        {
            this.StationeryQuantities = new List<StationeryQuantity>();
            this.Date = date;
            this.Status = "Pending";
        }

      
        

    }
}

