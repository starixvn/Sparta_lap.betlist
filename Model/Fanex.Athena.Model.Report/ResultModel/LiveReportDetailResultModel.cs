namespace Fanex.Athena.Model.Report.ResultModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO.LiveReports;

    public class LiveReportDetailResultModel
    {
        public IEnumerable<LiveReport> DataByCurrency { get; set; }

        public IEnumerable<LiveReport> DataBySport { get; set; }

        public IEnumerable<LiveReportByBetType> DataByBetType { get; set; }
    }
}