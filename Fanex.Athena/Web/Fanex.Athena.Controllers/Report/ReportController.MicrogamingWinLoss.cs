// -----------------------------------------------------------------------
// <copyright file="ReportController.MicrogamingWinLoss.cs" company="Nexcel Solution">
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
    /// TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult MicrogamingWinloss()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var products = _productService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language).ProductList;

            var product = products.Where(p => p.ProductCode == Product.MGC).SingleOrDefault();

            if (product == null)
            {
                return this.Redirect("~/Utility/AccessDenied");
            }

            var metaModel = new MicroGamingWinLossMetaModel
            {
                IsAllowReportby = true,
                FromDate = date,
                ToDate = date,
                LicenseeId = licenseeSession.LicenseeId,
                IsExcludedTestData = true,
                ProductId = product.ProductID,
                ReportBy = 1,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName
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