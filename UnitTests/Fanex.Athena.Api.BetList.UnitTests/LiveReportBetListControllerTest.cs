namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class LiveReportBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string SelWinlossByMatchUsp = "Lrf_BetList_LiveReport_SelWinlossByMatch";

        private const string SelWinlossOutrightUsp = "Lrf_BetList_LiveReport_SelWinlossOutright";

        private const string SelWinlossNumberGameUsp = "Lrf_BetList_LiveReport_SelWinlossNumberGame";

        private const string SelWinlossParlayUsp = "Lrf_BetList_LiveReport_SelWinlossParlay";

        private const string SelWinlossRacingUsp = "Lrf_BetList_LiveReport_SelWinlossRacing";

        private const string SelWinlossVHRacingUsp = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

        private readonly TicketResult _ticketResult = new TicketResult { SumComm = 1, SumPlayerComm = 2, SumLicenseeWL = 3, SumWinLoss = 4 };

        private readonly IList<TicketText> _ticketTexts = new List<TicketText>();

        private LiveReportBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<LiveReportBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void Match_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(1, new Dictionary<int, string>(), SelWinlossByMatchUsp);

            // Action
            var resutl = _controller.Match(metaModel);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void Outright_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(2, new Dictionary<int, string>(), SelWinlossOutrightUsp);

            // Action
            var resutl = _controller.Outright(metaModel);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void NumberGame_InvalidCurrency_ReturnNull()
        {
            // Arrange
            const int Page = 2;
            const int Rows = 50;

            var metaModel = SetUpForGetBetListResult(3, new Dictionary<int, string>(), SelWinlossNumberGameUsp);

            // Action
            var resutl = _controller.NumberGame(metaModel, Page, Rows);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void MixParlay_InvalidCurrency_ReturnNull()
        {
            // Arrange
            const int Page = 2;
            const int Rows = 50;

            var metaModel = SetUpForGetBetListResult(3, new Dictionary<int, string>(), SelWinlossParlayUsp);

            // Action
            var resutl = _controller.MixParlay(metaModel, Page, Rows);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void Racing_InvalidCurrency_ReturnNull()
        {
            // Arrange
            const int Page = 2;
            const int Rows = 50;

            var metaModel = SetUpForGetBetListResult(3, new Dictionary<int, string>(), SelWinlossRacingUsp);

            // Action
            var resutl = _controller.Racing(metaModel, Page, Rows);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void VirtualSports_InvalidCurrency_ReturnNull()
        {
            // Arrange
            const int Page = 2;
            const int Rows = 50;

            var metaModel = SetUpForGetBetListResult(3, new Dictionary<int, string>(), SelWinlossVHRacingUsp);

            // Action
            var resutl = _controller.VirtualSports(metaModel, Page, Rows);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        private LiveReportBetListMetaModel SetUpForGetBetListResult(int currencyId, Dictionary<int, string> sessionCurrencyList, string uspKey)
        {
            var metaModel = new LiveReportBetListMetaModel { CurrencyId = currencyId };

            StubUserSessionData(new Model.Core.DTO.Users.UserSession { CurrencyList = sessionCurrencyList });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            BetListService.GetBetListData(metaModel, uspKey).Returns(_ticketResult);

            _controller.When(x => x.Ticket(_ticketResult, Arg.Any<BetListMetaModel>(), Arg.Any<int>(), Arg.Any<bool>())).DoNotCallBase();
            _controller.Ticket(_ticketResult, Arg.Any<BetListMetaModel>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(_ticketTexts);

            return metaModel;
        }
    }
}