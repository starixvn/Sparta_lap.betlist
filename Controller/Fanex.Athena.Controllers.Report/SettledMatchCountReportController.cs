// -----------------------------------------------------------------------
// <copyright file="ReportController.SettledMatchCount.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Report;
    using Model.Core.DataDomain;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportSettledMatchCount)]
    public class SettledMatchCountReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var isShowAll = AppSettings.IsTesting;
            var producAccess = ProductService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var settledMatchCountService = new SettledMatchCountService();

            var model = settledMatchCountService.GetDataList(isShowAll, licenseeId, producAccess);

            return View("~/Views/Report/SettledMatchCount/Main.cshtml", model);
        }
    }
}