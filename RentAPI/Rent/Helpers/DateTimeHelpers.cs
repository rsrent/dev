using System;
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
    }
}
