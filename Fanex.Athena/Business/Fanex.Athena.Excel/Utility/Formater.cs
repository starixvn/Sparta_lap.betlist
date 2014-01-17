// --------------------------------------------------------------------------
// <copyright file="Formater.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
// ----------------------------------------------------------------------

namespace Fanex.Athena.Excel.Utility
{
    using System;
    using System.Globalization;
    using System.Web;

    using DNAide.Core;
    using DNAide.Utils;

    using Models.DataDomain;

    /// <summary>
    /// Utility class for format number, currency..
    /// </summary>
    public class Formater
    {
        public const string CultNameEng = "en-US";
        public const string CultNameVi = "vi-VN";
        public const string DecimalFormat = "{0:#,###}";
        public const string DecimalFormat0 = "{0:#,##0}";
        public const string DecimalFormat0Vi = "{0:#.##0}";
        public const string DecimalFormat1 = "{0:#,##0.#}";
        public const string DecimalFormat10 = "{0:#,##0.0}";
        public const string DecimalFormat10Vi = "{0:#.##0,0}";
        public const string DecimalFormat1Vi = "{0:#.##0,#}";
        public const string DecimalFormat2 = "{0:#,##0.##}";
        public const string DecimalFormat20 = "{0:#,##0.00}";
        public const string DecimalFormat20Vi = "{0:#.##0,00}";
        public const string DecimalFormat2C = "{0:###0.00}";
        public const string DecimalFormat2CVi = "{0:#.##0,00}";
        public const string DecimalFormat2Vi = "{0:#.##0,##}";
        public const string DecimalFormat3 = "{0:#,##0.###}";
        public const string DecimalFormat30 = "{0:#,##0.000}";
        public const string DecimalFormat30Vi = "{0:#.##0,000}";
        public const string DecimalFormat3C = "{0:###0.000}";
        public const string DecimalFormat3CVi = "{0:#.##0,000}";
        public const string DecimalFormat3Vi = "{0:#.##0,###}";
        public const string DecimalFormat4 = "{0:#,##0.####}";
        public const string DecimalFormat40 = "{0:#,##0.0000}";
        public const string DecimalFormat40Vi = "{0:#.##0,0000}";
        public const string DecimalFormat4Vi = "{0:#.##0,####}";
        public const string DecimalFormatVi = "{0:#.###}";
        public const string TimeFormatMilisec = "HH:mm:ss fff";

        public static string Comm(object value)
        {
            return string.Format(DecimalFormat20, value);
        }

        public static string CommPercentage(object value)
        {
            return string.Format(DecimalFormat20, CastAide.AsDecimal(value) * 100);
        }

        public static string DecFormat(decimal value, byte decDigits)
        {
            return NumberFormat.DecFormat(value, decDigits, false);
        }

        public static string DecFormat(object value, byte decDigits)
        {
            return NumberFormat.DecFormat(value, decDigits, false);
        }

        /// <summary>
        ///     Format Date time by 'Month/day/year'.
        /// </summary>
        /// <param name="obj">Date time object to convert.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatDateEn(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return string.Empty;
            }

