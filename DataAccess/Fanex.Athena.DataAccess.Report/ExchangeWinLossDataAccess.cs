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
    using Extension.Utility;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class ExchangeWinLossDataAccess : BaseDataAccess, IExchangeWinLossDataAccess
    {
        public ReportResultModel<ExchangeWinLoss> GetMainWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_ExchangeWinloss";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    db.SetParameterValue("ReportType", metaModel.ReportBy);
                    UpdateUspParams(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, currencyList);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public ReportResultModel<ExchangeWinLoss> GetCustomerWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_ExchangeWinloss_Mem";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    db.SetParameterValue("CustID", metaModel.CustId);
                    UpdateUspParams(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, currencyList);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public ReportResultModel<ExchangeWinLoss> GetLicenseeWinloss(ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspName = "Lrf_Rpt_ExchangeWinloss_Ag";
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, currencyList);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static void UpdateUspParams(IObjectDb db, ExchangeWinLossMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("Fromdate", metaModel.FromDate.Date);
            db.SetParameterValue("Todate", metaModel.ToDate.Date);
            db.SetParameterValue("LicenseeId", metaModel.LicenseeId);
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
                detail.LicenseeNetWL += reader.ToDecimal("LicNetWL" + currency);
                detail.LicenseeComm += reader.ToDecimal("AgentComm" + currency);
                detail.AmountDueTo += reader.ToDecimal("AmountDueTo" + currency);
                detail.TotalTransaction += reader.ToInt("TotalTrans");
            }
        }

        private static IEnumerable<ExchangeWinLoss> CalculateTotal(IEnumerable<ExchangeWinLoss> dataList)
        {
            if (!dataList.Any())
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
                LicenseeNetWL = g.Sum(x => x.LicenseeNetWL),
                LicenseeComm = g.Sum(x => x.LicenseeComm),
                AmountDueTo = g.Sum(x => x.AmountDueTo),
                NetTurnover = g.Sum(x => x.NetTurnover)
            }).ToList();
        }

        private ReportResultModel<ExchangeWinLoss> GetData(IDataReader reader, ExchangeWinLossMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            var dataList = new List<ExchangeWinLoss>();
            var summaryTotal = SystemConstants.BaseCurrencyList.ToDictionary(x => x, x => new ExchangeWinLoss());
            var hasSystemId = reader.HasColumn("username");
            var hasExternalId = reader.HasColumn("nickname");
            var hasCustomerId = reader.HasColumn("custid");
            var hasWinlostdate = reader.HasColumn("Winlostdate");
            var baseCurrency = metaModel.BaseCurrency.ToUpperInvariant();

            while (reader.Read())
            {
                var currencyId = reader.ToInt("CurrencyID");
                var item = new ExchangeWinLoss
                {
                    Date = hasWinlostdate ? reader.ToDateTime("Winlostdate") : metaModel.ToDate,
                    CurrencyName = currencyList?.GetValueOrDefault(currencyId, currencyId.ToString()),
                    CurrencyId = currencyId,
                    ExternalId = hasExternalId ? reader.ToString("nickname") : string.Empty,
                    SystemId = hasSystemId ? reader.ToString("username") : string.Empty,
                    CustId = hasCustomerId ? reader.ToInt("CustID") : 0,
                    TotalTransaction = reader.ToInt("TotalTrans"),
                    Turnover = reader.ToDecimal("TurnOver" + baseCurrency),
                    NetTurnover = reader.ToDecimal("NetTurnOver" + baseCurrency),
                    CustomerWL = reader.ToDecimal("Winlost" + baseCurrency),
                    CustomerComm = reader.ToDecimal("CustComm" + baseCurrency),
                    LicenseeWL = reader.ToDecimal("LicenseeWinlost" + baseCurrency),
                    LicenseeNetWL = reader.ToDecimal("LicNetWL" + baseCurrency),
                    LicenseeComm = reader.ToDecimal("AgentComm" + baseCurrency),
                    AmountDueTo = reader.ToDecimal("AmountDueTo" + baseCurrency)
                };

                dataList.Add(item);
                ReadSummaryTotal(reader, summaryTotal);
            }

            return new ReportResultModel<ExchangeWinLoss>
            {
                DataList = dataList,
                TotalDataList = CalculateTotal(dataList),
                SummaryTotal = summaryTotal,
                AdditionalInfo = baseCurrency
            };
        }
    }
}