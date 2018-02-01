namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Excel.Base.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class RunningBetListControllerTest : BetListBaseControllerHelperTest
    {
        private const string AgentListXml = "<xml/>";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.AB } }));

            SetUpDefaultFakeData();

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [Test]
        public void CleanSheet_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    BetType = (int)BetType.CleanSheet
                };

                // Action
                var result = controller.CleanSheet(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.CleanSheet, "TotalBetsCleanSheet", metaModel, result);
            }
        }

        [Test]
        public void ToWinToNil_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    BetType = (int)BetType.ToWinToNil
                };

                // Action
                var result = controller.ToWinToNil(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.ToWinToNil, "TotalBetsToWinToNil", metaModel, result);
            }
        }

        [Test]
        public void ThreeWayHandicap_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    BetType = (int)BetType.ThreeWayHandicap,
                    MultipleBettype = $"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}"
                };

                // Action
                var result = controller.ThreeWayHandicap(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.lbl3WayHandicap, "TotalBets3WayHandicap", metaModel, result);
            }
        }

        [Test]
        public void CleanSheet_ExportExcel_InvalidCurrency_ReturnEmptyResult()
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
                controller.CleanSheet(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void CleanSheet_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                CacheRefAdapter.GetBettype((int)BetType.CleanSheet).Returns("CleanSheet");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "CleanSheet - " + currencyTitle;

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_SelRunningByBettype", metaModel, controller);

                // Action
                controller.CleanSheet(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void ToWinToNil_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                CacheRefAdapter.GetBettype((int)BetType.ToWinToNil).Returns("ToWinToNil");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "ToWinToNil - " + currencyTitle;

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_SelRunningByBettype", metaModel, controller);

                // Action
                controller.ToWinToNil(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void ThreeWayHandicap_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                CacheRefAdapter.GetBettype((int)BetType.ThreeWayHandicap).Returns("ThreeWayHandicap");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "ThreeWayHandicap - " + currencyTitle;

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_SelRunningByBettypeList", metaModel, controller);

                // Action
                controller.ThreeWayHandicap(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        private TicketResult SetUpForExportingExcel(string uspKey, BetListMetaModel metaModel, RunningBetListController controller)
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

        private void AssertExcelParameters(string expectedTitle, TicketResult ticketResult, RunningBetListController controller)
        {
            controller.Received().ProcessExportExcel(
                Arg.Is<ShowingOption>(x => x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && x.IsShowSumStake && x.AgentListXml == AgentListXml),
                ticketResult,
                Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "7,8,9"),
                expectedTitle,
                "RunningBetList",
                1);
        }

        private void AssertViewResult(string reportTitle, string userGuide, BetListMetaModel metaModel, ViewResult viewResult)
        {
            var viewModel = (BetListBaseViewModel)viewResult.Model;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/BetList/RunningBetList/Main.cshtml"));
                Assert.That(viewModel.ShowEmptyContent, Is.True);
                Assert.That(viewModel.Param.BetType, Is.EqualTo(metaModel.BetType));
                Assert.That(viewModel.Param.MultipleBettype, Is.EqualTo(metaModel.MultipleBettype));
                Assert.That(viewResult.ViewBag.ReportTitle, Is.EqualTo(reportTitle));
                Assert.That(viewResult.ViewBag.UserGuideName, Is.EqualTo(userGuide));
                AssertDefaultBetListViewModel(viewModel.Param, metaModel);
            });
        }

        private RunningBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RunningBetListController>(
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