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

    /// <summary>
    /// VoidBridge report controller.
    /// </summary>
    public partial class ReportController
    {
        private const int ProductId = 12;
        private const bool IsAllowReportby = true;
        private const string ProductCode = Product.VB;

        /// <summary>
        /// VoidBridge win loss main action.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult VoidbridgeWinloss()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var productList = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            var productName = productList.Where(p => p.ProductCode == ProductCode).SingleOrDefault().ProductName;

            ViewBag.IsAllowReportby = IsAllowReportby;
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                FromDate = date.Day == 1 ? date.AddMonths(-1) : new DateTime(date.Year, date.Month, 1),
                ToDate = date.AddDays(-1),
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                ProductId = ProductId,
                ReportBy = 1,
                ProductName = productName
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