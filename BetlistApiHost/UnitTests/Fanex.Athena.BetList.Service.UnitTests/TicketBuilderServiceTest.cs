namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System.Collections.Generic;

    using Common.Constant;

    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    using Interfaces;

    using Model.DTO;
    using Model.MetaModel;

    using NSubstitute;

    using NUnit.Framework;

    using Service;

    using UnitTest.Common.Helpers;

    [TestFixture]
    public class TicketBuilderServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ITicketHelper _ticketHelper;
        private  ITicketBuilderService _ticketBuilderService;
        [OneTimeSetUp]
        public void SetUp()
        {
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _ticketBuilderService = new TicketBuilderService(
                _betlistHtmlBuilder,
                _ticketHelper);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _ticketHelper = null;
            _betlistHtmlBuilder = null;
            _ticketBuilderService = null;
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
            _ticketBuilderService.BuildTicketText(ticketResult, metaModel, false);

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
            _ticketBuilderService.BuildTicketText(ticketResult, metaModel, false);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Empty);
        }
      
    }
}