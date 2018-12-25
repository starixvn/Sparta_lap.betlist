namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.ResultModel;
    using Fanex.Athena.Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface IExchangeWinLossDataAccess
    {
        ReportResultModel<ExchangeWinLoss> GetMainWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        ReportResultModel<ExchangeWinLoss> GetLicenseeWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);

        ReportResultModel<ExchangeWinLoss> GetCustomerWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}