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

    public class LicenseeWinLossBetListControllerTest : BetListBaseControllerHelperTest
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
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/LicenseeWl/Main.cshtml"));
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

        [TestCase("USD", null, "USD", true, "Test", "Test")]
        [TestCase("USD", "eur", "EUR", false, null, "LicenseeWinLossBetList")]
        public async Task Index_ExportExcel_PassCorrectParam(
            string currency, 
            string baseCurrency, 
            string expCurrency, 
            bool isFakeLic, 
            string fileName, 
            string expFileName)
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
                    FromDate = new DateTime(2018, 11, 1),
                    ToDate = new DateTime(2018, 12, 2),
                    ExternalId = "External",
                    SystemId = "System",
                    ExcelFileName = fileName
                };
                var fakeLicensees = isFakeLic ? new[] { DefaultLicenseeSession.LicenseeId } : Enumerable.Empty<int>();
                AppSettings.FakeLicenseeList.Returns(fakeLicensees);

                var expectedTitle = $"{ViewResources.Customer} {ViewResources.BetList} - 11/1/2018 --> 12/2/2018 - "
                    + $"External{(isFakeLic ? "" : "(System)")} - {string.Format(ViewResources.Viewin, expCurrency)}";

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
                        && string.Join(",", x.HiddenColumns) == "1,2,8"
                        && x.UspKey == "Lrf_BetList_SelWinloss_Mem"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    expFileName,
                    null);
            }
        }

        private LicenseeWinLossBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<LicenseeWinLossBetListController>(
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