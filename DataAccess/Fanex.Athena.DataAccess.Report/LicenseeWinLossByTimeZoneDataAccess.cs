namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core;
    using Core.Configuration.Interfaces;
    using Data;
    using Extension.Helper;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.Helper;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class LicenseeWinLossByTimeZoneDataAccess : BaseDataAccess, ILicenseeWinLossByTimeZoneDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public LicenseeWinLossByTimeZoneDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public async Task<ReportResultModel<LicenseeWinLossByTimeZone>> GetData(
            LicenseeWinLossByTimeZoneMetaModel metaModel,
            IDictionary<int, string> currencyList)
        {
            const string SpName = "Lrf_Rpt_LicWinlossByTimeZone";
            var dataList = new SortableList<LicenseeWinLossByTimeZone>();
            var totalData = new LicenseeWinLossByTimeZone();
            using (var objectDb = this.GetObjectDb(SpName))
            {
                await AssignParameter(objectDb, metaModel).ConfigureAwait(false);

                using (var reader = objectDb.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = ReadDataInBaseCurrency(reader, currencyList, metaModel);
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

        private LicenseeWinLossByTimeZone ReadDataInBaseCurrency(
            IDataReader reader,
            IDictionary<int, string> currencyList,
            LicenseeWinLossByTimeZoneMetaModel metaModel)
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
                        currencyList?.GetValueOrDefault(Convert.ToInt32(item.GroupBy), item.GroupBy);

            return item;
        }

        private async Task AssignParameter(IObjectDb objectDb, LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            objectDb.SetParameterValue("FromDate", metaModel.FromDate);
            objectDb.SetParameterValue("ToDate", metaModel.ToDate);
            objectDb.SetParameterValue("MoveBetsDay", (await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false)).MoveBetsDay);
            objectDb.SetParameterValue("ProductList", metaModel.ProductCodeList);
            objectDb.SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.SetParameterValue("TimeZone", metaModel.TimeZone);
            objectDb.SetParameterValue("ReportBy", metaModel.ReportBy);
            objectDb.SetParameterValue("GroupBy", metaModel.GroupBy);
            objectDb.SetParameterValue("IncludeDraw", metaModel.IsIncludeDrawData);
        }
    }
}