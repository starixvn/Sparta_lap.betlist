namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Constant.DataDomain;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class RunningBettypesBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelRunningByBettypeList";

        private RunningBettypesBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<RunningBettypesBetListController>(
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
        public void MixParlay_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(1, new Dictionary<int, string>());

            // Action
            var result = _controller.MixParlay(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void DoubleChange_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(2, new Dictionary<int, string>());

            // Action
            var result = _controller.DoubleChange(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void BONTeamToScore_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(3, new Dictionary<int, string>());

            // Action
            var result = _controller.BONTeamToScore(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void MixParlay_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.MixParlay(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, UspKey);
                Assert.That(metaModel.MultipleBettype, Is.EqualTo(((int)BetType.CombinationMixParlay).ToString()));
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void DoubleChange_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.DoubleChange(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, UspKey);
                Assert.That(metaModel.MultipleBettype, Is.EqualTo("24,151,186,431,410"));
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void BONTeamToScore_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.BONTeamToScore(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, UspKey);
                Assert.That(metaModel.MultipleBettype, Is.EqualTo("26,145,146,433,170,188,427"));
            });
        }

        private BetListMetaModel SetUpForGetBetListResult(int currencyId, Dictionary<int, string> sessionCurrencyList)
        {
            var metaModel = new BetListMetaModel { CurrencyId = currencyId };

            StubUserSessionData(new Model.Core.DTO.Users.UserSession { CurrencyList = sessionCurrencyList });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            return metaModel;
        }
    }
}