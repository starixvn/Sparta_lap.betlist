namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Constant.DataDomain;
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
        public async Task CleanSheet_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    BetType = (int)BetType.CleanSheet
                };

                // Action
                var result = await controller.CleanSheet(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.CleanSheet, "TotalBetsCleanSheet", metaModel, result);
            }
        }

        [Test]
        public async Task ToWinToNil_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    BetType = (int)BetType.ToWinToNil
                };

                // Action
                var result = await controller.ToWinToNil(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.ToWinToNil, "TotalBetsToWinToNil", metaModel, result);
            }
        }

        [Test]
        public async Task ThreeWayHandicap_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    BetType = (int)BetType.ThreeWayHandicap,
                    MultipleBettype = $"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}"
                };

                // Action
                var result = await controller.ThreeWayHandicap(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.lbl3WayHandicap, "TotalBets3WayHandicap", metaModel, result);
            }
        }

        [Test]
        public async Task CleanSheet_ExportExcel_InvalidCurrency_ReturnEmptyResult()
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
                await controller.CleanSheet(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public async Task CleanSheet_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                CacheRefAdapter.GetBettype((int)BetType.CleanSheet).Returns("CleanSheet");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "CleanSheet - " + currencyTitle;

                SetUpForExportingExcel(metaModel, controller);

                // Action
                await controller.CleanSheet(metaModel);

                // Assert
                await AssertExcelParameters(expectedTitle, "Lrf_BetList_SelRunningByBettype", controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public async Task ToWinToNil_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                CacheRefAdapter.GetBettype((int)BetType.ToWinToNil).Returns("ToWinToNil");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "ToWinToNil - " + currencyTitle;

                SetUpForExportingExcel(metaModel, controller);

                // Action
                await controller.ToWinToNil(metaModel);

                // Assert
                await AssertExcelParameters(expectedTitle, "Lrf_BetList_SelRunningByBettype", controller);
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public async Task ThreeWayHandicap_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency
                };

                CacheRefAdapter.GetBettype((int)BetType.ThreeWayHandicap).Returns("ThreeWayHandicap");

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = "ThreeWayHandicap - " + currencyTitle;

                SetUpForExportingExcel(metaModel, controller);

                // Action
                await controller.ThreeWayHandicap(metaModel);

                // Assert
                await AssertExcelParameters(expectedTitle, "Lrf_BetList_SelRunningByBettypeList", controller);
            }
        }

        private void SetUpForExportingExcel(BetListMetaModel metaModel, RunningBetListController controller)
        {
            SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

            controller.When(async x => await x.ProcessExportExcel(
                Arg.Any<BetListExcelMetaModel>(),
                Arg.Any<string>(),
                Arg.Any<string>())).DoNotCallBase();
        }

        private async Task AssertExcelParameters(string expectedTitle, string uspKey, RunningBetListController controller)
        {
            await controller.Received().ProcessExportExcel(
                Arg.Is<BetListExcelMetaModel>(x => x.ShowingOption.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm
                    && x.Title == expectedTitle
                    && x.UspKey == uspKey
                    && string.Join(",", x.HiddenColumns) == "7,8,9"
                    && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                    && x.IsShowSumStake
                    && x.AgentListXml == AgentListXml),
                "RunningBetList",
               null);
        }

        private void AssertViewResult(string reportTitle, string userGuide, BetListMetaModel metaModel, ViewResult viewResult)
        {
            var viewModel = (BetListBaseViewModel)viewResult.Model;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/BetList/RunningBetList/Main.cshtml"));
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
            return Substitute.ForPartsOf<RunningBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ProductService,
                BalanceUpDataAccess,
                CacheRefAdapter,
                BetListApiClient);
        }
    }
}