using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class RetrievalList
    {
        public int RetrievalListId { get; set; }
        public virtual ICollection<DepartmentRequest> DepartmentRequests { get; set; }
        public RetrievalList()
        {
            this.DepartmentRequests = new List<DepartmentRequest>();
        }
    }
}