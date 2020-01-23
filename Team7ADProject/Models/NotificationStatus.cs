using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class NotificationStatus
    {
        [Key]
        public int NotificationStatusId { get; set; }

        //When User opens the Notification, bool Read = true.
        public bool Read { get; set; }
        public virtual Notification Notification { get; set; }

        public NotificationStatus() { }

        public NotificationStatus(Notification notification)
        {
            this.Notification = notification;
            this.Read = false;
        }

    }
}