namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Data;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;

    public class SubsidiaryPerformanceDataAccess : BaseDataAccess, ISubsidiaryPerformanceDataAccess
    {
        private readonly BalanceUp _balanceUp;

        public SubsidiaryPerformanceDataAccess(BalanceUp balanceUp)
        {
            _balanceUp = balanceUp;
        }

        public IEnumerable<SubsidiaryPerformanceEntity> GetData(SubsidiaryPerformanceMetaModel metaModel, out IEnumerable<SubsidiaryPerformanceEntity> totalDataList)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            var dataList = new List<SubsidiaryPerformanceEntity>();
            var totalDataListTemp = new List<SubsidiaryPerformanceEntity>();
            var baseCurrency = metaModel.ConvertAll ? metaModel.BaseCurrency : string.Empty;
            using (var objectDb = this.GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(GetEntityData(reader, baseCurrency));
                        totalDataListTemp.Add(GetEntityData(reader, metaModel.BaseCurrency));
                    }
                }
            }

            totalDataList = totalDataListTemp;

            return dataList;
        }

        private void AssignParameter(IObjectDb objectDb, SubsidiaryPerformanceMetaModel metaModel)
        {
            objectDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            objectDb.AddInParameter("MoveBetsDay", DbType.DateTime, _balanceUp.MoveBetsDay);
            objectDb.AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            objectDb.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }

        private static SubsidiaryPerformanceEntity GetEntityData(IDataReader reader, string baseCurrency)
        {
            return new SubsidiaryPerformanceEntity
            {
                SubsidiaryId = Convert.ToInt32(reader["recommend"]),
                CurrencyId = Convert.ToInt32(reader["Currency"]),
                CurrencyName = reader["CurrencyName"].ToString(),
                Product = reader["ProductCode"].ToString(),
                BetCount = Convert.ToInt32(reader["BetCount"]),
                Turnover = Convert.ToDecimal(reader["Turnover" + baseCurrency]),
                CustomerWinLoss = Convert.ToDecimal(reader["Winlost" + baseCurrency]),
                SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWL" + baseCurrency])
            };
        }
    }
}