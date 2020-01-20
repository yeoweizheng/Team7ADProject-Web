using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models { 
    public class StationeryRequestStockList
    {
        public int StationeryRequestId { get; set; }
        public virtual StationeryRequest StationeryRequest { get; set; }


        public int StockListId { get; set; }
        public virtual StockList StockList { get; set; }


    }

}