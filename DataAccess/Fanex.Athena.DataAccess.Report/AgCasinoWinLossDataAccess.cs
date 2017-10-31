// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;

    public class AgCasinoWinLossDataAccess : BaseDataAccess, IAgCasinoWinLossDataAccess
    {
        public ReportResultModel<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_AGCasinoWinloss";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel, false);

                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public ReportResultModel<AgCasinoWinLoss> GetCustomerWinlossDetail(
            AgCasinoWinLossMetaModel metaModel)
        {
            const string UspName = "LRF_Rpt_AGCasinoWinloss_Mem";
            try
            {
                using (var db = GetObjectDb(UspName))
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
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public ReportResultModel<AgCasinoWinLoss> GetAgentWinlossDetail(
            AgCasinoWinLossMetaModel metaModel)
        {
            const string UspName = "LRF_Rpt_AGCasinoWinloss_Ag";
            try
            {
                using (var db = GetObjectDb(UspName))
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
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static ReportResultModel<AgCasinoWinLoss> GetData(IDataReader reader, AgCasinoWinLossMetaModel metaModel)
        {
            var dataList = new List<AgCasinoWinLoss>();
            var summaryTotal = new Dictionary<string, AgCasinoWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new AgCasinoWinLoss());
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
                dataList.Add(new AgCasinoWinLoss
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
                    Date = hasWinlostDate ? reader.ToDateTime("winlostdate") : metaModel.DrillDownFromDate
                });
                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                    summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                    summaryTotal[currency].LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                    summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                }
            }

            return new ReportResultModel<AgCasinoWinLoss>
            {
                DataList = dataList,
                SummaryTotal = summaryTotal
            };
        }

        private static void UpdateUspParams(IObjectDb db, AgCasinoWinLossMetaModel metaModel, bool drilldown)
        {
            db.AddInParameter("fromdate", DbType.Date, drilldown ? metaModel.DrillDownFromDate : metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, drilldown ? metaModel.DrillDownToDate : metaModel.ToDate.Date);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}