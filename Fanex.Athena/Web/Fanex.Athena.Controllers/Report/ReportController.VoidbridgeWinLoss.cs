// -----------------------------------------------------------------------
// <copyright file="ReportController.VoidbridgeWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// VoidBridge report controller.
    /// </summary>
    public partial class ReportController
    {        
        /// <summary>
        /// VoidBridge win loss main action.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult VoidbridgeWinloss()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var productList = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            var product = productList.Where(p => p.ProductCode == Product.VB).SingleOrDefault();

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
                ProductName = product.ProductName
            };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new ThirdPartyWinLossViewModel
            {
                ShowEmptyContent = true,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList
            };

            return View("~/Views/Report/VoidbridgeWinLoss/Main.cshtml", model);
        }
    }
}