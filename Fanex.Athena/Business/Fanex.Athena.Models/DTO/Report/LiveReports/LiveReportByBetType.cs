// -----------------------------------------------------------------------
// <copyright file="LiveReportByBetType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report.LiveReports
{
    using System.Collections.Generic;

    /// <summary>
    /// LiveReportByBetType class.
    /// </summary>
    public class LiveReportByBetType
    {
        public string ProductName { get; set; }

        public IList<LiveReport> LiveReports { get; set; }
    }
}