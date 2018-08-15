namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Helper;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    public class SpecialLicenseeWinLossReportController : ReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        public SpecialLicenseeWinLossReportController()
        {
            _reportSettingService = new ReportSettingService();
        }

        public SpecialLicenseeWinLossReportController(
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

        public ActionResult Index()
        {
            var viewModel = CreateViewModel();
            return View("~/Views/Report/SpecialLicenseeWinLoss/Main.cshtml", viewModel);
        }

        private SpecialLicenseeWinLossViewModel CreateViewModel()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = ProductService.GetSingleWalletProductAccess(
                    licenseeSession.LicenseeId,
                    userSession.Language,
                    FunctionNames.SpecialLicenseeWinLoss);
            var reportSetting = _reportSettingService.GetSetting(PermissionKeys.ReportSpecialLicenseeWinLoss) ?? new ReportSetting
            {
                MinDate = AppSettings.HistoryDate,
                MaxDate = DateTime.Today
            };
            var selectedDate = DateTimeHelper.GetMinDate(reportSetting.MaxDate, DateTime.Today);

            var allBaseCurrencies = new Dictionary<int, string>
            {
                { BaseCurrencyType.Eur, "EUR" },
                { BaseCurrencyType.Usd, "USD" },
                { BaseCurrencyType.Gbp, "GBP" }
            };

            return new SpecialLicenseeWinLossViewModel
            {
                FromDate = selectedDate,
                ToDate = selectedDate,
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductOptionList = ToProductOptions(productAccess.ProductList),
                LicenseeId = licenseeSession.LicenseeId,
                CurrencyId = GetConvetedUSDCurrencyId(licenseeSession.LicenseeId),
                BaseCurrencyList = string.IsNullOrWhiteSpace(reportSetting.BaseCurrencies)
                    ? allBaseCurrencies
                    : allBaseCurrencies.Where(x => reportSetting.BaseCurrencies.Split(',').Contains(x.Value)).ToDictionary(x => x.Key, x => x.Value),
                ReportSetting = reportSetting
            };
        }
    }
}