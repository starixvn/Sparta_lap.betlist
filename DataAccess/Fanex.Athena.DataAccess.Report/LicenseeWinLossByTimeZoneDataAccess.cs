namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Common.Extensions;
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
            const string SpName = "Lrf_Rpt_LicWinlossByTimeZone";
            var dataList = new SortableList<LicenseeWinLossByTimeZone>();
            var totalData = new LicenseeWinLossByTimeZone();
            var baseCurrency = metaModel.IsConvertAll ? metaModel.BaseCurrency : string.Empty;
            using (var objectDb = this.GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(GetEntityData(reader, baseCurrency));
                    }
                }
            }

            return new SortableReportResultModel<LicenseeWinLossByTimeZone>
            {
                DataList = dataList,
                TotalData = totalData
            };
        }

        private static LicenseeWinLossByTimeZone GetEntityData(IDataReader reader, string baseCurrency)
        {
            return new LicenseeWinLossByTimeZone
            {
                CurrencyId = reader.ToInt("Currency"),
                CurrencyName = reader["CurrencyName"].ToString(),
                Product = reader["ProductCode"].ToString(),
                BetCount = reader.ToInt("BetCount"),
                Turnover = reader.ToDecimal("Turnover" + baseCurrency),
                CustomerComm = reader.ToDecimal("Winlost" + baseCurrency),
                LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + baseCurrency)
            };
        }

        private void AssignParameter(IObjectDb objectDb, LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            objectDb.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            objectDb.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            objectDb.AddInParameter("MoveBetsDay", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            objectDb.AddInParameter("ProductList", DbType.String, metaModel.ProductCodeList);
            objectDb.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            objectDb.AddInParameter("TimeZone", DbType.Int16, metaModel.TimeZone);
            objectDb.AddInParameter("ReportBy", DbType.Byte, metaModel.ReportBy);
            objectDb.AddInParameter("GroupBy", DbType.Byte, metaModel.GroupBy);
        }
    }
}