using Fanex.Athena.Extension.Helper;

namespace Fanex.Athena.Extension.UnitTests.Helper
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
        public void ToStringExt_OneArgumentWithDecimalType_ReturnFormattedNumberWithTwoDigits(decimal value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);

            // Action
            var result = decimalNumber.ToStringExt();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, "")]
        [TestCase(2, "2")]
        [TestCase(25, "25")]
        [TestCase(2530, "2,530")]
        [TestCase(10000, "10,000")]
        [TestCase(-1, "-1")]
        [TestCase(-25000, "-25,000")]
        public void ToStringExt_OneArgumentWithIntType_ReturnFormattedNumberWithThousandSeparator(int value, string expected)
        {
            // Action
            var result = value.ToStringExt();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, 3, "0.000")]
        [TestCase(2.5, 3, "2.500")]
        [TestCase(2.53, 3, "2.530")]
        [TestCase(2.534, 3, "2.534")]
        [TestCase(2.5354, 3, "2.535")]
        [TestCase(2.5366, 3, "2.537")]
        [TestCase(2, 3, "2.000")]
        [TestCase(-1, 3, "-1.000")]
        [TestCase(-2.5, 3, "-2.500")]
        public void ToStringExt_TwoArgumentsWithDecimalType_ReturnFormattedNumberWithThreeDigits(decimal value, byte digit, string expected)
        {
            // Action
            var result = value.ToStringExt(digit);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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
        public void DecFormat_OneArgumentWithDecimalType_ReturnFormattedNumberWithTwoDigits(decimal value, string expected)
        {
            // Action
            var result = NumberHelper.DecFormat(value);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}