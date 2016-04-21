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
        public ActionResult AgCasinoBalance()
        {
            var licenseeSession = this.SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var toDate = DateTime.Now.AddDays(-2);
            
            var metaModel = new AgCasinoBalanceMetaModel
                {
                    FromDate = new DateTime(toDate.Year, toDate.Month, 1),
                    ToDate = toDate,
                    LicenseeId = licenseeSession.LicenseeId,
                    IsExcludedTestData = true
                };

            SetConvetedUSDCurrencyId(metaModel);

            var model = new AgCasinoBalanceViewModel
                {
                    ShowEmptyContent = true,
                    Param = metaModel,
                    CurrencyList = userSession.CurrencyList
                };

            return View("~/Views/Report/AgCasinoBalance/Main.cshtml", model);
        }
    }
}