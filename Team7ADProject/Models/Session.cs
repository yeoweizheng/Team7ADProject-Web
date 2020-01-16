using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class Session
    {
        public Session() { }
        public Session(User user)
        {
            this.SessionId = Guid.NewGuid().ToString();
            this.User = user;
        }
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}