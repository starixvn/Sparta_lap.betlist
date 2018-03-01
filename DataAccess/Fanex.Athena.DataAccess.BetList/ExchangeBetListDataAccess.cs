namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Common.Extensions;
    using Core;
    using Core.Exception;
    using Data;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DataDomain;

    public class ExchangeBetListDataAccess : BaseDataAccess, IExchangeBetListDataAccess
    {
        public ExchangeBetListSummaryViewModel GetBetListData(ExchangeBetListMetaModel metaModel)
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
                    db.ExecuteNonQuery();

                    using (var reader = db.ExecuteReader())
                    {
                        var baseCurrency = SystemConstants.BaseCurrencyList.Contains(metaModel.BaseCurrency) ? metaModel.BaseCurrency : string.Empty;

                        return new ExchangeBetListSummaryViewModel
                        {
                            MainData = GetData(reader, baseCurrency),
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

        public IEnumerable<ExchangeTicketDetail> GetTicketDetailList() => GetFakeTicketDetailList();

        private IEnumerable<ExchangeTicketDetail> GetFakeTicketDetailList()
        {
            return new List<ExchangeTicketDetail>
            {
                new ExchangeTicketDetail
                {
                    BetTaken = "10/23/2017 10:55:15",
                    OddsReq = 6.80m,
                    Stake = 50.00m,
                    Liability = "-",
                    OddsMatched = 7.80m
                }
            };
        }

        private IEnumerable<ExchangeTicketSummary> GetData(IDataReader reader, string baseCurrency)
        {
            var mainData = ReadMainData(reader, baseCurrency);

            reader.NextResult();
            var detail = ReadDetailData(reader, baseCurrency);

            if (mainData.Any() && detail.Any())
            {
                return mainData.GroupJoin(
                    detail,
                    m => new { m.UserId, m.EvenId, m.MarketId },
                    d => new { d.UserId, d.EvenId, d.MarketId },
                   (main, detailList) => new ExchangeTicketSummary { Ticket = main, TicketList = detailList.ToList() }).ToList();
            }

            return new List<ExchangeTicketSummary>();
        }

        private IEnumerable<ExchangeBetList> ReadMainData(IDataReader reader, string baseCurrency)
        {
            var mainData = new List<ExchangeBetList>();

            while (reader.Read())
            {
                var data = new ExchangeBetList
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

        private IEnumerable<ExchangeBetListEntity> ReadDetailData(IDataReader reader, string baseCurrency)
        {
            var detailData = new List<ExchangeBetListEntity>();

            while (reader.Read())
            {
                var data = new ExchangeBetListEntity
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