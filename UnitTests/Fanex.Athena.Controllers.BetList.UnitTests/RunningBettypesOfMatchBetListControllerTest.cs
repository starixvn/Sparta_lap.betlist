namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class RunningBettypesOfMatchBetListControllerTest : BetListBaseControllerHelperTest
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
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/RunningBettypesOfMatchBetList/Main.cshtml"));
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
                AssertDefaultBetListViewModel(model, metaModel);
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

        [TestCase("USD", null, "USD", 0, "1,2,3")]
        [TestCase("USD", "eur", "EUR", 1, "2")]
        [TestCase("USD", "eur", "EUR", 0, "")]
        public void Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expCurrency, int betType, string multiBetType)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    BetType = betType,
                    MultipleBettype = multiBetType
                };

                var expectedTitle = GetExpectedExcelTitle(expCurrency, betType, multiBetType);

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
                        && x.UspKey == "Lrf_BetList_SelRunningByMultipleBettypeMatch"
                        && string.Join(",", x.HiddenColumns) == "7,8,9"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    "RunningBetList",
                    null);
            }
        }

        private string GetExpectedExcelTitle(string expCurrency, int betType, string multiBetType)
        {
            CacheRefAdapter.GetLeagueName(Arg.Any<int>()).Returns("League");
            CacheRefAdapter.GetBettype(betType).Returns("BetType");

            var bettypeName = "BetType";
            if (betType == 0 && !string.IsNullOrWhiteSpace(multiBetType))
            {
                CacheRefAdapter.GetBettype(int.Parse(multiBetType.Split(',')[0])).Returns("FirstMultipleBettype");
                bettypeName = "FirstMultipleBettype";
            }

            return $"(LEAGUENAME) - {bettypeName} - {string.Format(ViewResources.Viewin, expCurrency)}";
        }

        private RunningBettypesOfMatchBetListController CreateController()
        {
            return Substitute.ForPartsOf<RunningBettypesOfMatchBetListController>(
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