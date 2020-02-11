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
        public virtual ICollection<DepartmentStaff> DepartmentStaffs { get; set; }
        public virtual CollectionPoint CollectionPoint { get; set; }
        public Department() { }
        public Department(String name, CollectionPoint collectionPoint)
        {
            this.Name = name;
            this.CollectionPoint = collectionPoint;
        }
    }

}