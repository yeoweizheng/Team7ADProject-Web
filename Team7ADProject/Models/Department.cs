using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual DepartmentHead DepartmentHead { get; set; }
        public virtual ICollection<DepartmentStaff> DepartmentStaffs { get; set; }

        public Department() { }
        public Department(String Name, String Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
    }

}