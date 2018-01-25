namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Model.Report.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ReportBaseController
    {
        private readonly ISubsidiaryPerformanceService _service;

        public SubsidiaryPerformanceReportController() : this(null, null, null, null, null, null)
        {
        }

        public SubsidiaryPerformanceReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            ISubsidiaryPerformanceService service)
            : base(languageService, sessionService, appSetting, clientViewHelper, productService)
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
            var subsidiaryList = _service.GetOperatorList(licenseeSession.LicenseeName);
            var operatorNameList = subsidiaryList.Any() ? subsidiaryList.Select(x => x.OperatorName).Distinct() : new List<string>();

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