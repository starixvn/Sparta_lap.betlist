// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Web.ActionFilter;

    public class WeeklyLiveMatchReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var metaModel = new WeeklyLiveMatchMetaModel();
            return View("~/Views/Report/WeeklyLiveMatch/Main.cshtml", metaModel);
        }
    }
}