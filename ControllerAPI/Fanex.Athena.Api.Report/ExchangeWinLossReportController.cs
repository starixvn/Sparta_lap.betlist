namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossReportController : ApiReportBaseController
    {
        private readonly IExchangeWinLossDataAccess _dataAccess;

        public ExchangeWinLossReportController()
        {
            _dataAccess = new ExchangeWinLossDataAccess();
        }

        public ExchangeWinLossReportController(
            ISessionService sessionService,
            IProductService productService,
            IExchangeWinLossDataAccess dataAccess)
            : base(sessionService, productService)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost]
        public dynamic Index(ExchangeWinLossMetaModel metaModel)
        {
            if (metaModel.IsValid && IsValidRequest(metaModel, metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);
                return metaModel.Level == 0 ? _dataAccess.GetMainWinloss(metaModel) :
                                SessionService.LicenseeSession.LicenseeId != LicenseeIdentity.OneTwoBet || metaModel.Level == 2
                               ? _dataAccess.GetCustomerWinloss(metaModel)
                               : _dataAccess.GetLicenseeWinloss(metaModel);
            }

            return new ReportResultModel<ExchangeWinLoss>
            {
                DataList = Enumerable.Empty<ExchangeWinLoss>(),
                TotalDataList = Enumerable.Empty<ExchangeWinLoss>(),
                SummaryTotal = new Dictionary<string, ExchangeWinLoss>()
            };
        }
    }
}