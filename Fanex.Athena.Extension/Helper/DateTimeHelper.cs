namespace Fanex.Athena.Extension.Helper
{
    using System;

    public static class DateTimeHelper
    {
        public static DateTime GetMaxDate(DateTime dateOne, DateTime dateTwo)
        {
            return dateOne > dateTwo ? dateOne : dateTwo;
        }

        public static DateTime GetMinDate(DateTime dateOne, DateTime dateTwo)
        {
            return dateOne < dateTwo ? dateOne : dateTwo;
        }

        public static bool IsBetween(DateTime date, DateTime minDate, DateTime maxDate)
        {
            return date.Date >= minDate.Date && date.Date <= maxDate.Date;
        }

        public static DateTime GetFirstDateOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            const int DaysInAWeek = 7;

            var diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += DaysInAWeek;
            }

            return date.AddDays(-1 * diff).Date;
        }

        public static bool IsValidMonthRange(DateTime fromDate, DateTime toDate, int maximumMonthRange)
        {
            return toDate >= fromDate && GetMonthDifference(fromDate, toDate) <= maximumMonthRange;
        }

        private static int GetMonthDifference(DateTime fromDate, DateTime toDate)
        {
            const int MonthsInAYear = 12;

            return (MonthsInAYear * (toDate.Year - fromDate.Year)) + toDate.Month - fromDate.Month + 1;
        }
    }
}