namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
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
        public async Task Detail_NotExportExcel_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListExcelMetaModel();

                // Action
                var result = await controller.Detail(metaModel) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/BonusDetail/Main.cshtml"));
                Assert.That(result.Model, Is.EqualTo(metaModel));
            }
        }

        [Test]
        public async Task Detail_ExportExcel_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string AgentListXml = "<xml/>";
                var metaModel = new BetListExcelMetaModel
                {
                    IsExportExcel = true,
                    FromDate = new DateTime(2018, 11, 11),
                    ToDate = new DateTime(2018, 12, 12)
                };

                SessionService.GenerateAgentListXml(metaModel.CurrencyId, false).Returns(AgentListXml);
                controller.When(async x => await x.ProcessExportExcel(
                    Arg.Any<BetListExcelMetaModel>(),
                    Arg.Any<string>(),
                    Arg.Any<string>())).DoNotCallBase();

                // Action
                await controller.Detail(metaModel);

                // Assert
                await controller.Received().ProcessExportExcel(
                    Arg.Is<BetListExcelMetaModel>(x => x.ShowingOption.CustomerCommVisible == metaModel.HasPlayerCom
                        && !x.ShowingOption.GrandTotalVisible
                        && x.Title == ViewResources.TransactionDetail
                        && string.Join(",", x.HiddenColumns) == "1,2,8"
                        && x.AgentListXml == AgentListXml),
                    "BonusTransactionDetail",
                    null);
            }
        }

        private BonusBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<BonusBetListController>(
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