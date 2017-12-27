namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.DTO.SubsidiaryPerformance;
    using Model.Report.MetaModel;

    public class SubsidiaryPerformanceDataAccess : BaseDataAccess, ISubsidiaryPerformanceDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public SubsidiaryPerformanceDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public SubsidiaryPerformanceDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public IEnumerable<Operator> GetOperatorList(string licenseeName)
        {
            var operatorList = new List<Operator>();

            const string SpName = "Lrf_SubsidiarySel";

            using (var objectDb = this.GetObjectDb(SpName))
            {
                objectDb.AddInParameter("LicenseeName", DbType.String, licenseeName);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        operatorList.Add(new Operator
                        {
                            AgentId = Convert.ToInt32(reader["AgentID"]),
                            OperatorName = reader["OperatorID"].ToString(),
                            CurrencyId = Convert.ToInt32(reader["Currency"])
                        });
                    }
                }
            }

            return operatorList;
        }

        public ReportResultModel<SubsidiaryPerformanceEntity> GetData(SubsidiaryPerformanceMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            var dataList = new List<SubsidiaryPerformanceEntity>();
            var totalDataList = new List<SubsidiaryPerformanceEntity>();
            var baseCurrency = metaModel.ConvertAll ? metaModel.BaseCurrency : string.Empty;
            using (var objectDb = this.GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(GetEntityData(reader, baseCurrency));
                        totalDataList.Add(GetEntityData(reader, metaModel.BaseCurrency));
                    }
                }
            }

            return new ReportResultModel<SubsidiaryPerformanceEntity>
            {
                DataList = dataList,
                TotalDataList = totalDataList
            };
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
                SubsidiaryWinLoss = Convert.ToDecimal(reader["LicenseeWL" + baseCurrency]),
                SubsidiaryComm = Convert.ToDecimal(reader["SubsidiaryComm" + baseCurrency])
            };
        }

        private void AssignParameter(IObjectDb objectDb, SubsidiaryPerformanceMetaModel metaModel)
        {
            objectDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            objectDb.AddInParameter("MoveBetsDay", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            objectDb.AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            objectDb.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}