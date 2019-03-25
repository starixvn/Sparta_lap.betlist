// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalanceDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;

    public class AgCasinoBalanceDataAccess : BaseDataAccess, IAgCasinoBalanceDataAccess
    {
        public SortableReportResultModel<AgCasinoBalance> GetSummaryBalance(
            ThirdPartyWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_AGCasinoBalance";

            using (var db = GetObjectDb(UspName))
            {
                UpdateUspParams(db, metaModel, false);

                using (var reader = db.ExecuteReader())
                {
                    return GetData(reader, currencyList, metaModel);
                }
            }
        }

        public SortableReportResultModel<AgCasinoBalance> GetCustomerBalanceDetail(
            ThirdPartyWinLossMetaModel metaModel,
            IDictionary<int, string> currencyList)
        {
            const string UspName = "LRF_Rpt_AGCasinoBalance_Mem";

            using (var db = GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", metaModel.DrillDownCustomerId);
                db.SetParameterValue("pagesize", metaModel.IsExportExcel ? SystemConstants.MaxDataTotalRow : metaModel.PageSize);
                db.SetParameterValue("pageindex", metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
                db.SetParameterValue("TotalRows", null);

                UpdateUspParams(db, metaModel, true);
                SortableReportResultModel<AgCasinoBalance> result;
                using (var reader = db.ExecuteReader())
                {
                    result = GetData(reader, currencyList, metaModel);
                }

                result.TotalRows = (int)db.GetParameterValue("TotalRows");

                return result;
            }
        }

        public SortableReportResultModel<AgCasinoBalance> GetAgentBalanceDetail(ThirdPartyWinLossMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "LRF_Rpt_AGCasinoBalance_Ag";

            using (var db = GetObjectDb(UspName))
            {
                UpdateUspParams(db, metaModel, true);
                using (var reader = db.ExecuteReader())
                {
                    return GetData(reader, currencyList, metaModel);
                }
            }
        }

        private static void ReadSummaryTotal(IDataReader reader, Dictionary<string, AgCasinoBalance> summaryTotal)
        {
            foreach (var currencySuffix in SystemConstants.BaseCurrencyList)
            {
                summaryTotal[currencySuffix].CustomerWinLoss += reader.ToDecimal("winlost" + currencySuffix);
                summaryTotal[currencySuffix].TotalTransaction += reader.ToInt("totaltrans");
                summaryTotal[currencySuffix].LicenseeWL += reader.ToDecimal("licenseewinlost" + currencySuffix);
                summaryTotal[currencySuffix].TurnOver += reader.ToDecimal("turnover" + currencySuffix);
                summaryTotal[currencySuffix].OpeningBalance += reader.ToDecimal("OpeningBalance" + currencySuffix);
                summaryTotal[currencySuffix].TransferIn += reader.ToDecimal("TransferIn" + currencySuffix);
                summaryTotal[currencySuffix].TransferOut += reader.ToDecimal("TransferOut" + currencySuffix);
                summaryTotal[currencySuffix].ClosingBalance += reader.ToDecimal("ClosingBalance" + currencySuffix);
            }
        }

        private static void UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("fromdate", drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.SetParameterValue("todate", drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
        }

        private SortableReportResultModel<AgCasinoBalance> GetData(
            IDataReader reader,
            IDictionary<int, string> currencyList,
            ThirdPartyWinLossMetaModel metaModel)
        {
            var dataList = new SortableList<AgCasinoBalance>();

            var summaryTotal = SystemConstants.BaseCurrencyList.ToDictionary(x => x, x => new AgCasinoBalance());

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var openingBalanceColumn = "OpeningBalance" + metaModel.BaseCurrency;
            var transferInColumn = "TransferIn" + metaModel.BaseCurrency;
            var transferOutColumn = "TransferOut" + metaModel.BaseCurrency;
            var closingBalanceColumn = "ClosingBalance" + metaModel.BaseCurrency;

            var hasSystemIdColumn = reader.HasColumn("username");
            var hasExternalIdColumn = reader.HasColumn("nickname");
            var hasCustomerIdColumn = reader.HasColumn("custid");
            var hasWinlostDateColumn = reader.HasColumn("date");
            var index = metaModel.DrillDownLevel == 2 ? ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1 : 1;
            while (reader.Read())
            {
                var currencyId = reader.ToInt("currency");
                var item = new AgCasinoBalance
                {
                    Index = index++,
                    SystemId = hasSystemIdColumn ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalIdColumn ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerIdColumn ? reader.ToInt("custid") : 0,
                    CurrencyId = currencyId,
                    CurrencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    Date = hasWinlostDateColumn ? (DateTime?)reader.ToDateTime("date") : metaModel.DrillDownFromDate,
                    OpeningBalance = reader.ToDecimal(openingBalanceColumn),
                    TransferIn = reader.ToDecimal(transferInColumn),
                    TransferOut = reader.ToDecimal(transferOutColumn),
                    ClosingBalance = reader.ToDecimal(closingBalanceColumn),
                };

                dataList.Add(item);
                ReadSummaryTotal(reader, summaryTotal);
            }

            return new SortableReportResultModel<AgCasinoBalance>
            {
                DataList = dataList,
                SummaryTotal = summaryTotal
            };
        }
    }
}