using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentHead : User
    {
        public DepartmentHead() { }
        public DepartmentHead(string name, string username, string password)
            : base(name, username, password, "departmentHead") { }

        public string Email { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }

        public Boolean Authorize { get; set; }
        public Boolean Representative { get; set; }





    }

}