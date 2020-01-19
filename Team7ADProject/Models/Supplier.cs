using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public Supplier() { }
        public Supplier(string name)
        {
            this.Name = name;
        }
    }
}