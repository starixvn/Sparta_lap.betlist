// -----------------------------------------------------------------------
// <copyright file="ConvertHelper.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        /// Convert to date time.
        /// </summary>
        /// <param name="date">Date value.</param>
        /// <param name="isMonthly">
        /// Return first date of month if this is true.
        ///  value.</param>
        /// <returns>The result value.</returns>
        public static DateTime ToDateTime(string date, bool isMonthly)
        {
            var year = Convert.ToInt32(date.Substring(0, 4));
            var month = Convert.ToInt32(date.Substring(4, 2));
            var day = isMonthly ? 1 : Convert.ToInt32(date.Substring(6, 2));

            return new DateTime(year, month, day);            
        }
    }
}
