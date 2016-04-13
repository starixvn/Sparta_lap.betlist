// -----------------------------------------------------------------------
// <copyright file="ThirdPartyWinLossDataAccess.cs" company="Microsoft">
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
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ThirdPartyWinLossDataAccess : BaseDataAccess, IThirdPartyWinLossDataAccess
    {
        public ThirdPartyWinLossDataAccess(BalanceUp balanceUp)
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
        public SortableList<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
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
        public SortableList<ThirdPartyWinLoss> GetCustomerWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
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
        public SortableList<ThirdPartyWinLoss> GetAgentWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<ThirdPartyWinLoss> GetData(
            IDataReader reader,
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            var result = new SortableList<ThirdPartyWinLoss>();

            summaryTotal = new Dictionary<string, ThirdPartyWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new ThirdPartyWinLoss());
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
                var item = new ThirdPartyWinLoss
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
        private void UpdateUspParams(ObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            var currencyId = drilldown ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            currencyId = currencyId < 0 ? 0 : currencyId;

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("productid", DbType.Int32, metaModel.ProductId);
            db.AddInParameter("currency", DbType.Int32, currencyId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
        }

        #endregion Private
    }
}
