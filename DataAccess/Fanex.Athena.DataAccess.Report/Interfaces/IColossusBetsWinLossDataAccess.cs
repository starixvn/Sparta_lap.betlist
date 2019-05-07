// -----------------------------------------------------------------------
// <copyright file="IColossusBetsWinLossDataAccess.cs" company="Microsoft">
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

    public interface IColossusBetsWinLossDataAccess
    {
        Task<SortableReportResultModel<ColossusBetWinLoss>> GetWinlossDetail(ColossusBetsWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<SortableReportResultModel<ColossusBetWinLoss>> GetSummaryWinloss(ColossusBetsWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}