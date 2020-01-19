using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentRequest
    {
        public int DepartmentRequestId { get; set; }
        public String DepartmentRequestDescription { get; set; }

        public double ReOrderLevel { get; set; }
        public double ReOrderQuantity { get; set; }

        public virtual Department Department { get; set; }

        //public virtual Supplier Supplier { get; set; }


        public virtual ICollection<StationeryRequest> StationeryRequests { get; set; }
        public DepartmentRequest()
        {
            this.StationeryRequests = new List<StationeryRequest>();
        }


    }

}