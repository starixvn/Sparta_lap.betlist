// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportGoldDeluxeWinloss)]
    public class GoldDeluxeWinLossReportController : ReportBaseController
    {
        public GoldDeluxeWinLossReportController()
        {
        }

        public GoldDeluxeWinLossReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService)
            : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now.AddDays(-1);
            var productList = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            var product = productList.SingleOrDefault(p => p.ProductCode == Product.GD);

            if (product == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            var metaModel = new ThirdPartyWinLossMetaModel
            {
                FromDate = date,
                ToDate = date,
                LicenseeId = licenseeSession.LicenseeId,
                ReportBy = 1,
                IsExcludedTestData = true,
                ProductId = product.ProductID,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new ThirdPartyWinLossViewModel
            {
                Param = metaModel,
                CurrencyList = userSession.CurrencyList
            };

            return View("~/Views/Report/GoldDeluxeWinLoss/Main.cshtml", model);
        }
    }
}