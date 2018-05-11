namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Session.Interfaces;

    public class LicenseeWinLossDataAccess : BaseDataAccess, ILicenseeWinLossDataAccess
    {
        private static readonly string[] IgnoredLicenseeWLProducts = new[] { Product.BA, Product.RB };
        private static readonly string[] IgnoredTurnoverProducts = new[] { Product.BT, Product.CO };

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public LicenseeWinLossDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public LicenseeWinLossDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(appSetting, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableReportResultModel<LicenseeWinLoss> GetSummaryWinloss(
            LicenseeWinLossMetaModel metaModel)
        {
            var uspName = GetSpNameForFakeLicensee(metaModel.LicenseeId, "Lrf_Rpt_LicWinloss");
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    db.AddInParameter("reporttype", DbType.Byte, metaModel.ReportBy);
                    UpdateUspParams(db, metaModel, false);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, false);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, uspName, metaModel);
            }
        }

        public SortableReportResultModel<LicenseeWinLoss> GetCustomerWinlossDetail(LicenseeWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LicWinlossDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, true);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public SortableReportResultModel<LicenseeWinLoss> GetAgentWinlossDetail(LicenseeWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_LicWinlossDetail_Ag";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, true);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static string[] GetBaseCurrencyList(bool isFakeLicensee)
        {
            return isFakeLicensee ? new[] { "USD" } : SystemConstants.BaseCurrencyList.ToArray();
        }

        private static bool IsIgnoredLicenseeWLProduct(string productCode)
        {
            return IgnoredLicenseeWLProducts.Contains(productCode);
        }

        private static bool IsIgnoredTurnoverProduct(string productCode)
        {
            return IgnoredTurnoverProducts.Contains(productCode);
        }

        private static IDictionary<string, decimal> ReadTurnoverList(IDataReader reader, IDictionary<string, string> turnoverColumns)
        {
            var turnOver = new Dictionary<string, decimal>();
            foreach (var column in turnoverColumns)
            {
                turnOver.Add(column.Value, reader.ToDecimal(column.Key));
            }

            return turnOver;
        }

        private static IDictionary<string, decimal> ReadLicenseeWinLossList(IDataReader reader, IDictionary<string, string> licenseeWLColumns)
        {
            var licenseeWl = new Dictionary<string, decimal>();

            foreach (var column in licenseeWLColumns)
            {
                licenseeWl.Add(column.Value, reader.ToDecimal(column.Key));
            }

            return licenseeWl;
        }

        private static IDictionary<string, LicenseeWinLoss> InitSummaryTotal(bool isFakeLicensee)
        {
            var summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            var baseCurrencyList = GetBaseCurrencyList(isFakeLicensee);
            foreach (var currency in baseCurrencyList)
            {
                summaryTotal.Add(
                    currency,
                    new LicenseeWinLoss
                    {
                        LicenseeWL = new Dictionary<string, decimal>(),
                        TurnOver = new Dictionary<string, decimal>()
                    });
            }

            return summaryTotal;
        }

        private static void ReadSummaryTotal(
            IDataReader reader,
            IList<ProductOption> productOptions,
            IDictionary<string, LicenseeWinLoss> summaryTotal,
            bool isFakeLicensee)
        {
            var baseCurrencyList = GetBaseCurrencyList(isFakeLicensee);
            foreach (var currency in baseCurrencyList)
            {
                var summaryItem = summaryTotal[currency];
                summaryItem.CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                summaryItem.CustomerComm += reader.ToDecimal("playercomm" + currency);
                summaryItem.TotalTransaction += reader.ToInt("betcount");
                summaryItem.RacingRevenue += reader.ToDecimal("licenseewlrb" + currency) + reader.ToDecimal("commrb" + currency);
                summaryItem.CashOutCommission += reader.ToDecimal("CashOutComm" + currency);
                summaryItem.BACommission += reader.ToDecimal("commba" + currency);
                summaryItem.BetTradeCommission += reader.ToDecimal("BetTradeComm" + currency);
                summaryItem.AmtDueToBetTrade += reader.ToDecimal("amtduetoBT" + currency);
                summaryItem.AmtDueToBA += reader.ToDecimal("amtduetoba" + currency);
                summaryItem.AmtDueToRacing += reader.ToDecimal("amtduetorb" + currency);
                summaryItem.ActualStake += reader.ToDecimal("BTActualStake" + currency);
                summaryItem.BuyBackAmountSB += reader.ToDecimal("BuyBackAmtSB" + currency);
                summaryItem.BuyBackAmountBA += reader.ToDecimal("BuyBackAmtBA" + currency);
                decimal temp;
                var licenseeWlSummary = new Dictionary<string, decimal>();
                var turnOverSummary = new Dictionary<string, decimal>();
                foreach (var product in productOptions)
                {
                    var licenseeWlColumnName = "LicenseeWL" + product.Code + currency;
                    var turnOverColumnName = "turnover" + product.Code + currency;

                    if (!IsIgnoredLicenseeWLProduct(product.Code) && reader.HasColumn(licenseeWlColumnName))
                    {
                        summaryItem.LicenseeWL.TryGetValue(product.Code, out temp);
                        temp += reader.ToDecimal(licenseeWlColumnName);
                        licenseeWlSummary.Add(product.Code, temp);
                    }

                    if (!IsIgnoredTurnoverProduct(product.Code) && reader.HasColumn(turnOverColumnName))
                    {
                        summaryItem.TurnOver.TryGetValue(product.Code, out temp);
                        temp += reader.ToDecimal(turnOverColumnName);
                        turnOverSummary.Add(product.Code, temp);
                    }
                }

                summaryItem.LicenseeWL = licenseeWlSummary;
                summaryItem.TurnOver = turnOverSummary;
            }
        }

        private static IEnumerable<LicenseeWinLoss> CalculateTotal(IEnumerable<LicenseeWinLoss> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<LicenseeWinLoss>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g => new LicenseeWinLoss
            {
                CurrencyName = g.Key,
                TurnOver = g.SelectMany(k => k.TurnOver).GroupBy(j => j.Key).ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                ActualStake = g.Sum(p => p.ActualStake),
                BuyBackAmountSB = g.Sum(p => p.BuyBackAmountSB),
                BuyBackAmountBA = g.Sum(p => p.BuyBackAmountBA),
                TotalTransaction = g.Sum(p => p.TotalTransaction),
                CustomerWinLoss = g.Sum(p => p.CustomerWinLoss),
                CustomerComm = g.Sum(p => p.CustomerComm),
                LicenseeWL = g.SelectMany(k => k.LicenseeWL).GroupBy(j => j.Key).ToDictionary(p => p.Key, p => p.Sum(j => j.Value)),
                BetTradeCommission = g.Sum(p => p.BetTradeCommission),
                CashOutCommission = g.Sum(p => p.CashOutCommission),
                BACommission = g.Sum(p => p.BACommission),
                RacingRevenue = g.Sum(p => p.RacingRevenue),
                AmtDueToBetTrade = g.Sum(p => p.AmtDueToBetTrade),
                AmtDueToBA = g.Sum(p => p.AmtDueToBA),
                AmtDueToRacing = g.Sum(p => p.AmtDueToRacing),
            }).ToList();
        }

        private void UpdateUspParams(IObjectDb db, LicenseeWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("isold", DbType.Boolean, metaModel.IsOld);
            db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
            db.AddInParameter("movebetsday", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        private SortableReportResultModel<LicenseeWinLoss> GetData(
            IDataReader reader,
            LicenseeWinLossMetaModel metaModel,
            bool drilldown)
        {
            var baseCurrency = metaModel.BaseCurrency;
            var licenseeWLColumns = new Dictionary<string, string>();
            var turnoverColumns = new Dictionary<string, string>();
            if (reader.FieldCount > 0)
            {
                foreach (var product in metaModel.ProductOptions)
                {
                    var licenseeWlColumnName = "LicenseeWL" + product.Code + baseCurrency;
                    var turnOverColumnName = "turnover" + product.Code + baseCurrency;

                    if (!IsIgnoredLicenseeWLProduct(product.Code) && reader.HasColumn(licenseeWlColumnName))
                    {
                        licenseeWLColumns.Add(licenseeWlColumnName, product.Code);
                    }

                    if (!IsIgnoredTurnoverProduct(product.Code) && reader.HasColumn(turnOverColumnName))
                    {
                        turnoverColumns.Add(turnOverColumnName, product.Code);
                    }
                }
            }

            return ReadData(reader, metaModel, licenseeWLColumns, turnoverColumns, drilldown);
        }

        private SortableReportResultModel<LicenseeWinLoss> ReadData(
            IDataReader reader,
            LicenseeWinLossMetaModel metaModel,
            IDictionary<string, string> licenseeWLColumns,
            IDictionary<string, string> turnoverColumns,
            bool drilldown)
        {
            var isFakeLicensee = IsFakeLicensee(metaModel.LicenseeId);
            var dataList = new SortableList<LicenseeWinLoss>();
            var summaryTotal = InitSummaryTotal(isFakeLicensee);
            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");
            var baseCurrency = metaModel.BaseCurrency;
            var isMonthly = metaModel.ReportBy == ReportByType.Monthly;
            var index = 1;
            while (reader.Read())
            {
                var currencyId = reader.ToInt("currency");
                var item = new LicenseeWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = currencyId,
                    CurrencyName = GetCurrencyName(currencyId),
                    CustomerWinLoss = reader.ToDecimal("winlost" + baseCurrency),
                    CustomerComm = reader.ToDecimal("playercomm" + baseCurrency),
                    ActualStake = reader.ToDecimal("BTActualStake" + baseCurrency),
                    BuyBackAmountSB = reader.ToDecimal("BuyBackAmtSB" + baseCurrency),
                    BuyBackAmountBA = reader.ToDecimal("BuyBackAmtBA" + baseCurrency),
                    LicenseeWL = ReadLicenseeWinLossList(reader, licenseeWLColumns),
                    TurnOver = ReadTurnoverList(reader, turnoverColumns),
                    TotalTransaction = reader.ToInt("betcount"),
                    RacingRevenue = reader.ToDecimal("licenseewlrb" + baseCurrency) + reader.ToDecimal("commrb" + baseCurrency),
                    BACommission = reader.ToDecimal("commba" + baseCurrency),
                    CashOutCommission = reader.ToDecimal("cashoutcomm" + baseCurrency),
                    BetTradeCommission = reader.ToDecimal("bettradecomm" + baseCurrency),
                    AmtDueToBetTrade = reader.ToDecimal("AmtDueToBT" + baseCurrency),
                    AmtDueToBA = reader.ToDecimal("amtduetoba" + baseCurrency),
                    AmtDueToRacing = reader.ToDecimal("amtduetorb" + baseCurrency),
                    Date = drilldown ? metaModel.DrillDownFromDate : ConvertHelper.ToDateTime(reader.ToString("reporttype"), isMonthly)
                };

                dataList.Add(item);
                ReadSummaryTotal(reader, metaModel.ProductOptions, summaryTotal, isFakeLicensee);
            }

            return new SortableReportResultModel<LicenseeWinLoss>
            {
                DataList = dataList,
                TotalDataList = CalculateTotal(dataList),
                SummaryTotal = summaryTotal
            };
        }
    }
}