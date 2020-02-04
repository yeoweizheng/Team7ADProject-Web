using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Service
{
    public class DateService
    {
        public static String GetTodayDate()
        {
            return DateTime.Now.ToString("dd-MMM-yy");
        }
    }
}