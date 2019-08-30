using System;
using Rent.Helpers;
using Xunit;

namespace TimePlanning.Tests
{
    public class DateTimeHelpersTest
    {
        [Fact(DisplayName = "Add1DaysTo1-1-2019")]
        public void AddDays_Add1dayto1_1_2019()
        {
            var date = new DateTime(2019, 1, 1);

            var res = DateTimeHelpers.AddDays(date, 1);
            var expected = new DateTime(2019, 1, 2);

            Assert.Equal(res, expected);
           
        }

        [Fact(DisplayName = "Add-1DaysTo1-1-2019")]
        public void AddDays_subtract1dayfrom1_1_2019()
        {
            var date = new DateTime(2019, 1, 1);

            var res = DateTimeHelpers.AddDays(date, -1);
            var expected = new DateTime(2018, 12, 31);

            Assert.Equal(res, expected);
        }
        [Fact(DisplayName = "Add365DaysTo1-1-2019")]
        public void AddDays_Add365dayto1_1_2019()
        {
            var date = new DateTime(2019, 1, 1);

            var res = DateTimeHelpers.AddDays(date, 365);
            var expected = new DateTime(2020, 1, 1);

            Assert.Equal(res, expected);

        }
        [Fact(DisplayName = "Add730DaysTo1-1-2019_With_leap_year")]
        public void AddDays_Add730dayto1_1_2019()
        {
            var date = new DateTime(2019, 1, 1);

            var res = DateTimeHelpers.AddDays(date, 730);
            var expected = new DateTime(2020, 12, 31);

            Assert.Equal(expected, res);

        }

        [Fact(DisplayName = "Add0DaysTo1-1-2019")]
        public void AddDays_0days()
        {
            var date = new DateTime(2019, 1, 1);

            var res = DateTimeHelpers.AddDays(date, 0);
            var expected = new DateTime(2019, 1, 1);

            Assert.Equal(expected, res);

        }

        [Fact(DisplayName = "subtract1460Daysfrom1-1-2019_With_leap_year")]
        public void AddDays_Subtract1460dayfrom1_1_2019()
        {
            var date = new DateTime(2019, 1, 1);

            var res = DateTimeHelpers.AddDays(date, -1460);
            var expected = new DateTime(2015, 1, 2);

            Assert.Equal(expected, res);

        }
    }
}
