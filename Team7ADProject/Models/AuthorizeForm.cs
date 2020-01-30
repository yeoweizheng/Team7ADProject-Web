using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class AuthorizeForm
    {
        public int AuthorizeFormId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public virtual DepartmentStaff DepartmentStaff { get; set; }
        public AuthorizeForm() { }
        public AuthorizeForm(DepartmentStaff departmentStaff, string startDate, string endDate)
        {
            this.DepartmentStaff = departmentStaff;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}