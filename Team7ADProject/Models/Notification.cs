using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7ADProject.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Date { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public Notification() { }

        public Notification(string date, string sender, string subject, string message)
        {
            this.Date = date;
            this.Sender = sender;
            this.Subject = subject;
            this.Message = message;
        }


    }
}