// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ReportWeeklyLiveMatches)]
    public class WeeklyLiveMatchReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new WeeklyLiveMatchMetaModel();
            return View("~/Views/Report/WeeklyLiveMatch/Main.cshtml", metaModel);
        }
    }
}