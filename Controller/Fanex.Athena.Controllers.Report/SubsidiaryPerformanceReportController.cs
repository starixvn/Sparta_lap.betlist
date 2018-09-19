namespace Fanex.Athena.Controllers.Report
{
    using System.Linq;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using Model.Report.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ReportBaseController
    {
        private readonly ISubsidiaryService _subsidiaryService;

        public SubsidiaryPerformanceReportController() : this(null, null, null, null, null, null)
        {
        }

        public SubsidiaryPerformanceReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            ISubsidiaryService subsidiaryService)
            : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
            _subsidiaryService = subsidiaryService ?? new SubsidiaryService();
        }

        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = this.ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.SubsidiaryPerformance);
            var subsidiaryNames = userSession.UserLevel == UserLevelType.Subsidiary ? null : _subsidiaryService.GetSubsidiaryNames();

            var model = new SubsidiaryPerformanceViewModel
            {
                ProductOptionList = productAccess.ProductList.Select(i => new ProductOption
                {
                    Code = i.ProductCode,
                    Name = i.ProductName,
                    Status = true
                }).ToList(),
                SubsidiaryList = subsidiaryNames?.Select(x => new CheckedItem
                {
                    Name = x,
                    Checked = true
                }).ToList()
            };

            return View("~/Views/Report/SubsidiaryPerformance/Main.cshtml", model);
        }
    }
}