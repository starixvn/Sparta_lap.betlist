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
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
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

        public IEnumerable<Operator> GetOperatorList(string licenseeName)
        {
            var cacheKey = string.Format(CacheKey.MemorycacheSubsidiaryListByLicensee, licenseeName);

            var operatorList = _cache.Get<IEnumerable<Operator>>(cacheKey);
            if (operatorList == null)
            {
                operatorList = _subLicenseePerformanceDataAccess.GetOperatorList(licenseeName);
                _cache.Set(cacheKey, operatorList, CacheTimeoutSeconds);
            }

            return operatorList;
        }

        public IEnumerable<SubsidiaryPerformanceDataModel> GetData(
            SubsidiaryPerformanceMetaModel metaModel,
            out IEnumerable<SubsidiaryPerformanceEntity> totalDataList)
        {
            IEnumerable<SubsidiaryPerformanceEntity> entityTotalList = null;
            var entityDataList = _subLicenseePerformanceDataAccess.GetData(metaModel, out entityTotalList);

            if (entityDataList.Any())
            {
                var operatorList = GetOperatorList(metaModel.LicenseeName);
                MapSubsidiaryColumn(entityDataList, operatorList);
                MapSubsidiaryColumn(entityTotalList, operatorList);

                totalDataList = GetTotalDataList(entityTotalList, metaModel.BaseCurrency.ToUpperInvariant());

                return GetDataModel(entityDataList);
            }

            totalDataList = new List<SubsidiaryPerformanceEntity>();

            return new List<SubsidiaryPerformanceDataModel>();
        }

        private static void MapSubsidiaryColumn(IEnumerable<SubsidiaryPerformanceEntity> entityDataList, IEnumerable<Operator> operatorList)
        {
            foreach (var item in entityDataList)
            {
                var operatorItem = operatorList.FirstOrDefault(o => o.AgentId == item.SubsidiaryId);
                item.Subsidiary = operatorItem == null ? string.Empty : operatorItem.OperatorName;
            }
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetDataModel(
            IEnumerable<SubsidiaryPerformanceEntity> entityDataList)
        {
            var dataModelList = new List<SubsidiaryPerformanceDataModel>();

            var groupedData = entityDataList.Where(x => !string.IsNullOrEmpty(x.Subsidiary)).GroupBy(p => new { p.Subsidiary, p.CurrencyId });

            foreach (var summary in groupedData)
            {
                var dataModel = new SubsidiaryPerformanceDataModel
                {
                    Subsidiary = summary.Key.Subsidiary,
                    CurrencyName = summary.First().CurrencyName
                };

                var detailList = new List<SubsidiaryPerformanceModelBase>();

                detailList.Add(new SubsidiaryPerformanceModelBase
                {
                    Product = ViewResources.All,
                    BetCount = summary.Sum(p => p.BetCount),
                    Turnover = summary.Sum(p => p.Turnover),
                    CustomerWinLoss = summary.Sum(p => p.CustomerWinLoss),
                    SubsidiaryWinLoss = summary.Sum(p => p.SubsidiaryWinLoss),
                    SubsidiaryComm = summary.Sum(p => p.SubsidiaryComm)
                });

                var summaryDataByProduct = summary.GroupBy(p => p.Product);
                foreach (var itemByProduct in summaryDataByProduct)
                {
                    detailList.Add(new SubsidiaryPerformanceModelBase
                    {
                        Product = ViewResources.ResourceManager.GetString($"Product{itemByProduct.Key}"),
                        BetCount = itemByProduct.Sum(p => p.BetCount),
                        Turnover = itemByProduct.Sum(p => p.Turnover),
                        CustomerWinLoss = itemByProduct.Sum(p => p.CustomerWinLoss),
                        SubsidiaryWinLoss = itemByProduct.Sum(p => p.SubsidiaryWinLoss),
                        SubsidiaryComm = itemByProduct.Sum(p => p.SubsidiaryComm)
                    });
                }

                dataModel.Details = detailList;
                dataModelList.Add(dataModel);
            }

            return dataModelList;
        }

        private static IEnumerable<SubsidiaryPerformanceEntity> GetTotalDataList(
            IEnumerable<SubsidiaryPerformanceEntity> entityTotalDataList,
            string baseCurrency)
        {
            var totalDataList = new List<SubsidiaryPerformanceEntity>();
            var groupedDataBySubsidiary = entityTotalDataList.Where(x => !string.IsNullOrEmpty(x.Subsidiary)).GroupBy(p => p.Subsidiary);

            foreach (var item in groupedDataBySubsidiary)
            {
                totalDataList.Add(new SubsidiaryPerformanceEntity
                {
                    Subsidiary = $"{ViewResources.Total} {item.Key}",
                    CurrencyName = $"{ViewResources.Based} {baseCurrency}",
                    BetCount = item.Sum(x => x.BetCount),
                    Turnover = item.Sum(x => x.Turnover),
                    CustomerWinLoss = item.Sum(x => x.CustomerWinLoss),
                    SubsidiaryWinLoss = item.Sum(x => x.SubsidiaryWinLoss),
                    SubsidiaryComm = item.Sum(x => x.SubsidiaryComm)
                });
            }

            return totalDataList;
        }
    }
}