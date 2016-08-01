namespace Fanex.Athena.Extension.Tests.Helper
{
    using System;
    using Extension.Helper;
    using NUnit.Framework;

    [TestFixture]
    public class NumberHelperTest
    {
        [TestCase(0, "0.00")]
        [TestCase(2.5, "2.50")]
        [TestCase(2.53, "2.53")]
        [TestCase(2.534, "2.53")]
        [TestCase(2.535, "2.54")]
        [TestCase(2.5361, "2.54")]
        [TestCase(2, "2.00")]
        [TestCase(-1, "-1.00")]
        [TestCase(-2.5, "-2.50")]
        public void ToStringExt_OneArgumentWithDecimalType_ReturnFormattedNumberWithTwoDigits(object value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);

            // Action
            var result = NumberHelper.ToStringExt(decimalNumber);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(0, "")]
        [TestCase(2, "2")]
        [TestCase(25, "25")]
        [TestCase(2530, "2,530")]
        [TestCase(1000000, "1,000,000")]       
        [TestCase(-1, "-1")]
        [TestCase(-25000, "-25,000")]
        public void ToStringExt_OneArgumentWithIntType_ReturnFormattedNumberWithThousandSeparator(object value, string expected)
        {
            // Action
            var result = value.ToStringExt();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(0.2, "0.2")]
        [TestCase(0.158, "0.158")]
        [TestCase(2.57384, "2.57384")]
        [TestCase(-0.2, "-0.2")]
        [TestCase(-0.158, "-0.158")]
        [TestCase(-2.57384, "-2.57384")]
        public void ToStringExt_OneArgumentWithOtherType_ReturnOriginalNumber(object value, string expected)
        {
            // Action
            var result = value.ToStringExt();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(0, "0.000")]
        [TestCase(2.5, "2.500")]
        [TestCase(2.53, "2.530")]
        [TestCase(2.534, "2.534")]
        [TestCase(2.5354, "2.535")]
        [TestCase(2.5366, "2.537")]
        [TestCase(2, "2.000")]
        [TestCase(-1, "-1.000")]
        [TestCase(-2.5, "-2.500")]
        public void ToStringExt_TwoArgumentsWithDecimalType_ReturnFormattedNumberWithThreeDigits(object value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);
            byte digit = 3;

            // Action
            var result = NumberHelper.ToStringExt(decimalNumber, digit);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(0, "0.00")]
        [TestCase(2.5, "2.50")]
        [TestCase(2.53, "2.53")]
        [TestCase(2.534, "2.53")]
        [TestCase(2.535, "2.54")]
        [TestCase(2.5361, "2.54")]
        [TestCase(2, "2.00")]
        [TestCase(-1, "-1.00")]
        [TestCase(-2.5, "-2.50")]
        public void DecFormat_OneArgumentWithDecimalType_ReturnFormattedNumberWithTwoDigits(object value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);

            // Action
            var result = NumberHelper.DecFormat(decimalNumber);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(0, "0.000")]
        [TestCase(2.5, "2.500")]
        [TestCase(2.53, "2.530")]
        [TestCase(2.534, "2.534")]
        [TestCase(2.5354, "2.535")]
        [TestCase(2.5366, "2.537")]
        [TestCase(2, "2.000")]
        [TestCase(-1, "-1.000")]
        [TestCase(-2.5, "-2.500")]
        public void DecFormat_TwoArgumentsWithDecimalType_ReturnFormattedNumberWithThreeDigits(object value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);
            byte digit = 3;

            // Action
            var result = NumberHelper.DecFormat(decimalNumber, digit);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}