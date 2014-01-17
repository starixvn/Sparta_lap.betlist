// <copyright file="NumberHelper.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using DNAide.Utils;

    /// <summary>
    /// Number helper class.
    /// </summary>
    public static class NumberHelper
    {
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
                return NumberFormat.DecFormat(obj, digit);
            }

            if (obj is int)
            {
                return ((int)obj).ToString("#,###");
            }

            return obj.ToString();
        }
    }
}