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

        [TestCase("7/25/2016", "7/24/2016", 0)]
        [TestCase("7/25/2016", "7/24/2016", 0)]
        [TestCase("7/25/2016", "7/31/2016", 1)]
        [TestCase("7/24/2016", "7/25/2016", 2)]
        public void GetWeeks_WhenCall_ReturnCorrectDate(DateTime fromdate, DateTime todate, int numberOfWeeks)
        {
            // Action
            var result = DateTimeHelper.GetWeeks(fromdate, todate);

            // Assert
            Assert.That(result, Is.EqualTo(numberOfWeeks));
        }

        [TestCase("7/28/2016", "7/29/2016", 2)]
        [TestCase("7/30/2016", "7/31/2016", 1)]
        public void GetWeeks_StartOfWeekIsFriday_ReturnCorrectDate(DateTime fromdate, DateTime todate, int numberOfWeeks)
        {
            // Action
            var result = DateTimeHelper.GetWeeks(fromdate, todate, DayOfWeek.Friday);

            // Assert
            Assert.That(result, Is.EqualTo(numberOfWeeks));
        }
    }
}
