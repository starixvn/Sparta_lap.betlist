namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Constant.DataDomain;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class RunningBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelRunningByBettype";

        private const string MultipleBettypeUspKey = "Lrf_BetList_SelRunningByBettypeList";

        private RunningBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<RunningBetListController>(
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
        public void CleanSheet_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(1, new Dictionary<int, string>());

            // Action
            var resutl = _controller.CleanSheet(metaModel);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void ToWinToNil_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(2, new Dictionary<int, string>());

            // Action
            var resutl = _controller.ToWinToNil(metaModel);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [Test]
        public void ThreeWayHandicap_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(3, new Dictionary<int, string>());

            // Action
            var resutl = _controller.ThreeWayHandicap(metaModel);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void CleanSheet_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.CleanSheet(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, UspKey);
                Assert.That(metaModel.BetType, Is.EqualTo((int)BetType.CleanSheet));
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ToWinToNil_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } });

            // Action
            _controller.ToWinToNil(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, UspKey);
                Assert.That(metaModel.BetType, Is.EqualTo((int)BetType.ToWinToNil));
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void ThreeWayHandicap_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = SetUpForGetBetListResult(currencyId, new Dictionary<int, string> { { currencyId, "Currency" } }, MultipleBettypeUspKey);

            // Action
            _controller.ThreeWayHandicap(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Received().GetBetListResult(metaModel, MultipleBettypeUspKey);
                Assert.That(metaModel.MultipleBettype, Is.EqualTo($"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}"));
            });
        }

        private BetListMetaModel SetUpForGetBetListResult(int currencyId, Dictionary<int, string> sessionCurrencyList, string uspKey = UspKey)
        {
            var metaModel = new BetListMetaModel { CurrencyId = currencyId };

            StubUserSessionData(new Model.Core.DTO.Users.UserSession { CurrencyList = sessionCurrencyList });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.When(x => x.GetBetListResult(metaModel, uspKey)).DoNotCallBase();

            return metaModel;
        }
    }
}