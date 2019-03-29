// -----------------------------------------------------------------------
// <copyright file="ICashOutWinLossDataAccess.cs" company="">
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

    public interface ICashOutWinLossDataAccess
    {
        Task<SortableReportResultModel<CashOutWinLoss>> GetDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<SortableReportResultModel<CashOutWinLoss>> GetSummary(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}