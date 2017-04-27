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
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class ThirdPartyWinLossDataAccess : BaseDataAccess, IThirdPartyWinLossDataAccess
    {
        public SortableList<ThirdPartyWinLoss> GetSummaryWinloss(ThirdPartyWinLossMetaModel metaModel, out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            var uspName = "Lrf_Rpt_ThirdPartyWinloss";
            try
            {
                using (var db = this.GetObjectDb(uspName))
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

        public SortableList<ThirdPartyWinLoss> GetCustomerWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            var uspName = "LRF_Rpt_ThirdPartyWinloss_Mem";
            try
            {
                using (var db = this.GetObjectDb(uspName))
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

        public SortableList<ThirdPartyWinLoss> GetAgentWinlossDetail(
            ThirdPartyWinLossMetaModel metaModel,
            out IDictionary<string, ThirdPartyWinLoss> summaryTotal)
        {
            var uspName = "LRF_Rpt_ThirdPartyWinloss_Ag";
            try
            {
                using (var db = this.GetObjectDb(uspName))
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

        #region Private

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
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

        private static void UpdateUspParams(IObjectDb db, ThirdPartyWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml); 
            db.AddInParameter("productid", DbType.Int32, metaModel.ProductId);
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
        }

        #endregion Private
    }
}