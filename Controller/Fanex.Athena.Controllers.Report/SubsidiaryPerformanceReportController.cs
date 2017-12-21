using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Service.Report;
using Fanex.Athena.Service.Report.Interfaces;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ReportBaseController
    {
        private readonly ISubsidiaryPerformanceService _service;

        public SubsidiaryPerformanceReportController() : this(null)
        {
        }

        public SubsidiaryPerformanceReportController(ISubsidiaryPerformanceService service)
        {
            _service = service ?? new SubsidiaryPerformanceService();
        }

        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.SubsidiaryPerformance);

            var currencyIdList = string.Join(",", userSession.CurrencyList.Select(x => x.Key));
            var subsidiaryList = _service.GetSubsidiaryList(licenseeSession.LicenseeName, currencyIdList);

            var model = new SubsidiaryPerformanceViewModel
            {
                ProductOptionList = productAccess.ProductList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = true
                }).ToList(),

                SubsidiaryList = subsidiaryList.Any() ? subsidiaryList.Select(x => new CheckedItem
                {
                    ID = x.Key,
                    Name = x.Value,
                    Checked = true
                }).ToList() : new List<CheckedItem>()
            };

            return View("~/Views/Report/SubsidiaryPerformance/Main.cshtml", model);
        }
    }
}