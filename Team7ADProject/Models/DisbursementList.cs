using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DisbursementList
    {
        public int DisbursementListId { get; set; }

        public String DateTime { get; set; }

        public string DepartmentName { get; set; }
        public virtual Department Department { get; set; }

        public Boolean Status { get; set; }

        public String Location { get; set; }



        
    }
}