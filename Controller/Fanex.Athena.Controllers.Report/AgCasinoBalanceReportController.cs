// -----------------------------------------------------------------------
// <copyright file="ReportController.AgCasinoWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportAGCasinoBalance)]
    public class AgCasinoBalanceReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
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