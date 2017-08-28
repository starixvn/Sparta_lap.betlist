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

    [ApiFunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ApiReportBaseController
    {
        private readonly ISubsidiaryPerformanceService _subsidiaryPerformanceService;

        public SubsidiaryPerformanceReportController()
        {
            _subsidiaryPerformanceService = new SubsidiaryPerformanceService();
        }

        public SubsidiaryPerformanceReportController(
            ISessionService sessionService,
            IProductService productService,
            ISubsidiaryPerformanceService subLicenseePerformanceService)
            : base(sessionService, productService)
        {
            _subsidiaryPerformanceService = subLicenseePerformanceService;
        }

        [HttpPost]
        public dynamic Index(SubsidiaryPerformanceMetaModel metaModel)
        {
            return null;
        }
    }
}