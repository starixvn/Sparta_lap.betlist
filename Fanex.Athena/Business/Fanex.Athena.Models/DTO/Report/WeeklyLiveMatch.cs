// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatch.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

namespace Fanex.Athena.Models.DTO.Report
{
    public class WeeklyLiveMatch
    {
        public string SportName { get; set; }

        public int Index { get; set; }

        public IEnumerable<int> NumOfMatches { get; set; }
    }
}