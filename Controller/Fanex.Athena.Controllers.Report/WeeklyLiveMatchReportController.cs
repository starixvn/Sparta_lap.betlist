// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchReportController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportWeeklyLiveMatches)]
    public class WeeklyLiveMatchReportController : ReportBaseController
    {
        public WeeklyLiveMatchReportController()
        {
        }

        public WeeklyLiveMatchReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService)
        : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var metaModel = new WeeklyLiveMatchMetaModel();
            return View("~/Views/Report/WeeklyLiveMatch/Main.cshtml", metaModel);
        }
    }
}