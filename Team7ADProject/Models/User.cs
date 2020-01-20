using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public virtual ICollection<NotificationStatus> NotificationStatuses { get; set; }
        public User() { }
        public User(string name, string username, string password, string userType)
        {
            this.Name = name;
            this.Username = username;
            this.Password = password;
            this.UserType = userType;
            this.NotificationStatuses = new List<NotificationStatus>();
        }
    }
}