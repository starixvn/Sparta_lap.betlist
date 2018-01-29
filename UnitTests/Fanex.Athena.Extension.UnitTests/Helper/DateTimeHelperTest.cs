namespace Fanex.Athena.Extension.UnitTests.Helper
{
    using System;
    using Extension.Helper;
    using NUnit.Framework;

    [TestFixture]
    public class DateTimeHelperTest
    {
        [TestCase("1/1/2016", "12/28/2015")]
        [TestCase("7/25/2016", "7/25/2016")]
        [TestCase("7/28/2016", "7/25/2016")]
        [TestCase("7/31/2016", "7/25/2016")]
        public void GetFirstDateOfWeek_WhenCall_ReturnCorrectDate(DateTime input, DateTime output)
        {
            // Action
            var result = DateTimeHelper.GetFirstDateOfWeek(input);

            // Assert
            Assert.That(result, Is.EqualTo(output));
        }

        [TestCase("7/25/2016", "7/24/2016")]
        [TestCase("7/28/2016", "7/24/2016")]
        [TestCase("7/31/2016", "7/31/2016")]
        public void GetFirstDateOfWeek_StartOfWeekIsSunday_ReturnCorrectDate(DateTime input, DateTime output)
        {
            // Action
            var result = DateTimeHelper.GetFirstDateOfWeek(input, DayOfWeek.Sunday);

            // Assert
            Assert.That(result, Is.EqualTo(output));
        }

        [TestCase(2018, 1, 2, 2018, 1, 1)]
        [TestCase(2018, 1, 31, 2018, 7, 1)]
        [TestCase(2018, 2, 15, 2018, 8, 1)]
        [TestCase(2017, 1, 1, 2018, 1, 1)]
        public void IsValidMonthRange_MaximumRangeIsSix_InvalidToDate_ReturnFalse(int fromYear, int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
        {
            // Arrange
            const int MaximumMonthRange = 6;

            // Action & Assert
            TestIsValidMonthRange(new DateTime(fromYear, fromMonth, fromDay), new DateTime(toYear, toMonth, toDay), MaximumMonthRange, false);
        }

        [TestCase(2018, 2, 15, 2018, 7, 31)]
        [TestCase(2017, 10, 1, 2018, 1, 1)]
        public void IsValidMonthRange_ValidToDate_ReturnTrue(int fromYear, int fromMonth, int fromDay, int toYear, int toMonth, int toDay)
        {
            // Arrange
            const int MaximumMonthRange = 6;

            // Action & Assert
            TestIsValidMonthRange(new DateTime(fromYear, fromMonth, fromDay), new DateTime(toYear, toMonth, toDay), MaximumMonthRange, true);
        }

        private void TestIsValidMonthRange(DateTime fromDate, DateTime toDate, int monthRange, bool expected)
        {
            // Action
            var result = DateTimeHelper.IsValidMonthRange(fromDate, toDate, monthRange);

            // Assert
            Assert.That(result, Is.EqualTo(expected), $"{fromDate.ToShortDateString()} - {toDate.ToShortDateString()}: {expected}");
        }
    }
}
