// -----------------------------------------------------------------------
// <copyright file="ReportController.GoldDeluxeWinLoss.cs" company="Microsoft">
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

    [FunctionAuthorization(PermissionKeys.ReportGoldDeluxeWinloss)]
    public partial class GoldDeluxeWinLossReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
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