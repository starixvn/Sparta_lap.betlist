namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Report.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    public class CasinoTurnoverReportController : ReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        public CasinoTurnoverReportController()
        {
            _reportSettingService = new ReportSettingService();
        }

        public CasinoTurnoverReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            IReportSettingService reportSettingService)
        : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
            _reportSettingService = reportSettingService ?? new ReportSettingService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var productAccess = ProductService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.CasinoTurnover);
            var setting = _reportSettingService.GetSetting(PermissionKeys.ReportCasinoTurnover);

            var viewModel = new CasinoTurnoverViewModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                CurrencyList = SessionService.UserSession.CurrencyList,
                BaseCurrencies = SystemConstants.NewBaseCurrencyList,
                ProductOptions = ToProductOptions(productAccess.ProductList),
                MinDate = setting.MinDate,
                MaxDate = setting.MaxDate
            };

            return View("~/Views/Report/CasinoTurnover/Main.cshtml", viewModel);
        }
    }
}