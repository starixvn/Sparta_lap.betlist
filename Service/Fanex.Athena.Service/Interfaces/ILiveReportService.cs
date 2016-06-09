// -----------------------------------------------------------------------
// <copyright file="ILiveReportService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;

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