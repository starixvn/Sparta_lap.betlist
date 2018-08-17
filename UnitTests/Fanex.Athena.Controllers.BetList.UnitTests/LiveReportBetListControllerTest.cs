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

    public class LiveReportBetListControllerTest : BetListBaseControllerHelperTest
    {
        private const string AgentListXml = "<xml/>";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Match_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.Match(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/Match.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void MixParlay_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.MixParlay(null) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/MixParlay.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, new BetListMetaModel());
                });
            }
        }

        [Test]
        public void Outright_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.Outright(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/Outright.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void NumberGame_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.NumberGame(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/NumberGame.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void VirtualSports_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string SportName = "VirtualSports";

                var metaModel = CreateMetaModel();
                CacheRefAdapter.GetSportName(metaModel.SportType).Returns(SportName);

                // Action
                var result = controller.VirtualSports(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/VirtualSports.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.EqualTo(SportName));
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void Match_ExportExcel_InvalidCurrency_ReturnEmptyResult()
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
                controller.Match(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void Match_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12),
                    PageIndex = 2,
                    PageSize = 10,
                    HomeId = 2,
                    AwayId = 3
                };
                var startIndex = 11;

                CacheRefAdapter.GetTeamName(metaModel.HomeId).Returns("Home");
                CacheRefAdapter.GetTeamName(metaModel.AwayId).Returns("Away");

                var viewInCaption = string.Format(ViewResources.Viewin, expectedCurrency);
                var betlistCaption = "Home vs Away ";
                var expectedTitle = $"{ViewResources.BetList} - {betlistCaption} - 11/11/2018 --> 12/12/2018 - {viewInCaption}";

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_LiveReport_SelWinlossByMatch", metaModel, controller);

                // Action
                controller.Match(metaModel);

                // Assert
                AssertExcelParameters(startIndex, expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void MixParlay_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12),
                    PageIndex = 3,
                    PageSize = 20,
                    HomeId = 2,
                    AwayId = 0
                };
                var startIndex = 41;

                var viewInCaption = string.Format(ViewResources.Viewin, expectedCurrency);
                var betlistCaption = string.Empty;
                var expectedTitle = $"{ViewResources.BetList} - {betlistCaption} - 11/11/2018 --> 12/12/2018 - {viewInCaption}";

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_LiveReport_SelWinlossParlay", metaModel, controller);

                // Action
                controller.MixParlay(metaModel);

                // Assert
                AssertExcelParameters(startIndex, expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void Outright_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12),
                    PageIndex = 1,
                    PageSize = 20
                };
                var startIndex = 1;

                var viewInCaption = string.Format(ViewResources.Viewin, expectedCurrency);
                var betlistCaption = ViewResources.Outright;
                var expectedTitle = $"{ViewResources.BetList} - {betlistCaption} - 11/11/2018 --> 12/12/2018 - {viewInCaption}";

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_LiveReport_SelWinlossOutright", metaModel, controller);

                // Action
                controller.Outright(metaModel);

                // Assert
                AssertExcelParameters(startIndex, expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void NumberGame_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12),
                    PageIndex = 1,
                    PageSize = 20
                };
                var startIndex = 1;

                var viewInCaption = string.Format(ViewResources.Viewin, expectedCurrency);
                var betlistCaption = ViewResources.ProductNG;
                var expectedTitle = $"{ViewResources.BetList} - {betlistCaption} - 11/11/2018 --> 12/12/2018 - {viewInCaption}";

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_LiveReport_SelWinlossNumberGame", metaModel, controller);

                // Action
                controller.NumberGame(metaModel);

                // Assert
                AssertExcelParameters(startIndex, expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void VirtualSports_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12),
                    PageIndex = 1,
                    PageSize = 20
                };
                var startIndex = 1;

                const string BetlistCaption = "VirtualSports";
                CacheRefAdapter.GetSportName(metaModel.SportType).Returns(BetlistCaption);
                var viewInCaption = string.Format(ViewResources.Viewin, expectedCurrency);
                var betlistCaption = ViewResources.Racing;
                var expectedTitle = $"{ViewResources.BetList} - {BetlistCaption} - 11/11/2018 --> 12/12/2018 - {viewInCaption}";

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_LiveReport_SelWinlossVHRacing", metaModel, controller);

                // Action
                controller.VirtualSports(metaModel);

                // Assert
                AssertExcelParameters(startIndex, expectedTitle, ticketResult, controller);
            }
        }

        private TicketResult SetUpForExportingExcel(string uspKey, LiveReportBetListMetaModel metaModel, LiveReportBetListController controller)
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

        private void AssertExcelParameters(int expectedStartIndex, string expectedTitle, TicketResult ticketResult, LiveReportBetListController controller)
        {
            controller.Received().ProcessExportExcel(
                Arg.Is<ShowingOption>(x => x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake && x.AgentListXml == AgentListXml),
                ticketResult,
                Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "8"),
                expectedTitle,
                "LiveReportBetList",
                expectedStartIndex);
        }

        private LiveReportBetListMetaModel CreateMetaModel()
        {
            return new LiveReportBetListMetaModel
            {
                FromDate = new DateTime(2018, 2, 1),
                ToDate = new DateTime(2018, 2, 10),
                Date = new DateTime(2018, 2, 5)
            };
        }

        private LiveReportBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<LiveReportBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}