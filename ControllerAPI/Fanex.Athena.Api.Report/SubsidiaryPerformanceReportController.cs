namespace Fanex.Athena.Api.Report
{
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
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml();
            IEnumerable<SubsidiaryPerformanceEntity> totalList = null;

            var summaryData = _subsidiaryPerformanceService.GetData(metaModel, out totalList);
            var grandTotal = totalList == null ? null : GetGrandTotal(metaModel.BaseCurrency.ToUpperInvariant(), totalList);

            return new
            {
                Data = summaryData,
                Total = totalList,
                GrandTotal = grandTotal
            };
        }

        private static SubsidiaryPerformanceEntity GetGrandTotal(string baseCurrency, IEnumerable<SubsidiaryPerformanceEntity> totalList)
        {
            return new SubsidiaryPerformanceEntity
            {
                Currency = baseCurrency,
                BetCount = totalList.Sum(p => p.BetCount),
                Turnover = totalList.Sum(p => p.Turnover),
                CustomerWinLoss = totalList.Sum(p => p.CustomerWinLoss),
                SubsidiaryWinLoss = totalList.Sum(p => p.SubsidiaryWinLoss)
            };
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetFakeSummaryData()
        {
            var detailData = new List<SubsidiaryPerformanceDetail>
            {
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m},
                new SubsidiaryPerformanceDetail { Product = "Sportsbook", BetCount = 50, Turnover = 100000m, CustomerWinLoss = 10000m, SubsidiaryWinLoss = -10000m}
            };

            return new List<SubsidiaryPerformanceDataModel>
            {
                new SubsidiaryPerformanceDataModel { Subsidiary = "New York", Currency = "USD", Details = detailData},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "THB", Details = detailData},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "THB", Details = detailData},
                new SubsidiaryPerformanceDataModel { Subsidiary = "India", Currency = "THB", Details = detailData}
            };
        }
    }
}