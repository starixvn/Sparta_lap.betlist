namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Athena.BetList.DataAccess.Interfaces;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Athena.BetList.Service;
    using Athena.BetList.Service.Interfaces;
    using Common.Constant;
    using Common.Utility.Services.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BettingTicketCenterServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

        private IBettingTicketCenterDataAccess _bettingTicketCenterDataAccess;
        private IBetListHTMLBuilder _betlistHtmlBuilder;
        private ITicketHelper _ticketHelper;
        private IBettingTicketCenterService _bettingTicketCenterService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _bettingTicketCenterDataAccess = Substitute.For<IBettingTicketCenterDataAccess>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _bettingTicketCenterService = new BettingTicketCenterService(_bettingTicketCenterDataAccess, _betlistHtmlBuilder, _ticketHelper);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _bettingTicketCenterDataAccess = null;
            _bettingTicketCenterService = null;
        }

        [Test]
        public void BuildTicketText_IsCombinationMixParlay_ReturnCustomOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.CombinationMixParlay, Status = BetStatus.Lose };
            ticketResult.MainTicketList.Add(ticket);

            // Action
            _bettingTicketCenterService.BuildTicketText(ticketResult, metaModel, false);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Not.Empty);
        }

        [Test]
        public void BuildTicketText_NotCombinationMixParlay_ReturnNormalOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.Handicap, Status = BetStatus.Won };
            ticketResult.MainTicketList.Add(ticket);

            _ticketHelper.ClientDetailSystemParlayFunction.Returns(string.Empty);

            // Action
            _bettingTicketCenterService.BuildTicketText(ticketResult, metaModel, false);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Empty);
        }

        [TestCase("  ", "1/1/0001", "1/1/0001", "Betting Ticket Center")]
        [TestCase("  ", "1/1/2001", "12/1/2002", "Betting Ticket Center, 01/01/2001-->12/01/2002")]
        [TestCase("EUR", "1/1/2001", "12/1/2002", "Betting Ticket Center, 01/01/2001-->12/01/2002, Base Currency: EUR")]
        public void GetTitle_WhenCall_ReturnCorrectTitle(string baseCurrency, string fromDate, string toDate, string expectedTitle)
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Parse(fromDate),
                ToDate = DateTime.Parse(toDate),
                BaseCurrency = baseCurrency
            };

            // Action
            var title = _bettingTicketCenterService.GetTitle(metaModel);

            // Assert
            Assert.That(title, Is.EqualTo(expectedTitle));
        }
    }
}