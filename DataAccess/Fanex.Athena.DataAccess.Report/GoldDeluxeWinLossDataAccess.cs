namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Constant.DataDomain;
    using Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Session.Interfaces;

    public class GoldDeluxeWinLossDataAccess : BaseDataAccess, IGoldDeluxeWinLossDataAccess
    {
        public GoldDeluxeWinLossDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_GoldDeluxeWinloss";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel, false);
                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetSummaryWinloss(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_GoldDeluxeWinloss_Mem";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_GoldDeluxeWinloss_Ag";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static void ReadSummaryTotal(IDataReader reader, IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                summaryTotal[currency].LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                summaryTotal[currency].TipsAmount += reader.ToDecimal("tipsamount" + currency);
                summaryTotal[currency].TipsShare += reader.ToDecimal("tipssharing" + currency);
                summaryTotal[currency].AmountDue += reader.ToDecimal("amountdue" + currency);
            }
        }

        private static void UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("licenseeId", DbType.Int32, metaModel.LicenseeId);
        }

        private ReportResultModel<GoldDeluxeWinLoss> GetData(IDataReader reader, ThirdPartyWinLossMetaModel metaModel, int rowSpan = 0)
        {
            var result = new List<GoldDeluxeWinLoss>();
            var summaryTotal = SystemConstants.BaseCurrencyList.ToDictionary(x => x, x => new GoldDeluxeWinLoss());

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var tipsAmountColumn = "tipsamount" + metaModel.BaseCurrency;
            var tipsShareColumn = "tipssharing" + metaModel.BaseCurrency;
            var amountDueColumn = "amountdue" + metaModel.BaseCurrency;

            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");
            var hasWinlostDate = reader.HasColumn("winlostdate");

            var index = 1;
            while (reader.Read())
            {
                var currencyId = reader.ToInt("currency");
                var item = new GoldDeluxeWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = currencyId,
                    CurrencyName = GetCurrencyName(currencyId),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    Date = hasWinlostDate ? (DateTime?)reader.ToDateTime("winlostdate") : metaModel.DrillDownFromDate,
                    TipsAmount = reader.ToDecimal(tipsAmountColumn),
                    TipsShare = reader.ToDecimal(tipsShareColumn),
                    AmountDue = reader.ToDecimal(amountDueColumn),
                    RowSpan = rowSpan
                };

                result.Add(item);
                ReadSummaryTotal(reader, summaryTotal);
            }

            return new ReportResultModel<GoldDeluxeWinLoss>
            {
                DataList = result,
                SummaryTotal = summaryTotal,
                TotalDataList = CalculationTotal(result)
            };
        }

        private IList<GoldDeluxeWinLoss> CalculationTotal(IList<GoldDeluxeWinLoss> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<GoldDeluxeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new GoldDeluxeWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.Sum(p => p.TurnOver),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                LicenseeWL = g.Sum(p => p.LicenseeWL),
                TipsAmount = g.Sum(p => p.TipsAmount),
                TipsShare = g.Sum(p => p.TipsShare),
                AmountDue = g.Sum(p => p.AmountDue)
            }).ToList();
        }

        public ReportResultModel<GoldDeluxeWinLoss> GetSummaryWinloss(IDataReader reader, ThirdPartyWinLossMetaModel metaModel)
        {
            var result = GetData(reader, metaModel, -1);
            DateTime? date = new DateTime();
            var index = 1;
            var dataList = result.DataList;
            var i = 0;
            var total = dataList.Count();
            
            while (i < total)
            {
                var item = dataList.ElementAt(i);
                date = item.Date;

                item.RowSpan = dataList.Count(x => x.Date == date);
                item.Index = index;

                index++;
                i += item.RowSpan;
            }

            return result;
        }
    }
}