            DateTime dt;
            return DateTime.TryParse(obj.ToString(), out dt) ? string.Empty : dt.ToString("MM/dd/yyyy");
        }

        /// <summary>
        ///     Format date time by 'Month/day/year' hour:minute:second time'.
        /// </summary>
        /// <param name="obj">Date time object to convert.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatDateTimeEn12(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return string.Empty;
            }

            DateTime dt;
            return DateTime.TryParse(obj.ToString(), out dt) ? string.Empty : dt.ToString("MM/dd/yyyy hh:mm:ss tt");
        }

        /// <summary>
        ///     Format number with 1 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatNumber1(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat1Vi : DecimalFormat1, obj);
        }

        /// <summary>
        ///     Format number with 2 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatNumber2(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat2Vi : DecimalFormat2, obj);
        }

        public static string FormatNumber20(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat20Vi : DecimalFormat20, obj);
        }

        /// <summary>
        ///     Format number with 2 decimal, automatically remove redundant '0'.
        ///     And do not include comma.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatNumber2C(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat2CVi : DecimalFormat2C, obj);
        }

        /// <summary>
        ///     Format number with 3 decimal, automatically remove redundant '0'.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatNumber3(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat3Vi : DecimalFormat3, obj);
        }

        public static string FormatNumber30(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat30Vi : DecimalFormat30, obj);
        }

        /// <summary>
        ///     Format number with 3 decimal, automatically remove redundant '0'.
        ///     And do not include comma.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Formatted string.</returns>
        public static string FormatNumber3C(object obj, string culture)
        {
            return Convert.IsDBNull(obj) || obj == null
                ? "0"
                : string.Format(culture == CultNameVi ? DecimalFormat3CVi : DecimalFormat3C, obj);
        }

        public static string FormatSignNumber(int number)
        {
            return number > 0 ? "+" + number : number.ToString(CultureInfo.InvariantCulture);
        }

        public static string FullDateFormat(object obj)
        {
            return DateFormat.FullDateFormat(obj);
        }

        public static string FullDateFormat2Line(object obj)
        {
            return DateFormat.FullDateFormat2Ln(obj);
        }

        public static string Hdp(object value)
        {
            return CastAide.AsDecimal(value).ToString("0.##");
        }

        /// <summary>
        ///     Highlight the negative number by red color.
        /// </summary>
        /// <param name="amount">Amount object.</param>
        /// <returns>String value.</returns>
        public static string HighlightNegativeNumber(object amount)
        {
            decimal value = 0;
            if (amount is double)
            {
                value = (decimal)((double)amount);
            }
            else if (amount is decimal)
            {
                value = (decimal)amount;
            }

            return value < 0
                   ? "<font color='#ff0000'>" + DecFormat(amount, 2) + "</font>"
                   : value == 0 ? "0" : DecFormat(amount, 2);
        }

        /// <summary>
        /// Get Odds Text.
        /// </summary>
        /// <param name="value">Original odd value.</param>
        /// <returns>Odds text value.</returns>
        public static string Odds(object value)
        {
            decimal odds;
            if (!decimal.TryParse(value.ToString(), out odds))
            {
                throw new InvalidCastException("Invalid format odds");
            }

            if (odds < 0)
            {
                return string.Format("<span style='color: #B50000'>{0:0.00########}</span>", odds);
            }

            return odds == 0 ? "-" : string.Format("{0:0.00########}", odds);
        }

        /// <summary>
        /// Get odds text by bet type.
        /// </summary>
        /// <param name="betType">Bet type value.</param>
        /// <param name="value">Original odds value.</param>
        /// <returns>Odds text value.</returns>
        public static string Odds(int betType, object value)
        {
            return betType == (int)BetType.MixParlay || betType == (int)BetType.CombinationMixParlay
                   ? OddsMixParlay(value)
                   : Odds(value);
        }

        /// <summary>
        /// Get odds text of mix parlay.
        /// </summary>
        /// <param name="value">Original odd value.</param>
        /// <returns>Odds text value.</returns>
        public static string OddsMixParlay(object value)
        {
            decimal odds;
            if (!decimal.TryParse(value.ToString(), out odds))
            {
                throw new InvalidCastException("Invalid format odds");
            }

            if (odds < 0)
            {
                return string.Format("<span style='color: #B50000'>{0}</span>", FormatNumber3(odds, null));
            }

            return odds == 0 ? "-" : FormatNumber3(odds, null);
        }

        public static string Pt(object value)
        {
            return (CastAide.AsDecimal(value) * 100).ToString("N0");
        }

        public static string PtValue(object value)
        {
            decimal d = CastAide.AsDecimal(value);

            return d < 0
                   ? string.Join(null, new[] { "<font color=\"#B50000\">", string.Format(DecimalFormat20, d), "</font>" })
                   : string.Format(DecimalFormat20, d);
        }

        public static string ShortDateFormat(object obj)
        {
            return DateFormat.ShortDateFormat(obj);
        }

        public static string Stake(object value)
        {
            return string.Format(DecimalFormat20, value);
        }

        public static string StakeOrginCurrency(object value)
        {
            return string.Format(DecimalFormat0, value);
        }

        /// <summary>
        ///     Truncate text by tool tip if it is too long.
        /// </summary>
        /// <param name="value">Value to truncate.</param>
        /// <param name="maxLength">Max length of text.</param>
        /// <param name="hasSpanReturn">Return span or not.</param>
        /// <returns>Truncated string.</returns>
        public static string TruncateWithTip(string value, int maxLength, bool hasSpanReturn)
        {
            if (value.Length > maxLength)
            {
                value = string.Format(
                                        "<span title='{0}'>{1}...</span>",
                                        value.Replace("'", "&#39;"),
                                        HttpUtility.HtmlEncode(value.Substring(0, maxLength - 3)));
            }
            else if (hasSpanReturn)
            {
                value = string.Format("<span>{0}</span>", HttpUtility.HtmlEncode(value));
            }

            return value;
        }

        public static string TruncateWithTip(string value, int maxLength)
        {
            return TruncateWithTip(value, maxLength, false);
        }

        public static string Winloss(object value)
        {
            decimal d = CastAide.AsDecimal(value);

            return d < 0
                ? string.Join(null, new[] { "<font color=\"#B50000\">", string.Format(DecimalFormat20, d), "</font>" })
                : string.Format(DecimalFormat20, d);
        }
    }
}