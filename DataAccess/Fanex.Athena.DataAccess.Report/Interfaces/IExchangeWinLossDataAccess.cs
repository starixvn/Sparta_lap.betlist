namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.ResultModel;
    using Fanex.Athena.Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface IExchangeWinLossDataAccess
    {
        Task<ReportResultModel<ExchangeWinLoss>> GetMainWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<ReportResultModel<ExchangeWinLoss>> GetLicenseeWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<ReportResultModel<ExchangeWinLoss>> GetCustomerWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}