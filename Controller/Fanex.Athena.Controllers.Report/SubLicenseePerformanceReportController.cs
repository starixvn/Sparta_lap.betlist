using System.Web.Mvc;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.ReportSubLicenseePerformance)]
    public class SubLicenseePerformanceReportController : BaseController
    {
        public ActionResult Index()
        {
            return View("~/Views/Report/SubLicenseePerformance/Main.cshtml");
        }
    }
}
