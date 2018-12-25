// -----------------------------------------------------------------------
// <copyright file="WinLossByProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.DTO.Common;
    using Model.Core.Helper;
    using Model.Report.ResultModel;

    public class WinLossByProductDataAccess : BaseDataAccess, IWinLossByProductDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public WinLossByProductDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public WinLossByProductResultModel GetWinlossByProducts(WinLossByProductMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_WinlossByProduct";
            try
            {
                WinLossByProductResultModel data;
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("reporttype", DbType.Byte, metaModel.ReportBy);
                    db.AddInParameter("username", DbType.String, metaModel.Username);
                    db.AddOutParameter("SystemName", DbType.String, 100);
                    db.AddOutParameter("CustID", DbType.Int32, 10);
                    UpdateUspParams(db, metaModel, false);
                    AddResultParam(db);
                    using (var reader = db.ExecuteReader())
                    {
                        data = GetData(reader, currencyList, metaModel, false);
                    }

                    var resultInfo = GetResult(db);
                    resultInfo.ExtendedData = db.GetParameterValue("CustID") + ";" + db.GetParameterValue("SystemName");
                    data.ResultInfo = resultInfo;
                }

                return data;
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public WinLossByProductResultModel GetWinlossByProductAgents(WinLossByProductMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_WinlossByProductDetail_Ag";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel, true);
                    db.AddInParameter("CustID", DbType.Int32, metaModel.MemberId);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, currencyList, metaModel, true);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public WinLossByProductResultModel GetWinlossByProductCustomers(WinLossByProductMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_WinlossByProductDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, metaModel.CustomerId);
                    db.AddInParameter("MemberID", DbType.Int32, metaModel.MemberId);
                    UpdateUspParams(db, metaModel, true);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, currencyList, metaModel, true);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static void ReadSummary(IDataReader reader, IDictionary<string, IList<WinLossByProduct>> summaryTotalTemp)
        {
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                var code = reader.ToString("productcode");
                var summaryItem = new WinLossByProduct
                {
                    ProductCode = code,
                    ProductName = ProductHelper.GetProductName(code),
                    TurnOver = reader.ToDecimal("turnover" + currency.ToLowerInvariant()),
                    CustomerWinLoss = reader.ToDecimal("winlost" + currency.ToLowerInvariant()),
                    CustomerComm = reader.ToDecimal("playercomm" + currency.ToLowerInvariant()),
                    LicenseeWL = reader.ToDecimal("licenseewl" + currency.ToLowerInvariant()),
                    TotalTransaction = reader.ToInt("betcount"),
                    AmountDue = reader.ToDecimal("amtdueto3rd" + currency.ToLowerInvariant()),
                    LicenseeWLComm = reader.ToDecimal("comm" + currency.ToLowerInvariant()),
                };
                summaryTotalTemp[currency].Add(summaryItem);
            }
        }

        private static IDictionary<string, IEnumerable<WinLossByProduct>> CalculateSummaryTotal(
            IDictionary<string, IList<WinLossByProduct>> summaryTotalTemp)
        {
            var summaryTotal = new Dictionary<string, IEnumerable<WinLossByProduct>>();
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

            return summaryTotal;
        }

        private static IEnumerable<WinLossByProduct> CalculateTotal(IEnumerable<WinLossByProduct> dataList)
        {
            var temp = from item in dataList
                       group item by new { item.CurrencyName, item.ProductCode } into g
                       select new WinLossByProduct
                       {
                           ProductCode = g.Key.ProductCode,
                           ProductName = g.First().ProductName,
                           CurrencyName = g.Key.CurrencyName,
                           AmountDue = g.Sum(i => i.AmountDue),
                           CustomerComm = g.Sum(i => i.CustomerComm),
                           CustomerWinLoss = g.Sum(i => i.CustomerWinLoss),
                           LicenseeWL = g.Sum(i => i.LicenseeWL),
                           TurnOver = g.Sum(i => i.TurnOver),
                           TotalTransaction = g.Sum(i => i.TotalTransaction),
                           LicenseeWLComm = g.Sum(i => i.LicenseeWLComm),
                       };
            return temp.OrderBy(i => i.CurrencyName).ToList();
        }

        private void UpdateUspParams(IObjectDb db, WinLossByProductMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate.Date : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate.Date : metaModel.ToDate.Date);
            db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
            db.AddInParameter("movebetsday", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            db.AddInParameter("productList", DbType.String, string.Join(",", metaModel.ProductOptions.Where(x => x.Status).Select(x => x.Code)));
        }

        private WinLossByProductResultModel GetData(
            IDataReader reader,
             IDictionary<int, string> currencyList,
            WinLossByProductMetaModel metaModel,
            bool drilldown)
        {
            var result = new List<WinLossByProduct>();
            var summaryTotalTemp = new Dictionary<string, IList<WinLossByProduct>>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotalTemp.Add(currency, new List<WinLossByProduct>());
            }

            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");

            while (reader.Read())
            {
                var currencyId = reader.ToInt("currency");
                var productCode = reader.ToString("productcode");
                var item = new WinLossByProduct
                {
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    CustomerId = hasCustomerId ? reader.ToInt("custid") : 0,
                    ProductCode = productCode,
                    ProductName = ProductHelper.GetProductName(productCode),
                    CurrencyId = currencyId,
                    CurrencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),
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

                ReadSummary(reader, summaryTotalTemp);
            }

            return new WinLossByProductResultModel
            {
                DataList = result,
                TotalData = CalculateTotal(result),
                SummaryTotal = CalculateSummaryTotal(summaryTotalTemp),
                ResultInfo = new ResultInfo()
            };
        }
    }
}