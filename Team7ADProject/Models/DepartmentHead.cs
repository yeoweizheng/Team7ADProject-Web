using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentHead : User
    {
        public DepartmentHead() { }
        public DepartmentHead(string name, string username, string password, Department department, string email)
            : base(name, username, password, "departmentHead", email)
        {
            this.Department = department;
        }
        public virtual AuthorizeForm AuthorizeForm { get; set; }
        public virtual Department Department { get; set; }
    }

}