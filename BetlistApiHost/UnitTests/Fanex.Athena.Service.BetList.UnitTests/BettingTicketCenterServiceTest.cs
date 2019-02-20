namespace Fanex.Athena.Service.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Constant.DataDomain;
    using DataAccess.BetList.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class BettingTicketCenterServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

        private IBettingTicketCenterDataAccess _bettingTicketCenterDataAccess;
        private IBetListHTMLBuilder _betlistHtmlBuilder;
        private ICacheRefAdapter _cacheRefAdapter;
        private ITicketHelper _ticketHelper;
        private IBettingTicketCenterService _bettingTicketCenterService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _bettingTicketCenterDataAccess = Substitute.For<IBettingTicketCenterDataAccess>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _bettingTicketCenterService = new BettingTicketCenterService(_bettingTicketCenterDataAccess, _betlistHtmlBuilder, _cacheRefAdapter, _ticketHelper);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _bettingTicketCenterDataAccess = null;
            _cacheRefAdapter = null;
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