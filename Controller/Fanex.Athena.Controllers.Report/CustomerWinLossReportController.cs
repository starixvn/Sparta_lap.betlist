// <copyright file="ReportController.CustomerWinloss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportCustomerWinLoss)]
    public class CustomerWinLossReportController : ReportBaseController
    {
        public CustomerWinLossReportController()
        {
        }

        public CustomerWinLossReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService)
            : base(languageService, sessionService, appSetting, productService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return GetView(false);
        }

        [HttpGet]
        public ActionResult History()
        {
            return GetView(true);
        }

        private ActionResult GetView(bool isHistory)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new CustomerWinLossMetaModel
            {
                IsExcludedTestData = true,
                FromDate = isHistory ? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1) : DateTime.Today
            };

            SetConvetedUSDCurrencyName(metaModel);
            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CustomerWinLoss);

            metaModel.ProductOptions = ToProductOptions(productAccess.ProductList);
            var model = new CustomerWinLossViewModel
            {
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = userSession.CurrencyList,
                Param = metaModel,
                IsHistory = isHistory,
                ProductAccess = productAccess,
                IsFakeLicensee = IsFakeLicensee()
            };

            return View("~/Views/Report/CustomerWinLoss/Main.cshtml", model);
        }
    }
}