// -----------------------------------------------------------------------
// <copyright file="ILicenseeWinLossDataAccess.cs" company="">
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

    public interface ILicenseeWinLossDataAccess
    {
        Task<SortableReportResultModel<LicenseeWinLoss>> GetAgentWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList);

        Task<SortableReportResultModel<LicenseeWinLoss>> GetCustomerWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList);

        Task<SortableReportResultModel<LicenseeWinLoss>> GetSummaryWinloss(
            LicenseeWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList);
    }
}