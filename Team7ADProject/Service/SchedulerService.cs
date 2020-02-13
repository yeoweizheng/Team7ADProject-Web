using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using Team7ADProject.Database;
using Team7ADProject.Models;

namespace Team7ADProject.Service
{
    public class SchedulerService
    {
        private static Team7ADProjectDbContext db;
        private static StationeryService stationeryService;
        private static RequestService requestService;
        public static void StartScheduler()
        {
            db = new Team7ADProjectDbContext();
            stationeryService = new StationeryService();
            requestService = new RequestService();
            Timer timer = new System.Timers.Timer();
            timer.Interval = 5000;
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            //Generate dept requests every Wed
            ScheduledJob job1 = db.ScheduledJob.Where(x => x.Name == "generateDepartmentRequests").FirstOrDefault();
            if(now.DayOfWeek == DayOfWeek.Wednesday && now.ToString("dd-MMM-yy") != job1.DateLastCalled)
            {
                requestService.GenerateDepartmentRequests();
            }
            // Upload demand data every 28th of the month
            ScheduledJob job2 = db.ScheduledJob.Where(x => x.Name == "uploadDemandData").FirstOrDefault();
            if(now.Day == 28 && now.ToString("dd-MMM-yy") != job2.DateLastCalled)
            {
                stationeryService.UploadDemandData(DateService.GetTodayDate());
            }
        }
    }
}