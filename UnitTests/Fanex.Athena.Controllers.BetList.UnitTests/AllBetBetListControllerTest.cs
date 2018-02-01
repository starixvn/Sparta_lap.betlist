namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Resources;
    using Model.BetList.DTO;
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
                var result = controller.Detail(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/AGCasinoDetail/Main.cshtml"));
            }
        }

        [Test]
        public void Detail_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.Detail(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

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
                var metaModel = new BetListMetaModel
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
                var metaModel = new BetListMetaModel
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

                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyId = 0,
                    Date = new DateTime(2018, 2, 5)
                };
                var expectedTitle = $"{ViewResources.ProductAB} - {ViewResources.DetailedBetlist} - {metaModel.Date.ToString("MM/dd/yyyy")}";

                var ticketResult = new TicketResult();
                BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), "Lrf_BetList_LC_Detail").Returns(ticketResult);

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);

                controller.When(x => x.ProcessExportExcel(
                    Arg.Any<ShowingOption>(),
                    Arg.Any<BetListMetaModel>(),
                    Arg.Any<TicketResult>(),
                    Arg.Any<IEnumerable<int>>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int>())).DoNotCallBase();

                // Action
                controller.Detail(metaModel);

                // Assert
                controller.Received().ProcessExportExcel(
                    Arg.Is<ShowingOption>(x => !x.GrandTotalVisible && x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                    Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake && x.AgentListXml == AgentListXml),
                    ticketResult,
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "1,2,8,9"),
                    expectedTitle,
                    "AllBetDetail",
                    1);
            }
        }

        private AllBetBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<AllBetBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                AuthorizationService,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}