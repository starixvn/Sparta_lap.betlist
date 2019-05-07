namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IGoldDeluxeWinLossDataAccess
    {
        Task<ReportResultModel<GoldDeluxeWinLoss>> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<ReportResultModel<GoldDeluxeWinLoss>> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<ReportResultModel<GoldDeluxeWinLoss>> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}