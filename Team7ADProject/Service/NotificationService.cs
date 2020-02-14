using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Models;
using Team7ADProject.Database;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace Team7ADProject.Service
{
    public class NotificationService
    {
        private Team7ADProjectDbContext db;
        public NotificationService() { }
        public Notification GetNotificationById(int notificationId)
        {
            db = new Team7ADProjectDbContext();
            return db.Notification.Where(x => x.NotificationId == notificationId).FirstOrDefault();
        }
        public NotificationStatus GetNotificationStatusById(int notificationStatusId)
        {
            db = new Team7ADProjectDbContext();
            return db.NotificationStatus.Where(x => x.NotificationStatusId == notificationStatusId).FirstOrDefault();
        }
        public void SendNotificationToUser(int userId, string date, string sender, string subject, string message, Team7ADProjectDbContext db)
        {
            if(db == null) db = new Team7ADProjectDbContext();
            Notification notification = new Notification(date, sender, subject, message);
            NotificationStatus notificationStatus = new NotificationStatus(notification);
            User user = db.User.Where(x => x.UserId == userId).FirstOrDefault();
            user.NotificationStatuses.Add(notificationStatus);
            Task.Run(() => SendEmail(userId, notification));
            db.SaveChanges();
        }
        public void MarkAsRead(int notificationStatusId)
        {
            db = new Team7ADProjectDbContext();
            NotificationStatus notificationStatus = db.NotificationStatus.Where(x => x.NotificationStatusId == notificationStatusId).FirstOrDefault();
            notificationStatus.Read = true;
            db.SaveChanges();
        }
        public List<NotificationStatus> GetNotificationStatusesFromUser(int userId)
        {
            db = new Team7ADProjectDbContext();
            User user = db.User.Where(x => x.UserId == userId).FirstOrDefault();
            return (List<NotificationStatus>) user.NotificationStatuses;
        }
        public void SendEmail(int userId, Notification notification)
        {
            db = new Team7ADProjectDbContext();
            User user = db.User.Where(x => x.UserId == userId).FirstOrDefault();
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                       "***REMOVED***");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "***REMOVED***", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Stationery Store Inventory System <admin@example.org>");
            request.AddParameter("to", user.Name + " <" + user.Email + ">");
            request.AddParameter("subject", notification.Subject);
            request.AddParameter("text", notification.Message);
            request.Method = Method.POST;
            client.Execute(request);
        }
      
    }
}