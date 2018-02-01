namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Model.BetList.DTO;
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
        public void MatchBetType_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.MatchBetType(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MatchWl/MatchBetType.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void MixParlay_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.MixParlay(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MatchWl/MixParlay.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void Outright_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.Outright(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MatchWl/Outright.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void MatchBetType_ExportExcel_InvalidCurrency_ReturnEmptyResult()
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
                controller.MatchBetType(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase(1, "Lrf_BetList_MatchWL_Bettype")]
        [TestCase(31, "Lrf_BetList_MatchWL_Racing")]
        public void MatchBetType_ExportExcel_PassCorrectParam(int betTypeId, string uspKey)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
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

                var ticketResult = SetUpForExportingExcel(uspKey, metaModel, controller);

                // Action
                controller.MatchBetType(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void MixParlay_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
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

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_MatchWL_Parlay", metaModel, controller);

                // Action
                controller.MixParlay(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        [Test]
        public void Outright_ExportExcel_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = "USD",
                    FromDate = new DateTime(2018, 11, 11)
                };

                CacheRefAdapter.GetBettype(metaModel.BetType).Returns(string.Empty);

                var currencyTitle = string.Format(ViewResources.Viewin, "USD");
                var expectedTitle = $"{ViewResources.BetList} - 11/11/2018 - {currencyTitle}";

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_MatchWL_Outright", metaModel, controller);

                // Action
                controller.Outright(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        private TicketResult SetUpForExportingExcel(string uspKey, BetListMetaModel metaModel, MatchWinLossBetListController controller)
        {
            var ticketResult = new TicketResult();
            BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), uspKey).Returns(ticketResult);

            SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

            controller.When(x => x.ProcessExportExcel(
                Arg.Any<ShowingOption>(),
                Arg.Any<BetListMetaModel>(),
                Arg.Any<TicketResult>(),
                Arg.Any<IEnumerable<int>>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<int>())).DoNotCallBase();

            return ticketResult;
        }

        private void AssertExcelParameters(string expectedTitle, TicketResult ticketResult, MatchWinLossBetListController controller)
        {
            controller.Received().ProcessExportExcel(
                Arg.Is<ShowingOption>(x => x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake 
                    && x.AgentListXml == AgentListXml && x.IsServerPaging && x.PageIndex == 1),
                ticketResult,
                Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "8"),
                expectedTitle,
                "MatchWinLossBetList",
                1);
        }

        private MatchWinLossBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<MatchWinLossBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                AuthorizationService,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}