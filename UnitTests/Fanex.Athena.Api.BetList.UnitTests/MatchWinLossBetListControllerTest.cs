namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class MatchWinLossBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const int Page = 1;

        private const int Rows = 50;

        private MatchWinLossBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<MatchWinLossBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter,
                BetListHTMLBuilder);

            _controller.When(x => x.GetBetListResult(Arg.Any<BetListMetaModel>(), Arg.Any<string>())).DoNotCallBase();
            _controller.When(x => x.GetPagingBetListResult(Arg.Any<BetListMetaModel>(), Arg.Any<string>(), Page, Rows)).DoNotCallBase();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void MatchBetType_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(1, new Dictionary<int, string>());

            // Action
            var result = _controller.MatchBetType(Page, Rows, metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Outright_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(1, new Dictionary<int, string>());

            // Action
            var result = _controller.Outright(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void MixParlay_InvalidCurrency_ReturnNull()
        {
            // Action
            var result = _controller.MixParlay(0, 0, 1, string.Empty, 0, DateTime.Today, DateTime.Today, string.Empty);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        public void MatchBetType_IsNotRacing_GetBetListResultWithCorrectParameter(int currencyId, int betType)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Bettype";

            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });
            metaModel.BetType = betType;

            // Action
            _controller.MatchBetType(Page, Rows, metaModel);

            // Assert
            _controller.Received().GetPagingBetListResult(metaModel, UspKey, Page, Rows);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Outright_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Outright";

            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.Outright(metaModel);

            // Assert
            _controller.Received().GetBetListResult(metaModel, UspKey);
        }

        [TestCase(0, "", "")]
        [TestCase(1, null, "")]
        [TestCase(2, "usd", "usd")]
        public void MixParlay_WhenCall_GetBetListResultWithCorrectParameter(int currencyId, string baseCurrency, string expectedBaseCurrency)
        {
            // Arrange
            const string UspKey = "Lrf_BetList_MatchWL_Parlay";

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });

            SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.MixParlay(Page, Rows, currencyId, baseCurrency, 0, DateTime.Today, DateTime.Today, string.Empty);

            // Assert
            _controller.Received().GetPagingBetListResult(Arg.Is<BetListMetaModel>(x => x.BaseCurrency == expectedBaseCurrency), UspKey, Page, Rows);
        }

        private BetListMetaModel SetUpForGetBetListResult(int currencyId, Dictionary<int, string> sessionCurrencyList)
        {
            var metaModel = new BetListMetaModel { CurrencyId = currencyId };

            StubUserSessionData(new UserSession { CurrencyList = sessionCurrencyList });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            return metaModel;
        }
    }
}