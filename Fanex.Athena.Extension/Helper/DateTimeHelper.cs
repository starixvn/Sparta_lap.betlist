namespace Fanex.Athena.Extension.Helper
{
    using System;
    using System.Globalization;

    public static class DateTimeHelper
    {
        public static DateTime GetFirstDateOfWeek(DateTime date, DayOfWeek firstDayofWeek = DayOfWeek.Sunday)
        {
            int delta = firstDayofWeek - date.DayOfWeek;
            return date.AddDays(delta);
        }

        /// <summary>
        /// Get number of weeks between two date.
        /// </summary>
        /// <param name="firstDayofWeek"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetWeeks(DateTime startDate, DateTime endDate, DayOfWeek firstDayofWeek = DayOfWeek.Sunday)
        {
            var weekRule = CalendarWeekRule.FirstDay;
            var calendar = DateTimeFormatInfo.CurrentInfo.Calendar;
            int startWeekNumber = 0;
            int endWeekNumber = 0;
            int numberOfWeeksInRange = 0;

            if (startDate.Year == endDate.Year)
            {
                startWeekNumber = calendar.GetWeekOfYear(startDate, weekRule, firstDayofWeek);
                endWeekNumber = calendar.GetWeekOfYear(endDate, weekRule, firstDayofWeek);
                numberOfWeeksInRange = endWeekNumber - startWeekNumber;
            }
            else
            {
                for (int year = startDate.Year; year <= endDate.Year; year++)
                {
                    if (year == startDate.Year)
                    {
                        startWeekNumber = calendar.GetWeekOfYear(startDate, weekRule, firstDayofWeek);
                        endWeekNumber = calendar.GetWeekOfYear(new DateTime(year + 1, 1, 1).AddDays(-1), weekRule, firstDayofWeek);
                    }
                    else if (year == endDate.Year)
                    {
                        startWeekNumber = calendar.GetWeekOfYear(new DateTime(year, 1, 1), weekRule, firstDayofWeek);
                        endWeekNumber = calendar.GetWeekOfYear(endDate, weekRule, firstDayofWeek);
                    }
                    else
                    {
                        startWeekNumber = calendar.GetWeekOfYear(new DateTime(year, 1, 1), weekRule, firstDayofWeek);
                        endWeekNumber = calendar.GetWeekOfYear(new DateTime(year + 1, 1, 1).AddDays(-1), weekRule, firstDayofWeek);
                    }

                    numberOfWeeksInRange += endWeekNumber - startWeekNumber;
                }
            }

            return numberOfWeeksInRange + 1;
        }
    }
}