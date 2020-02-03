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
        public string Remarks { get; set;}
        public string Date { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<StationeryRequest> StationeryRequests { get; set; }
        public virtual ICollection<StationeryQuantity> StationeryQuantities { get; set; }
        public string CollectionCode { get; set; }
        public DepartmentRequest() 
        { 
            this.StationeryRequests = new List<StationeryRequest>();
            this.StationeryQuantities = new List<StationeryQuantity>();
        }
        public DepartmentRequest(Department department, string date, string remarks)
        {
            this.StationeryRequests = new List<StationeryRequest>();
            this.StationeryQuantities = new List<StationeryQuantity>();
            this.Department = department;
            this.Status = "Not Retrieved";
            this.Date = date;
            this.Remarks = remarks;
        }
    }
}