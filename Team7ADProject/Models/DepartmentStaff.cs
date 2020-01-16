using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentStaff : User
    {
        public DepartmentStaff() { }
        public DepartmentStaff(string name, string username, string password)
            : base(name, username, password, "departmentStaff") { }
    }
}