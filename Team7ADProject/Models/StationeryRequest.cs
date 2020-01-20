using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationeryRequest
    {
        public int StationeryRequestId { get; set; }
        public virtual ICollection<StationeryQuantity> StationeryQuantities { get; set; }
        public StationeryRequest()
        {
            this.StationeryQuantities = new List<StationeryQuantity>();
        }

    }
}

