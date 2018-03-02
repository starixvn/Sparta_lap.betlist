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
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;

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
                    db.AddOutParameter("SumWinLost", DbType.Currency, 0);
                    db.AddOutParameter("SumWinLostEUR", DbType.Currency, 0);
                    db.AddOutParameter("SumWinLostMYR", DbType.Currency, 0);
                    db.AddOutParameter("SumWinLostUSD", DbType.Currency, 0);

                    using (var reader = db.ExecuteReader())
                    {
                        var baseCurrency = SystemConstants.BaseCurrencyList.Contains(metaModel.BaseCurrency) ? metaModel.BaseCurrency : string.Empty;

                        return new ExchangeBetListResult
                        {
                            SumWinLossList = ReadSumWinLossData(reader, baseCurrency),
                            TicketList = ReadTicketData(reader, baseCurrency),
                            GrandTotal = Converter.ToDecimal(db.GetParameterValue("SumWinLost" + baseCurrency))
                        };
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                throw new DataAccessException(exception, UspName, metaModel);
            }
        }

        public IEnumerable<ExchangeTicketDetail> GetTicketDetailList(ExchangeBetListMetaModel metaModel)
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
                        var baseCurrency = SystemConstants.BaseCurrencyList.Contains(metaModel.BaseCurrency) ? metaModel.BaseCurrency : string.Empty;

                        return ReadTicketDetailData(reader, baseCurrency, metaModel.Type);
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                throw new DataAccessException(exception, UspName, metaModel);
            }
        }

        private IEnumerable<ExchangeTicketDetail> ReadTicketDetailData(IDataReader reader, string baseCurrency, string type)
        {
            var mainData = new List<ExchangeTicketDetail>();

            while (reader.Read())
            {
                var data = new ExchangeTicketDetail
                {
                    BetTaken = reader.ToString("BetTaken"),
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
                var data = new ExchangeSumWinLoss
                {
                    UserId = reader.ToString("UserId"),
                    EvenId = reader.ToInt("EvenId"),
                    EvenName = reader.ToString("EvenName"),
                    EvenTypeName = reader.ToString("EvenTypeName"),
                    MarketId = reader.ToString("MarketId"),
                    MarketName = reader.ToString("MarketName"),
                    WinLost = reader.ToDecimal("WinLost" + baseCurrency),
                    Commission = reader.ToDecimal("Commission" + baseCurrency)
                };
                mainData.Add(data);
            }

            return mainData;
        }

        private IEnumerable<ExchangeTicket> ReadTicketData(IDataReader reader, string baseCurrency)
        {
            var detailData = new List<ExchangeTicket>();

            while (reader.Read())
            {
                var data = new ExchangeTicket
                {
                    UserId = reader.ToString("UserId"),
                    EvenId = reader.ToInt("EvenId"),
                    MarketId = reader.ToString("MarketId"),
                    BetId = reader.ToInt("BetId"),
                    SelectionName = reader.ToString("SelectionName"),
                    OddsAvg = reader.ToDecimal("OddsAvg"),
                    Stake = reader.ToDecimal("Stake" + baseCurrency),
                    Type = reader.ToString("Type"),
                    BetPlaced = reader.ToDateTime("BetPlaced")
                };
                detailData.Add(data);
            }

            return detailData;
        }
    }
}