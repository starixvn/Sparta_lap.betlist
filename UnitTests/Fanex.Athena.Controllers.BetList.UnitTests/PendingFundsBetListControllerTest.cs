namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Model.BetList.DTO;
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
                var result = controller.Index(new BetListMetaModel()) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/PendingFunds/Main.cshtml"));
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

        [TestCase("USD", null, "USD", true)]
        [TestCase("USD", "eur", "EUR", false)]
        public void Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expectedCurrency, bool isFakeLicensee)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";

                var metaModel = new BetListMetaModel
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

                var ticketResult = new TicketResult();
                BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), "Lrf_BetList_SelRunningByDate_Mem").Returns(ticketResult);

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
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "1,2,7,8,9"),
                    expectedTitle,
                    "CustomerPendingFundsBetList",
                    1);
            }
        }

        private PendingFundsBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<PendingFundsBetListController>(
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