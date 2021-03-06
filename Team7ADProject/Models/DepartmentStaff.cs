﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DepartmentStaff : User
    {
        public ICollection<StationeryRequest> StationeryRequests { get; set; }
        public virtual DepartmentRequest DepartmentRequest { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<AuthorizeForm> AuthorizeForms { get; set; }
        public bool Representative { get; set; }
        public DepartmentStaff() 
        {
            this.StationeryRequests = new List<StationeryRequest>();
            this.AuthorizeForms = new List<AuthorizeForm>();
        }
        public DepartmentStaff(string name, string username, string password, Department department, String email)
            : base(name, username, password, "departmentStaff", email)
        {
            this.StationeryRequests = new List<StationeryRequest>();
            this.Department = department;
            this.AuthorizeForms = new List<AuthorizeForm>();
        }
    }
}