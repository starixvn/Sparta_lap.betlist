namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class DailyStatementBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelByDayAll_Mem";

        private const string UspKeyOld = "Lrf_BetList_SelByDayAll_Mem_Old";

        private const int RunningTicketTransType = 3;

        private DailyStatementBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<DailyStatementBetListController>(
                SessionService,
                ProductService,
                LangService,
                BetListService,
                BalanceUpDataAccess,
                TicketHelper,
                CacheRefAdapter,
                BetListHTMLBuilder);
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

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0, "01/01/2018", "01/02/2018", UspKeyOld)]
        [TestCase(1, "01/03/2018", "01/02/2018", UspKey)]
        [TestCase(2, "01/03/2018", "01/02/2018", UspKey)]
        public void Index_WhenCall_GetBetListResultWithCorrectParameter(int currencyId, string fromDate, string balanceUpDay, string uspKey)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = currencyId, FromDate = DateTime.Parse(fromDate), ToDate = new DateTime(2018, 1, 1) };

            BalanceUpDataAccess.GetBalanceUp().Returns(new Model.Core.DTO.Configuration.BalanceUp { BalanceUpDay = DateTime.Parse(balanceUpDay) });

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.When(x => x.GetBetListResult(metaModel, uspKey)).DoNotCallBase();

            dynamic betListResult = new ExpandoObject();
            betListResult.Main = "Main";
            betListResult.Total = "Total";
            betListResult.LeagueName = "Name";
            _controller.GetBetListResult(metaModel, uspKey).Returns(betListResult);

            // Action
            _controller.Index(metaModel);

            // Assert
            _controller.Received().GetBetListResult(metaModel, uspKey);
        }

        [TestCase(1, true)]
        [TestCase(RunningTicketTransType, false)]
        public void Index_WhenCall_ReturnCorrectResult(int transType, bool isShowTotal)
        {
            // Arrange
            var metaModel = new BetListMetaModel { CurrencyId = 0, FromDate = new DateTime(2018, 1, 1), ToDate = new DateTime(2018, 1, 1), TransType = transType };

            BalanceUpDataAccess.GetBalanceUp().Returns(new Model.Core.DTO.Configuration.BalanceUp { BalanceUpDay = new DateTime(2018, 1, 2) });

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { 0, "Currency" } } });
            SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns("<Xml/>");

            _controller.When(x => x.GetBetListResult(metaModel, UspKeyOld)).DoNotCallBase();

            dynamic betListResult = new ExpandoObject();
            betListResult.Main = "Main";
            betListResult.Total = "Total";
            betListResult.LeagueName = "Name";
            _controller.GetBetListResult(metaModel, UspKeyOld).Returns(betListResult);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Main, Is.EqualTo(betListResult.Main));
                Assert.That(result.Total, Is.EqualTo(betListResult.Total));
                Assert.That(result.LeagueName, Is.EqualTo(betListResult.LeagueName));
                Assert.That(result.IsShowTotal, Is.EqualTo(isShowTotal));
            });
        }
    }
}