using System;
using System.Collections.Generic;
using System.Globalization;
using Rent.Models;
using Nager.Date;
using System.Linq;
using Rent.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Rent.Repositories
{
    public class EconomyRepository
    {
        List<DateTime> holidays;
        RentContext _context;

        public EconomyRepository(RentContext context)
        {
            _context = context;

            CultureInfo.CurrentCulture = new CultureInfo("da-DK");

            holidays = DateSystem.GetPublicHoliday("DK", DateTime.Now.Year).Select(h => h.Date).ToList();

            var palmSunday = holidays[3].AddDays(-7);
            var christmasEve = holidays[9].AddDays(-1);
            var newYearseve = new DateTime(DateTime.Now.Year, 12, 31);

            holidays.Insert(3, palmSunday);
            holidays.Insert(10, christmasEve);
            holidays.Insert(13, newYearseve);
        }

        public float GetDgForAll()
        {
            return GetDg(e => true, h => true);
        }

        public float GetDgForCustomer(int customerId)
        {
            var locations = _context.Location.Include(l => l.LocationHour).Where(l => l.CustomerID == customerId);
            return GetDgForLocations(locations);
        }

        public float GetDgForUser(int userId)
        {
            var locations = _context.Location.Include(l => l.LocationHour).Where(l => l.LocationUsers.Any(lu => lu.UserID == userId));
            return GetDgForLocations(locations);
        }

        public float GetDgForLocation(int locationId)
        {
            var locations = _context.Location.Include(l => l.LocationHour).Where(l => l.ID == locationId);
            return GetDgForLocations(locations);
        }

        float GetDgForLocations(IEnumerable<Location> locations)
        {
            //return GetDg(e => locations.Any(l => l.ID == e.LocationID), h => locations.Any(l => l.LocationHourID == h.ID));

            var ids = locations.Select(l => l.ID).ToList();

            //var locationEconomys = _context.LocationEconomy.Where((e) => ids.Contains(e.LocationID)).ToList();
            var locationEconomys = locations.Select(l => l.LocationEconomy).ToList();
            var locationHours = locations.Select(l => l.LocationHour).ToList();

            return CalculateDg(locationEconomys, locationHours);
        }

        float GetDg(Expression<Func<LocationEconomy, bool>> whereEconomy, Expression<Func<LocationHour, bool>> whereHour)
        {
            var locationEconomys = _context.LocationEconomy.Where(whereEconomy).ToList();
            var locationHours = _context.LocationHour.Where(whereHour).ToList();

            return CalculateDg(locationEconomys, locationHours);
        }


        public float CalculateDg(List<LocationEconomy> _locationEconomys, List<LocationHour> _locationHours)
        {
            float[][][] months = new float[_locationHours.Count][][];
            for (int i = 0; i < _locationHours.Count; i++)
            {
                months[i] = GetMonthsForLocation(_locationHours[i], _locationEconomys[i]);
            }
            /*
            SetValue(Jan, (months.Sum(m => m[0][1]) * 0.97 - months.Sum(m => m[0][0])));
            SetValue(Feb, (months.Sum(m => m[1][1]) * 0.97 - months.Sum(m => m[1][0])));
            SetValue(Mar, (months.Sum(m => m[2][1]) * 0.97 - months.Sum(m => m[2][0])));
            SetValue(Apr, (months.Sum(m => m[3][1]) * 0.97 - months.Sum(m => m[3][0])));
            SetValue(May, (months.Sum(m => m[4][1]) * 0.97 - months.Sum(m => m[4][0])));
            SetValue(Jun, (months.Sum(m => m[5][1]) * 0.97 - months.Sum(m => m[5][0])));
            SetValue(Jul, (months.Sum(m => m[6][1]) * 0.97 - months.Sum(m => m[6][0])));
            SetValue(Aug, (months.Sum(m => m[7][1]) * 0.97 - months.Sum(m => m[7][0])));
            SetValue(Sep, (months.Sum(m => m[8][1]) * 0.97 - months.Sum(m => m[8][0])));
            SetValue(Oct, (months.Sum(m => m[9][1]) * 0.97 - months.Sum(m => m[9][0])));
            SetValue(Nov, (months.Sum(m => m[10][1]) * 0.97 - months.Sum(m => m[10][0])));
            SetValue(Dec, (months.Sum(m => m[11][1]) * 0.97 - months.Sum(m => m[11][0])));
            */

            var monthlyPrice = _locationEconomys.Sum(e => e.PriceRegularCleaning + e.PriceWindowCleaning);
            var yearlyPrice = months.Sum(m => m.Sum(e => e[1]));
            var wageCost = - months.Sum(m => m.Sum(e => e[0]));
            var productPercentage = - (yearlyPrice * 0.03f);
            var totalExpense = wageCost + productPercentage;
            var earnings = yearlyPrice + totalExpense;
            var dg = earnings / yearlyPrice;

            return dg;
            /*
            SetValue(TotalMonthlyPrice, monthlyPrice);
            SetValue(TotalYearlyPrice, yearlyPrice);
            SetValue(WageCost, -wageCost);
            SetValue(ProductPercentage, -productPercentage);
            SetValue(TotalExpense, -totalExpense);
            SetValue(Earning, earnings);
            SetValue(DG, dg, "P"); */
        }
        /*
        void SetValue(UILabel label, double value, string format = "C")
        {
            if (value > 0)
                label.TextColor = UIColor.FromRGB(50, 122, 42);
            else if (value < 0)
                label.TextColor = UIColor.Red;
            else label.TextColor = UIColor.Black;

            label.Text = value.ToString(format);
        } */

        float[][] GetMonthsForLocation(LocationHour hours, LocationEconomy economy)
        {
            float[][] months = new float[12][];

            var date = new DateTime(DateTime.Now.Year, 1, 1);
            var thisYear = date.Year;

            while (date.Year == thisYear)
            {
                int[] week = new int[14];

                int thisMonth = date.Month;

                int daysThisMonth = 0;
                int daysChargedThisMonth = 0;

                while (date.Month == thisMonth && date.Year == thisYear)
                {
                    daysThisMonth++;
                    if (Include(date, hours) && (economy.StartDate == null || date >= economy.StartDate))
                    {
                        daysChargedThisMonth++;
                        week[((int)date.DayOfWeek) + (7 * (GetIso8601WeekOfYear(date) % 2 * (hours.DifferentWeeks ? 1 : 0)))]++;
                    }
                    date = date.AddDays(1);
                }

                float totalHours = 0;

                for (int i = 0; i < 14; i++)
                {
                    int days = week[i];

                    if (i == 0) totalHours += days * hours.L_Sun;
                    if (i == 1) totalHours += days * hours.L_Mon;
                    if (i == 2) totalHours += days * hours.L_Tue;
                    if (i == 3) totalHours += days * hours.L_Wed;
                    if (i == 4) totalHours += days * hours.L_Thu;
                    if (i == 5) totalHours += days * hours.L_Fri;
                    if (i == 6) totalHours += days * hours.L_Sat;
                    if (i == 7) totalHours += days * hours.U_Sun;
                    if (i == 8) totalHours += days * hours.U_Mon;
                    if (i == 9) totalHours += days * hours.U_Tue;
                    if (i == 10) totalHours += days * hours.U_Wed;
                    if (i == 11) totalHours += days * hours.U_Thu;
                    if (i == 12) totalHours += days * hours.U_Fri;
                    if (i == 13) totalHours += days * hours.U_Sat;
                }
                months[thisMonth - 1] = new float[2];
                months[thisMonth - 1][0] = totalHours * GetPricePerHour(economy.PricePerHourCategory);
                months[thisMonth - 1][1] = ((float)daysChargedThisMonth / (float)daysThisMonth) * (economy.PriceRegularCleaning + economy.PriceWindowCleaning);
                //System.Diagnostics.Debug.WriteLine(thisMonth + "  " +  totalHours + " - " + months[thisMonth - 1][0] + " % " + months[thisMonth - 1][1]);
            }
            return months;
        }


        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }
            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        } 

        public  bool Include(DateTime date, LocationHour hours)
        {
            if(!hours.NewyearsDay && SameDate(date, holidays[0])) return false;
            if(!hours.Palmsunday && SameDate(date, holidays[1])) return false;
            if(!hours.MaundyThursday && SameDate(date, holidays[2])) return false;
            if(!hours.GoodFriday && SameDate(date, holidays[3])) return false;
            if(!hours.EasterDay && SameDate(date, holidays[4])) return false;
            if(!hours.SecondEasterDay && SameDate(date, holidays[5])) return false;
            if(!hours.PrayerDay && SameDate(date, holidays[6])) return false;
            if(!hours.ChristAscension && SameDate(date, holidays[7])) return false;
            if(!hours.WhitSunday && SameDate(date, holidays[8])) return false;
            if(!hours.SndPentecost && SameDate(date, holidays[9])) return false;
            if(!hours.ChristmasEve && SameDate(date, holidays[10])) return false;
            if(!hours.ChristmasDay && SameDate(date, holidays[11])) return false;
            if(!hours.SndChristmasDay && SameDate(date, holidays[12])) return false;
            if(!hours.NewyearsEve && SameDate(date, holidays[13])) return false;

            return true;
        }

        bool SameDate(DateTime date1, DateTime date2)
        {
            return date1.Year == date2.Year && date1.DayOfYear == date2.DayOfYear;
        }

        public int GetPricePerHour(PricePerHourCategory category)
        {
            if (category == PricePerHourCategory.Drift)
                return 205;
            if (category == PricePerHourCategory.Hotel)
                return 175;
            if (category == PricePerHourCategory.Rent)
                return 180;
            if (category == PricePerHourCategory.UL)
                return 175;
            return 0;
        }
    }
}
