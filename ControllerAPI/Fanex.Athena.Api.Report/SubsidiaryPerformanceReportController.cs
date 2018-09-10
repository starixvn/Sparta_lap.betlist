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
                var agentIdListByCurrency = GetSelectedSubsidiaryList(metaModel.SubsidiaryList, licenseeName, metaModel.IsExcludeTest);

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
                var agentIdListByCurrency = GetSelectedSubsidiaryList(metaModel.SubsidiaryList, licenseeName, metaModel.IsExcludeTest);

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

        private IDictionary<int, IEnumerable<int>> GetSelectedSubsidiaryList(string selectedOperator, string licenseeName, bool isExcludeTest)
        {
            var currencyAgentList = new Dictionary<int, IEnumerable<int>>();
            var selectedOperatorList = selectedOperator.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var subsidiaryData = _subsidiaryService.GetOperatorList(licenseeName);
            var dataGroupByCurrency = subsidiaryData
                .Where(s => selectedOperatorList.Contains(s.SubsidiaryName) && (!isExcludeTest || s.CurrencyId != CurrencyType.Uus))
                .GroupBy(x => x.CurrencyId);

            foreach (var item in dataGroupByCurrency)
            {
                currencyAgentList.Add(item.Key, item.Select(x => x.AgentId));
            }

            return currencyAgentList;
        }
    }
}