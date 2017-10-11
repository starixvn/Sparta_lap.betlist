// -----------------------------------------------------------------------
// <copyright file="ILiveReportService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;

    public interface ILiveReportService
    {
        IList<LiveReport> GetLiveReportSummary(LiveReportMetaModel model);

        LiveReportDetailResultModel GetLiveReportSumaryDetail(LiveReportMetaModel model, IEnumerable<Product> products);

        IList<LiveReportWinLossByMatch> GetLiveReportWinLossByMatch(LiveReportMatchListMetalModel model);
    }
}