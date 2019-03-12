namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class PendingFundsBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index(new BetListExcelMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/PendingFunds/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewModel()
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
                var result = controller.Index(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.UserId, Is.EqualTo(DefaultUserSession.UserId));
                    Assert.That(model.BalanceUpDay.Date, Is.EqualTo(BalanceUp.BalanceUpDay.Date));
                    Assert.That(model.MoveBetsDay.Date, Is.EqualTo(BalanceUp.MoveBetsDay.Date));
                    Assert.That(model.Date.Date, Is.EqualTo(metaModel.Date.Date));
                });
            }
        }

        [Test]
        public void Index_ExportExcel_InvalidCurrency_ReturnEmptyResult()
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
                controller.Index(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [TestCase("USD", null, "USD", true)]
        [TestCase("USD", "eur", "EUR", false)]
        public void Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency, bool isFakeLicensee)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    ExternalId = "External",
                    SystemId = "System"
                };

                var fakeLicensees = isFakeLicensee ? new[] { DefaultLicenseeSession.LicenseeId } : Enumerable.Empty<int>();
                AppSettings.FakeLicenseeList.Returns(fakeLicensees);

                var currencyTitle = string.Format(ViewResources.Viewin, expectedCurrency);
                var expectedTitle = $"Running {ViewResources.BetList} - External{(isFakeLicensee ? "" : "(System)")} - {currencyTitle}";

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

                controller.When(x => x.ProcessExportExcel(
                    Arg.Any<BetListExcelMetaModel>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())).DoNotCallBase();

                // Action
                controller.Index(metaModel);

                // Assert
                controller.Received().ProcessExportExcel(
                    Arg.Is<BetListExcelMetaModel>(x => x.ShowingOption.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm
                        && x.Title == expectedTitle
                        && x.UspKey == "Lrf_BetList_SelRunningByDate_Mem"
                        && string.Join(",", x.HiddenColumns) == "1,2,7,8,9"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    "CustomerPendingFundsBetList",
                    null);
            }
        }

        private PendingFundsBetListController CreateController()
        {
            return Substitute.ForPartsOf<PendingFundsBetListController>(
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