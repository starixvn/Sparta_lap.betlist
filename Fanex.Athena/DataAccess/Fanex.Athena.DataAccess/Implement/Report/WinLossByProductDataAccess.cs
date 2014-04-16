// -----------------------------------------------------------------------
// <copyright file="WinLossByProductDataAccess.cs" company="Nexcel Solution">
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
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Win loss by product data access.
    /// </summary>
    public class WinLossByProductDataAccess : BaseDataAccess, IWinLossByProductDataAccess
    {
        public WinLossByProductDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region IWinLossByProductDataAccess Members

        /// <summary>
        /// Get win loss by product.
        /// </summary>
        /// <param name="metaModel"> Meta model value.</param>
        /// <param name="summaryTotal"> Summary total value.</param>
        /// <returns>The result value.</returns>
        public IList<WinLossByProduct> GetWinlossByProducts(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            const string UspName = "Lrf_Rpt_WinlossByProduct";
            try
            {
                using (var db = new ObjectDb(UspName))
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
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        /// <summary>
        /// Get win loss by product agent.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public IList<WinLossByProduct> GetWinlossByProductAgents(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            const string UspName = "Lrf_Rpt_WinlossByProductDetail_Ag";
            try
            {
                using (var db = new ObjectDb(UspName))
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
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        /// <summary>
        /// Get win loss by product customer.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        public IList<WinLossByProduct> GetWinlossByProductCustomers(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            const string UspName = "Lrf_Rpt_WinlossByProductDetail";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.CustomerId);
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
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        #endregion IWinLossByProductDataAccess Members

        #region Private

        /// <summary>
        /// Update USP parameter.
        /// </summary>
        /// <param name="db">Database value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
        private void UpdateUspParams(ObjectDb db, WinLossByProductMetaModel metaModel, bool drilldown)
        {
            var currencyId = drilldown ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            currencyId = currencyId < 0 ? 0 : currencyId;

            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("currencyid", DbType.Int32, currencyId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate.Date : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate.Date : metaModel.ToDate.Date);
            db.AddInParameter("isold", DbType.Boolean, metaModel.IsOld);
            db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
            db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);
            db.AddInParameter("movebetsday", DbType.DateTime, this.BalanceUp.MoveBetsDay);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private IList<WinLossByProduct> GetData(
            IDataReader reader,
            WinLossByProductMetaModel metaModel,
            bool drilldown,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            var result = new List<WinLossByProduct>();
            var summaryCurrencyList = new List<string> { "EUR", "USD" };
            var summaryTotalTemp = new Dictionary<string, IList<WinLossByProduct>>();
            summaryTotal = new Dictionary<string, IList<WinLossByProduct>>();
            foreach (var c in summaryCurrencyList)
            {
                summaryTotalTemp.Add(c, new List<WinLossByProduct>());
            }

            var systemIdOrdinal = reader.TryGetOrdinal("username");
            var externalIdOrdinal = reader.TryGetOrdinal("nickname");
            var customerIdOrdinal = reader.TryGetOrdinal("custid");

            while (reader.Read())
            {
                // Read Item
                var item = new WinLossByProduct
                    {
                        SystemId = systemIdOrdinal >= 0 ? reader.ToString("username") : string.Empty,
                        ExternalId = externalIdOrdinal >= 0 ? reader.ToString("nickname") : string.Empty,
                        CustomerId = customerIdOrdinal >= 0 ? reader.ToInt("custid") : 0,
                        ProductCode = reader.ToString("productcode"),
                        CurrencyId = reader.ToInt("currency"),
                        CurrencyName = reader.ToString("currencyname"),
                        TurnOver = reader.ToDecimal("turnover" + metaModel.BaseCurrency),
                        CustomerWinLoss = reader.ToDecimal("winlost" + metaModel.BaseCurrency),
                        CustomerComm = reader.ToDecimal("playercomm" + metaModel.BaseCurrency),
                        LicenseeWL = reader.ToDecimal("licenseewl" + metaModel.BaseCurrency),
                        TotalTransaction = reader.ToInt("betcount"),
                        AmountDue = reader.ToDecimal("amtdueto3rd" + metaModel.BaseCurrency),
                        LicenseeWLComm = reader.ToDecimal("comm" + metaModel.BaseCurrency),
                        Date = drilldown
                        ? metaModel.DrillDownFromDate
                        : ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly)
                    };
                result.Add(item);

                // Read Summary
                foreach (var c in summaryCurrencyList)
                {
                    var summaryItem = new WinLossByProduct
                        {
                            ProductCode = reader.ToString("productcode"),
                            TurnOver = reader.ToDecimal("turnover" + c.ToLower()),
                            CustomerWinLoss = reader.ToDecimal("winlost" + c.ToLower()),
                            CustomerComm = reader.ToDecimal("playercomm" + c.ToLower()),
                            LicenseeWL = reader.ToDecimal("licenseewl" + c.ToLower()),
                            TotalTransaction = reader.ToInt("betcount"),
                            AmountDue = reader.ToDecimal("amtdueto3rd" + c.ToLower()),
                            LicenseeWLComm = reader.ToDecimal("comm" + c.ToLower()),
                        };
                    summaryTotalTemp[c].Add(summaryItem);
                }
            }

            // Calculate summary
            foreach (var c in summaryCurrencyList)
            {
                summaryTotal[c] = summaryTotalTemp[c].GroupBy(i => new { i.ProductCode })
                                    .Select(i => new WinLossByProduct
                                    {
                                        ProductCode = i.Key.ProductCode,
                                        TurnOver = i.Sum(j => j.TurnOver),
                                        CustomerWinLoss = i.Sum(j => j.CustomerWinLoss),
                                        CustomerComm = i.Sum(j => j.CustomerComm),
                                        LicenseeWL = i.Sum(j => j.LicenseeWL),
                                        TotalTransaction = i.Sum(j => j.TotalTransaction),
                                        AmountDue = i.Sum(j => j.AmountDue),
                                        LicenseeWLComm = i.Sum(j => j.LicenseeWLComm),
                                    }).ToList();
            }

            return result;
        }

        #endregion Private
    }
}