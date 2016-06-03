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
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    public class MicrogamingWinlossReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
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

            var metaModel = new MicroGamingWinLossMetaModel
            {
                IsAllowReportby = true,
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