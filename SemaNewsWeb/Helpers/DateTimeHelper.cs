using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SemaNewsWeb.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime ToDay(){
            return DateTime.Today;
        }
        public static DateTime WeekStartDay()
        {
            var dayOfWeek = Convert.ToInt32(DateTime.Today.DayOfWeek);
            return DateTime.Today.AddDays(-dayOfWeek);
        }
        public static DateTime MonthStartDay()
        {
            var dayOfMonth = Convert.ToInt32(DateTime.Today.Day);
            return DateTime.Today.AddDays(-dayOfMonth);
        }
        public static DateTime YearStartDay()
        {
            var dayOfYear = DateTime.Today.DayOfYear;
            return DateTime.Today.AddDays(-dayOfYear);
        }
    }
}