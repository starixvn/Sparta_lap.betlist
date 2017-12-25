// -----------------------------------------------------------------------
// <copyright file="ReportController.ActiveCustomer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportActiveCustomerSummary)]
    public class ActiveCustomerSummaryReportController : ReportBaseController
    {
        public ActiveCustomerSummaryReportController()
        {
        }

        public ActiveCustomerSummaryReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IClientViewHelper clientViewHelper)
        : base(languageService, sessionService, appSetting, productService, clientViewHelper)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetProductAccessForActiveCustomerSummary(licenseeSession.LicenseeId, userSession.Language);
            var defaultDate = DateTime.Now.AddDays(-1);
            var metaModel = new ActiveCustomerMetaModel
            {
                FromDate = defaultDate,
                ToDate = defaultDate,
                LicenseeId = licenseeSession.LicenseeId,
                ProductOptions = ToProductOptions(productAccess.ProductList)
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new ActiveCustomerViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
            };

            return View("~/Views/Report/ActiveCustomerSummary/Main.cshtml", model);
        }
    }
}