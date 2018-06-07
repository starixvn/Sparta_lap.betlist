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

    public class LicenseeWinLossBetListControllerTest : BetListBaseControllerHelperTest
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
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LicenseeWl/Main.cshtml"));
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

        [TestCase("USD", null, "USD", true, "Test", "Test")]
        [TestCase("USD", "eur", "EUR", false, null, "LicenseeWinLossBetList")]
        public void Index_ExportExcel_PassCorrectParam(string currency, string baseCurrency, string expCurrency, bool isFakeLic, string fileName, string expFileName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";
                var metaModel = new BetListMetaModel
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

                var ticketResult = new TicketResult();
                BetListService.GetBetListData(Arg.Any<BetListMetaModel>(), "Lrf_BetList_SelWinloss_Mem").Returns(ticketResult);

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
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "1,2,8"),
                    expectedTitle,
                    expFileName,
                    1);
            }
        }

        private LicenseeWinLossBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<LicenseeWinLossBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}