namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;
    using Constant.DataDomain;
    using Model.Core.DataDomain;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportSubsidiaryPerformance)]
    public class SubsidiaryPerformanceReportController : ApiReportBaseController
    {
        private readonly ISubsidiaryService _subsidiaryService;

        private readonly ISubsidiaryPerformanceService _subsidiaryPerformanceService;

        public SubsidiaryPerformanceReportController()
        {
            _subsidiaryService = new SubsidiaryService();
            _subsidiaryPerformanceService = new SubsidiaryPerformanceService();
        }

        public SubsidiaryPerformanceReportController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            ISubsidiaryService subsidiaryService,
            ISubsidiaryPerformanceService subLicenseePerformanceService)
            : base(sessionService, productService, langService)
        {
            _subsidiaryService = subsidiaryService;
            _subsidiaryPerformanceService = subLicenseePerformanceService;
        }

        [HttpPost]
        public SubsidiaryResultViewModel Index(SubsidiaryPerformanceMetaModel metaModel)
        {
            CheckValidProducts(FunctionNames.SubsidiaryPerformance, metaModel.ProductCodeList);
            var isSubsidiary = SessionService.UserSession.UserLevel == UserLevelType.Subsidiary;
            var isValidSubsidiaryList = isSubsidiary || !string.IsNullOrWhiteSpace(metaModel.SubsidiaryList);
            if (!isValidSubsidiaryList || !metaModel.IsValid)
            {
                return new SubsidiaryResultViewModel
                {
                    Data = new List<SubsidiaryPerformanceDataModel>()
                };
            }

            var licenseeName = SessionService.LicenseeSession.LicenseeName;
            metaModel.LicenseeName = licenseeName;
            string agentListXml = null;
            if (isSubsidiary)
            {
                agentListXml = SessionService.GenerateAgentListXml(isExcludeTestData: metaModel.IsExcludeTest);
            }
            else
            {
                var agentIdListByCurrency = _subsidiaryService.GetCurrencyAgentList(metaModel.SubsidiaryList, metaModel.IsExcludeTest);
                agentListXml = SessionService.GenerateAgentListXml(agentIdListByCurrency);
            }

            metaModel.AgentListXml = agentListXml;
            return _subsidiaryPerformanceService.GetData(metaModel);
        }

        [HttpPost]
        public dynamic Detail(SubsidiaryPerformanceMetaModel metaModel)
        {
            if (metaModel.IsValid)
            {
                CheckValidProducts(FunctionNames.SubsidiaryPerformance, metaModel.ProductCodeList);
                var licenseeName = SessionService.LicenseeSession.LicenseeName;
                metaModel.LicenseeName = licenseeName;
                var agentIdListByCurrency = _subsidiaryService.GetCurrencyAgentList(metaModel.SubsidiaryId, metaModel.IsExcludeTest);

                if (agentIdListByCurrency.Any())
                {
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(agentIdListByCurrency);

                    var result = _subsidiaryPerformanceService.GetDetailData(metaModel);

                    return new
                    {
                        Data = result.DataList,
                        Total = result.SummaryTotal,
                        GrandTotal = result.TotalData
                    };
                }
            }

            return new
            {
                Data = new List<SubsidiaryPerformanceDetail>()
            };
        }
    }
}