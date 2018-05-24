namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Data;
    using Common.Extensions;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Data;
    using Extension.Helper;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.Helper;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Session.Interfaces;

    public class LicenseeWinLossByTimeZoneDataAccess : BaseDataAccess, ILicenseeWinLossByTimeZoneDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public LicenseeWinLossByTimeZoneDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public LicenseeWinLossByTimeZoneDataAccess(ISessionStorage sessionStorage, IBalanceUpDataAccess balanceUpDataAccess)
            : base(null, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ReportResultModel<LicenseeWinLossByTimeZone> GetData(LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            const string SpName = "Lrf_Rpt_LicWinlossByTimeZone";
            var dataList = new SortableList<LicenseeWinLossByTimeZone>();
            var totalData = new LicenseeWinLossByTimeZone();
            using (var objectDb = this.GetObjectDb(SpName))
            {
                AssignParameter(objectDb, metaModel);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = ReadDataInBaseCurrency(reader, metaModel);
                        totalData.BetCount += item.BetCount;
                        totalData.Turnover += item.Turnover;
                        totalData.CustomerComm += item.CustomerComm;
                        totalData.LicenseeWinLoss += item.LicenseeWinLoss;
                        totalData.LicenseeComm += item.LicenseeComm;

                        if (!metaModel.IsConvertAll)
                        {
                            item.Turnover = reader.ToDecimal("Turnover");
                            item.CustomerComm = reader.ToDecimal("PlayerComm");
                            item.LicenseeWinLoss = reader.ToDecimal("LicenseeWL");
                            item.LicenseeComm = reader.ToDecimal("LicenseeComm");
                        }

                        dataList.Add(item);
                    }
                }
            }

            return new ReportResultModel<LicenseeWinLossByTimeZone>
            {
                DataList = dataList,
                TotalData = totalData
            };
        }

        private LicenseeWinLossByTimeZone ReadDataInBaseCurrency(IDataReader reader, LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            const byte GroupByProduct = 1;
            const int ReportBySummary = 1;
            const int ReportByMonthly = 3;
            var baseCurrency = metaModel.BaseCurrency;
            var item = new LicenseeWinLossByTimeZone
            {
                GroupBy = reader["GroupBy"].ToString(),
                BetCount = reader.ToInt("BetCount"),
                Turnover = reader.ToDecimal("Turnover" + baseCurrency),
                CustomerComm = reader.ToDecimal("PlayerComm" + baseCurrency),
                LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + baseCurrency),
                LicenseeComm = reader.ToDecimal("LicenseeComm" + baseCurrency)
            };
            if (metaModel.ReportBy != ReportBySummary)
            {
                item.ReportBy = ConvertHelper.ToDateTime(reader.ToString("reportby"), metaModel.ReportBy == ReportByMonthly);
            }

            item.GroupBy = metaModel.GroupBy == GroupByProduct ?
                        ProductHelper.GetProductName(item.GroupBy) :
                        GetCurrencyName(Convert.ToInt32(item.GroupBy));

            return item;
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
            objectDb.AddInParameter("IncludeDraw", DbType.Boolean, metaModel.IsIncludeDrawData);
        }
    }
}