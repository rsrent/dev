using System;
using System.Globalization;
using System.Threading.Tasks;
namespace Rent.Helpers
{
    public class DateTimeHelpers
    {

        public static DateTime GmtPlusOneDateTime()
        {
            try
            {
                return TimeZoneInfo.ConvertTime(DateTime.UtcNow,
                                                   TimeZoneInfo.FindSystemTimeZoneById("UTC"),
                                                   TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time")).AddHours(1);
            }
            catch (Exception e)
            {

            }
            return DateTime.UtcNow;
        }
        //public static DateTime GmtPlusOneDateTime => TimeZoneInfo.ConvertTime(DateTime.UtcNow,
        // TimeZoneInfo.FindSystemTimeZoneById("UTC"),
        //TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time")).AddHours(1);

        public static int GetDayOfWeek(DateTime date)
        {
            int dayOfWeek = ((int)date.DayOfWeek) - 1;
            if (dayOfWeek == -1)
            {
                return 6;
            }
            return dayOfWeek;
        }

        public static int DaysInMonthInt(int month, int year)
        {
            if (month == 1) return 31;
            if (month == 2)
            {
                if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0) return 29;
                return 28;
            }
            if (month == 3) return 31;
            if (month == 4) return 30;
            if (month == 5) return 31;
            if (month == 6) return 30;
            if (month == 7) return 31;
            if (month == 8) return 31;
            if (month == 9) return 30;
            if (month == 10) return 31;
            if (month == 11) return 30;
            if (month == 12) return 31;
            throw new Exception(month + " Is not a month");
        }

        public static bool IsEvenWeek(int week)
        {
            if (week % 2 == 0)
            {
                return true;
            }
            return false;
        }

        public static int WeekOfYear(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static DateTime AddDays(DateTime date, int days)
        {

            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            while (days != 0)
            {
                if (days <= 0)
                {
                    int daysToSubtract = Math.Min(Math.Abs(days), day);
                    day -= daysToSubtract;
                    days += daysToSubtract;
                    if (day < 1)
                    {
                        month--;
                        if (month < 1)
                        {
                            month = 12;
                            year--;
                        }
                        day = DaysInMonthInt(month, year);
                    }
                }
                else
                {
                    int daysToAdd = Math.Min(days, DaysInMonthInt(month, year) - day + 1);
                    day += daysToAdd;
                    days -= daysToAdd;
                    if (day > DaysInMonthInt(month, year))
                    {
                        day = 1;
                        month++;
                        if (month > 12)
                        {
                            month = 1;
                            year++;
                        }
                    }
                }
            }
            return new DateTime(year, month, day);
        }

        public static void IterateDates(DateTime start, DateTime end, Action<DateTime> callback)
        {
            int year = start.Year;
            int month = start.Month;
            int day = start.Day;
            DateTime counter;
            do
            {
                counter = new DateTime(year, month, day);
                callback(counter);
                day++;
                if (day > DaysInMonthInt(month, year))
                {
                    day = 1;
                    month++;
                    if (month > 12)
                    {
                        month = 1;
                        year++;
                    }
                }

            }
            while (counter < end.Date);

        }

        public static bool DoesDatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 <= end2 && start2 <= end1)
            {
                return true;
            }
            return false;
        }

        public static bool DoesSingleDateOverlapWithDates(DateTime singleDate, DateTime start, DateTime end)
        {
            return DoesDatesOverlap(start, end, singleDate, singleDate);
        }

        public static async Task IterateDates(DateTime start, DateTime end, Func<DateTime, Task> callback)
        {
            int year = start.Year;
            int month = start.Month;
            int day = start.Day;
            DateTime counter;
            do
            {
                counter = new DateTime(year, month, day);
                await callback(counter);
                day++;
                if (day > DaysInMonthInt(month, year))
                {
                    day = 1;
                    month++;
                    if (month > 12)
                    {
                        month = 1;
                        year++;
                    }
                }

            }
            while (counter < end.Date);

        }

    }
}
