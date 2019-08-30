using System;
using Foundation;

namespace ModuleLibraryiOS.Date
{
    public static class Dates
    {
        public static DateTime NSDateToDateTime(this NSDate date)
        {
            return ((DateTime)date).ToLocalTime();
        }

        public static NSDate DateTimeToNSDate(this DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Local);
            return (NSDate)date;
        }

        public static DateTime StartOfDay(this DateTime dateTime)
        {
            return new DateTime(
               dateTime.Year,
               dateTime.Month,
               dateTime.Day,
               0, 0, 0, 0);
        }

        public static DateTime EndOfDay(this DateTime dateTime)
        {
            return new DateTime(
               dateTime.Year,
               dateTime.Month,
               dateTime.Day,
               23, 59, 59, 999);
        }
    }
}
