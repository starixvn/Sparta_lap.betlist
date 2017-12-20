namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Resources;

    public class SubsidiaryPerformanceService : ISubsidiaryPerformanceService
    {
        private const int CacheTimeoutSeconds = 300;

        private readonly IMemoryCacheService _cache;

        private readonly ISubsidiaryPerformanceDataAccess _subLicenseePerformanceDataAccess;

        public SubsidiaryPerformanceService()
        {
            _cache = new MemoryCacheService();
            _subLicenseePerformanceDataAccess = new SubsidiaryPerformanceDataAccess();
        }

        public SubsidiaryPerformanceService(ISubsidiaryPerformanceDataAccess subLicenseePerformanceDataAccess, IMemoryCacheService cache)
        {
            _cache = cache;
            _subLicenseePerformanceDataAccess = subLicenseePerformanceDataAccess;
        }

        public IDictionary<int, string> GetSubsidiaryList(string licenseeName)
        {
            var cacheKey = $"SubsidiaryList_{licenseeName}";

            var subsidiaryList = _cache.Get<IDictionary<int, string>>(cacheKey);
            if (subsidiaryList == null)
            {
                subsidiaryList = _subLicenseePerformanceDataAccess.GetSubsidiaryList(licenseeName);
                _cache.Set(cacheKey, subsidiaryList, CacheTimeoutSeconds);
            }

            return subsidiaryList;
        }

        public IEnumerable<SubsidiaryPerformanceDataModel> GetData(
            SubsidiaryPerformanceMetaModel metaModel,
            out IEnumerable<SubsidiaryPerformanceEntity> totalDataList)
        {
            IEnumerable<SubsidiaryPerformanceEntity> entityTotalList = null;
            var entityDataList = _subLicenseePerformanceDataAccess.GetData(metaModel, out entityTotalList);

            totalDataList = entityTotalList.Any() ?
                GetTotalDataList(entityTotalList, metaModel.BaseCurrency.ToUpperInvariant()) : new List<SubsidiaryPerformanceEntity>();

            return entityDataList.Any() ? GetDataModel(entityDataList) : new List<SubsidiaryPerformanceDataModel>();
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetDataModel(IEnumerable<SubsidiaryPerformanceEntity> entityDataList)
        {
            var dataModelList = new List<SubsidiaryPerformanceDataModel>();
            var groupedData = entityDataList.GroupBy(p => new { p.SubsidiaryId, p.CurrencyId });

            foreach (var summary in groupedData)
            {
                var dataModel = new SubsidiaryPerformanceDataModel { Subsidiary = summary.Key.SubsidiaryId.ToString(), CurrencyName = summary.First().CurrencyName };

                var detailList = new List<SubsidiaryPerformanceDetail>();

                detailList.Add(new SubsidiaryPerformanceDetail
                {
                    Product = ViewResources.All,
                    BetCount = summary.Sum(p => p.BetCount),
                    Turnover = summary.Sum(p => p.Turnover),
                    CustomerWinLoss = summary.Sum(p => p.CustomerWinLoss),
                    SubsidiaryWinLoss = summary.Sum(p => p.SubsidiaryWinLoss)
                });

                foreach (var detail in summary)
                {
                    detailList.Add(new SubsidiaryPerformanceDetail
                    {
                        Product = ViewResources.ResourceManager.GetString($"Product{detail.Product}"),
                        BetCount = detail.BetCount,
                        Turnover = detail.Turnover,
                        CustomerWinLoss = detail.CustomerWinLoss,
                        SubsidiaryWinLoss = detail.SubsidiaryWinLoss
                    });
                }

                dataModel.Details = detailList;
                dataModelList.Add(dataModel);
            }

            return dataModelList;
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetTotalDataList(IEnumerable<SubsidiaryPerformanceEntity> entityTotalDataList, string baseCurrency)
        {
            var totalDataList = new List<SubsidiaryPerformanceEntity>();
            var groupedDataBySubsidiary = entityTotalDataList.GroupBy(p => p.SubsidiaryId);

            foreach (var item in groupedDataBySubsidiary)
            {
                totalDataList.Add(new SubsidiaryPerformanceEntity
                {
                    SubsidiaryId = item.Key,
                    Subsidiary = $"{ViewResources.Total} {item.Key.ToString()}",
                    CurrencyName = $"{ViewResources.Based} {baseCurrency}",
                    BetCount = item.Sum(x => x.BetCount),
                    Turnover = item.Sum(x => x.Turnover),
                    CustomerWinLoss = item.Sum(x => x.CustomerWinLoss),
                    SubsidiaryWinLoss = item.Sum(x => x.SubsidiaryWinLoss)
                });
            }

            return totalDataList;
        }
    }
}