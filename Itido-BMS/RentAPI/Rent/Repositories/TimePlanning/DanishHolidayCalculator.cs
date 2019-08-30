using System;
using System.Collections.Generic;
using Rent.Models.TimePlanning;
using Rent.Helpers;
using Rent.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rent.Repositories.TimePlanning

{
    public class DanishHolidayCalculator : IHolidayCalculator
    {

        //Nytårsdag
        public const string NewyearsDay = "NewyearsDay";
        //Palmesøndag
        public const string Palmsunday = "Palmsunday";
        //Skærtorsdag
        public const string MaundyThursday = "MaundyThursday";
        //Langfredag
        public const string GoodFriday = "GoodFriday";
        //1. påskedag
        public const string EasterDay = "EasterDay";
        //2. påskedag
        public const string SecondEasterDay = "SecondEasterDay";
        //Store bededag
        public const string PrayerDay = "PrayerDay";
        //kristi himmelfartsdag
        public const string ChristAscension = "ChristAscension";
        // 1. Maj
        public const string FstOfMay = "FstOfMay";
        //Pinse
        public const string WhitSunday = "WhitSunday";
        //2. pinsedag
        public const string SndPentecost = "SndPentecost";
        //Grundlovsdag
        public const string ConstitutionDay = "ConstitutionDay";
        //Juleaften
        public const string ChristmasEve = "ChristmasEve";
        //Juledag
        public const string ChristmasDay = "ChristmasDay";
        //2. Juledag
        public const string SndChristmasDay = "SndChristmasDay";
        //Nytårsaften
        public const string NewyearsEve = "NewyearsEve";

        public DanishHolidayCalculator()
        {
            
        }
        public override List<DateTime> GetDatesOfHolidays(List<Holiday> holidays, DateTime from, DateTime to)
        {
            List<DateTime> datesOfHolidays = new List<DateTime>();

            for(int i = from.Year; i <= to.Year; i++)
            {
                foreach(var holiday in holidays)
                {
                   var result = DateOfHoliday(holiday, i);
                   if(result != null)
                   {
                       datesOfHolidays.Add(result.Value);
                   }
                }
            }
            return datesOfHolidays;            
        }
        public Tuple<int, int> GetDayMonthForEasterDay(int year)
        {
            var a = year % 19;
            // print('a: $a');
            var b = (int)Math.Floor(year / 100.0);
            // print('b: $b');
            var c = year % 100;
            // print('c: $c');
            var d = (int)Math.Floor(b / 4.0);
            // print('d: $d');
            var e = b % 4;
            // print('e: $e');
            var f = (int)Math.Floor((b + 8) / 25.0);
            // print('f: $f');
            var g = (int)Math.Floor((b - f + 1) / 3.0);
            // print('g: $g');
            var h = (19 * a + b - d - g + 15) % 30;
            // print('h: $h');
            var j = (int)Math.Floor((c / 4.0));
            // print('j: $j');
            var k = c % 4;
            // print('k: $k');
            var l = (32 + 2 * e + 2 * j - h - k) % 7;
            // print('l: $l');
            var m = (int)Math.Floor((a + 11 * h + 22 * l) / 451.0);
            // print('m: $m');
            var n = (int)Math.Floor((h + l - 7 * m + 114) / 31.0);
            // print('n: $n');
            var p = (h + l - 7 * m + 114) % 31;
            // print('p: $p');
            var q = (n - 3) * 31 + p - 20;
            var day = p + 1;
            var month = n;
            return new Tuple<int, int>(day, month);
        }

        public DateTime? DateOfHoliday(Holiday holiday, int year)
        {
            if(holiday == null || !holiday.CountryCode.Equals("DK"))
            {
                return null;
            }
            switch (holiday.Name)
            {
                case NewyearsDay:
                    return new DateTime(year, 1, 1);
                case ConstitutionDay:
                    return new DateTime(year, 6, 5);
                case ChristmasEve:
                    return new DateTime(year, 12, 24);
                case ChristmasDay:
                    return new DateTime(year, 12, 25);
                case FstOfMay:
                    return new DateTime(year, 5, 1);
                case SndChristmasDay:
                    return new DateTime(year, 12, 26);
                case NewyearsEve:
                    return new DateTime(year, 12, 31);
                default:
                    var dayMonth = GetDayMonthForEasterDay(year);
                    var day = dayMonth.Item1;
                    var month = dayMonth.Item2;

                    var easterDay = new DateTime(year, month, day);

                    if (holiday.Name == MaundyThursday)
                        return DateTimeHelpers.AddDays(easterDay, -3);
                        //return easterDay.subtract(Duration(days: 3));
                    if (holiday.Name == GoodFriday)
                        return DateTimeHelpers.AddDays(easterDay, -2);
                        //return easterDay.subtract(Duration(days: 2));
                    if (holiday.Name == EasterDay) return easterDay;
                    if (holiday.Name == SecondEasterDay)
                        //return easterDay.add(Duration(days: 1));
                        return DateTimeHelpers.AddDays(easterDay, 1);
                    if (holiday.Name == PrayerDay)
                        //return easterDay.add(Duration(days: 26));
                        return DateTimeHelpers.AddDays(easterDay, 26);
                    if (holiday.Name == ChristAscension)
                        //return easterDay.add(Duration(days: 39));
                        return DateTimeHelpers.AddDays(easterDay, 39);
                    if (holiday.Name == WhitSunday)
                        //return easterDay.add(Duration(days: 49));
                        return DateTimeHelpers.AddDays(easterDay, 49);
                    if (holiday.Name == SndPentecost)
                        //return easterDay.add(Duration(days: 50));
                        return DateTimeHelpers.AddDays(easterDay, 50);
                    break;
            }
            return null;
        }

    }
}