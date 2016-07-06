// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Licensee win loss data access.
    /// </summary>
    public class LicenseeWinLossDataAccess : BaseDataAccess, ILicenseeWinLossDataAccess
    {
        private static readonly string[] IgnoredLicenseeWLProducts = new string[] { Product.BA, Product.RB };
        private static readonly string[] IgnoredTurnoverProducts = new string[] { Product.BT };

        public LicenseeWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region ILicenseeWinLossDataAccess Members

        /// <summary>
        /// Get summary win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<LicenseeWinLoss> GetSummaryWinloss(LicenseeWinLossMetaModel metaModel, out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_LicWinloss";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("reporttype", DbType.Byte, metaModel.ReportBy);
                    UpdateUspParams(db, metaModel, false);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, false, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        /// <summary>
        /// Get customer win loss detail.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<LicenseeWinLoss> GetCustomerWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_LicWinlossDetail";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, true, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        /// <summary>
        /// Get agent win loss detail.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<LicenseeWinLoss> GetAgentWinlossDetail(
            LicenseeWinLossMetaModel metaModel,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_LicWinlossDetail_Ag";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, true, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #endregion ILicenseeWinLossDataAccess Members

        #region Private

        private static bool IsIgnoredLicenseeWLProduct(string productCode)
        {
            return IgnoredLicenseeWLProducts.Contains(productCode);
        }

        private static bool IsIgnoredTurnoverProduct(string productCode)
        {
            return IgnoredTurnoverProducts.Contains(productCode);
        }

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
        /// <param name="summaryTotal">Summary value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<LicenseeWinLoss> GetData(
            IDataReader reader,
            LicenseeWinLossMetaModel metaModel,
            bool drilldown,
            out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var result = new SortableList<LicenseeWinLoss>();
            summaryTotal = new Dictionary<string, LicenseeWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(
                    currency,
                    new LicenseeWinLoss
                    {
                        LicenseeWL = new Dictionary<string, decimal>(),
                        TurnOver = new Dictionary<string, decimal>()
                    });
            }

            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");
            var index = 1;
            var baseCurrency = metaModel.BaseCurrency;
            var licenseeWLColumns = new Dictionary<string, string>();
            var turnoverColumns = new Dictionary<string, string>();
            if (reader.FieldCount > 0)
            {
                foreach (var product in metaModel.ProductOptions)
                {
                    var licenseeWlColumnName = "LicenseeWL" + product.Code + baseCurrency;
                    var turnOverColumnName = "turnover" + product.Code + baseCurrency;

                    if (!IsIgnoredLicenseeWLProduct(product.Code) &&
                        reader.HasColumn(licenseeWlColumnName))
                    {
                        licenseeWLColumns.Add(licenseeWlColumnName, product.Code);
                    }

                    if (!IsIgnoredTurnoverProduct(product.Code) && reader.HasColumn(turnOverColumnName))
                    {
                        turnoverColumns.Add(turnOverColumnName, product.Code);
                    }
                }
            }

            while (reader.Read())
            {
                var licenseeWl = new Dictionary<string, decimal>();
                var turnOver = new Dictionary<string, decimal>();
                foreach (var column in licenseeWLColumns)
                {
                    licenseeWl.Add(column.Value, reader.ToDecimal(column.Key));
                }

                foreach (var column in turnoverColumns)
                {
                    turnOver.Add(column.Value, reader.ToDecimal(column.Key));
                }

                var item = new LicenseeWinLoss
                {
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    CustomerWinLoss = reader.ToDecimal("winlost" + baseCurrency),
                    CustomerComm = reader.ToDecimal("playercomm" + baseCurrency),
                    ActualStake = reader.ToDecimal("BTActualStake" + baseCurrency),
                    BuyBackAmountSB = reader.ToDecimal("BuyBackAmtSB" + baseCurrency),
                    BuyBackAmountBA = reader.ToDecimal("BuyBackAmtBA" + baseCurrency),
                    LicenseeWL = licenseeWl,
                    TurnOver = turnOver,
                    TotalTransaction = reader.ToInt("betcount"),
                    RacingRevenue = reader.ToDecimal("licenseewlrb" + baseCurrency) + reader.ToDecimal("commrb" + baseCurrency),
                    BACommission = reader.ToDecimal("commba" + baseCurrency),
                    BetTradeCommission = reader.ToDecimal("bettradecomm" + baseCurrency),
                    AmtDueToBetTrade = reader.ToDecimal("AmtDueToBT" + baseCurrency),
                    AmtDueToBA = reader.ToDecimal("amtduetoba" + baseCurrency),
                    AmtDueToRacing = reader.ToDecimal("amtduetorb" + baseCurrency),
                    Date = drilldown
                        ? metaModel.DrillDownFromDate
                        : ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item);

                ReadSummaryTotal(reader, metaModel.ProductOptions, summaryTotal);

                index++;
            }

            return result;
        }

        private static void ReadSummaryTotal(IDataReader reader, IList<ProductOption> productOptions, IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                var summaryItem = summaryTotal[currency];
                summaryItem.CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                summaryItem.CustomerComm += reader.ToDecimal("playercomm" + currency);
                summaryItem.TotalTransaction += reader.ToInt("betcount");
                summaryItem.RacingRevenue += reader.ToDecimal("licenseewlrb" + currency) + reader.ToDecimal("commrb" + currency);
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

        private void UpdateUspParams(ObjectDb db, LicenseeWinLossMetaModel metaModel, bool drilldown)
        {
            var currencyId = drilldown ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            currencyId = currencyId < 0 ? 0 : currencyId;

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("currencyid", DbType.Int32, currencyId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("isold", DbType.Boolean, metaModel.IsOld);
            db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            db.AddInParameter("movebetsday", DbType.DateTime, this.BalanceUp.MoveBetsDay);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        #endregion Private
    }
}