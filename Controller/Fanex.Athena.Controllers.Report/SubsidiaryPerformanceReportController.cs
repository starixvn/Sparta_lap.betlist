using System.Web.Mvc;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : BaseController
    {
        public ActionResult Index()
        {
            return View("~/Views/Report/SubsidiaryPerformance/Main.cshtml");
        }
    }
}
