using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team7ADProject.Database;
using Team7ADProject.Models;

namespace Team7ADProject
{
    public class NotificationService
    {
        //NOTE: MUST have notificationType!
        public String NotificationSubject(String notificationType, User sender)
        {
            string subject = "";

            switch(notificationType)
            {
                case "StationeryRequest_ForApproval":
                    subject = "For Approval: Stationery Request From " + sender.Name;
                    break;

                case "StationeryRequest_Approved":
                    subject = "Approved: Stationery Request";
                    break;

                case "StationeryRequest_Rejected":
                    subject = "Rejected: Stationery Request";
                    break;

                case "StationeryRequest_StoreClerk":
                    subject = "New Stationery Request From " + sender.Name;
                    break;

                case "Disbursement_Ready":
                    subject = "Stationery Is Ready For Collection";
                    break;

                case "Disbursement_Collected":
                    subject = "Stationery Has Been Collected";
                    break;

                case "Disbursement_NotCollected":
                    subject = "Oh No! Not Collected: Stationery";
                    break;
                
                /*case "":
                    subject = "" + ;
                    break;

                case "":
                    subject = "" + ;
                    break;*/

                default:
                    subject = "";
                    break;
            }
            return subject;
        }

        public String NotificationContent(String notificationType, User sender)
        {
            string content = "";

            switch (notificationType)
            {
                case "StationeryRequest_ForApproval":
                    content = "Dear Sir/Mdm,\n" + sender.Name + " has submitted a new request for stationery.";
                    //INCLUDE: Link to Approval Page
                    break;

                case "StationeryRequest_Approved":
                    content = "Your Stationery Request has been Approved.";
                    //Anything else to say?
                    break;

                case "StationeryRequest_Rejected":
                    content = "Your Stationery Request has been Rejected.";
                    //INCLUDE: Link to view Remarks?
                    break;

                case "StationeryRequest_StoreClerk":
                    content = "There is a new Stationery Request from " + sender.Name + ".";
                    //INCLUDE: Link to view Request
                    break;

                case "Disbursement_Ready":
                    content = "Your stationery is ready for collection from ";
                    //INCLUDE: Link to view Item List; Location & Date & Time of Collection & from who
                    break;

                case "Disbursement_Collected":
                    content = "Thank you for collecting your stationery. Here's a list of the items you've collected:\n\n";
                    //INCLUDE: Either List or Link to List
                    break;

                case "Disbursement_NotCollected":
                    content = "Oh dear. It seems you did not collect the stationery you requested. How about ";
                    //URGENT: What's the Resolution???
                    break;

                /*case "":
                    content = "" + ;
                    break;

                case "":
                    content = "" + ;
                    break;*/

                default:
                    content = "";
                    break;
            }
            return content;
        }


        //Get Subject & Message from where?
        public Notification CreateNotification(User sendingUser, String notificationType)
        {
            User sender = sendingUser;
            Notification notification = new Notification(DateTime.Now.ToString("dddd-MMMM-yyyy"), sender.Name, NotificationSubject(notificationType, sender), NotificationContent(notificationType, sender));
            return notification;
        }

        //Add New Notification to Notification Table in DB
        public void AddNotificationToDB(Notification notification)
        {
            var db = new Team7ADProjectDbContext();
            db.Notification.Add(notification);
            db.SaveChanges();

        }

        //Add Pair New Notification to Recipient in Notification_Users table in DB
        //Get the recipient from where?
        public void SendNotificationToRecipient(Notification notification)
        {
            
        }

    }

}
