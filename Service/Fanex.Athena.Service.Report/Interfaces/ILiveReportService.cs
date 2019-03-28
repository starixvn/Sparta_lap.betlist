// -----------------------------------------------------------------------
// <copyright file="ILiveReportService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;

    public interface ILiveReportService
    {
        Task<IList<LiveReport>> GetLiveReportSummary(LiveReportMetaModel model);

        Task<LiveReportDetailResultModel> GetLiveReportSumaryDetail(LiveReportMetaModel model, IDictionary<int, string> currencyList, IEnumerable<Product> products);

        Task<IList<LiveReportWinLossByMatch>> GetLiveReportWinLossByMatch(LiveReportMatchListMetalModel model);
    }
}