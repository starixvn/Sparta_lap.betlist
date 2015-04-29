// -----------------------------------------------------------------------
// <copyright file="ReportController.GoldDeluxeWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// Gold deluxe win loss main action.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult GoldDeluxeWinLoss()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var metaModel = new GoldDeluxeWinLossMetaModel
                {
                    FromDate = date.AddDays(-1),
                    ToDate = date.AddDays(-1),
                    LicenseeId = licenseeSession.LicenseeId,
                    ReportBy = 1,
                    IsExcludedTestData = true
                };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new GoldDeluxeWinLossViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = userSession.CurrencyList
                };

            return View("~/Views/Report/GoldDeluxeWinLoss/Main.cshtml", model);
        }
    }
}