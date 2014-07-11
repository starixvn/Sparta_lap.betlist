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
        /// <param name="obj">The object value.</param>
        /// <returns>The formatted string.</returns>
        public static string ToStringExt(this object obj)
        {
            return ToStringExt(obj, 2);
        }

        /// <summary>
        ///  Formatting numbers for decimals and  significant digits.
        /// </summary>
        /// <param name="obj">The object value.</param>
        /// <param name="digit">The number of digits.</param>
        /// <returns>The formatted string.</returns>
        public static string ToStringExt(this object obj, byte digit)
        {
            if (obj is decimal)
            {
                var format = "F" + digit.ToString();
                return ((decimal)obj).ToString(format, _culture.NumberFormat);
            }

            if (obj is int)
            {
                return ((int)obj).ToString("#,###");
            }

            return obj.ToString();
        }

        public static string DecFormat(decimal obj, byte digit)
        {
            var format = "F" + digit.ToString();
            return obj.ToString(format, _culture.NumberFormat);
        }
    }
}