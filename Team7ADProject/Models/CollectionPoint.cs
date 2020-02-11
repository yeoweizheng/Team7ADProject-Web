using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class CollectionPoint
    {
        public int CollectionPointId { get; set; }
        public string Name { get; set; }
        public CollectionPoint() { }
        public CollectionPoint(string name)
        {
            this.Name = name;
        }
    }
}