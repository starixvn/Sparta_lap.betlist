// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatch.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System.Collections.Generic;

    public class WeeklyLiveMatch
    {
        public int SortingWeekValue { get; set; }

        public string SportName { get; set; }

        public int Index { get; set; }

        public IEnumerable<int> NumbersOfMatches { get; set; }
    }
}