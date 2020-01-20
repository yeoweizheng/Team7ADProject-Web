using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentRequest
    {
        public int DepartmentRequestId { get; set; }
        public string Status { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<StationeryRequest> StationeryRequests { get; set; }
        public DepartmentRequest(Department department)
        {
            this.StationeryRequests = new List<StationeryRequest>();
            this.Department = department;
            this.Status = "Not Retrieved";
        }
    }
}