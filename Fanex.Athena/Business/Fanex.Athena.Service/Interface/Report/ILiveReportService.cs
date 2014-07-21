// -----------------------------------------------------------------------
// <copyright file="ILiveReportService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report.LiveReports;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ILiveReportService
    {
        IList<LiveReport> GetLiveReportSummary(LiveReportMetalModel model);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        void GetLiveReportSumaryDetail(
            LiveReportMetalModel model,
            string productCode,
            IList<Product> products,
            out IList<LiveReport> resultBySport,
             out IList<LiveReport> resultByByCurrency,
              out IList<LiveReportByBetType> resultByBettype);

        IList<LiveReportWinLossByMatch> GetLiveReportWinLossByMatch(LiveReportMatchListMetalModel model);
    }
}