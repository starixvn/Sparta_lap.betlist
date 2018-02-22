namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class LicenseeWinLossBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinloss_Mem";

        private LicenseeWinLossBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<LicenseeWinLossBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter);

            StubLicenseeSessionData(new Model.Core.DTO.Users.LicenseeSession { LicenseeId = SessionLicenseeId });
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [Test]
        public void Index_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 1 };

            StubUserSessionData(new Model.Core.DTO.Users.UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var resutl = _controller.Index(metaModel);

            // Assert
            Assert.That(resutl, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Index_WhenCall_GetBetListResultWithCorrectParameter(int currencyId)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = currencyId };

            StubUserSessionData(new Model.Core.DTO.Users.UserSession { CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.When(x => x.GetBetListResult(metaModel, UspKey)).DoNotCallBase();

            // Action
            _controller.Index(metaModel);

            // Assert
            _controller.Received().GetBetListResult(metaModel, UspKey);
        }
    }
}