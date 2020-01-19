using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StockList
    {
        public int StockListId { get; set; }

        public virtual Stationery Stationery { get; set; }
        public virtual ICollection<Stationery> Stationeries { get; set; }

    }
}