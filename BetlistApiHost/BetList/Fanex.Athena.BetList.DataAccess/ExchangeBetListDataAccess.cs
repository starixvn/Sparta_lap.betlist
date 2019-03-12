namespace Fanex.Athena.BetList.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Constant;
    using Common.DataAccess;
    using Common.Utility.Helpers;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;

    public class ExchangeBetListDataAccess : BaseDataAccess, IExchangeBetListDataAccess
    {
        private const string ExchangeMatchTypeLay = "Lay";

        public ExchangeBetListResult GetBetListData(ExchangeBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_Exchange_Mem";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);
                db.SetParameterValue("SumWinLost", null);
                db.SetParameterValue("SumWinLostEUR", null);
                db.SetParameterValue("SumWinLostGBP", null);
                db.SetParameterValue("SumWinLostUSD", null);

                var baseCurrency = GetBaseCurrency(metaModel.BaseCurrency);
                var result = new ExchangeBetListResult();

                using (var reader = db.ExecuteReader())
                {
                    result.SumWinLossList = ReadSumWinLossData(reader, baseCurrency);
                    result.TicketList = ReadTicketData(reader, baseCurrency);
                }

                result.GrandTotal = Converter.ToDecimal(db.GetParameterValue("SumWinLost" + baseCurrency));
                return result;
            }
        }

        public IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_Exchange_Detail";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("BetID", metaModel.BetId);

                using (var reader = db.ExecuteReader())
                {
                    var baseCurrency = GetBaseCurrency(metaModel.BaseCurrency);

                    return ReadTicketDetailData(reader, baseCurrency, metaModel.Type);
                }
            }
        }

        private static string GetBaseCurrency(string baseCurrency) => SystemConstants.BaseCurrencyList.Contains(baseCurrency) ? baseCurrency : string.Empty;

        private IEnumerable<ExchangeTicketDetail> ReadTicketDetailData(IDataReader reader, string baseCurrency, string type)
        {
            var mainData = new List<ExchangeTicketDetail>();

            while (reader.Read())
            {
                var data = new ExchangeTicketDetail
                {
                    BetTaken = reader.ToDateTime("BetTaken"),
                    OddsReq = reader.ToDecimal("OddsRequest"),
                    Stake = reader.ToDecimal("Stake" + baseCurrency),
                    OddsMatched = reader.ToDecimal("OddsMatched")
                };

                data.Liability = type == ExchangeMatchTypeLay ? NumberHelper.DecFormat((data.OddsMatched - 1) * data.Stake, 2) : "-";
                mainData.Add(data);
            }

            return mainData;
        }

        private IEnumerable<ExchangeSumWinLoss> ReadSumWinLossData(IDataReader reader, string baseCurrency)
        {
            var mainData = new List<ExchangeSumWinLoss>();

            while (reader.Read())
            {
                mainData.Add(new ExchangeSumWinLoss
                {
                    UserId = reader.ToString("UserId"),
                    EventId = reader.ToInt("EventId"),
                    EventName = reader.ToString("EventName"),
                    EventTypeName = reader.ToString("EventTypeName"),
                    MarketId = reader.ToString("MarketId"),
                    MarketName = reader.ToString("MarketName"),
                    WinLost = reader.ToDecimal("WinLost" + baseCurrency),
                    Commission = reader.ToDecimal("Commission" + baseCurrency)
                });
            }

            return mainData;
        }

        private IEnumerable<ExchangeTicket> ReadTicketData(IDataReader reader, string baseCurrency)
        {
            var detailData = new List<ExchangeTicket>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    detailData.Add(new ExchangeTicket
                    {
                        UserId = reader.ToString("UserId"),
                        EventId = reader.ToInt("EventId"),
                        MarketId = reader.ToString("MarketId"),
                        BetId = reader.ToLong("BetId"),
                        SelectionName = reader.ToString("SelectionName"),
                        OddsAvg = reader.ToDecimal("OddsAvg"),
                        Stake = reader.ToDecimal("Stake" + baseCurrency),
                        Type = reader.ToString("Type"),
                        BetPlaced = reader.ToDateTime("BetPlaced"),
                        WinLost = reader.ToDecimal("WinLost" + baseCurrency)
                    });
                }
            }

            return detailData;
        }
    }
}