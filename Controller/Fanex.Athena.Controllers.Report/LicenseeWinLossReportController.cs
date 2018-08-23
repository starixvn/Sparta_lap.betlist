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
    using Session.Model;

    [FunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss)]
    public class LicenseeWinLossReportController : ReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        private readonly LicenseeSession _licenseeSession;

        private readonly UserSession _userSession;

        public LicenseeWinLossReportController()
        {
            _reportSettingService = new ReportSettingService();
            _licenseeSession = SessionService.LicenseeSession;
            _userSession = SessionService.UserSession;
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
            _licenseeSession = SessionService.LicenseeSession;
            _userSession = SessionService.UserSession;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var reportSetting = _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss);
            var selectedDate = DateTimeHelper.GetMinDate(reportSetting.MaxDate, DateTime.Now);
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                _licenseeSession.LicenseeId,
                _userSession.Language,
                FunctionNames.LicenseeWinLoss);
            var metaModel = new LicenseeWinLossMetaModel
            {
                FromDate = selectedDate,
                ToDate = selectedDate,
                LicenseeId = _licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                DrawFilter = DrawFilterType.Normal,
                ReportBy = ReportByType.Daily,
                ProductOptions = ToProductOptions(productAccess.ProductList)
            };

            SetConvetedUSDCurrencyId(metaModel);

            if (IsFakeLicensee())
            {
                return MapToFakeView(reportSetting, metaModel);
            }

            return MapToView(reportSetting, metaModel, productAccess);
        }

        private ActionResult MapToView(ReportSetting reportSetting, LicenseeWinLossMetaModel metaModel, ProductAccess productAccess)
        {
            var model = new LicenseeWinLossViewModel
            {
                Param = metaModel,
                CurrencyList = _userSession.CurrencyList,
                IsAllowedCustomerCommission = _licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                ReportSetting = reportSetting
            };
            return View("~/Views/Report/LicenseeWinLoss/Main.cshtml", model);
        }

        private ActionResult MapToFakeView(ReportSetting reportSetting, LicenseeWinLossMetaModel metaModel)
        {
            var allBaseCurrencies = new Dictionary<int, string>
            {
                { BaseCurrencyType.Eur, "EUR" },
                { BaseCurrencyType.Usd, "USD" },
                { BaseCurrencyType.Gbp, "GBP" }
            };

            var model = new SpecialLicenseeWinLossViewModel
            {
                Param = metaModel,
                CurrencyList = _userSession.CurrencyList,
                IsAllowedCustomerCommission = _licenseeSession.IsAllowedCustomerComm,
                ReportSetting = reportSetting,
                BaseCurrencyList = string.IsNullOrWhiteSpace(reportSetting.BaseCurrencies)
                    ? allBaseCurrencies
                    : allBaseCurrencies.Where(x => reportSetting.BaseCurrencies.Split(',').Contains(x.Value)).ToDictionary(x => x.Key, x => x.Value)
            };
            return View("~/Views/Report/SpecialLicenseeWinLoss/Main.cshtml", model);
        }
    }
}