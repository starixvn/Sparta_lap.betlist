namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Athena.BetList.DataAccess.Interfaces;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Athena.BetList.Service;
    using Athena.BetList.Service.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class ExchangeBetListServiceTest
    {
        private IExchangeBetListDataAccess _dataAccess;
        private IExchangeBetListService _service;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dataAccess = Substitute.For<IExchangeBetListDataAccess>();
            _service = new ExchangeBetListService(_dataAccess);
        }

        [Test]
        public void GetTicketDetailList_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ExchangeBetListDetailMetaModel>();

            // Action
            _service.GetTicketDetailList(metaModel);

            // Assert
            _dataAccess.Received().GetTicketDetailList(metaModel);
        }

        [Test]
        public void GetTicketDetailList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedData = new List<ExchangeTicketDetail>
            {
                new ExchangeTicketDetail { BetTaken = new System.DateTime(2018, 5, 16), OddsReq = 1, Stake = 2, Liability = "3", OddsMatched = 4 }
            };
            _dataAccess.GetTicketDetailList(Arg.Any<ExchangeBetListDetailMetaModel>()).Returns(expectedData);

            // Action
            var result = _service.GetTicketDetailList(new ExchangeBetListDetailMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(expectedData));
                Assert.That(expectedData.First().BetTakenDateString, Is.EqualTo("05/16/2018 12:00:00 AM"));
            });
        }

        [Test]
        public void GetBetListData_WhenCall_CallDataAccess()
        {
            // Action
            _service.GetBetListData(new ExchangeBetListMetaModel());

            // Assert
            _dataAccess.Received().GetBetListData(Arg.Any<ExchangeBetListMetaModel>());
        }

        [Test]
        public void GetBetListData_NoDataReturnedFromDataAccess_ReturnEmptyData()
        {
            // Arrange
            _dataAccess.GetBetListData(Arg.Any<ExchangeBetListMetaModel>())
                .Returns(new ExchangeBetListResult { SumWinLossList = new List<ExchangeSumWinLoss>(), TicketList = new List<ExchangeTicket>() });

            // Action
            var result = _service.GetBetListData(new ExchangeBetListMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainData, Is.Null);
                Assert.That(result.GrandTotal, Is.Zero);
            });
        }

        [Test]
        public void GetBetListData_HasDataReturnedFromDataAccess_ReturnCorrectData()
        {
            // Arrange
            var sumWinLossList = new List<ExchangeSumWinLoss>()
            {
                new ExchangeSumWinLoss { UserId = "1", MarketId = "1", EventId = 1, EventName = "Event1" },
                new ExchangeSumWinLoss { UserId = "1", MarketId = "2", EventId = 2, EventName = "Event2" },
                new ExchangeSumWinLoss { UserId = "2", MarketId = "3", EventId = 3, EventName = "Event3" }
            };

            var ticketList = new List<ExchangeTicket>()
            {
                new ExchangeTicket { UserId = "1", MarketId = "1", EventId = 1, BetId = 1, SelectionName = "Selection1" },
                new ExchangeTicket { UserId = "1", MarketId = "1", EventId = 1, BetId = 2, SelectionName = "Selection2" },
                new ExchangeTicket { UserId = "1", MarketId = "2", EventId = 2, BetId = 3, SelectionName = "Selection3" },
                new ExchangeTicket { UserId = "1", MarketId = "3", EventId = 3, BetId = 4, SelectionName = "Selection4" }
            };

            _dataAccess.GetBetListData(Arg.Any<ExchangeBetListMetaModel>())
                .Returns(new ExchangeBetListResult { SumWinLossList = sumWinLossList, TicketList = ticketList });

            // Action
            var result = _service.GetBetListData(new ExchangeBetListMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MainData.Count, Is.EqualTo(2));
                Assert.That(result.MainData.Any(x => x.Ticket.UserId == "2"), Is.False);
                Assert.That(result.MainData.ElementAt(0).Details.Select(x => x.BetId).ToArray(), Is.EqualTo(new[] { 1, 2 }));
                Assert.That(result.MainData.ElementAt(1).Details.Select(x => x.BetId).ToArray(), Is.EqualTo(new[] { 3 }));
            });
        }
    }
}