using System;
using System.Collections.Generic;
using System.Text;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace TimePlanning.Tests
{
    public class DanishHolidayTest
    {
        [Fact(DisplayName = "GetEasterDayIn2019")]
        public void GetDayMonthEsterDay_2019()
        {
            var dkholidayCalc = new DanishHolidayCalculator();
            var res = dkholidayCalc.GetDayMonthForEasterDay(2019);

            var expected = new Tuple<int, int>(21, 4);

            Assert.Equal(res, expected);
        }

        [Fact(DisplayName = "GetEasterDayIn2021")]
        public void GetDayMonthEsterDay_2021()
        {
            var dkholidayCalc = new DanishHolidayCalculator();
            var res = dkholidayCalc.GetDayMonthForEasterDay(2021);

            var expected = new Tuple<int, int>(4, 4);

            Assert.Equal(res, expected);
        }

        [Fact(DisplayName = "GetEasterDayIn2090")]
        public void GetDayMonthEsterDay_2090()
        {
            var dkholidayCalc = new DanishHolidayCalculator();
            var res = dkholidayCalc.GetDayMonthForEasterDay(2090);

            var expected = new Tuple<int, int>(16, 4);

            Assert.Equal(res, expected);
        }

        [Fact(DisplayName = "DayOfChristAscension_2019")]
        public  void DayOfChristAscension_2019()
        {
            var dkholidayCalc = new DanishHolidayCalculator();
            var holiday = new Holiday
            {
                Name = "ChristAscension",
                CountryCode = "DK"
            };

            var res = dkholidayCalc.DateOfHoliday(holiday, 2019);

            var expected = new DateTime(2019, 5, 30);

            Assert.Equal(res, expected);
        }

        [Fact(DisplayName = "DayOfChristAscension_2019NotEqual")]
        public void DayOfChristAscension_2019NotEqual()
        {
            var dkholidayCalc = new DanishHolidayCalculator();
            var holiday = new Holiday
            {
                Name = "ChristAscension",
                CountryCode = "DK"
            };

            var res = dkholidayCalc.DateOfHoliday(holiday, 2019);

            var expected = new DateTime(2019, 6, 30);

            Assert.NotEqual(res, expected);
        }

        [Fact(DisplayName = "DayOfChristAscension_2019NotEqualWrongYear")]
        public void DayOfChristAscension_2019SameDayWrongyearNotEqual()
        {
            var dkholidayCalc = new DanishHolidayCalculator();
            var holiday = new Holiday
            {
                Name = "ChristAscension",
                CountryCode = "DK"
            };

            var res = dkholidayCalc.DateOfHoliday(holiday, 2019);

            var expected = new DateTime(2020, 5, 30);

            Assert.NotEqual(res, expected);
        }

        [Fact(DisplayName = "DayOfChristAscension_2019NotEqual")]
        public void GetDatesOfHolidays_Between22_apr_and_30_may_2019()
        {
            var dkholidayCalc = new DanishHolidayCalculator();

            var from = new DateTime(2019, 4, 22);
            var to = new DateTime(2019, 5, 30);

            var storBedeDag = new Holiday
            {
                Name = "PrayerDay",
                CountryCode = "DK"
            };

            var KristHimmelfart = new Holiday
            {
                Name = "ChristAscension",
                CountryCode = "DK"
            };



            var holidays = new List<Holiday> {storBedeDag, KristHimmelfart };

            var res = dkholidayCalc.GetDatesOfHolidays(holidays, from, to);

            var expected = new List<DateTime> { new DateTime(2019, 5, 17), new DateTime(2019, 5, 30)};

            Assert.Equal(res, expected);
        }


    }
}
