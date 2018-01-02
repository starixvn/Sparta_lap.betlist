// -----------------------------------------------------------------------
// <copyright file="ReportController.MicrogamingWinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportMicrogamingWinloss)]
    public class MicrogamingWinlossReportController : ReportBaseController
    {
        public MicrogamingWinlossReportController()
        {
        }

        public MicrogamingWinlossReportController(
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
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var products = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            if (!products.Any(p => p.ProductCode == Product.MGCLC || p.ProductCode == Product.MGCRNG))
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            var metaModel = new ThirdPartyWinLossMetaModel
            {
                FromDate = date,
                ToDate = date,
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                ReportBy = 1
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new MicroGamingWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList
            };

            return View("~/Views/Report/MicrogamingWinLoss/Main.cshtml", model);
        }
    }
}