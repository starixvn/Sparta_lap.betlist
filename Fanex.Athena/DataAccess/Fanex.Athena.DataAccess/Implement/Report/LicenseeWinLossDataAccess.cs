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
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
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
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #endregion ILicenseeWinLossDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
        /// <param name="summaryTotal">Summary value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
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

            var systemIdOrdinal = reader.TryGetOrdinal("username");
            var externalIdOrdinal = reader.TryGetOrdinal("nickname");
            var customerIdOrdinal = reader.TryGetOrdinal("custid");
            var index = 1;
            while (reader.Read())
            {
                // Read Item
                var licenseeWl = new Dictionary<string, decimal>();
                var turnOver = new Dictionary<string, decimal>();
                foreach (var product in metaModel.ProductOptions)
                {
                    var licenseeWlColumnName = "LicenseeWL" + product.Code + metaModel.BaseCurrency;
                    var turnOverColumnName = "turnover" + product.Code + metaModel.BaseCurrency;

                    if (reader.FieldExists(licenseeWlColumnName))
                    {
                        if (!product.Code.Equals(Product.BA) && !product.Code.Equals(Product.RB))
                        {
                            licenseeWl.Add(product.Code, reader.ToDecimal(licenseeWlColumnName));
                        }
                    }

                    if (reader.FieldExists(turnOverColumnName))
                    {
                        turnOver.Add(product.Code, reader.ToDecimal(turnOverColumnName));
                    }
                }

                var item = new LicenseeWinLoss
                {
                    SystemId = systemIdOrdinal >= 0 ? reader.ToString("username") : string.Empty,
                    ExternalId = externalIdOrdinal >= 0 ? reader.ToString("nickname") : string.Empty,
                    CustomerId = customerIdOrdinal >= 0 ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    CustomerWinLoss = reader.ToDecimal("winlost" + metaModel.BaseCurrency),
                    CustomerComm = reader.ToDecimal("playercomm" + metaModel.BaseCurrency),
                    LicenseeWL = licenseeWl,
                    TurnOver = turnOver,
                    TotalTransaction = reader.ToInt("betcount"),
                    RacingRevenue = reader.ToDecimal("licenseewlrb" + metaModel.BaseCurrency) + reader.ToDecimal("commrb" + metaModel.BaseCurrency),
                    BACommission = reader.ToDecimal("commba" + metaModel.BaseCurrency),
                    AmtDueToBA = reader.ToDecimal("amtduetoba" + metaModel.BaseCurrency),
                    AmtDueToRacing = reader.ToDecimal("amtduetorb" + metaModel.BaseCurrency),
                    Date = drilldown
                        ? metaModel.DrillDownFromDate
                        : ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                };

                result.Add(item);

                // Read Summary
                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                    summaryTotal[currency].CustomerComm += reader.ToDecimal("playercomm" + currency);
                    summaryTotal[currency].TotalTransaction += reader.ToInt("betcount");
                    summaryTotal[currency].RacingRevenue += reader.ToDecimal("licenseewlrb" + currency) + reader.ToDecimal("commrb" + currency);
                    summaryTotal[currency].BACommission += reader.ToDecimal("commba" + currency);
                    summaryTotal[currency].AmtDueToBA += reader.ToDecimal("amtduetoba" + currency);
                    summaryTotal[currency].AmtDueToRacing += reader.ToDecimal("amtduetorb" + currency);
                    decimal temp;
                    var licenseeWlSummary = new Dictionary<string, decimal>();
                    var turnOverSummary = new Dictionary<string, decimal>();
                    foreach (var product in metaModel.ProductOptions)
                    {
                        var licenseeWlColumnName = "LicenseeWL" + product.Code + currency;
                        var turnOverColumnName = "turnover" + product.Code + currency;

                        if (reader.FieldExists(licenseeWlColumnName))
                        {
                            if (!product.Code.Equals(Product.BA) && !product.Code.Equals(Product.RB))
                            {
                                summaryTotal[currency].LicenseeWL.TryGetValue(product.Code, out temp);
                                temp += reader.ToDecimal(licenseeWlColumnName);
                                licenseeWlSummary.Add(product.Code, temp);
                            }
                        }

                        if (reader.FieldExists(turnOverColumnName))
                        {
                            summaryTotal[currency].TurnOver.TryGetValue(product.Code, out temp);
                            temp += reader.ToDecimal(turnOverColumnName);
                            turnOverSummary.Add(product.Code, temp);
                        }
                    }

                    summaryTotal[currency].LicenseeWL = licenseeWlSummary;
                    summaryTotal[currency].TurnOver = turnOverSummary;
                }

                index++;
            }

            return result;
        }

        /// <summary>
        /// Update USP parameter.
        /// </summary>
        /// <param name="db">Database value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
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