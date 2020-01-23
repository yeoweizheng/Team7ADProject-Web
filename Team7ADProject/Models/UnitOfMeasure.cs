using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class UnitOfMeasure
    {
        public int UnitOfMeasureId { get; set; }
        public string Name { get; set; }
        public UnitOfMeasure() { }
        public UnitOfMeasure(string name)
        {
            this.Name = name;
        }
    }
}