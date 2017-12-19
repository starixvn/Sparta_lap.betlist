namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Core.ResultModel;
    using Fanex.Athena.Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface IExchangeWinLossDataAccess
    {
        ReportResultModel<ExchangeWinLoss> GetMainWinloss(ExchangeWinLossMetaModel metaModel);

        ReportResultModel<ExchangeWinLoss> GetLicenseeWinloss(ExchangeWinLossMetaModel metaModel);

        ReportResultModel<ExchangeWinLoss> GetCustomerWinloss(ExchangeWinLossMetaModel metaModel);
    }
}