namespace Fanex.Athena.Extension.Helper
{
    using System;
    using System.Globalization;

    public static class DateTimeHelper
    {
        public static DateTime GetFirstDateOfWeek(DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = date.DayOfWeek - startOfWeek;
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
            var weekRule = CalendarWeekRule.FirstDay;
            var calendar = DateTimeFormatInfo.CurrentInfo.Calendar;
            int startWeekNumber = 0;
            int endWeekNumber = 0;
            int numberOfWeeksInRange = 0;

            if (startDate.Year == endDate.Year)
            {
                startWeekNumber = calendar.GetWeekOfYear(startDate, weekRule, startOfWeek);
                endWeekNumber = calendar.GetWeekOfYear(endDate, weekRule, startOfWeek);
                numberOfWeeksInRange = endWeekNumber - startWeekNumber;
            }
            else
            {
                for (int year = startDate.Year; year <= endDate.Year; year++)
                {
                    if (year == startDate.Year)
                    {
                        startWeekNumber = calendar.GetWeekOfYear(startDate, weekRule, startOfWeek);
                        endWeekNumber = calendar.GetWeekOfYear(new DateTime(year + 1, 1, 1).AddDays(-1), weekRule, startOfWeek);
                    }
                    else if (year == endDate.Year)
                    {
                        startWeekNumber = calendar.GetWeekOfYear(new DateTime(year, 1, 1), weekRule, startOfWeek);
                        endWeekNumber = calendar.GetWeekOfYear(endDate, weekRule, startOfWeek);
                    }
                    else
                    {
                        startWeekNumber = calendar.GetWeekOfYear(new DateTime(year, 1, 1), weekRule, startOfWeek);
                        endWeekNumber = calendar.GetWeekOfYear(new DateTime(year + 1, 1, 1).AddDays(-1), weekRule, startOfWeek);
                    }

                    numberOfWeeksInRange += endWeekNumber - startWeekNumber;
                }
            }

            return numberOfWeeksInRange + 1;
        }
    }
}