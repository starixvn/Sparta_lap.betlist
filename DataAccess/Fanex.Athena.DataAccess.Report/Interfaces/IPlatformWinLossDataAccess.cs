// -----------------------------------------------------------------------
// <copyright file="IPlatformWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IPlatformWinLossDataAccess
    {
        SortableReportResultModel<PlatformWinLossMember> GetMemberData(PlatformWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        ReportResultModel<PlatformWinLossSummary> GetSummaryData(PlatformWinLossMetaModel metaModel);
    }
}