namespace Fanex.Athena.Extension.Helper
{
    using System;
    using System.Globalization;

    public static class DateTimeHelper
    {
        public static DateTime GetFirstDateOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return date.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Get number of weeks between two date.
        /// </summary>
        /// <param name="startOfWeek"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetWeeks(DateTime startDate, DateTime endDate, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            const CalendarWeekRule WeekRule = CalendarWeekRule.FirstDay;
            var calendar = DateTimeFormatInfo.CurrentInfo.Calendar;
            var startWeekNumber = 0;
            var endWeekNumber = 0;
            var numberOfWeeksInRange = 0;

            if (startDate.Year == endDate.Year)
            {
                startWeekNumber = calendar.GetWeekOfYear(startDate, WeekRule, startOfWeek);
                endWeekNumber = calendar.GetWeekOfYear(endDate, WeekRule, startOfWeek);
                numberOfWeeksInRange = endWeekNumber - startWeekNumber;
            }
            else
            {
                for (int year = startDate.Year; year <= endDate.Year; year++)
                {
                    if (year == startDate.Year)
                    {
                        startWeekNumber = calendar.GetWeekOfYear(startDate, WeekRule, startOfWeek);
                        endWeekNumber = calendar.GetWeekOfYear(new DateTime(year + 1, 1, 1).AddDays(-1), WeekRule, startOfWeek);
                    }
                    else if (year == endDate.Year)
                    {
                        startWeekNumber = calendar.GetWeekOfYear(new DateTime(year, 1, 1), WeekRule, startOfWeek);
                        endWeekNumber = calendar.GetWeekOfYear(endDate, WeekRule, startOfWeek);
                    }
                    else
                    {
                        startWeekNumber = calendar.GetWeekOfYear(new DateTime(year, 1, 1), WeekRule, startOfWeek);
                        endWeekNumber = calendar.GetWeekOfYear(new DateTime(year + 1, 1, 1).AddDays(-1), WeekRule, startOfWeek);
                    }

                    numberOfWeeksInRange += endWeekNumber - startWeekNumber;
                }
            }

            return numberOfWeeksInRange + 1;
        }
    }
}