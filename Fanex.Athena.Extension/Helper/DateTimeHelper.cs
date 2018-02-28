namespace Fanex.Athena.Extension.Helper
{
    using System;

    public static class DateTimeHelper
    {
        public static bool IsBetween(DateTime date, DateTime minDate, DateTime maxDate)
        {
            return date.Date >= minDate.Date && date.Date <= maxDate.Date;
        }

        public static DateTime GetFirstDateOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return date.AddDays(-1 * diff).Date;
        }

        public static bool IsValidMonthRange(DateTime fromDate, DateTime toDate, int maximumMonthRange)
        {
            return toDate >= fromDate && GetMonthDifference(fromDate, toDate) <= maximumMonthRange;
        }

        private static int GetMonthDifference(DateTime fromDate, DateTime toDate)
        {
            return (12 * (toDate.Year - fromDate.Year)) + toDate.Month - fromDate.Month + 1;
        }
    }
}