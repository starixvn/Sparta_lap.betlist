namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Model.BetList.DTO;
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
                var result = controller.Index(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/RunningBettypesOfMatchBetList/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewModel()
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
                var metaModel = new BetListMetaModel
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

        [TestCase("USD", null, "USD", 0, "1,2,3", true)]
        [TestCase("USD", "eur", "EUR", 1, "2", false)]
        [TestCase("USD", "eur", "EUR", 0, "", false)]
        public void Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expCurrency, int betType, string multiBetType, bool hasMainTickets)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";
                var metaModel = new BetListMetaModel
                {
                    IsExportExcel = true,
                    CurrencyName = currency,
                    BaseCurrency = baseCurrency,
                    BetType = betType,
                    MultipleBettype = multiBetType
                };

                var expectedTitle = GetExpectedExcelTitle(expCurrency, betType, multiBetType, hasMainTickets);

                var ticketResult = new TicketResult();
                if (hasMainTickets)
                {
                    ticketResult.MainTicketList.Add(new LrfTicket { LeagueId = 2 });
                }

                BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), "Lrf_BetList_SelRunningByMultipleBettypeMatch").Returns(ticketResult);
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
                controller.Index(metaModel);

                // Assert
                controller.Received().ProcessExportExcel(
                    Arg.Is<ShowingOption>(x => x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                    Arg.Is<BetListMetaModel>(x => x.LicenseeId == DefaultLicenseeSession.LicenseeId && !x.IsShowSumStake && x.AgentListXml == AgentListXml),
                    ticketResult,
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "7,8,9"),
                    expectedTitle,
                    "RunningBetList",
                    1);
            }
        }

        private string GetExpectedExcelTitle(string expCurrency, int betType, string multiBetType, bool hasMainTickets)
        {
            CacheRefAdapter.GetLeagueName(Arg.Any<int>()).Returns("League");
            CacheRefAdapter.GetBettype(betType).Returns("BetType");

            var bettypeName = "BetType";
            if (betType == 0 && !string.IsNullOrWhiteSpace(multiBetType))
            {
                CacheRefAdapter.GetBettype(int.Parse(multiBetType.Split(',')[0])).Returns("FirstMultipleBettype");
                bettypeName = "FirstMultipleBettype";
            }

            return $"{(hasMainTickets ? "League" : string.Empty)} - {bettypeName} - {string.Format(ViewResources.Viewin, expCurrency)}";
        }

        private RunningBettypesOfMatchBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RunningBettypesOfMatchBetListController>(
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