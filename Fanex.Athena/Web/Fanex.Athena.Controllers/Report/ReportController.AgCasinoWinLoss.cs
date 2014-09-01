// -----------------------------------------------------------------------
// <copyright file="ReportController.AgCasinoWinLoss.cs" company="Microsoft">
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
    ///     TODO: Update summary.
    /// </summary>
    public partial class ReportController
    {
        /// <summary>
        /// Default licenseeWL action.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult AgCasinoWinLoss()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var date = DateTime.Now;

            var metaModel = new AgCasinoWinLossMetaModel
                {
                    FromDate = date.Day == 1 ? date.AddMonths(-1) : new DateTime(date.Year, date.Month, 1),
                    ToDate = date.AddDays(-1),
                    LicenseeId = licenseeSession.LicenseeId,
                    IsExcludedTestData = true
                };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new AgCasinoWinLossViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = userSession.CurrencyList
                };

            return View("~/Views/Report/AgCasinoWinLoss/Main.cshtml", model);
        }
    }
}