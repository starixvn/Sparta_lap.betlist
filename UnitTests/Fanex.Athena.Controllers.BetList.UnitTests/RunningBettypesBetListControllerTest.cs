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

    public class RunningBettypesBetListControllerTest : BetListBaseControllerHelperTest
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
        public void MixParlay_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    MultipleBettype = ((int)BetType.CombinationMixParlay).ToString()
                };

                // Action
                var result = controller.MixParlay(metaModel) as ViewResult;

                // Assert
                AssertViewResult(Menu.Menu_12, "TotalBetsMixParlay", metaModel, result);
            }
        }

        [Test]
        public void DoubleChange_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    MultipleBettype = "24,151,186,410,431"
                };

                // Action
                var result = controller.DoubleChange(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.DoubleChance, "TotalBetsDoubleChance", metaModel, result);
            }
        }

        [Test]
        public void BONTeamToScore_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    MultipleBettype = "26,145,146,170,188,427,433"
                };

                // Action
                var result = controller.BONTeamToScore(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.BONTeamToScore, "TotalBetsBON", metaModel, result);
            }
        }

        [Test]
        public void MixParlay_ExportExcel_InvalidCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = -1000,
                    MultipleBettype = "29"
                };

                controller.When(x => x.EmptyExcel()).DoNotCallBase();

                // Action
                controller.MixParlay(metaModel);

                // Assert
                controller.Received().EmptyExcel();
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
                    MultipleBettype = "29"
                };

                CacheRefAdapter.GetBettype("29").Returns("MixParlay");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "MixParlay - " + currencyTitle;

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_SelRunningByBettypeList", metaModel, controller);

                // Action
                controller.MixParlay(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void DoubleChange_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
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
                    MultipleBettype = "24,151,186,410,431"
                };

                CacheRefAdapter.GetBettype("24").Returns("DoubleChange");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "DoubleChange - " + currencyTitle;

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_SelRunningByBettypeList", metaModel, controller);

                // Action
                controller.DoubleChange(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public void BONTeamToScore_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
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
                    MultipleBettype = "26,145,146,170,188,427,433"
                };

                CacheRefAdapter.GetBettype("26").Returns("BONTeamToScore");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "BONTeamToScore - " + currencyTitle;

                var ticketResult = SetUpForExportingExcel("Lrf_BetList_SelRunningByBettypeList", metaModel, controller);

                // Action
                controller.BONTeamToScore(metaModel);

                // Assert
                AssertExcelParameters(expectedTitle, ticketResult, controller);
            }
        }

        private TicketResult SetUpForExportingExcel(string uspKey, BetListMetaModel metaModel, RunningBettypesBetListController controller)
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

        private void AssertExcelParameters(string expectedTitle, TicketResult ticketResult, RunningBettypesBetListController controller)
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/BetList/RunningBetListMultipleBettypes/Main.cshtml"));
                Assert.That(viewModel.ShowEmptyContent, Is.True);
                Assert.That(viewModel.Param.BetType, Is.EqualTo(metaModel.BetType));
                Assert.That(viewModel.Param.MultipleBettype, Is.EqualTo(metaModel.MultipleBettype));
                Assert.That(viewResult.ViewBag.ReportTitle, Is.EqualTo(reportTitle));
                Assert.That(viewResult.ViewBag.UserGuideName, Is.EqualTo(userGuide));
                AssertDefaultBetListViewModel(viewModel.Param, metaModel);
            });
        }

        private RunningBettypesBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RunningBettypesBetListController>(
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