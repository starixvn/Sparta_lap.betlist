// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;

    // [FunctionAuthorization(PermissionKeys.ReportGoldDeluxeWinloss)]
    public partial class ColossusBetsWinLossReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;
            var productList = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            var product = productList.SingleOrDefault(p => p.ProductCode == Product.GD);

            if (product == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            var metaModel = new ThirdPartyWinLossMetaModel
            {
                FromDate = date.AddDays(-1),
                ToDate = date.AddDays(-1),
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
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList
            };

            return View("~/Views/Report/ColossusBetsWinLoss/Main.cshtml", model);
        }
    }
}