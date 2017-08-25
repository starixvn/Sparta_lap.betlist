namespace Fanex.Athena.Api.Report
{
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportSubLicenseePerformance)]
    public class SubLicenseePerformanceReportController : ApiReportBaseController
    {
        private readonly ISubLicenseePerformanceService _subLicenseePerformanceService;

        public SubLicenseePerformanceReportController()
        {
            _subLicenseePerformanceService = new SubLicenseePerformanceService();
        }

        public SubLicenseePerformanceReportController(
            ISessionService sessionService,
            IProductService productService,
            ISubLicenseePerformanceService subLicenseePerformanceService)
            : base(sessionService, productService)
        {
            _subLicenseePerformanceService = subLicenseePerformanceService;
        }

        [HttpPost]
        public dynamic Index(SubLicenseePerformanceMetaModel metaModel)
        {
            return null;
        }
    }
}