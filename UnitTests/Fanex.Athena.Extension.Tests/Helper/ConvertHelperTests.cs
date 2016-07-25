namespace Fanex.Athena.Extension.UnitTests.Helper
{
    using System;
    using Extension.Helper;
    using NUnit.Framework;

    [TestFixture]
    public class ConvertHelperTests
    {
        [TestCase(null, typeof(NullReferenceException))]
        [TestCase("", typeof(ArgumentOutOfRangeException))]
        [TestCase("123", typeof(ArgumentOutOfRangeException))]
        [TestCase("2016/01/01", typeof(FormatException))]
        public void ToDateTime_InvalidDateParam_ThrowException(string dateString, Type exceptionType)
        {
            // Action
            TestDelegate testDelegate = () => ConvertHelper.ToDateTime(dateString, false);

            // Assert
            Assert.Throws(exceptionType, testDelegate);
        }

        public void ToDateTime_ValidDateParam_ReturnDateTime()
        {
            // Arrange
            var dateString = "20160203";

            // Action
            var result = ConvertHelper.ToDateTime(dateString, false);

            // Assert
            Assert.That(result, Is.EqualTo(new DateTime(2016, 2, 3)));
        }

        public void ToDateTime_MonthlyParamIsTrue_ReturnFirstDayOfMonth()
        {
            // Arrange
            var dateString = "20160203";

            // Action
            var result = ConvertHelper.ToDateTime(dateString, true);

            // Assert
            Assert.That(result, Is.EqualTo(new DateTime(2016, 2, 1)));
        }
    }
}
