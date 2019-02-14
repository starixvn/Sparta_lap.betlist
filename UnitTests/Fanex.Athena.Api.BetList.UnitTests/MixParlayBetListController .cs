namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class MixParlayBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private MixParlayBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<MixParlayBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter,
                BetListHTMLBuilder);

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void Detail_WhenCall_GetBetListTicketWithCorrectParameter()
        {
            // Arrange
            const long TransId = 1;
            const int CustId = 2;

            var ticketResult = new TicketResult();
            BetListService.GetMixParlayDetailTicket(UserCurrencyList, SessionLicenseeId, TransId, CustId).Returns(ticketResult);

            _controller.When(x => x.BuildParlayDetailTickets(ticketResult, true)).DoNotCallBase();

            // Action
            _controller.Detail(TransId, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received(1).GetMixParlayDetailTicket(UserCurrencyList, SessionLicenseeId, TransId, CustId);
                _controller.Received(1).BuildParlayDetailTickets(ticketResult, true);
            });
        }

        [Test]
        public void NewParlayDetail_NoTicketReturn_ReturnOrginalText()
        {
            // Arrange
            const long TransId = 1;
            const string BetTeam = "Team";
            const string BetCheck = "Check";
            const decimal Handicap1 = 1;
            const decimal Handicap2 = 2;
            const int CustId = 2;

            var combinationTypeCombo = new[] { "OrginalText" };
            CacheRefAdapter.GetCombinationTypeCombo(BetTeam, BetCheck, Handicap1, Handicap2).Returns(combinationTypeCombo);

            var ticketResult = new TicketResult();
            BetListService.GetMixParlayDetailTicket(UserCurrencyList, SessionLicenseeId, TransId, CustId).Returns(ticketResult);

            var ticketText = new List<TicketText>();
            _controller.When(x => x.Ticket(ticketResult, null, 1, true)).DoNotCallBase();
            _controller.Ticket(ticketResult, null, 1, true).Returns(ticketText);

            // Action
            var result = _controller.NewParlayDetail(TransId, BetTeam, BetCheck, Handicap1, Handicap2, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(ticketText));
                Assert.That(result.Texts[0], Is.EqualTo("OrginalText"));
            });
        }

        [Test]
        public void NewParlayDetail_SystemParlay_ReturnCorrectText()
        {
            // Arrange
            const long TransId = 1;
            const string BetTeam = "Team";
            const string BetCheck = "Check";
            const decimal Handicap1 = 0;
            const decimal Handicap2 = 2;
            const int CustId = 2;

            var combinationTypeCombo = new[] { "CombinationTypeCombo" };
            CacheRefAdapter.GetCombinationTypeCombo(BetTeam, BetCheck, Handicap1, Handicap2).Returns(combinationTypeCombo);

            var ticketResult = new TicketResult();
            var lrfTicket = new LrfTicket { Stake = 1, Currency = "usd" };
            ticketResult.MainTicketList.Add(lrfTicket);
            BetListService.GetMixParlayDetailTicket(UserCurrencyList, SessionLicenseeId, TransId, CustId).Returns(ticketResult);

            var ticketText = new List<string>();
            _controller.When(x => x.BuildParlayDetailTickets(ticketResult, true)).DoNotCallBase();
            _controller.BuildParlayDetailTickets(ticketResult, true).Returns(ticketText);

            var expectedText = "<p class='system-parlay-ticket-infor'>"
                                + $"<span class='system-parlay-combo-type'>{combinationTypeCombo[0]}</span> x "
                                + $"<span class='system-parlay-unit-stake'>{lrfTicket.Currency} {lrfTicket.Stake.ToString("N0")}</span>"
                                + "</p>";

            // Action
            var result = _controller.NewParlayDetail(TransId, BetTeam, BetCheck, Handicap1, Handicap2, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(ticketText));
                Assert.That(result.Texts[0], Is.EqualTo(expectedText));
            });
        }

        [Test]
        public void NewParlayDetail_ComboParlay_ReturnCorrectext()
        {
            // Arrange
            const long TransId = 1;
            const string BetTeam = "Team";
            const string BetCheck = "Check";
            const decimal Handicap1 = 1;
            const decimal Handicap2 = 2;
            const int CustId = 2;

            var combinationTypeCombo = new[] { "CombinationTypeCombo" };
            CacheRefAdapter.GetCombinationTypeCombo(BetTeam, BetCheck, Handicap1, Handicap2).Returns(combinationTypeCombo);

            var ticketResult = new TicketResult();
            var lrfTicket1 = new LrfTicket { Stake = 1, Currency = "usd", MatchId = 2 };
            ticketResult.MainTicketList.Add(lrfTicket1);
            var lrfTicket2 = new LrfTicket { Stake = 1, Currency = "usd", MatchId = 1 };
            ticketResult.MainTicketList.Add(lrfTicket2);
            BetListService.GetMixParlayDetailTicket(UserCurrencyList, SessionLicenseeId, TransId, CustId).Returns(ticketResult);

            var ticketText = new List<string>();
            _controller.When(x => x.BuildParlayDetailTickets(ticketResult, true)).DoNotCallBase();
            _controller.BuildParlayDetailTickets(ticketResult, true).Returns(ticketText);

            var expectedText = "<p class='system-parlay-ticket-infor'>"
                                + $"<span class='system-parlay-combo-type'>{combinationTypeCombo[0]}</span> x "
                                + $"<span class='system-parlay-unit-stake'>{lrfTicket2.Currency} {lrfTicket2.Stake.ToString("N0")}</span>"
                                + "</p>";

            // Action
            var result = _controller.NewParlayDetail(TransId, BetTeam, BetCheck, Handicap1, Handicap2, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(ticketText));
                Assert.That(result.Texts[0], Is.EqualTo(expectedText));
            });
        }
    }
}