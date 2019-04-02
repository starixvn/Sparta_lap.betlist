namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class DailyBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();

            ProductService.GetSingleWalletProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.AB } }));

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [Test]
        public async Task Index_WhenGet_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new DailyBetListMetaModel();

                // Action
                var result = await controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/DailyBetList/Main.cshtml"));
            }
        }

        [Test]
        public async Task Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new DailyBetListMetaModel();

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var model = (BetListBaseViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.ShowEmptyContent, Is.True);
                    Assert.That(model.Param.BalanceUpDay, Is.EqualTo(BalanceUp.BalanceUpDay));
                    Assert.That(model.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.Param.UserId, Is.EqualTo(DefaultUserSession.UserId));
                });
            }
        }

        [Test]
        public async Task Index_ExportExcel_InvalidCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new DailyBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = -1000
                };

                controller.When(x => x.EmptyExcel()).DoNotCallBase();

                // Action
                await controller.Index(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase("USD", null, "USD")]
        [TestCase("USD", "eur", "EUR")]
        public async Task Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new DailyBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    Date = new System.DateTime(2018, 1, 1)
                };

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = $"{ViewResources.DailyBetList} {ViewResources.On} 01/01/2018 - {currencyTitle}";

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

                controller.When(async x => await x.ProcessExportExcel(
                    Arg.Any<BetListExcelMetaModel>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())).DoNotCallBase();

                // Action
                await controller.Index(metaModel);

                // Assert
                await controller.Received().ProcessExportExcel(
                    Arg.Is<BetListExcelMetaModel>(x => x.ShowingOption.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm
                        && x.Title == expectedTitle
                        && string.Join(",", x.HiddenColumns) == "8"
                        && x.UspKey == "Lrf_BetList_SelWinlossDaily"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml
                        && x.IsServerPaging
                        && x.PageIndex == 1),
                    "DailyBetList",
                    "/BetList/ExportExcelDailyBetList");
            }
        }

        private DailyBetListController CreateController()
        {
            return Substitute.ForPartsOf<DailyBetListController>(
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