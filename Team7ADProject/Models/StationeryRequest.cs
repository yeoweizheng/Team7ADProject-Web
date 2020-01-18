using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StationeryRequest
    {
        public int StationeryRequestId { get; set; }
        public virtual ICollection<StationeryGroup> StationeryGroups { get; set; }
        public int DepartmentStaffId { get; set; }
        public virtual DepartmentStaff DepartmentStaff { get; set; }
        public StationeryRequest() 
        {
            this.StationeryGroups = new List<StationeryGroup>();
        }
    }
}