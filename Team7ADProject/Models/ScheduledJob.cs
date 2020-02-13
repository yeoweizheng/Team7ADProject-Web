using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class ScheduledJob
    {
        public int ScheduledJobId { get; set; }
        public string Name { get; set; }
        public string DateLastCalled { get; set; }
        public ScheduledJob() { }
        public ScheduledJob(string name) 
        {
            this.Name = name;
        }
    }
}