using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class StoreSupervisor : User
    {
        public StoreSupervisor() { }
        public StoreSupervisor(string name, string username, string password, string email)
            : base(name, username, password, "storeSupervisor", email) { }
    }
}