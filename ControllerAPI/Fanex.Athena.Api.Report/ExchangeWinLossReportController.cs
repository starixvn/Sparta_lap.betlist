namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Extension.Helper;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossReportController : ApiReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        private readonly IExchangeWinLossDataAccess _dataAccess;

        public ExchangeWinLossReportController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IReportSettingService reportSettingService,
            IExchangeWinLossDataAccess dataAccess)
            : base(sessionService, productService, langService)
        {
            _reportSettingService = reportSettingService;
            _dataAccess = dataAccess;
        }

        [HttpPost]
        public dynamic Index(ExchangeWinLossMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            if (metaModel.IsValid && IsValidParams(metaModel) && IsValidRequest(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.ExcludeTestData);
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                return metaModel.Level == 0 ? _dataAccess.GetMainWinloss(metaModel, currencyList) :
                                metaModel.LicenseeId != LicenseeIdentity.OneTwoBet || metaModel.Level == 2
                               ? _dataAccess.GetCustomerWinloss(metaModel, currencyList)
                               : _dataAccess.GetLicenseeWinloss(metaModel, currencyList);
            }

            return new ReportResultModel<ExchangeWinLoss>
            {
                DataList = Enumerable.Empty<ExchangeWinLoss>(),
                TotalDataList = Enumerable.Empty<ExchangeWinLoss>(),
                SummaryTotal = new Dictionary<string, ExchangeWinLoss>()
            };
        }

        private bool IsValidParams(ExchangeWinLossMetaModel metaModel)
        {
            var setting = _reportSettingService.GetSetting(PermissionKeys.ReportExchangeWinLoss);
            if (setting == null)
            {
                return true;
            }

            return DateTimeHelper.IsBetween(metaModel.FromDate, setting.MinDate, setting.MaxDate) &&
                DateTimeHelper.IsBetween(metaModel.ToDate, setting.MinDate, setting.MaxDate);
        }
    }
}