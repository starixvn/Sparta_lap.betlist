// -----------------------------------------------------------------------
// <copyright file="ReportController.SettledMatchCount.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportSettledMatchCount)]
    public class SettledMatchCountReportController : ReportBaseController
    {
        private readonly ISettledMatchCountService _settledMatchCountService;

        public SettledMatchCountReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            ISettledMatchCountService settledMatchCountService)
            : base(languageService, sessionService, appSetting, productService)
        {
            _settledMatchCountService = settledMatchCountService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var producAccess = ProductService.GetProductAccess(licenseeId, SessionService.UserSession.Language);
            var model = await _settledMatchCountService.GetDataList(AppSettings.IsShowAllDataForSettleMatchCountReport, licenseeId, producAccess).ConfigureAwait(false);

            return View("~/Views/Desktop/Report/SettledMatchCount/Main.cshtml", model);
        }
    }
}