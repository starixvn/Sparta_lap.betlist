namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Extensions;
    using Core;
    using Core.Exception;
    using Core.Extensions;
    using Data;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class ExchangeWinLossDataAccess : BaseDataAccess, IExchangeWinLossDataAccess
    {
        public ReportResultModel<ExchangeWinLoss> GetMainWinloss(ExchangeWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExchangeWinloss";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    db.AddInParameter("ReportType", DbType.Byte, metaModel.ReportBy);
                    UpdateUspParams(db, metaModel);

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

        public ReportResultModel<ExchangeWinLoss> GetCustomerWinloss(ExchangeWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExchangeWinloss_Mem";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                    UpdateUspParams(db, metaModel);

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

        public ReportResultModel<ExchangeWinLoss> GetLicenseeWinloss(ExchangeWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ExchangeWinloss_Ag";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);

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

        private static IDictionary<string, ExchangeWinLoss> InitSummaryTotal()
        {
            return SystemConstants.BaseCurrencyList.ToDictionary(x => x, x => new ExchangeWinLoss());
        }

        private static void UpdateUspParams(IObjectDb db, ExchangeWinLossMetaModel metaModel)
        {
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("Fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("Todate", DbType.Date, metaModel.ToDate.Date);
        }

        private static void ReadSummaryTotal(IDataReader reader, IDictionary<string, ExchangeWinLoss> summaryTotal)
        {
            foreach (var item in summaryTotal)
            {
                var currency = item.Key;
                var detail = item.Value;
                detail.Turnover += reader.ToDecimal("TurnOver" + currency);
                detail.NetTurnover += reader.ToDecimal("NetTurnOver" + currency);
                detail.CustomerWL += reader.ToDecimal("Winlost" + currency);
                detail.CustomerComm += reader.ToDecimal("CustComm" + currency);
                detail.LicenseeWL += reader.ToDecimal("LicenseeWinlost" + currency);
                detail.LicenseeComm += reader.ToDecimal("AgentComm" + currency);
                detail.AmountDueTo += reader.ToDecimal("AmountDueTo" + currency);
                detail.TotalTransaction += reader.ToInt("TotalTrans");
            }
        }

        private static IEnumerable<ExchangeWinLoss> CalculateTotal(IEnumerable<ExchangeWinLoss> dataList)
        {
            if (dataList == null || !dataList.Any())
            {
                return new List<ExchangeWinLoss>();
            }

            return dataList.GroupBy(x => new { x.CurrencyId, x.CurrencyName }).Select(g => new ExchangeWinLoss
            {
                CurrencyId = g.Key.CurrencyId,
                CurrencyName = g.Key.CurrencyName,
                Turnover = g.Sum(x => x.Turnover),
                TotalTransaction = g.Sum(x => x.TotalTransaction),
                CustomerWL = g.Sum(x => x.CustomerWL),
                CustomerComm = g.Sum(x => x.CustomerComm),
                LicenseeWL = g.Sum(x => x.LicenseeWL),
                LicenseeComm = g.Sum(x => x.LicenseeComm),
                AmountDueTo = g.Sum(x => x.AmountDueTo),
                NetTurnover = g.Sum(x => x.NetTurnover)
            }).ToList();
        }

        private ReportResultModel<ExchangeWinLoss> GetData(IDataReader reader, ExchangeWinLossMetaModel metaModel)
        {
            var dataList = new List<ExchangeWinLoss>();
            var summaryTotal = InitSummaryTotal();
            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");
            var hasWinlostdate = reader.HasColumn("Winlostdate");

            while (reader.Read())
            {
                var item = new ExchangeWinLoss
                {
                    Date = hasWinlostdate ? reader.ToDateTime("Winlostdate") : metaModel.ToDate,
                    CurrencyName = reader.ToString("CurrencyName"),
                    CurrencyId = reader.ToInt("CurrencyID"),
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    CusId = hasCustomerId ? reader.ToInt("CustID") : 0,
                    Turnover = reader.ToDecimal("TurnOver"),
                    NetTurnover = reader.ToDecimal("NetTurnOver"),
                    CustomerWL = reader.ToDecimal("Winlost"),
                    CustomerComm = reader.ToDecimal("CustComm"),
                    LicenseeWL = reader.ToDecimal("LicenseeWinlost"),
                    LicenseeComm = reader.ToDecimal("AgentComm"),
                    TotalTransaction = reader.ToInt("TotalTrans"),
                    AmountDueTo = reader.ToDecimal("AmountDueTo")
                };

                dataList.Add(item);
                ReadSummaryTotal(reader, summaryTotal);
            }

            return new ReportResultModel<ExchangeWinLoss>
            {
                DataList = dataList,
                TotalDataList = CalculateTotal(dataList),
                SummaryTotal = summaryTotal
            };
        }
    }
}