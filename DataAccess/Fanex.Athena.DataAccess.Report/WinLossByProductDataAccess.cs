// -----------------------------------------------------------------------
// <copyright file="WinLossByProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Model.Core.Helper;

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
        /// <param name="resultInfo">Result information.</param>
        /// <returns>The result value.</returns>
        public IList<WinLossByProduct> GetWinlossByProducts(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal,
            out IResultInfo resultInfo)
        {
            const string UspName = "Lrf_Rpt_WinlossByProduct";
            try
            {
                IList<WinLossByProduct> data;
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("reporttype", DbType.Byte, metaModel.ReportBy);
                    db.AddInParameter("username", DbType.String, metaModel.Username);
                    db.AddOutParameter("SystemName", DbType.String, 100);
                    db.AddOutParameter("CustID", DbType.Int32, 10);
                    UpdateUspParams(db, metaModel, false);
                    AddResultParam(db);
                    using (var reader = db.ExecuteReader())
                    {
                        data = GetData(reader, metaModel, false, out summaryTotal);
                    }

                    resultInfo = GetResult(db);
                    resultInfo.ExtendedData = db.GetParameterValue("CustID") + ";" + db.GetParameterValue("SystemName");
                }

                return data;
            }
            catch (ObjectDbException sqlException)
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
                    db.AddInParameter("CustID", DbType.Int32, metaModel.MemberId);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, true, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
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
                    db.AddInParameter("MemberID", DbType.Int32, metaModel.MemberId);
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
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        #endregion IWinLossByProductDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="drilldown">Drill down value.</param>
        /// <param name="summaryTotal">Summary total value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static IList<WinLossByProduct> GetData(
            IDataReader reader,
            WinLossByProductMetaModel metaModel,
            bool drilldown,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal)
        {
            var result = new List<WinLossByProduct>();
            var summaryTotalTemp = new Dictionary<string, IList<WinLossByProduct>>();
            summaryTotal = new Dictionary<string, IList<WinLossByProduct>>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotalTemp.Add(currency, new List<WinLossByProduct>());
            }

            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");

            while (reader.Read())
            {
                var productCode = reader.ToString("productcode");
                var item = new WinLossByProduct
                {
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    ProductCode = productCode,
                    ProductName = ProductHelper.GetProductName(productCode),
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
                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    var code = reader.ToString("productcode");
                    var summaryItem = new WinLossByProduct
                    {
                        ProductCode = code,
                        ProductName = ProductHelper.GetProductName(code),
                        TurnOver = reader.ToDecimal("turnover" + currency.ToLower()),
                        CustomerWinLoss = reader.ToDecimal("winlost" + currency.ToLower()),
                        CustomerComm = reader.ToDecimal("playercomm" + currency.ToLower()),
                        LicenseeWL = reader.ToDecimal("licenseewl" + currency.ToLower()),
                        TotalTransaction = reader.ToInt("betcount"),
                        AmountDue = reader.ToDecimal("amtdueto3rd" + currency.ToLower()),
                        LicenseeWLComm = reader.ToDecimal("comm" + currency.ToLower()),
                    };
                    summaryTotalTemp[currency].Add(summaryItem);
                }
            }

            // Calculate summary
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal[currency] = summaryTotalTemp[currency].GroupBy(i => new { i.ProductCode })
                                    .Select(i => new WinLossByProduct
                                    {
                                        ProductCode = i.Key.ProductCode,
                                        ProductName = i.First().ProductName,
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

        #endregion Private
    }
}