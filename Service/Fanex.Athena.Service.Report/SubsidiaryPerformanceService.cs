namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Configuration;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    public class SubsidiaryPerformanceService : ISubsidiaryPerformanceService
    {
        private readonly ISubsidiaryPerformanceDataAccess _subLicenseePerformanceDataAccess;

        public SubsidiaryPerformanceService()
        {
            var balanceUpService = new BalanceUpService();
            _subLicenseePerformanceDataAccess = new SubsidiaryPerformanceDataAccess(balanceUpService.BalanceUpInfo);
        }

        public SubsidiaryPerformanceService(ISubsidiaryPerformanceDataAccess subLicenseePerformanceDataAccess)
        {
            _subLicenseePerformanceDataAccess = subLicenseePerformanceDataAccess;
        }

        public IEnumerable<SubsidiaryPerformanceDataModel> GetData(
            SubsidiaryPerformanceMetaModel metaModel,
            out IEnumerable<SubsidiaryPerformanceEntity> totalList)
        {
            IEnumerable<SubsidiaryPerformanceEntity> totalDataList = null;
            var entityDataList = metaModel.IsValid ?
                _subLicenseePerformanceDataAccess.GetData(metaModel, out totalDataList) : null;

            totalList = totalDataList;

            return entityDataList == null ? new List<SubsidiaryPerformanceDataModel>() : GetDataModel(entityDataList);
        }

        private static IEnumerable<SubsidiaryPerformanceDataModel> GetDataModel(IEnumerable<SubsidiaryPerformanceEntity> entityDataList)
        {
            var dataModelList = new List<SubsidiaryPerformanceDataModel>();
            var dataGrouped = entityDataList.GroupBy(p => new { p.Subsidiary, p.Currency });

            foreach (var summary in dataGrouped)
            {
                var dataModel = new SubsidiaryPerformanceDataModel { Subsidiary = summary.Key.Subsidiary, Currency = summary.Key.Currency };

                var detailList = new List<SubsidiaryPerformanceDetail>();
                foreach (var detail in summary)
                {
                    detailList.Add(new SubsidiaryPerformanceDetail
                    {
                        Product = detail.Product,
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
    }
}