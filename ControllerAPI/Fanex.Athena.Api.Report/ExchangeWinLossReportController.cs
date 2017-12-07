namespace Fanex.Athena.Api.Report
{
    using System.Web.Http;
    using Model.Report.MetaModel;
    using Service.Report;
    using Service.Report.Interfaces;

    public class ExchangeWinLossReportController : ApiReportBaseController
    {
        private readonly IExchangeWinLossService _service;

        public ExchangeWinLossReportController()
        {
            _service = new ExchangeWinLossService();
        }

        [HttpPost]
        public dynamic Index(ExchangeWinLossMetaModel metaModel)
        {
            return _service.GetData(metaModel);
        }
    }
}