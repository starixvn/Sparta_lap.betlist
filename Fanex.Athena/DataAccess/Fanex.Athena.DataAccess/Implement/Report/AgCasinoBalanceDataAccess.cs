// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalanceDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Ag casino win loss data access.
    /// </summary>
    public class AgCasinoBalanceDataAccess : BaseDataAccess, IAgCasinoBalanceDataAccess
    {
        public AgCasinoBalanceDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region IAgCasinoBalanceDataAccess Members

        /// <summary>
        /// Get summary win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<AgCasinoBalance> GetSummaryWinloss(AgCasinoBalanceMetaModel metaModel, out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var uspName = "Lrf_Rpt_AGCasinoBalance";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, false);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
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
        public SortableList<AgCasinoBalance> GetCustomerWinlossDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var uspName = "LRF_Rpt_AGCasinoBalance_Mem";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.DrillDownCustomerId);
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
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
        public SortableList<AgCasinoBalance> GetAgentWinlossDetail(
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var uspName = "LRF_Rpt_AGCasinoBalance_Ag";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, true);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #endregion IAgCasinoBalanceDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<AgCasinoBalance> GetData(
            IDataReader reader,
            AgCasinoBalanceMetaModel metaModel,
            out IDictionary<string, AgCasinoBalance> summaryTotal)
        {
            var result = new SortableList<AgCasinoBalance>();
            var summaryCurrencyList = new List<string> { "EUR", "USD", "GBP" };

            summaryTotal = new Dictionary<string, AgCasinoBalance>();
            foreach (var c in summaryCurrencyList)
            {
                summaryTotal.Add(c, new AgCasinoBalance());
            }

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var openingBalanceColumn = "OpeningBalance" + metaModel.BaseCurrency;
            var transferInColumn = "TransferIn" + metaModel.BaseCurrency;
            var transferOutColumn = "TransferOut" + metaModel.BaseCurrency;
            var closingBalanceColumn = "ClosingBalance" + metaModel.BaseCurrency;

            var systemIdOrdinal = reader.TryGetOrdinal("username");
            var externalIdOrdinal = reader.TryGetOrdinal("nickname");
            var customerIdOrdinal = reader.TryGetOrdinal("custid");
            var winlostDateOrdinal = reader.TryGetOrdinal("date");
            var index = 1;
            while (reader.Read())
            {
                var item = new AgCasinoBalance
                {
                    Index = index++,
                    SystemId = systemIdOrdinal >= 0 ? reader.ToString("username") : string.Empty,
                    ExternalId = externalIdOrdinal >= 0 ? reader.ToString("nickname") : string.Empty,
                    CustomerId = customerIdOrdinal >= 0 ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    Date = winlostDateOrdinal >= 0 ? (DateTime?)reader.ToDateTime("date") : metaModel.DrillDownFromDate,
                    OpeningBalance = reader.ToDecimal(openingBalanceColumn),
                    TransferIn = reader.ToDecimal(transferInColumn),
                    TransferOut = reader.ToDecimal(transferOutColumn),
                    ClosingBalance = reader.ToDecimal(closingBalanceColumn),
                };

                result.Add(item);

                // Read Summary
                foreach (var currencySuffix in summaryCurrencyList)
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

            return result;
        }

        /// <summary>
        /// Update USP parameter.
        /// </summary>
        /// <param name="db">Database value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
        private void UpdateUspParams(ObjectDb db, AgCasinoBalanceMetaModel metaModel, bool drilldown)
        {
            var currencyId = drilldown ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            currencyId = currencyId < 0 ? 0 : currencyId;

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("currency", DbType.Int32, currencyId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
        }

        #endregion Private
    }
}