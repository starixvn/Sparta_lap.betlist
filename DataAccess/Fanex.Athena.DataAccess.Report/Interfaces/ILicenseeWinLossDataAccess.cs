// -----------------------------------------------------------------------
// <copyright file="ILicenseeWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface ILicenseeWinLossDataAccess
    {
        SortableReportResultModel<LicenseeWinLoss> GetAgentWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList);

        SortableReportResultModel<LicenseeWinLoss> GetCustomerWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList);

        SortableReportResultModel<LicenseeWinLoss> GetSummaryWinloss(
            LicenseeWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList);
    }
}