// <copyright file="NumberHelper.cs" company="Nexcel Solution">
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
        private static CultureInfo _culture = new CultureInfo("en-US");

        /// <summary>
        /// Formatting numbers for decimals and two significant digits.
        /// </summary>
        /// <param name="value">The object value.</param>
        /// <returns>The formatted string.</returns>
        public static string ToStringExt(this object value)
        {
            return ToStringExt(value, 2);
        }

        /// <summary>
        ///  Formatting numbers for decimals and  significant digits.
        /// </summary>
        /// <param name="value">The object value.</param>
        /// <param name="digit">The number of digits.</param>
        /// <returns>The formatted string.</returns>
        public static string ToStringExt(this object value, byte digit = 2)
        {
            if (value is decimal)
            {
                var format = "N" + digit.ToString();
                return ((decimal)value).ToString(format, _culture.NumberFormat);
            }

            if (value is int)
            {
                return ((int)value).ToString("#,###");
            }

            return value.ToString();
        }

        public static string DecFormat(decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, _culture.NumberFormat);
        }
    }
}