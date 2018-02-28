namespace Fanex.Athena.Service.BetList
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public class ExchangeWinLossBetListService : IExchangeWinLossBetListService
    {
        public IEnumerable<ExchangeTicket> GetBetListData(BetListMetaModel metaModel)
        {
            return new List<ExchangeTicket>
                {
                    new ExchangeTicket
                    {
                        Market = "CRICKET> Ireland A v Bangladesh A > Match Odds",
                        BetId = 43154,
                        Selection = "Bangladesh A",
                        Type = "Black",
                        BetPlaced = new DateTime(2017, 10, 25, 1, 5, 46),
                        OddsAvg = 1.5m,
                        Stake = 30,
                        WinLoss = 15,
                        WlComm = -0.45m,
                        LicWl = -9,
                        LicWlComm = 0.15m
                    },
                    new ExchangeTicket
                    {
                        Market = "CRICKET> Zimbabwe v Indies > Match Odds",
                        BetId = 43116,
                        Selection = "Zimbabwe",
                        Type = "Lay",
                        BetPlaced = new DateTime(2017, 10, 24, 5, 45, 14),
                        OddsAvg = 7,
                        Stake = 30,
                        WinLoss = 30,
                        WlComm = 0,
                        LicWl = 14,
                        LicWlComm = 0
                    },
                    new ExchangeTicket
                    {
                        Market = "CRICKET> Zimbabwe v Indies > Match Odds",
                        BetId = 43112,
                        Selection = "Zimbabwe",
                        Type = "Black",
                        BetPlaced = new DateTime(2017, 10, 24, 5, 45, 14),
                        OddsAvg = 9.8m,
                        Stake = 50,
                        WinLoss = -50,
                        WlComm = 0,
                        LicWl = 14,
                        LicWlComm = 0
                    }
            };
        }
    }
}