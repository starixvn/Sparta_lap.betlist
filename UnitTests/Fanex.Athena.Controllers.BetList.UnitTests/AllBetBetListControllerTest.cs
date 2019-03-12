namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Resources;
    using NSubstitute;
    using NUnit.Framework;

    public class AllBetBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Detail(new BetListExcelMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/AGCasinoDetail/Main.cshtml"));
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewModel()
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
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (BetListExcelMetaModel)result.Model;

                // Assert
                AssertDefaultBetListViewModel(model, metaModel);
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewBag()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel
                {
                    Date = new DateTime(2018, 2, 5)
                };
                var expectedTitle = $"{ViewResources.ProductAB} - {ViewResources.DetailedBetlist} - {metaModel.Date.ToString("MM/dd/yyyy")}";

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewBag.BetListTitle, Is.EqualTo(expectedTitle));
            }
        }

        [Test]
        public void Detail_ExportExcel_InvalidCurrency_ReturnEmptyResult()
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
                controller.Detail(metaModel);

                // Assert
                controller.Received().EmptyExcel();
            }
        }

        [Test]
        public void Detail_ExportExcel_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    Date = new DateTime(2018, 2, 5)
                };
                var expectedTitle = $"{ViewResources.ProductAB} - {ViewResources.DetailedBetlist} - {metaModel.Date.ToString("MM/dd/yyyy")}";

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

                controller.When(x => x.ProcessExportExcel(
                    Arg.Any<BetListExcelMetaModel>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())).DoNotCallBase();

                // Action
                controller.Detail(metaModel);

                // Assert
                controller.Received().ProcessExportExcel(
                    Arg.Is<BetListExcelMetaModel>(x => !x.ShowingOption.GrandTotalVisible
                        && x.ShowingOption.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm
                        && x.Title == expectedTitle
                        && string.Join(",", x.HiddenColumns) == "1,2,8,9"
                        && x.LicenseeId == DefaultLicenseeSession.LicenseeId
                        && !x.IsShowSumStake
                        && x.AgentListXml == AgentListXml),
                    "AllBetDetail",
                    null);
            }
        }

        private AllBetBetListController CreateController()
        {
            return Substitute.ForPartsOf<AllBetBetListController>(
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