// -----------------------------------------------------------------------
// <copyright file="IPlatformWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IPlatformWinLossDataAccess
    {
        Task<SortableReportResultModel<PlatformWinLossMember>> GetMemberData(PlatformWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<ReportResultModel<PlatformWinLossSummary>> GetSummaryData(PlatformWinLossMetaModel metaModel);
    }
}