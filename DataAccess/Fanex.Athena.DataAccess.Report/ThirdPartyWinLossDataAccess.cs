// -----------------------------------------------------------------------
// <copyright file="ThirdPartyWinLossDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
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

    public class ThirdPartyWinLossDataAccess<T> : BaseDataAccess, IThirdPartyWinLossDataAccess<T> where T : ThirdPartyWinLoss
    {
        public ThirdPartyWinLossDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public ReportResultModel<T> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, string spname)
        {
            try
            {
                using (var db = this.GetObjectDb(spname))
                {
                    UpdateUspParams(db, metaModel, false);
                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);

                    using (var reader = db.ExecuteReader())
                    {
                        var result = GetData(reader, metaModel, -1);
                        AddRowSpan(result.DataList);
                        return result;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, spname, metaModel);
            }
        }

        public ReportResultModel<T> GetCustomerWinlossDetail(ThirdPartyWinLossMetaModel metaModel, string spname)
        {
            try
            {
                using (var db = this.GetObjectDb(spname))
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
                throw new DataAccessException(sqlException, spname, metaModel);
            }
        }

        public ReportResultModel<T> GetAgentWinlossDetail(ThirdPartyWinLossMetaModel metaModel, string spname)
        {
            try
            {
                using (var db = this.GetObjectDb(spname))
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
                throw new DataAccessException(sqlException, spname, metaModel);
            }
        }

        private static void AddRowSpan(IEnumerable<T> dataList)
        {
            DateTime? date = new DateTime();
            var index = 1;
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
        }

        [CLSCompliant(false)]
        protected virtual void UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            if (metaModel.ProductId > 0)
            {
                db.AddInParameter("productid", DbType.Int32, metaModel.ProductId);
            }
        }

        protected virtual void ReadSummaryTotal(IDataReader reader, IDictionary<string, T> summaryTotal)
        {
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                var item = summaryTotal[currency];
                item.CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                item.TotalTransaction += reader.ToInt("totaltrans");
                item.LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                item.TurnOver += reader.ToDecimal("turnover" + currency);
            }
        }

        private ReportResultModel<T> GetData(IDataReader reader, ThirdPartyWinLossMetaModel metaModel, int rowSpan = 0)
        {
            var result = new SortableList<T>();

            var summaryTotal = new Dictionary<string, T>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, Activator.CreateInstance<T>());
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
                var currencyId = reader.ToInt("currency");
                var item = Activator.CreateInstance<T>();

                item.Index = index++;
                item.SystemId = hasSystemId ? reader.ToString("username") : string.Empty;
                item.ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty;
                item.CustomerId = hasCustomerId ? reader.ToInt("custid") : 0;
                item.CurrencyId = currencyId;
                item.CurrencyName = GetCurrencyName(currencyId);
                item.CustomerWinLoss = reader.ToDecimal(winlostColumn);
                item.LicenseeWL = reader.ToDecimal(licenseewlColumn);
                item.TurnOver = reader.ToDecimal(turnoverColumn);
                item.TotalTransaction = reader.ToInt("totaltrans");
                item.Date = hasWinlostDate ? (DateTime?)reader.ToDateTime("winlostdate") : metaModel.DrillDownFromDate;
                item.RowSpan = rowSpan;
                PostReadItem(reader, item, metaModel.BaseCurrency);

                result.Add(item);
                ReadSummaryTotal(reader, summaryTotal);
            }

            return new ReportResultModel<T>
            {
                DataList = result,
                TotalDataList = CalculateTotal(result),
                SummaryTotal = summaryTotal
            };
        }

        protected virtual void PostReadItem(IDataReader reader, T item, string baseCurrency)
        {
            // Read other columns for other products.
        }

        protected virtual IEnumerable<T> CalculateTotal(IEnumerable<T> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<T>();
            }

            return dataList.GroupBy(d => d.CurrencyName).Select(g =>
            {
                var item = Activator.CreateInstance<T>();
                item.CurrencyName = g.Key;
                item.TurnOver = g.Sum(p => p.TurnOver);
                item.TotalTransaction = g.Sum(p => p.TotalTransaction);
                item.CustomerWinLoss = g.Sum(p => p.CustomerWinLoss);
                item.LicenseeWL = g.Sum(p => p.LicenseeWL);
                return item;
            }).ToList();
        }
    }
}