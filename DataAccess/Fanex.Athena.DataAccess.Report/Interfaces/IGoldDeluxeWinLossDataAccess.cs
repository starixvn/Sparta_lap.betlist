namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IGoldDeluxeWinLossDataAccess
    {
        ReportResultModel<GoldDeluxeWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel);

        ReportResultModel<GoldDeluxeWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel);

        ReportResultModel<GoldDeluxeWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel);
    }
}