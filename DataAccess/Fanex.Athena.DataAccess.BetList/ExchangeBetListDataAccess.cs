namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Common.Extensions;
    using Core;
    using Core.Exception;
    using Data;
    using Extension.Helper;
    using Extension.Utility;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Session.Interfaces;

    public class ExchangeBetListDataAccess : BaseDataAccess, IExchangeBetListDataAccess
    {
        public ExchangeBetListResult GetBetListData(ExchangeBetListMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_Exchange_Mem";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("CustId", DbType.Int32, metaModel.CustId);
                    db.AddInParameter("FromDate", DbType.Date, metaModel.FromDate);
                    db.AddInParameter("ToDate", DbType.Date, metaModel.ToDate);
                    db.AddOutParameter("SumWinLost", DbType.Currency, 8);
                    db.AddOutParameter("SumWinLostEUR", DbType.Currency, 8);
                    db.AddOutParameter("SumWinLostGBP", DbType.Currency, 8);
                    db.AddOutParameter("SumWinLostUSD", DbType.Currency, 8);

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
            catch (ObjectDbException exception)
            {
                throw new DataAccessException(exception, UspName, metaModel);
            }
        }

        public IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_Exchange_Detail";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("CustId", DbType.Int32, metaModel.CustId);
                    db.AddInParameter("BetID", DbType.Int64, metaModel.BetId);

                    using (var reader = db.ExecuteReader())
                    {
                        var baseCurrency = GetBaseCurrency(metaModel.BaseCurrency);

                        return ReadTicketDetailData(reader, baseCurrency, metaModel.Type);
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                throw new DataAccessException(exception, UspName, metaModel);
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

                data.Liability = type == ExchangeMatchType.Lay ? NumberHelper.DecFormat((data.OddsMatched - 1) * data.Stake, 2) : "-";
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