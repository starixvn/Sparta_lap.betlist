namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class LicenseeWinLossByTimeZoneDataAccess : BaseDataAccess, ILicenseeWinLossByTimeZoneDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public LicenseeWinLossByTimeZoneDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public LicenseeWinLossByTimeZoneDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableReportResultModel<LicenseeWinLossByTimeZone> GetData(LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_SubsidiaryWinloss";
            var dataList = new SortableList<LicenseeWinLossByTimeZone>();
            var totalDataList = new List<LicenseeWinLossByTimeZone>();
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

            return new SortableReportResultModel<LicenseeWinLossByTimeZone>
            {
                DataList = dataList,
                TotalDataList = totalDataList
            };
        }

        private static LicenseeWinLossByTimeZone GetEntityData(IDataReader reader, string baseCurrency)
        {
            return new LicenseeWinLossByTimeZone
            {
                CurrencyId = Convert.ToInt32(reader["Currency"]),
                CurrencyName = reader["CurrencyName"].ToString(),
                Product = reader["ProductCode"].ToString(),
                BetCount = Convert.ToInt32(reader["BetCount"]),
                Turnover = Convert.ToDecimal(reader["Turnover" + baseCurrency]),
                CustomerComm = Convert.ToDecimal(reader["Winlost" + baseCurrency]),
                LicenseeWinLoss = Convert.ToDecimal(reader["LicenseeWL" + baseCurrency])
            };
        }

        private void AssignParameter(IObjectDb objectDb, LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            objectDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            objectDb.AddInParameter("MoveBetsDay", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            objectDb.AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            objectDb.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}