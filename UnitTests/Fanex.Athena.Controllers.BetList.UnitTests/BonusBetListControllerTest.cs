namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Excel.Base.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class BonusBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.AB } }));

            SetUpDefaultFakeData();

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [Test]
        public void Detail_NotExportExcel_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel();

                // Action
                var result = controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/BonusDetail/Main.cshtml"));
                Assert.That(result.Model, Is.EqualTo(metaModel));
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
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12)
                };

                var ticketResult = new TicketResult();
                BetListService.GetBetListData(
                    Arg.Any<BetListMetaModel>(), 
                    Arg.Any<IDictionary<int, string>>(),
                    "Lrf_BetList_LicenseeBonus_Detail")
                .Returns(ticketResult);

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
                    Arg.Is<ShowingOption>(x => x.CustomerCommVisible && !x.GrandTotalVisible),
                    Arg.Is<BetListMetaModel>(x => x.AgentListXml == AgentListXml),
                    ticketResult,
                    Arg.Is<IEnumerable<int>>(x => string.Join(",", x) == "1,2,8"),
                    ViewResources.TransactionDetail,
                    "BonusTransactionDetail");
            }
        }

        private BonusBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<BonusBetListController>(
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