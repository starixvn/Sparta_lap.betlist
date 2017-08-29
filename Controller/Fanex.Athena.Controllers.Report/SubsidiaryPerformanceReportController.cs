using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ReportBaseController
    {
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var model = new SubsidiaryPerformanceViewModel
            {
                ProductOptionList = productAccess.ProductList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = i.ProductCode == Product.SB
                }).ToList(),

                SubsidiaryList = GetSubsidiaryList()
            };

            return View("~/Views/Report/SubsidiaryPerformance/Main.cshtml", model);
        }

        private static IDictionary<int, string> GetSubsidiaryList()
        {
            return new Dictionary<int, string>
            {
                { 1, "New York"},
                { 2, "India"},
                { 3, "London"},
                { 4, "Dubai"}
            };
        }
    }
}