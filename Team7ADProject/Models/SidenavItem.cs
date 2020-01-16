using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class SidenavItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public SidenavItem(string name, string url)
        {
            this.Name = name;
            this.Url = url;
        }
    }
}