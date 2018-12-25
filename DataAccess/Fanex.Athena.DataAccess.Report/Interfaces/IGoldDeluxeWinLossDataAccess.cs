namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IGoldDeluxeWinLossDataAccess
    {
        ReportResultModel<GoldDeluxeWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        ReportResultModel<GoldDeluxeWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList );

        ReportResultModel<GoldDeluxeWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}