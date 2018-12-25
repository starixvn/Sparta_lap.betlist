namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using System.Collections.Generic;

    public interface IAgCasinoBalanceDataAccess
    {
        SortableReportResultModel<AgCasinoBalance> GetAgentBalanceDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        SortableReportResultModel<AgCasinoBalance> GetCustomerBalanceDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        SortableReportResultModel<AgCasinoBalance> GetSummaryBalance(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}