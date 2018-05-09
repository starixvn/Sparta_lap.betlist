namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;

    public class WinlossByBetTypeBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string UspKey = "Lrf_BetList_SelWinLossByBetType";

        private WinlossByBetTypeBetListController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _controller = Substitute.ForPartsOf<WinlossByBetTypeBetListController>(
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
        public void Detail_InvalidCurrency_ReturnNull()
        {
            // Arrange
            const int CurrencyId = 1;

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            // Action
            var result = _controller.Index(1, 50, CurrencyId, string.Empty, string.Empty, DateTime.Today, DateTime.Today, 1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0, "usd", "usd")]
        [TestCase(1, null, "")]
        [TestCase(2, "", "")]
        public void Detail_WhenCall_GetBetListResultWithCorrectParameter(int currencyId, string baseCurrency, string expectedBaseCurrency)
        {
            // Arrange
            const int Page = 2;
            const int Rows = 50;
            const string CurrencyName = "USD";
            var fromDate = new DateTime(2018, 1, 1);
            var toDate = new DateTime(2018, 1, 2);
            const int BetType = 1;
            const string SortBy = "Index";
            const string SortOrder = "asc";

            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } } });
            SessionService.GenerateAgentListXml(currencyId).Returns("<Xml/>");

            _controller.When(x => x.GetPagingBetListResult(Arg.Any<BetListMetaModel>(), UspKey, Page, Rows)).DoNotCallBase();

            // Action
            _controller.Index(Page, Rows, currencyId, CurrencyName, baseCurrency, fromDate, toDate, BetType, SortBy, SortOrder);

            // Assert
            _controller.Received().GetPagingBetListResult(
                Arg.Is<BetListMetaModel>(x => x.CurrencyName == CurrencyName && x.BaseCurrency == expectedBaseCurrency && x.SortBy == SortBy && x.SortOrder == SortOrder),
                UspKey,
                Page,
                Rows);
        }
    }
}