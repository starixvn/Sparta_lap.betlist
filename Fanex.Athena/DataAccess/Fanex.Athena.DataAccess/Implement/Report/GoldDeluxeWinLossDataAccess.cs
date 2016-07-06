// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Gold deluxe win loss data access.
    /// </summary>
    public class GoldDeluxeWinLossDataAccess : BaseDataAccess, IGoldDeluxeWinLossDataAccess
    {
        private const int GoldDeluxeProductId = 11;

        public GoldDeluxeWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region IGoldDeluxeWinLossDataAccess Members

        /// <summary>
        /// Get summary win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<GoldDeluxeWinLoss> GetSummaryWinloss(GoldDeluxeWinLossMetaModel metaModel, out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_ThirdPartyWinloss";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel, false);
                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
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
        public SortableList<GoldDeluxeWinLoss> GetCustomerWinlossDetail(
            GoldDeluxeWinLossMetaModel metaModel,
            out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            var uspName = "LRF_Rpt_ThirdPartyWinloss_Mem";
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
        public SortableList<GoldDeluxeWinLoss> GetAgentWinlossDetail(
            GoldDeluxeWinLossMetaModel metaModel,
            out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            var uspName = "LRF_Rpt_ThirdPartyWinloss_Ag";
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #endregion IGoldDeluxeWinLossDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<GoldDeluxeWinLoss> GetData(
            IDataReader reader,
            GoldDeluxeWinLossMetaModel metaModel,
            out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            var result = new SortableList<GoldDeluxeWinLoss>();

            summaryTotal = new Dictionary<string, GoldDeluxeWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new GoldDeluxeWinLoss());
            }

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;

            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");
            var hasWinlostDate = reader.HasColumn("winlostdate");
            var index = 1;
            while (reader.Read())
            {
                var item = new GoldDeluxeWinLoss
                {
                    Index = index++,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    CurrencyId = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    Date = hasWinlostDate ? (DateTime?)reader.ToDateTime("winlostdate") : metaModel.DrillDownFromDate
                };

                result.Add(item);

                // Read Summary
                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                    summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                    summaryTotal[currency].LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                    summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
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
        private void UpdateUspParams(ObjectDb db, GoldDeluxeWinLossMetaModel metaModel, bool drilldown)
        {
            var currencyId = drilldown ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            currencyId = currencyId < 0 ? 0 : currencyId;

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("productid", DbType.Int32, GoldDeluxeProductId);
            db.AddInParameter("currency", DbType.Int32, currencyId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
        }

        #endregion Private
    }
}