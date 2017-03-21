namespace Fanex.Athena.Service.BetList.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using Core.Common;
    using DataAccess.BetList.Interfaces;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LastBetsMonitoringBetListServiceTest
    {
        private ILastBetsMonitoringBetListDataAccess _lastBetsMonitoringBetListDataAccess;
        private ILastBetsMonitoringBetListService _betListService;

        [SetUp]
        public void SetUp()
        {
            _lastBetsMonitoringBetListDataAccess = Substitute.For<ILastBetsMonitoringBetListDataAccess>();
            _betListService = new LastBetsMonitoringBetListService(_lastBetsMonitoringBetListDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _betListService = null;
            _lastBetsMonitoringBetListDataAccess = null;
            CacheService.Clear();
        }

        [Test]
        public void GetLastBetsMonitoringData_WhenCall_TicketIsAssignedDataCorrectlyFromMatchList()
        {
            // Arrange
            const int SportType = 111;
            const int LeagueId = 222;
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    MatchId = 999,
                    BetTypeId = (int)BetType.Outright
                }
            });

            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _lastBetsMonitoringBetListDataAccess.GetMatchList(Arg.Any<string>())
                .Returns(new List<BetListMatch>
                {
                    new BetListMatch
                    {
                        MatchId = 999,
                        SportType = SportType,
                        LeagueId = LeagueId
                    }
                });

            // Action
            var result = _betListService.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel
            {
                OddsType = string.Empty
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainTicketList[0].SportTypeId, Is.EqualTo(SportType));
                Assert.That(result.MainTicketList[0].LeagueId, Is.EqualTo(LeagueId));
            });
        }

        [Test]
        public void GetLastBetsMonitoringData_GetDataFromCache_TicketIsAssignedDataCorrectlyFromMatchList()
        {
            // Arrange
            const int SportType = 111;
            const int LeagueId = 222;
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    MatchId = 999,
                    BetTypeId = (int)BetType.Outright
                }
            });

            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            _lastBetsMonitoringBetListDataAccess.GetMatchList(Arg.Any<string>())
                .Returns(new List<BetListMatch>
                {
                    new BetListMatch
                    {
                        MatchId = 999,
                        SportType = SportType,
                        LeagueId = LeagueId
                    }
                });
            var metaModel = new LastBetsMonitoringMetaModel { OddsType = string.Empty };
            _betListService.GetLastBetsMonitoringData(metaModel);

            // Action
            var result = _betListService.GetLastBetsMonitoringData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainTicketList[0].SportTypeId, Is.EqualTo(SportType));
                Assert.That(result.MainTicketList[0].LeagueId, Is.EqualTo(LeagueId));
            });
        }

        [TestCase(OddsType.Decimal, -2, -1)]
        [TestCase(OddsType.Malay, -2, 1.5)]
        [TestCase(OddsType.American, -2, 1.5)]
        [TestCase(OddsType.American, 4, 2)]
        [TestCase(OddsType.American, 1.31, 2.31)]
        [TestCase("0", 5, 5)]
        public void GetLastBetsMonitoringData_WhenCall_OddsIsConvertedToDecimal(string oddsType, decimal oddsValue, decimal expectedValue)
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = oddsValue,
                    OddsType = oddsType
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            // Action
            var result = _betListService.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel
            {
                OddsType = "dec"
            });

            // Assert
            Assert.That(result.MainTicketList[0].Odds, Is.EqualTo(expectedValue));
        }

        [TestCase(OddsType.Decimal, -2, -2)]
        [TestCase(OddsType.Malay, -2, 0.5)]
        [TestCase(OddsType.American, -2, 0.5)]
        [TestCase(OddsType.American, 0, 0)]
        [TestCase(OddsType.Hongkong, 2, -0.5)]
        [TestCase(OddsType.Indo, 2, 2)]
        public void GetLastBetsMonitoringData_WhenCall_OddsIsConvertedToMy(string oddsType, decimal oddsValue, decimal expectedValue)
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = oddsValue,
                    OddsType = oddsType
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>())
                .Returns(ticketResult);

            // Action
            var result = _betListService.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel
            {
                OddsType = "my"
            });

            // Assert
            Assert.That(result.MainTicketList[0].Odds, Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetLastBetsMonitoringData_WhenCallWithTimeZoneGreaterThanNegativeNumber4_TransDateOfTicketUpdateValue()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = -2,
                    OddsType = OddsType.American,
                    TransDate = new DateTime(2017, 3, 17, 0, 0, 0)
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>()).Returns(ticketResult);
            var expectedResult = new DateTime(2017, 3, 17, 4, 0, 0);

            // Action
            var result = _betListService.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel
            {
                OddsType = "my",
                TimeZone = 0
            });

            // Assert
            Assert.That(result.MainTicketList[0].TransDate, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetLastBetsMonitoringData_WhenCallWithTimeZoneLessThanNegativeNumber4_TransDateOfTicketUpdateValue()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.AddRange(new List<LrfTicket>
            {
                new LrfTicket
                {
                    BetTypeId = 1,
                    Odds = -2,
                    OddsType = OddsType.American,
                    TransDate = new DateTime(2017, 3, 17, 0, 0, 0)
                }
            });
            _lastBetsMonitoringBetListDataAccess.GetLastBetsMonitoringData(Arg.Any<LastBetsMonitoringMetaModel>()).Returns(ticketResult);
            var expectedResult = new DateTime(2017, 3, 17, 0, 0, 0);

            // Action
            var result = _betListService.GetLastBetsMonitoringData(new LastBetsMonitoringMetaModel
            {
                OddsType = "my",
                TimeZone = -5
            });

            // Assert
            Assert.That(result.MainTicketList[0].TransDate, Is.EqualTo(expectedResult));
        }
    }
}