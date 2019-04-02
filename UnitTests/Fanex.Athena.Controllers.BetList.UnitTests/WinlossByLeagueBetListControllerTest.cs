namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class WinlossByLeagueBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = await controller.Index(new WinLossByLeagueBetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/WinlossByLeague/Main.cshtml"));
            }
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossByLeagueBetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                AssertDefaultBetListViewModel(model, metaModel);
            }
        }

        [Test]
        public async Task Index_ExportExcel_InvalidCurrency_ReturnEmptyResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossByLeagueBetListMetaModel
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

        [TestCase(1, "USD", null, "USD", true)]
        [TestCase(1, "USD", "eur", "EUR", false)]
        [TestCase(0, "USD", null, null, false)]
        public async Task Index_ExportExcel_PassCorrectParam(int currencyId, string currency, string baseCurrency, string expectedCurrency, bool isFakeLicensee)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";
                var metaModel = new WinLossByLeagueBetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = currencyId,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    ExternalId = "External",
                    SystemId = "System",
                    FromDate = new DateTime(2018, 1, 1),
                    ToDate = new DateTime(2018, 1, 2)
                };

                var fakeLicensees = isFakeLicensee ? new[] { DefaultLicenseeSession.LicenseeId } : Enumerable.Empty<int>();
                AppSettings.FakeLicenseeList.Returns(fakeLicensees);

                var currencyTitle = currencyId == 0 ? ViewResources.AllCurrencies : string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = $"{ViewResources.CustomerBetList} - 1/1/2018 → 1/2/2018 - External{(isFakeLicensee ? "" : "(System)")} - {currencyTitle}";

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
                        && x.UspKey == "Lrf_BetList_WinlostByLeague_Mem"
                        && string.Join(",", x.HiddenColumns) == "1,2,8"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    "WinLossByLeagueBetList",
                    "/BetList/ExportExcelWinlossByLeague");
            }
        }

        private WinlossByLeagueBetListController CreateController()
        {
            return Substitute.ForPartsOf<WinlossByLeagueBetListController>(
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