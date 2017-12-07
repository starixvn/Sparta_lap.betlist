namespace Fanex.Athena.Service.Report.Interfaces
{
    using Fanex.Athena.Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public interface IExchangeWinLossService
    {
        ReportResultModel<ExchangeWinLoss> GetData(ExchangeWinLossMetaModel metaModel);
    }
}