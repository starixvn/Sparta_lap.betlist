// -----------------------------------------------------------------------
// <copyright file="IThirdPartyWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IThirdPartyWinLossDataAccess
    {
        SortableReportResultModel<ThirdPartyWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<ThirdPartyWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel);

        SortableReportResultModel<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel);
    }
}