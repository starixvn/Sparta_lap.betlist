namespace Fanex.Athena.Service.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;

    public class ExchangeWinLossBetListService : IExchangeWinLossBetListService
    {
        public ExchangeBetListSummaryViewModel GetBetListData(BetListMetaModel metaModel)
        {
            var mainData = GetFakeData();

            return new ExchangeBetListSummaryViewModel
            {
                MainData = mainData,
                GrandTotal = mainData.Sum(p => p.WinLost)
            };
        }

        private IEnumerable<ExchangeTicketSummary> GetFakeData()
        {
            return new List<ExchangeTicketSummary>
                {
                    new ExchangeTicketSummary
                    {
                        MarketId= "1",
                        MarketName = "CRICKET> Ireland A v Bangladesh A > Match Odds",
                        WinLost = 15,
                        Commission = -0.45m,
                        TicketList = new List<ExchangeBetListEntity>
                        {
                            new ExchangeBetListEntity
                            {
                                BetId = 43154,
                                Selection = "Bangladesh A",
                                EventTypeName = "Black",
                                SettledDate = new DateTime(2017, 10, 25, 1, 5, 46),
                                MatchAvgOdds = 1.5m,
                                MatchStake = 30,
                                WinLost = 15
                            }
                        }
                    },

                    new ExchangeTicketSummary
                    {
                        MarketId= "2",
                        MarketName = "CRICKET> Zimbabwe v Indies > Match Odds",
                        WinLost = 30,
                        Commission = 0,
                        TicketList = new List<ExchangeBetListEntity>
                        {
                            new ExchangeBetListEntity
                            {
                                BetId = 43116,
                                Selection = "Zimbabwe",
                                EventTypeName = "Lay",
                                SettledDate = new DateTime(2017, 10, 24, 5, 45, 14),
                                MatchAvgOdds = 7m,
                                MatchStake = 30,
                                WinLost = 30
                            },

                            new ExchangeBetListEntity
                            {
                                BetId = 43112,
                                Selection = "Zimbabwe",
                                EventTypeName = "Black",
                                SettledDate = new DateTime(2017, 10, 24, 5, 45, 14),
                                MatchAvgOdds = 9.8m,
                                MatchStake = 50,
                                WinLost = -50
                            },

                            new ExchangeBetListEntity
                            {
                                BetId = 43118,
                                Selection = "Zimbabwe",
                                EventTypeName = "Black",
                                SettledDate = new DateTime(2017, 10, 24, 5, 45, 14),
                                MatchAvgOdds = 7.8m,
                                MatchStake = 50,
                                WinLost = -50
                            }
                        }
                    }
            };
        }
    }
}