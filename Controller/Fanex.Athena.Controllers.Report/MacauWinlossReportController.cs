// -----------------------------------------------------------------------
// <copyright file="ReportController.MacauWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;

    public class MacauWinlossReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var productList = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            // Use product code of Vegas Games to Macau.
            var product = productList.Where(p => p.ProductCode == Product.VG).SingleOrDefault();

            if (product == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            var metaModel = new ThirdPartyWinLossMetaModel
            {
                IsAllowReportby = true,
                FromDate = date.Day == 1 ? date.AddMonths(-1) : new DateTime(date.Year, date.Month, 1),
                ToDate = date.AddDays(-1),
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                ProductId = product.ProductID,
                ReportBy = 1,
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

            return View("~/Views/Report/MacauWinLoss/Main.cshtml", model);
        }
    }
}