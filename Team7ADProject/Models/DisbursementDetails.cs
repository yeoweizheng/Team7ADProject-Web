using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class DisbursementDetails
    {
        public int DisbursementDetailsId { get; set; }

        public int QuantityDisbursed { get; set; }

        public virtual Department Department { get; set; }

        public virtual DisbursementList DisbursementList { get; set; }

        public virtual ICollection<Stationery> Stationeries { get; set; }
    }

}