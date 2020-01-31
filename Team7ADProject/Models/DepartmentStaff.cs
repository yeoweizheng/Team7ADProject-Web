using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentStaff : User
    {
        public ICollection<StationeryRequest> StationeryRequests { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<AuthorizeForm> AuthorizeForms { get; set; }
        public virtual ICollection<AssignForm> AssignForms { get; set; }
        public DepartmentStaff() { }
        public DepartmentStaff(string name, string username, string password, Department department)
            : base(name, username, password, "departmentStaff") 
        {
            this.StationeryRequests = new List<StationeryRequest>();
            this.Department = department;
            this.AuthorizeForms = new List<AuthorizeForm>();
            this.AssignForms = new List<AssignForm>();
        }
    }
}