namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class PlatformWinLossBetListControllerTest : BetListBaseControllerHelperTest
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
                var result = await controller.Index(new BetListExcelMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/PlatformWinLoss/Main.cshtml"));
            }
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectViewModel()
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
                var metaModel = new BetListExcelMetaModel
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

        [TestCase("USD", "eur", "EUR", false, PlatformType.Asia, nameof(PlatformType.Asia))]
        [TestCase("USD", "usd", "USD", true, PlatformType.China, nameof(PlatformType.China))]
        [TestCase("USD", "gbp", "GBP", true, PlatformType.Euro, nameof(PlatformType.Euro))]
        [TestCase("USD", null, "USD", true, PlatformType.Wap, nameof(PlatformType.Wap))]
        [TestCase("USD", null, "USD", true, PlatformType.Mobile, nameof(PlatformType.Mobile))]
        [TestCase("USD", null, "USD", true, PlatformType.Tablet, nameof(PlatformType.Tablet))]
        [TestCase("USD", null, "USD", true, PlatformType.NewAsia, nameof(PlatformType.NewAsia))]
        [TestCase("USD", null, "USD", true, PlatformType.All, nameof(PlatformType.All))]
        public async Task Index_ExportExcel_PassCorrectParam(
            string currency, 
            string baseCurrency, 
            string expectedCurrency, 
            bool isFakeLic, 
            int platform, 
            string platformName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 1,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    FromDate = new DateTime(2018, 1, 1),
                    ToDate = new DateTime(2018, 2, 2),
                    ExternalId = "External",
                    SystemId = "System",
                    PlatformType = platform
                };

                var fakeLicensees = isFakeLic ? new[] { DefaultLicenseeSession.LicenseeId } : Enumerable.Empty<int>();
                AppSettings.FakeLicenseeList.Returns(fakeLicensees);

                var expectedTitle = $"{ViewResources.Customer} {ViewResources.BetList} - 1/1/2018 --> 2/2/2018 - External{(isFakeLic ? "" : "(System)")}"
                    + $" - {ViewResources.Platform}: {ViewResources.ResourceManager.GetString(platformName)} - {string.Format(ViewResources.Viewin, expectedCurrency)}";

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
                        && x.UspKey == "Lrf_BetList_PlatformWinLoss_Mem"
                        && string.Join(",", x.HiddenColumns) == "1,2,8"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    "PlatformWinLossBetList",
                    null);
            }
        }

        private PlatformWinLossBetListController CreateController()
        {
            return Substitute.ForPartsOf<PlatformWinLossBetListController>(
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