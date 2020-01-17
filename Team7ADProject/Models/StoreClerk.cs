using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StoreClerk : User
    {
        public StoreClerk() { }
        public StoreClerk(string name, string username, string password)
            : base(name, username, password, "storeClerk") { }

    }
}