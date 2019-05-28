namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class LiveReportBetListControllerTest : ApiBetListBaseControllerHelperTest
    {
        private const string SelWinlossByMatchUsp = "Lrf_BetList_LiveReport_SelWinlossByMatch";

        private const string SelWinlossOutrightUsp = "Lrf_BetList_LiveReport_SelWinlossOutright";

        private const string SelWinlossNumberGameUsp = "Lrf_BetList_LiveReport_SelWinlossNumberGame";

        private const string SelWinlossParlayUsp = "Lrf_BetList_LiveReport_SelWinlossParlay";

        private const string SelWinlossVHRacingUsp = "Lrf_BetList_LiveReport_SelWinlossVHRacing";

        private const int DefaultPageSize = 100;

        private const int DefaultPageIndex = 1;

        private const string ApiPath = "/LiveReportBetList/GetData";

        private const string PageApiPath = "/LiveReportBetList/GetPagingData";

        [SetUp]
        public void SetUp()
        {
            MockSessionData();

            AuthorizationService.HasPermissionOnData(SessionLicenseeId, Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public async Task Match_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateInvalidCurrencyMetaModel();

                // Action
                var result = await controller.Match(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task Outright_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateInvalidCurrencyMetaModel();

                // Action
                var result = await controller.Outright(metaModel);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task NumberGame_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var page = FakeHelper.Create<int>();
                var rows = FakeHelper.Create<int>();

                var metaModel = CreateInvalidCurrencyMetaModel();

                // Action
                var result = await controller.NumberGame(metaModel, page, rows);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task MixParlay_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var page = FakeHelper.Create<int>();
                var rows = FakeHelper.Create<int>();

                var metaModel = CreateInvalidCurrencyMetaModel();

                // Action
                var result = await controller.MixParlay(metaModel, page, rows);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public async Task VirtualSports_InvalidCurrency_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var page = FakeHelper.Create<int>();
                var rows = FakeHelper.Create<int>();

                var metaModel = CreateInvalidCurrencyMetaModel();

                // Action
                var result = await controller.VirtualSports(metaModel, page, rows);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Match_ValidCurrency_CallBetListApiWithCorrectParameters(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateValidCurrencyMetaModel(currencyId);

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, ApiPath, expect);

                // Action
                var actual = await controller.Match(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(SelWinlossByMatchUsp));
                    BetListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public async Task Outright_ValidCurrency_CallBetListApiWithCorrectParameters(int currencyId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateValidCurrencyMetaModel(currencyId);

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, ApiPath, expect);

                // Action
                var actual = await controller.Outright(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(SelWinlossOutrightUsp));
                    BetListApiClient.Received().PostAsync(ApiPath, metaModel, SessionLanguage);
                });
            }
        }

        [TestCase(0, 1, 50, 1, 50)]
        [TestCase(1, -1, 100, DefaultPageIndex, 100)]
        [TestCase(2, 2, -1, 2, DefaultPageSize)]
        public async Task NumberGame_ValidCurrency_CallBetListApiWithCorrectParameters(
            int currencyId,
            int pageIndex,
            int pageSize,
            int expectedPageIndex,
            int expectedPageSize)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateValidCurrencyMetaModel(currencyId);

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, PageApiPath, expect);

                // Action
                var actual = await controller.NumberGame(metaModel, pageIndex, pageSize);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(SelWinlossNumberGameUsp));
                    Assert.That(metaModel.PageIndex, Is.EqualTo(expectedPageIndex));
                    Assert.That(metaModel.PageSize, Is.EqualTo(expectedPageSize));
                    BetListApiClient.Received().PostAsync(PageApiPath, metaModel, SessionLanguage);
                });
            }
        }

        [TestCase(0, 1, 50, 1, 50)]
        [TestCase(1, -1, 100, DefaultPageIndex, 100)]
        [TestCase(2, 2, -1, 2, DefaultPageSize)]
        public async Task MixParlay_ValidCurrency_CallBetListApiWithCorrectParameters(
            int currencyId,
            int pageIndex,
            int pageSize,
            int expectedPageIndex,
            int expectedPageSize)
        {
            using (var controller = CreateController())
            {
                // Arrange
                LiveReportBetListMetaModel metaModel = CreateValidCurrencyMetaModel(currencyId);

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, PageApiPath, expect);

                // Action
                var actual = await controller.MixParlay(metaModel, pageIndex, pageSize);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(SelWinlossParlayUsp));
                    Assert.That(metaModel.PageIndex, Is.EqualTo(expectedPageIndex));
                    Assert.That(metaModel.PageSize, Is.EqualTo(expectedPageSize));
                    BetListApiClient.Received().PostAsync(PageApiPath, metaModel, SessionLanguage);
                });
            }
        }

        [TestCase(0, 1, 50, 1, 50)]
        [TestCase(1, -1, 100, DefaultPageIndex, 100)]
        [TestCase(2, 2, -1, 2, DefaultPageSize)]
        public async Task VirtualSports_ValidCurrency_CallBetListApiWithCorrectParameters(
            int currencyId,
            int pageIndex,
            int pageSize,
            int expectedPageIndex,
            int expectedPageSize)
        {
            using (var controller = CreateController())
            {
                // Arrange
                LiveReportBetListMetaModel metaModel = CreateValidCurrencyMetaModel(currencyId);

                var expect = new HttpResponseMessage();
                StubBetListApiPostAsync(metaModel, PageApiPath, expect);

                // Action
                var actual = await controller.VirtualSports(metaModel, pageIndex, pageSize);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(expect, Is.EqualTo(actual));
                    Assert.That(metaModel.UspKey, Is.EqualTo(SelWinlossVHRacingUsp));
                    Assert.That(metaModel.PageIndex, Is.EqualTo(expectedPageIndex));
                    Assert.That(metaModel.PageSize, Is.EqualTo(expectedPageSize));
                    BetListApiClient.Received().PostAsync(PageApiPath, metaModel, SessionLanguage);
                });
            }
        }

        private LiveReportBetListMetaModel CreateInvalidCurrencyMetaModel()
        {
            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            return new LiveReportBetListMetaModel { CurrencyId = FakeHelper.Create<int>() };
        }

        private LiveReportBetListMetaModel CreateValidCurrencyMetaModel(int currency)
        {
            StubUserSessionData(new UserSession { Language = SessionLanguage, CurrencyList = new Dictionary<int, string> { { currency, "Currency Name" } } });

            return new LiveReportBetListMetaModel { CurrencyId = currency };
        }

        private LiveReportBetListController CreateController()
        {
            return new LiveReportBetListController(
                SessionService,
                ProductService,
                LangService,
                AuthorizationService,
                BetListApiClient,
                BalanceUpDataAccess);
        }
    }
}