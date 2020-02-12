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
        public static String GetLastYear()
        {
            return DateTime.Now.AddYears(-1).ToString("dd-MMM-yy");
        }
        public static DateTime ParseDate(string dateStr)
        {
            return DateTime.ParseExact(dateStr, "dd-MMM-yy", null).Date;
        }
        public static bool IsOverlap(string startDate1, string endDate1, string startDate2, string endDate2)
        {
            DateTime s1 = ParseDate(startDate1);
            DateTime e1 = ParseDate(endDate1);
            DateTime s2 = ParseDate(startDate2);
            DateTime e2 = ParseDate(endDate2);
            if (s1.CompareTo(s2) == 0 || s1.CompareTo(e2) == 0 || e1.CompareTo(s2) == 0 || e1.CompareTo(e2) == 0) return true;
            if (s1.CompareTo(s2) < 0 && e1.CompareTo(s2) > 0) return true;
            if (s2.CompareTo(s1) < 0 && e2.CompareTo(s1) > 0) return true;
            return false;
        }
        public static bool IsValidStartEnd(string startDate, string endDate)
        {
            DateTime s = ParseDate(startDate);
            DateTime e = ParseDate(endDate);
            if (s.CompareTo(e) <= 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public static bool IsEqualOrAfter(string date1, string date2)
        {
            DateTime d1 = ParseDate(date1);
            DateTime d2 = ParseDate(date2);
            if(d1.CompareTo(d2) >= 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public static bool IsAfter(string date1, string date2)
        {
            DateTime d1 = ParseDate(date1);
            DateTime d2 = ParseDate(date2);
            if(d1.CompareTo(d2) > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public static string AddMonth(string originalDateStr)
        {
            DateTime originalDate = ParseDate(originalDateStr);
            DateTime newDate = originalDate.AddMonths(1);
            return newDate.ToString("dd-MMM-yy");
        }
    }
}