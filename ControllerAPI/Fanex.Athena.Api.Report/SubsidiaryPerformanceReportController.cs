namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;
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
            if (metaModel.IsValid)
            {
                var licenseeName = SessionService.LicenseeSession.LicenseeName;
                metaModel.LicenseeName = licenseeName;
                var agentIdListByCurrency = _subsidiaryService.GetCurrencyAgentList(metaModel.SubsidiaryList, metaModel.IsExcludeTest);

                if (agentIdListByCurrency.Any())
                {
                    metaModel.AgentListXml = SessionService.GenerateAgentListXml(agentIdListByCurrency);

                    return _subsidiaryPerformanceService.GetData(metaModel);
                }
            }

            return new SubsidiaryResultViewModel
            {
                Data = new List<SubsidiaryPerformanceDataModel>()
            };
        }

        [HttpPost]
        public dynamic Detail(SubsidiaryPerformanceMetaModel metaModel)
        {
            if (metaModel.IsValid)
            {
                var licenseeName = SessionService.LicenseeSession.LicenseeName;
                metaModel.LicenseeName = licenseeName;
                var agentIdListByCurrency = _subsidiaryService.GetCurrencyAgentList(metaModel.SubsidiaryList, metaModel.IsExcludeTest);

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