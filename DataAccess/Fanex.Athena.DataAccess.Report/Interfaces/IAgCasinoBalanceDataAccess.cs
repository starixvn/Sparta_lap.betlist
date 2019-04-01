namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IAgCasinoBalanceDataAccess
    {
        Task<SortableReportResultModel<AgCasinoBalance>> GetAgentBalanceDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<SortableReportResultModel<AgCasinoBalance>> GetCustomerBalanceDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<SortableReportResultModel<AgCasinoBalance>> GetSummaryBalance(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}