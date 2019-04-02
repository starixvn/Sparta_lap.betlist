namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class MatchWinLossBetListControllerTest : BetListBaseControllerHelperTest
    {
        private const string AgentListXml = "<xml/>";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public async Task MatchBetType_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = await controller.MatchBetType(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/MatchWl/MatchBetType.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public async Task MixParlay_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = await controller.MixParlay(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/MatchWl/MixParlay.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public async Task Outright_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = await controller.Outright(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/MatchWl/Outright.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public async Task MatchBetType_ExportExcel_InvalidCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = -1000
                };

                controller.When(x => x.EmptyExcel()).DoNotCallBase();

                // Action
                await controller.MatchBetType(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase(1, "Lrf_BetList_MatchWL_Bettype")]
        public async Task MatchBetType_ExportExcel_PassCorrectParam(int betTypeId, string uspKey)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = "USD",
                    FromDate = new DateTime(2018, 11, 11),
                    BetType = betTypeId,
                    HomeId = 2,
                    AwayId = 0
                };

                CacheRefAdapter.GetBettype(metaModel.BetType).Returns("BetType");

                var currencyTitle = string.Format(ViewResources.Viewin, "USD");
                var expectedTitle = $"{ViewResources.BetList} - BetType - 11/11/2018 - {currencyTitle}";

                SetUpForExportingExcel(metaModel, controller);

                // Action
                await controller.MatchBetType(metaModel);

                // Assert
                await AssertExcelParameters(expectedTitle, uspKey, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public async Task MixParlay_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 11, 11),
                    BetType = 1,
                    HomeId = 2,
                    AwayId = 3
                };

                CacheRefAdapter.GetTeamName(metaModel.HomeId).Returns("Home");
                CacheRefAdapter.GetTeamName(metaModel.AwayId).Returns("Away");
                CacheRefAdapter.GetBettype(metaModel.BetType).Returns("BetType");

                var matchName = "Home vs Away - BetType - ";
                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = $"{ViewResources.BetList} - {matchName}11/11/2018 - {currencyTitle}";

                SetUpForExportingExcel(metaModel, controller);

                // Action
                await controller.MixParlay(metaModel);

                // Assert
                await AssertExcelParameters(expectedTitle, "Lrf_BetList_MatchWL_Parlay", controller);
            }
        }

        [Test]
        public async Task Outright_ExportExcel_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = "USD",
                    FromDate = new DateTime(2018, 11, 11)
                };

                CacheRefAdapter.GetBettype(metaModel.BetType).Returns(string.Empty);

                var currencyTitle = string.Format(ViewResources.Viewin, "USD");
                var expectedTitle = $"{ViewResources.BetList} - 11/11/2018 - {currencyTitle}";

                SetUpForExportingExcel(metaModel, controller);

                // Action
                await controller.Outright(metaModel);

                // Assert
                await AssertExcelParameters(expectedTitle, "Lrf_BetList_MatchWL_Outright", controller);
            }
        }

        private void SetUpForExportingExcel(BetListMetaModel metaModel, MatchWinLossBetListController controller)
        {
            SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

            controller.When(async x => await x.ProcessExportExcel(
                Arg.Any<BetListExcelMetaModel>(),
                Arg.Any<string>(),
                Arg.Any<string>())).DoNotCallBase();
        }

        private async Task AssertExcelParameters(string expectedTitle, string uspKey, MatchWinLossBetListController controller)
        {
            await controller.Received().ProcessExportExcel(
                Arg.Is<BetListExcelMetaModel>(x => x.ShowingOption.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm
                    && x.Title == expectedTitle
                    && x.UspKey == uspKey
                    && string.Join(",", x.HiddenColumns) == "8"
                    && x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake
                    && x.AgentListXml == AgentListXml && x.IsServerPaging && x.PageIndex == 1),
                "MatchWinLossBetList",
                null);
        }

        private MatchWinLossBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<MatchWinLossBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ProductService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                BetListApiClient);
            return controller;
        }
    }
}