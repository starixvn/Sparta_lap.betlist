namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class RNGCasinoBetListControllerTest : BetListBaseControllerHelperTest
    {
        private static IEnumerable BetListTitleTestCase
        {
            get
            {
                const long TransTypeYesterday = 0;
                const long TransTypeTodayRunning = 3;
                var title = $"{ViewResources.ProductCS} - {ViewResources.BetList} - {ViewResources.DetailTickets} - ";

                yield return new TestCaseData(TransTypeYesterday, title + ViewResources.Yesterday);
                yield return new TestCaseData(TransTypeTodayRunning, title + ViewResources.Today + " - " + ViewResources.Running);
                yield return new TestCaseData(1, title + ViewResources.Today + " - " + ViewResources.Settled);
            }
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public async Task Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = await controller.Detail(new BetListExcelMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/RngCasinoDetail/Main.cshtml"));
            }
        }

        [Test]
        public async Task Detail_WhenCall_ReturnCorrectViewModel()
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
                var result = await controller.Detail(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                AssertDefaultBetListViewModel(model, metaModel);
            }
        }

        [TestCaseSource(nameof(BetListTitleTestCase))]
        public async Task Detail_WhenCall_ReturnCorrectViewBag(long transType, string expectedTitle)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    TransType = transType
                };

                // Action
                var result = await controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewBag.BetListTitle, Is.EqualTo(expectedTitle));
            }
        }

        [Test]
        public async Task Detail_ExportExcel_InvalidCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = -1000
                };

                controller.When(x => x.EmptyExcel()).DoNotCallBase();

                // Action
                await controller.Detail(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCaseSource(nameof(BetListTitleTestCase))]
        public async Task Detail_ExportExcel_PassCorrectParam(long transType, string expectedTitle)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new BetListExcelMetaModel
                {
                    TransType = transType,
                    IsExportExcel = true,
                    CurrencyId = 0
                };

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

                controller.When(async x => await x.ProcessExportExcel(
                    Arg.Any<BetListExcelMetaModel>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())).DoNotCallBase();

                // Action
                await controller.Detail(metaModel);

                // Assert
                await controller.Received().ProcessExportExcel(
                    Arg.Is<BetListExcelMetaModel>(x => x.Title == expectedTitle
                        && x.UspKey == "Lrf_BetList_RNG_Detail"
                        && string.Join(",", x.HiddenColumns) == "1,2,8,9"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    "RngCasinoDetail",
                    null);
            }
        }

        private RNGCasinoBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RNGCasinoBetListController>(
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