using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Service.Core.Common.Interfaces;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Athena.Service.Report;
using Fanex.Athena.Service.Report.Interfaces;
using Fanex.Athena.Web.ActionFilter;

namespace Fanex.Athena.Controllers.Report
{
    [FunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ReportBaseController
    {
        private readonly ISubsidiaryPerformanceService _service;

        public SubsidiaryPerformanceReportController() : this(null, null, null, null, null)
        {
        }

        public SubsidiaryPerformanceReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            ISubsidiaryPerformanceService service)
            : base(languageService, sessionService, appSetting, productService)
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

            var currencyIdList = userSession.CurrencyList.Select(x => x.Key);
            var subsidiaryList = _service.GetOperatorList(licenseeSession.LicenseeName, currencyIdList);
            var operatorNameList = subsidiaryList.Any() ? subsidiaryList.Select(x => x.Value).Distinct() : new List<string>();

            var model = new SubsidiaryPerformanceViewModel
            {
                ProductOptionList = productAccess.ProductList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = true
                }).ToList(),

                SubsidiaryList = operatorNameList.Select(x => new CheckedItem
                {
                    Name = x,
                    Checked = true
                }).ToList()
            };

            return View("~/Views/Report/SubsidiaryPerformance/Main.cshtml", model);
        }
    }
}