﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class AssignForm
    {
        public int AssignFormId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public virtual DepartmentStaff DepartmentStaff { get; set; }
        public AssignForm() { }
        public AssignForm(DepartmentStaff departmentStaff, string startDate, string endDate)
        {
            this.DepartmentStaff = departmentStaff;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
    }
}