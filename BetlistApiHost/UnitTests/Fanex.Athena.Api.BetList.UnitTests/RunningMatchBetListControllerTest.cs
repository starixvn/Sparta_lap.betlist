namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class RunningMatchBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelRunningByBettypeMatch";

        private RunningMatchBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<RunningMatchBetListController>(
                SessionService,
                ProductService,
                LangService,
                BridgeClientService,
                BalanceUpDataAccess);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void Detail_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 1 };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 5)]
        [TestCase(5, 6)]
        public void Detail_IsNotRacing_GetBetListResultWithCorrectParameter(int currencyId, int betType)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = currencyId, BetType = betType };

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            _controller.Index(metaModel);

            // Assert
            _controller.Received().GetBetListResult(metaModel, UspKey);
        }
    }
}