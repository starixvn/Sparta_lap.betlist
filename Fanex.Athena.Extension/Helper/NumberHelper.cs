// <copyright file="NumberHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using System.Globalization;

    /// <summary>
    /// Number helper class.
    /// </summary>
    public static class NumberHelper
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        /// <summary>
        ///  Formatting numbers for decimals and  significant digits.
        /// </summary>
        /// <param name="value">The object value.</param>
        /// <param name="digit">The number of digits.</param>
        /// <returns>The formatted string.</returns>
        public static string ToStringExt(this decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }

        public static string ToStringExt(this int value)
        {
            return value.ToString("#,###");
        }

        public static string DecFormat(decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }

        public static string PercentageFormat(decimal value, byte digit = 2)
        {
            return DecFormat(value.ToPercentage(), digit);
        }

        public static decimal ToPercentage(this decimal value)
        {
#pragma warning disable S109 // Magic numbers should not be used
            return value * 100;
#pragma warning restore S109 // Magic numbers should not be used
        }
    }
}