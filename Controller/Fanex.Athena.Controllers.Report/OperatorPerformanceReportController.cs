using System.Web.Mvc;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.OperatorPerformanceReport)]
    public class OperatorPerformanceReportController : BaseController
    {
        public ActionResult Index()
        {
            return View("~/Views/Report/OperatorPerformance/Main.cshtml");
        }
    }
}
