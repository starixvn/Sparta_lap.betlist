// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class WeeklyLiveMatchReportController : ReportBaseController
    {
        [HttpGet]
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var metaModel = new WeeklyLiveMatchMetaModel();
            var model = new WeeklyLiveMatchViewModel()
            {
                Param = metaModel
            };

            return View("~/Views/Report/WeeklyLiveMatch/Main.cshtml", model);
        }
    }
}