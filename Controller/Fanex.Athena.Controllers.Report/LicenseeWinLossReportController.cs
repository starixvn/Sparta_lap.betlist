// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Extension.Helper;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
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
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.LicenseeWinLoss);
            var reportSetting = _reportSettingService.GetSetting(licenseeSession.LicenseeName, PermissionKeys.ReportLicenseeWinLoss);
            var selectedDate = DateTimeHelper.GetMinDate(reportSetting.MaxDate, DateTime.Now);
            var metaModel = new LicenseeWinLossMetaModel
            {
                FromDate = selectedDate,
                ToDate = selectedDate,
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                DrawFilter = DrawFilterType.Normal,
                ReportBy = ReportByType.Daily,
                ProductOptions = ToProductOptions(productAccess.ProductList)
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new LicenseeWinLossViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
                IsFakeLicensee = IsFakeLicensee(),
                ReportSetting = reportSetting
            };
            return View("~/Views/Report/LicenseeWinLoss/Main.cshtml", model);
        }
    }
}