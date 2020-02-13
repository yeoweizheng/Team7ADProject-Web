using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StoreClerk : User
    {
        public virtual ICollection<Order> Orders { get; set; }
        public virtual RetrievalList RetrievalList { get; set; }
        public virtual DisbursementList DisbursementList { get; set; }
        public StoreClerk() { }
        public StoreClerk(string name, string username, string password, string email)
            : base(name, username, password, "storeClerk", email) 
        {
            this.Orders = new List<Order>();
            this.RetrievalList = new RetrievalList();
            this.DisbursementList = new DisbursementList();
        }

    }
}
