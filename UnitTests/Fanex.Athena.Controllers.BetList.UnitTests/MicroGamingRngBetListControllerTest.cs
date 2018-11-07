namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class MicroGamingRngBetListControllerTest : BetListBaseControllerHelperTest
    {
        private static IEnumerable BetListTitleTestCase
        {
            get
            {
                const long TransTypeYesterday = 0;
                const long TransTypeTodayRunning = 3;
                var title = $"{ViewResources.Microgaming} - {ViewResources.ProductCS} {ViewResources.BetList} - {ViewResources.DetailTickets} - ";

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
        public void Detail_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Detail(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MicroGamingRngDetail/Main.cshtml"));
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

        [TestCaseSource(nameof(BetListTitleTestCase))]
        public void Detail_WhenCall_ReturnCorrectViewBag(long transType, string expectedTitle)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    TransType = transType
                };

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

        [TestCaseSource(nameof(BetListTitleTestCase))]
        public void Detail_ExportExcel_PassCorrectParam(long transType, string expectedTitle)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new BetListMetaModel
                {
                    TransType = transType,
                    IsExportExcel = true,
                    CurrencyId = 0
                };

                var ticketResult = new TicketResult();
                BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), "Lrf_BetList_RNG_Detail").Returns(ticketResult);

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
                    Arg.Is<ShowingOption>(x => !x.GrandTotalVisible),
                    Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake && x.AgentListXml == AgentListXml),
                    ticketResult,
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "1,2,8,9"),
                    expectedTitle,
                    "MicrogamingRNGDetail",
                    1);
            }
        }

        private MicroGamingRngBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<MicroGamingRngBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}