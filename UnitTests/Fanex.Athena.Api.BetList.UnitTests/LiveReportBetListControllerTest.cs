namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System.Collections;
    using System.Collections.Generic;
    using Extension.Helper;
    using Model.BetList.DTO;
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

        private readonly TicketResult _ticketResult = new TicketResult { SumComm = 1, SumPlayerComm = 2, SumLicenseeWL = 3, SumWinLoss = 4 };

        private readonly IList<TicketText> _ticketTexts = new List<TicketText>();

        private LiveReportBetListController _controller;

        private static IEnumerable GetPagingDataTestCaseValue
        {
            get
            {
                yield return new TestCaseData(new
                {
                    CurrencyId = 0,
                    Page = 1,
                    Rows = 50,
                    TotalRows = 90,
                    MetaModelPageIndex = 1,
                    MetaModelPageSize = 50,
                    ResultTotalPage = 2,
                    SumLicenseeWL = 2,
                    SumComm = 1,
                    SumLicenseeWLText = "3.00",
                });
                yield return new TestCaseData(new
                {
                    CurrencyId = 1,
                    Page = 0,
                    Rows = 0,
                    TotalRows = 100,
                    MetaModelPageIndex = DefaultPageIndex,
                    MetaModelPageSize = DefaultPageSize,
                    ResultTotalPage = 1,
                    SumLicenseeWL = -2,
                    SumComm = 1,
                    SumLicenseeWLText = "<span class='negative-number'>-1.00</span>",
                });
            }
        }

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
                CacheRefAdapter,
                BetListHTMLBuilder);

            StubLicenseeSessionData(new LicenseeSession { LicenseeId = SessionLicenseeId });
            SetUpForGetBetListResult();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
        }

        [TearDown]
        public void SetUp()
        {
            BetListService.ClearReceivedCalls();
        }

        [Test]
        public void Match_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = CreateInvalidCurrencyMetaModel();

            // Action
            var result = _controller.Match(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Outright_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var metaModel = CreateInvalidCurrencyMetaModel();

            // Action
            var result = _controller.Outright(metaModel);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void NumberGame_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var page = FakeHelper.Create<int>();
            var rows = FakeHelper.Create<int>();

            var metaModel = CreateInvalidCurrencyMetaModel();

            // Action
            var result = _controller.NumberGame(metaModel, page, rows);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void MixParlay_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var page = FakeHelper.Create<int>();
            var rows = FakeHelper.Create<int>();

            var metaModel = CreateInvalidCurrencyMetaModel();

            // Action
            var result = _controller.MixParlay(metaModel, page, rows);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void VirtualSports_InvalidCurrency_ReturnNull()
        {
            // Arrange
            var page = FakeHelper.Create<int>();
            var rows = FakeHelper.Create<int>();

            var metaModel = CreateInvalidCurrencyMetaModel();

            // Action
            var result = _controller.VirtualSports(metaModel, page, rows);

            // Assert
            Assert.That(result, Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Match_ValidCurrency_ReturnCorrectResult(int currencyId)
        {
            // Arrange
            var metaModel = CreateValidCurrencyMetaModel(currencyId);

            // Action
            var result = _controller.Match(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received().GetBetListData(metaModel, SelWinlossByMatchUsp);
                AssertData(result);
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Outright_ValidCurrency_ReturnCorrectResult(int currencyId)
        {
            // Arrange
            var metaModel = CreateValidCurrencyMetaModel(currencyId);

            // Action
            var result = _controller.Outright(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received().GetBetListData(metaModel, SelWinlossOutrightUsp);
                AssertData(result);
            });
        }

        [TestCaseSource(nameof(GetPagingDataTestCaseValue))]
        public void NumberGame_ValidCurrency_ReturnCorrectResult(dynamic testCaseData)
        {
            // Arrange
            var metaModel = CreateValidCurrencyMetaModel(testCaseData.CurrencyId);
            metaModel.TotalRows = testCaseData.TotalRows;
            metaModel.SumLicenseeWL = testCaseData.SumLicenseeWL;
            metaModel.SumComm = testCaseData.SumComm;

            // Action
            var result = _controller.NumberGame(metaModel, testCaseData.Page, testCaseData.Rows);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received().GetBetListData(metaModel, SelWinlossNumberGameUsp);
                AssertPagingData(
                result,
                metaModel,
                testCaseData.MetaModelPageSize,
                testCaseData.MetaModelPageIndex,
                testCaseData.ResultTotalPage,
                testCaseData.SumLicenseeWLText);
            });
        }

        [TestCaseSource(nameof(GetPagingDataTestCaseValue))]
        public void MixParlay_ValidCurrency_ReturnCorrectResult(dynamic testCaseData)
        {
            // Arrange
            var metaModel = CreateValidCurrencyMetaModel(testCaseData.CurrencyId);
            metaModel.TotalRows = testCaseData.TotalRows;
            metaModel.SumLicenseeWL = testCaseData.SumLicenseeWL;
            metaModel.SumComm = testCaseData.SumComm;

            // Action
            var result = _controller.MixParlay(metaModel, testCaseData.Page, testCaseData.Rows);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received().GetBetListData(metaModel, SelWinlossParlayUsp);
                AssertPagingData(
                result,
                metaModel,
                testCaseData.MetaModelPageSize,
                testCaseData.MetaModelPageIndex,
                testCaseData.ResultTotalPage,
                testCaseData.SumLicenseeWLText);
            });
        }

        [TestCaseSource(nameof(GetPagingDataTestCaseValue))]
        public void RVirtualSports_ValidCurrency_ReturnCorrectResult(dynamic testCaseData)
        {
            // Arrange
            var metaModel = CreateValidCurrencyMetaModel(testCaseData.CurrencyId);
            metaModel.TotalRows = testCaseData.TotalRows;
            metaModel.SumLicenseeWL = testCaseData.SumLicenseeWL;
            metaModel.SumComm = testCaseData.SumComm;

            // Action
            var result = _controller.VirtualSports(metaModel, testCaseData.Page, testCaseData.Rows);

            // Assert
            Assert.Multiple(() =>
            {
                BetListService.Received().GetBetListData(metaModel, SelWinlossVHRacingUsp);
                AssertPagingData(
                    result,
                    metaModel,
                    testCaseData.MetaModelPageSize,
                    testCaseData.MetaModelPageIndex,
                    testCaseData.ResultTotalPage,
                    testCaseData.SumLicenseeWLText);
            });
        }

        private LiveReportBetListMetaModel CreateInvalidCurrencyMetaModel()
        {
            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string>() });

            return new LiveReportBetListMetaModel { CurrencyId = FakeHelper.Create<int>() };
        }

        private LiveReportBetListMetaModel CreateValidCurrencyMetaModel(int currency)
        {
            StubUserSessionData(new UserSession { CurrencyList = new Dictionary<int, string> { { currency, "Currency Name" } } });

            return new LiveReportBetListMetaModel { CurrencyId = currency };
        }

        private void SetUpForGetBetListResult()
        {
            SessionService.GenerateAgentListXml(Arg.Any<int>()).Returns("<Xml/>");

            BetListService.GetBetListData(Arg.Any<LiveReportBetListMetaModel>(), Arg.Any<string>()).Returns(_ticketResult);

            _controller.When(x => x.Ticket(_ticketResult, Arg.Any<BetListMetaModel>(), Arg.Any<int>(), Arg.Any<bool>())).DoNotCallBase();
            _controller.Ticket(_ticketResult, Arg.Any<BetListMetaModel>(), Arg.Any<int>(), Arg.Any<bool>()).Returns(_ticketTexts);
        }

        private void AssertData(dynamic result)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.Main, Is.EqualTo(_ticketTexts));
                Assert.That(result.Total.SumWinLoss, Is.EqualTo(_ticketResult.SumWinLoss + _ticketResult.SumPlayerComm));
                Assert.That(result.Total.SumLicenseeWl, Is.EqualTo(_ticketResult.SumLicenseeWL + _ticketResult.SumComm));
            });
        }

        private void AssertPagingData(dynamic result, LiveReportBetListMetaModel metaModel, int pageSize, int pageIndex, int totalPage, string sumLicenseeWlText)
        {
            var sumWinLossText = NumberHelper.DecFormat(metaModel.SumWinlost, 2);

            Assert.Multiple(() =>
            {
                Assert.That(result.rows, Is.EqualTo(_ticketTexts));
                Assert.That(result.total, Is.EqualTo(totalPage));
                Assert.That(result.records, Is.EqualTo(metaModel.TotalRows));
                Assert.That(result.userdata.WinLoss, Is.EqualTo(sumWinLossText));
                Assert.That(result.userdata.LicenseeWL, Is.EqualTo(sumLicenseeWlText));

                Assert.That(metaModel.PageIndex, Is.EqualTo(pageIndex));
                Assert.That(metaModel.PageSize, Is.EqualTo(pageSize));
            });
        }
    }
}