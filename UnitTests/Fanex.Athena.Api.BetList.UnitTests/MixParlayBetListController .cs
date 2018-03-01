namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using NSubstitute;
    using NUnit.Framework;

    public class MixParlayBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinLossRacingByDate_Mem";

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

            StubLicenseeSessionData(new Model.Core.DTO.Users.LicenseeSession { LicenseeId = SessionLicenseeId });
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

            var ticketResult = new TicketResult();
            BetListService.GetMixParlayDetailTicket(SessionLicenseeId, TransId).Returns(ticketResult);

            _controller.When(x => x.Ticket(ticketResult, null, 1, true)).DoNotCallBase();

            // Action
            _controller.Detail(TransId);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received().GetMixParlayDetailTicket(SessionLicenseeId, TransId);
                _controller.Received().Ticket(ticketResult, null, 1, true);
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

            var combinationTypeCombo = new[] { "OrginalText" };
            CacheRefAdapter.GetCombinationTypeCombo(BetTeam, BetCheck, Handicap1, Handicap2).Returns(combinationTypeCombo);

            var ticketResult = new TicketResult();
            BetListService.GetMixParlayDetailTicket(SessionLicenseeId, TransId).Returns(ticketResult);

            var ticketText = new List<TicketText>();
            _controller.When(x => x.Ticket(ticketResult, null, 1, true)).DoNotCallBase();
            _controller.Ticket(ticketResult, null, 1, true).Returns(ticketText);

            // Action
            var result = _controller.NewParlayDetail(TransId, BetTeam, BetCheck, Handicap1, Handicap2);

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

            var combinationTypeCombo = new[] { "CombinationTypeCombo" };
            CacheRefAdapter.GetCombinationTypeCombo(BetTeam, BetCheck, Handicap1, Handicap2).Returns(combinationTypeCombo);

            var ticketResult = new TicketResult();
            var lrfTicket = new LrfTicket { Stake = 1, Currency = "usd" };
            ticketResult.MainTicketList.Add(lrfTicket);
            BetListService.GetMixParlayDetailTicket(SessionLicenseeId, TransId).Returns(ticketResult);

            var ticketText = new List<TicketText>();
            _controller.When(x => x.Ticket(ticketResult, null, 1, true)).DoNotCallBase();
            _controller.Ticket(ticketResult, null, 1, true).Returns(ticketText);

            var expectedText = "<p class='system-parlay-ticket-infor'>"
                                + $"<span class='system-parlay-combo-type'>{combinationTypeCombo[0]}</span> x "
                                + $"<span class='system-parlay-unit-stake'>{lrfTicket.Currency} {lrfTicket.Stake.ToString("N0")}</span>"
                                + "</p>";

            // Action
            var result = _controller.NewParlayDetail(TransId, BetTeam, BetCheck, Handicap1, Handicap2);

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

            var combinationTypeCombo = new[] { "CombinationTypeCombo" };
            CacheRefAdapter.GetCombinationTypeCombo(BetTeam, BetCheck, Handicap1, Handicap2).Returns(combinationTypeCombo);

            var ticketResult = new TicketResult();
            var lrfTicket1 = new LrfTicket { Stake = 1, Currency = "usd", MatchId = 2 };
            ticketResult.MainTicketList.Add(lrfTicket1);
            var lrfTicket2 = new LrfTicket { Stake = 1, Currency = "usd", MatchId = 1 };
            ticketResult.MainTicketList.Add(lrfTicket2);
            BetListService.GetMixParlayDetailTicket(SessionLicenseeId, TransId).Returns(ticketResult);

            var ticketText = new List<TicketText>();
            _controller.When(x => x.Ticket(ticketResult, null, 1, true)).DoNotCallBase();
            _controller.Ticket(ticketResult, null, 1, true).Returns(ticketText);

            var expectedText = "<p class='system-parlay-ticket-infor'>"
                                + $"<span class='system-parlay-combo-type'>{combinationTypeCombo[0]}</span> x "
                                + $"<span class='system-parlay-unit-stake'>{lrfTicket2.Currency} {lrfTicket2.Stake.ToString("N0")}</span>"
                                + "</p>";

            // Action
            var result = _controller.NewParlayDetail(TransId, BetTeam, BetCheck, Handicap1, Handicap2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(ticketText));
                Assert.That(result.Texts[0], Is.EqualTo(expectedText));
            });
        }
    }
}