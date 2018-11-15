// -----------------------------------------------------------------------
// <copyright file="IThirdPartyWinLossService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IThirdPartyWinLossService
    {
        SortableReportResultModel<ThirdPartyWinLoss> GetAgentWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            string spname);

        SortableReportResultModel<ThirdPartyWinLoss> GetCustomerWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            string spname);

        SortableReportResultModel<ThirdPartyWinLoss> GetSummaryWinloss(
            ThirdPartyWinLossMetaModel metaModel,
            string spname);

        IList<ThirdPartyWinLoss> TotalCalc(IEnumerable<ThirdPartyWinLoss> dataList);
    }
}