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
    using Model.Core.ResultModel;
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
            return _cache.Get(cacheKey, () => _subLicenseePerformanceDataAccess.GetOperatorList(licenseeName), CacheTimeoutSeconds);
        }

        public SubsidiaryResultViewModel GetData(SubsidiaryPerformanceMetaModel metaModel)
        {
            var dataList = _subLicenseePerformanceDataAccess.GetData(metaModel);

            if (dataList.Any())
            {
                var operatorList = GetSelectedOpertor(metaModel).ToList();

                return GetDataModel(dataList, operatorList);
            }

            return new SubsidiaryResultViewModel
            {
                Data = new List<SubsidiaryPerformanceDataModel>()
            };
        }

        public ReportResultModel<SubsidiaryPerformanceDetail> GetDetailData(SubsidiaryPerformanceMetaModel metaModel)
        {
            return _subLicenseePerformanceDataAccess.GetDetailData(metaModel);
        }

        private static void AssignSumData(SubsidiaryPerformanceBase total, SubsidiaryPerformanceBase detail)
        {
            total.BetCount += detail.BetCount;
            total.Turnover += detail.Turnover;
            total.SubsidiaryComm += detail.SubsidiaryComm;
            total.SubsidiaryWinLoss += detail.SubsidiaryWinLoss;
            total.CustomerWinLoss += detail.CustomerWinLoss;
        }

        private IEnumerable<Operator> GetSelectedOpertor(SubsidiaryPerformanceMetaModel metaModel)
        {
            var allOperators = GetOperatorList(metaModel.LicenseeName);
            var selectedSubsidiary = metaModel.SubsidiaryList.Split(',');

            return allOperators.Join(selectedSubsidiary, oper => oper.OperatorName, sub => sub, (oper, sub) => oper);
        }

        private SubsidiaryResultViewModel GetDataModel(
            IEnumerable<SubsidiaryPerformance> entityDataList,
            IEnumerable<Operator> operators)
        {
            var dataModel = new List<SubsidiaryPerformanceDataModel>();
            var totalModel = new Dictionary<string, SubsidiaryPerformanceBase>();
            var grandTotal = new SubsidiaryPerformanceBase();

            var subsidiaryGroup = operators.Join(
                entityDataList,
                oper => oper.AgentId,
                data => data.SubsidiaryId,
                (oper, data) => new
                {
                    Subsidiary = oper.OperatorName,
                    Data = data
                }).GroupBy(
                    data => data.Subsidiary,
                    (subsidiary, subsidiaryData) => new
                    {
                        Subsidiary = subsidiary,
                        Details = subsidiaryData.GroupBy(x => x.Data.CurrencyId, (currency, currencyData) => currencyData.Select(x => x.Data))
                    });

            foreach (var subsidiary in subsidiaryGroup)
            {
                var main = new SubsidiaryPerformanceDataModel
                {
                    Subsidiary = subsidiary.Subsidiary,
                    Details = new Dictionary<string, IEnumerable<SubsidiaryPerformanceBase>>()
                };
                var total = new SubsidiaryPerformanceBase();

                foreach (var currency in subsidiary.Details)
                {
                    var products = GetProducts(currency, total);

                    main.Details.Add(currency.First().CurrencyName, products);
                    main.TotalItem += products.Count;
                }

                dataModel.Add(main);
                totalModel.Add(subsidiary.Subsidiary, total);
                AssignSumData(grandTotal, total);
            }

            return new SubsidiaryResultViewModel
            {
                Data = dataModel,
                Total = totalModel,
                GrandTotal = grandTotal
            };
        }

        private IList<SubsidiaryPerformanceBase> GetProducts(IEnumerable<SubsidiaryPerformance> productList, SubsidiaryPerformanceBase baseSumTotal)
        {
            var groupedProduct = productList.GroupBy(
                x => x.ProductCode,
                (key, item) => new
                {
                    ProductCode = key,
                    Data = new SubsidiaryPerformanceBase
                    {
                        BetCount = item.Sum(x => x.BetCount),
                        Turnover = item.Sum(x => x.Turnover),
                        CustomerWinLoss = item.Sum(x => x.CustomerWinLoss),
                        SubsidiaryWinLoss = item.Sum(x => x.SubsidiaryWinLoss),
                        SubsidiaryComm = item.Sum(x => x.SubsidiaryComm)
                    },
                    BaseData = new SubsidiaryPerformanceBase
                    {
                        BetCount = item.Sum(x => x.BaseCurrencyData.BetCount),
                        Turnover = item.Sum(x => x.BaseCurrencyData.Turnover),
                        CustomerWinLoss = item.Sum(x => x.BaseCurrencyData.CustomerWinLoss),
                        SubsidiaryWinLoss = item.Sum(x => x.BaseCurrencyData.SubsidiaryWinLoss),
                        SubsidiaryComm = item.Sum(x => x.BaseCurrencyData.SubsidiaryComm)
                    }
                });

            var products = new List<SubsidiaryPerformanceBase>();
            var productAll = new SubsidiaryPerformanceBase
            {
                Product = ViewResources.All
            };
            products.Add(productAll);

            foreach (var product in groupedProduct)
            {
                product.Data.Product = ViewResources.ResourceManager.GetString($"Product{product.ProductCode}");
                products.Add(product.Data);

                AssignSumData(productAll, product.Data);
                AssignSumData(baseSumTotal, product.BaseData);
            }

            return products;
        }
    }
}