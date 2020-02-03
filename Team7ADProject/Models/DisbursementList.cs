using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DisbursementList
    {
        public int DisbursementListId { get; set; }
        public virtual ICollection<DepartmentRequest> DepartmentRequests { get; set; }
        public DisbursementList()
        {
            this.DepartmentRequests = new List<DepartmentRequest>();
        }
    }
}