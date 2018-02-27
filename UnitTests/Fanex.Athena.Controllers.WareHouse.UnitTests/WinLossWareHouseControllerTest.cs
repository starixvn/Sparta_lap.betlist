namespace Fanex.Athena.Controllers.WareHouse.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Warehouse.Interfaces;
    using Excel.Base.Entities;
    using Excel.Warehouse;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Warehouse.DTO;
    using Model.Warehouse.MetaModel;
    using Model.Warehouse.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Web.ResultExtension;

    public class WinLossWareHouseControllerTest : BaseControllerHelperTest
    {
        private IBalanceUpDataAccess _balancUpDataAccess;

        private IProductService _productService;

        private IWinLossDataAccess _winLossDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _balancUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _productService = Substitute.For<IProductService>();
            _winLossDataAccess = Substitute.For<IWinLossDataAccess>();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                var productAccess = new ProductAccess(new List<Product>());
                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language).Returns(productAccess);

                // Action
                var result = controller.Index() as ViewResult;
                var model = (WinLossViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/WareHouse/WinLoss/Main.cshtml"));
                    Assert.That(result.ViewBag.MaxDate, Is.EqualTo(balanceUp.MoveBetsDay));

                    Assert.That(model.ShowEmptyContent, Is.True);
                    Assert.That(model.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.Param.FromDate, Is.EqualTo(balanceUp.MoveBetsDay));
                    Assert.That(model.Param.ToDate, Is.EqualTo(balanceUp.MoveBetsDay));
                    Assert.That(model.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
                    Assert.That(model.ProductAccess, Is.EqualTo(productAccess));
                    Assert.That(model.IsAllowedCustomerCommission, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
                });
            }
        }

        [Test]
        public void Index_WhenPost_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 2), ToDate = new DateTime(2018, 1, 1) };

                const string AgentListXml = "<xml/>";
                SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns(AgentListXml);

                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                var productAccess = new ProductAccess(new List<Product>());
                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language).Returns(productAccess);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (WinLossViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/WareHouse/WinLoss/Main.cshtml"));
                    Assert.That(result.ViewBag.MaxDate, Is.EqualTo(balanceUp.MoveBetsDay));

                    Assert.That(model.ShowEmptyContent, Is.False);
                    Assert.That(model.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(model.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
                    Assert.That(model.ProductAccess, Is.EqualTo(productAccess));
                    Assert.That(model.IsAllowedCustomerCommission, Is.EqualTo(DefaultLicenseeSession.IsAllowedCustomerComm));
                });
            }
        }

        [Test]
        public void Index_WhenPost_InvalidModel_ReturnEmptyData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 2), ToDate = new DateTime(2018, 1, 1) };

                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (WinLossViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.MainResult, Is.Empty);

                    Assert.That(model.TotalValues["totalrows"], Is.EqualTo(0));
                    Assert.That(model.TotalValues["betcount"], Is.EqualTo(0));
                    Assert.That(model.TotalValues["turnover"], Is.EqualTo(0));
                    Assert.That(model.TotalValues["memberwinloss"], Is.EqualTo(0));
                    Assert.That(model.TotalValues["memberwinlosscomm"], Is.EqualTo(0));
                    Assert.That(model.TotalValues["licenseewinloss"], Is.EqualTo(0));
                    Assert.That(model.TotalValues["bacomm"], Is.EqualTo(0));
                });
            }
        }

        [Test]
        public void Index_WhenPost_ValidModel_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 1), ToDate = new DateTime(2018, 1, 2) };

                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                var data = new List<WinLoss>
                {
                    new WinLoss { ReportBy = (int)WarehouseReportBy.Day, GroupByValue = "2018/1/1", BetCount = 1 },
                    new WinLoss { ReportBy = (int)WarehouseReportBy.Month, GroupByValue = "a", Turnover = 2 },
                    new WinLoss { ReportBy = (int)WarehouseReportBy.Currency, GroupByValue = "b", MemberWinLoss = 3 },
                    new WinLoss { ReportBy = (int)WarehouseReportBy.League, GroupByValue = "-9", MemberWinLossCom = 4 },
                    new WinLoss { ReportBy = (int)WarehouseReportBy.League, GroupByValue = "-1", LicenseeWinloss = 5 },
                    new WinLoss { ReportBy = (int)WarehouseReportBy.None, BAComm = 6 }
                };
                _winLossDataAccess.GetDataList(metaModel).Returns(data);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (WinLossViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.MainResult.Count, Is.EqualTo(data.Count));
                    Assert.That(model.MainResult[0].GroupByString, Is.Empty);

                    Assert.That(model.TotalValues["totalrows"], Is.EqualTo(data.Count));
                    Assert.That(model.TotalValues["betcount"], Is.EqualTo(1));
                    Assert.That(model.TotalValues["turnover"], Is.EqualTo(2));
                    Assert.That(model.TotalValues["memberwinloss"], Is.EqualTo(3));
                    Assert.That(model.TotalValues["memberwinlosscomm"], Is.EqualTo(4));
                    Assert.That(model.TotalValues["licenseewinloss"], Is.EqualTo(5));
                    Assert.That(model.TotalValues["bacomm"], Is.EqualTo(6));
                });
            }
        }

        [TestCase("usd", "eur", "USD", 1, "date")]
        [TestCase("usd", "eur", "USD", 2, "month")]
        [TestCase("usd", "eur", "USD", 3, "currency")]
        [TestCase("", "eur", "eur", 4, "betType")]
        [TestCase(null, "eur", "eur", 5, "league")]
        public void Excel_WhenCall_PassParamsToCreateExcelCorrectly(
            string baseCurrency,
            string currency,
            string expectedCurrency,
            int reportBy,
            string expectedReportByName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new WinLossMetaModel
                {
                    CurrencyId = 1,
                    FromDate = new DateTime(2018, 1, 2),
                    ToDate = new DateTime(2018, 1, 1),
                    IsExportExcel = true,
                    BaseCurrency = baseCurrency,
                    CurrencyName = currency,
                    ReportBy = reportBy
                };

                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA }, new Product { ProductCode = Product.RB } }));

                var excel = Substitute.ForPartsOf<TopWinLossExcel>(string.Empty, null, new List<WinLoss>(), null, string.Empty);
                excel.When(x => x.Export()).DoNotCallBase();
                controller.CreateExcel(string.Empty, null, new List<WinLoss>(), null, string.Empty, 0).ReturnsForAnyArgs(excel);

                var expectedTitle = $"{ViewResources.ReportBy} {expectedReportByName} {ViewResources.From} 01/02/2018 --> 01/01/2018"
                    + $" - {string.Format(ViewResources.Viewin, expectedCurrency)}";

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received().CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<ShowingOption>(x => x.BAVisible && x.RBVisible && !x.NGVisible && !x.SBVisible && x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                        Arg.Any<IList<WinLoss>>(),
                        Arg.Any<IDictionary<string, decimal>>(),
                        expectedTitle,
                        reportBy);
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private WinLossWareHouseController CreateController()
        {
            return Substitute.ForPartsOf<WinLossWareHouseController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                _balancUpDataAccess,
                _productService,
                _winLossDataAccess);
        }
    }
}