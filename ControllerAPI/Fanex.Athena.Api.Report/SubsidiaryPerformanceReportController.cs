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
    using Resources;
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
            if (metaModel.IsValid)
            {
                var agentIdList = GetSelectedSubsidiaryList(metaModel.SubsidiaryList);
                metaModel.AgentListXml = SessionService.GenerateAgentListXmlByAgentIdList(agentIdList);

                IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;

                var mainData = _subsidiaryPerformanceService.GetData(metaModel, out totalDataList);
                var grandTotal = totalDataList.Any() ? GetGrandTotal(metaModel.BaseCurrency.ToUpperInvariant(), totalDataList) : null;

                return new
                {
                    Data = mainData,
                    Total = totalDataList,
                    GrandTotal = grandTotal
                };
            }

            return new
            {
                Data = new List<SubsidiaryPerformanceDataModel>()
            };
        }

        private static SubsidiaryPerformanceEntity GetGrandTotal(string baseCurrency, IEnumerable<SubsidiaryPerformanceEntity> totalDataList)
        {
            return new SubsidiaryPerformanceEntity
            {
                Subsidiary = ViewResources.Total,
                CurrencyName = $"{ViewResources.Based} {baseCurrency}",
                BetCount = totalDataList.Sum(p => p.BetCount),
                Turnover = totalDataList.Sum(p => p.Turnover),
                CustomerWinLoss = totalDataList.Sum(p => p.CustomerWinLoss),
                SubsidiaryWinLoss = totalDataList.Sum(p => p.SubsidiaryWinLoss)
            };
        }

        private IEnumerable<int> GetSelectedSubsidiaryList(string selectedOperator)
        {
            var selectedOperatorList = selectedOperator.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var currencyIdList = userSession.CurrencyList.Select(x => x.Key);
            var subsidiaryData = _subsidiaryPerformanceService.GetOperatorList(licenseeSession.LicenseeName, currencyIdList);

            return subsidiaryData.Where(s => selectedOperatorList.Contains(s.Value)).Select(x => Convert.ToInt32(x.Key));
        }
    }
}