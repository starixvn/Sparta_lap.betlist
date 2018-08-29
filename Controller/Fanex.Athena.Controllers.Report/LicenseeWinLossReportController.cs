// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Helper;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss)]
    public class LicenseeWinLossReportController : ReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        public LicenseeWinLossReportController()
        {
            _reportSettingService = new ReportSettingService();
        }

        public LicenseeWinLossReportController(
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
            if (IsFakeLicensee())
            {
                return MapToFakeView();
            }

            return MapToView();
        }

        private static IDictionary<int, string> GetFakeBaseCurrencies(string baseCurrencySetting)
        {
            var allBaseCurrencies = new Dictionary<int, string>
            {
                { BaseCurrencyType.Eur, "EUR" },
                { BaseCurrencyType.Usd, "USD" },
                { BaseCurrencyType.Gbp, "GBP" }
            };

            return string.IsNullOrWhiteSpace(baseCurrencySetting)
                    ? allBaseCurrencies
                    : allBaseCurrencies.Where(x => baseCurrencySetting.Split(',').Contains(x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        private static DateTime GetSelectedDate(ReportSetting reportSetting)
        {
            return DateTimeHelper.GetMinDate(
                reportSetting.MaxDate,
                reportSetting.TimeZone.HasValue ? DateTime.UtcNow.AddHours(reportSetting.TimeZone.Value) : DateTime.Now);
        }

        private ProductAccess GetProducts(int licenseeId, string lang)
        {
            return ProductService.GetSingleWalletProductAccess(
                licenseeId,
                lang,
                FunctionNames.LicenseeWinLoss);
        }

        private ActionResult MapToView()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var reportSetting = _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss);
            var selectedDate = GetSelectedDate(reportSetting);
            var productAccess = GetProducts(licenseeSession.LicenseeId, userSession.Language);

            var param = new LicenseeWinLossMetaModel
            {
                FromDate = selectedDate,
                ToDate = selectedDate,
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                DrawFilter = DrawFilterType.Normal,
                ReportBy = ReportByType.Daily,
                ProductOptions = ToProductOptions(productAccess.ProductList)
            };
            SetConvetedUSDCurrencyId(param);

            var model = new LicenseeWinLossViewModel
            {
                Param = param,
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                ReportSetting = reportSetting
            };
            return View("~/Views/Report/LicenseeWinLoss/Main.cshtml", model);
        }

        private ActionResult MapToFakeView()
        {
            var userSession = SessionService.UserSession;
            var reportSetting = _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss);
            var selectedDate = GetSelectedDate(reportSetting);
            var productAccess = GetProducts(SessionService.LicenseeSession.LicenseeId, userSession.Language);

            var param = new LicenseeWinLossMetaModel
            {
                FromDate = new DateTime(selectedDate.Year, selectedDate.Month, 1),
                ToDate = selectedDate,
                ReportBy = ReportByType.Daily,
                ProductOptions = ToProductOptions(productAccess.ProductList)
            };
            SetConvetedUSDCurrencyId(param);

            var model = new SpecialLicenseeWinLossViewModel
            {
                Param = param,
                CurrencyList = userSession.CurrencyList,
                ReportSetting = reportSetting,
                BaseCurrencyList = GetFakeBaseCurrencies(reportSetting.BaseCurrencies)
            };

            return View("~/Views/Report/SpecialLicenseeWinLoss/Main.cshtml", model);
        }
    }
}