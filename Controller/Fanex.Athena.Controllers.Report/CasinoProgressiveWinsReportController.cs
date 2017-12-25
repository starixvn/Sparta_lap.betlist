// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsReportController.cs" company="Microsoft">
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

    [FunctionAuthorization(PermissionKeys.ReportCasinoProgressiveWins)]
    public class CasinoProgressiveWinsReportController : ReportBaseController
    {
        public CasinoProgressiveWinsReportController()
        {
        }

        public CasinoProgressiveWinsReportController(
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
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.CasinoProgressiveWins);
            var metaModel = new BaseReportMetaModel
            {
                FromDate = DateTime.Now.AddMonths(-3),
                ToDate = DateTime.Now,
                LicenseeId = licenseeSession.LicenseeId,
                ProductOptions = ToProductOptions(productAccess.ProductList)
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new CasinoProgressiveWinsViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                ProductAccess = productAccess,
            };

            return View("~/Views/Report/CasinoProgressiveWins/Main.cshtml", model);
        }
    }
}