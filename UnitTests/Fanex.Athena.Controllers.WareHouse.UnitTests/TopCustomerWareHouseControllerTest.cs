namespace Fanex.Athena.Controllers.WareHouse.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Common.Interfaces;
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

    public class TopCustomerWareHouseControllerTest : BaseControllerHelperTest
    {
        private IBalanceUpDataAccess _balancUpDataAccess;

        private IProductService _productService;

        private ISportTypeDataAccess _sportTypeDataAccess;

        private ITopCustomerDataAccess _topCustomerDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _balancUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _productService = Substitute.For<IProductService>();
            _sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();
            _topCustomerDataAccess = Substitute.For<ITopCustomerDataAccess>();

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

                var sportList = new List<Sport>();
                _sportTypeDataAccess.GetAllWHSports().Returns(sportList);

                // Action
                var result = controller.Index() as ViewResult;
                var model = (TopCustomerViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/WareHouse/TopCustomer/Main.cshtml"));
                    Assert.That(result.ViewBag.MaxDate, Is.EqualTo(balanceUp.MoveBetsDay));

                    Assert.That(model.ShowEmptyContent, Is.True);
                    Assert.That(model.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.Param.FromDate, Is.EqualTo(balanceUp.MoveBetsDay));
                    Assert.That(model.Param.ToDate, Is.EqualTo(balanceUp.MoveBetsDay));
                    Assert.That(model.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
                    Assert.That(model.ProductAccess, Is.EqualTo(productAccess));
                    Assert.That(model.SportList, Is.EqualTo(sportList));
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
                var metaModel = new TopCustomerMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 2), ToDate = new DateTime(2018, 1, 1) };

                const string AgentListXml = "<xml/>";
                SessionService.GenerateAgentListXml(metaModel.CurrencyId).Returns(AgentListXml);

                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1), BalanceUpDay = new DateTime(2018, 1, 2) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                var productAccess = new ProductAccess(new List<Product>());
                _productService.GetProductAccess(metaModel.LicenseeId, DefaultUserSession.Language).Returns(productAccess);

                var sportList = new List<Sport>();
                _sportTypeDataAccess.GetAllWHSports().Returns(sportList);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (TopCustomerViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/WareHouse/TopCustomer/Main.cshtml"));
                    Assert.That(result.ViewBag.BalanceUpDay, Is.EqualTo(balanceUp.BalanceUpDay.ToShortDateString()));
                    Assert.That(result.ViewBag.MaxDate, Is.EqualTo(balanceUp.MoveBetsDay));

                    Assert.That(model.ShowEmptyContent, Is.False);
                    Assert.That(model.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(model.CurrencyList, Is.EqualTo(DefaultUserSession.CurrencyList));
                    Assert.That(model.ProductAccess, Is.EqualTo(productAccess));
                    Assert.That(model.SportList, Is.EqualTo(sportList));
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
                var metaModel = new TopCustomerMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 2), ToDate = new DateTime(2018, 1, 1) };

                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                _sportTypeDataAccess.GetAllWHSports().Returns(new List<Sport>());

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (TopCustomerViewModel)result.Model;

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
                var metaModel = new TopCustomerMetaModel { CurrencyId = 1, FromDate = new DateTime(2018, 1, 1), ToDate = new DateTime(2018, 1, 2) };

                var balanceUp = new BalanceUp { MoveBetsDay = new DateTime(2018, 1, 1) };
                _balancUpDataAccess.GetBalanceUp().Returns(balanceUp);

                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                var data = new List<TopCustomer>
                {
                    new TopCustomer { BetCount = 1 },
                    new TopCustomer { Turnover = 2 },
                    new TopCustomer { MemberWinLoss = 3 },
                    new TopCustomer { MemberWinLossCom = 4 },
                    new TopCustomer { BAComm = 6 }
                };
                _topCustomerDataAccess.GetDataList(metaModel).Returns(data);

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var model = (TopCustomerViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.MainResult, Is.EqualTo(data));

                    Assert.That(model.TotalValues["totalrows"], Is.EqualTo(data.Count));
                    Assert.That(model.TotalValues["betcount"], Is.EqualTo(1));
                    Assert.That(model.TotalValues["turnover"], Is.EqualTo(2));
                    Assert.That(model.TotalValues["memberwinloss"], Is.EqualTo(3));
                    Assert.That(model.TotalValues["memberwinlosscomm"], Is.EqualTo(4));
                    Assert.That(model.TotalValues["licenseewinloss"], Is.EqualTo(-3));
                    Assert.That(model.TotalValues["bacomm"], Is.EqualTo(6));
                });
            }
        }

        [TestCase("usd", "eur", "USD", "12/12/2018", "11/11/2018")]
        [TestCase("usd", "eur", "USD", "11/11/2018", "11/11/2018")]
        [TestCase("", "eur", "eur", "11/11/2018", "12/12/2018")]
        [TestCase(null, "eur", "eur", "11/11/2018", "12/12/2018")]
        public void Excel_WhenCall_PassParamsToCreateExcelCorrectly(
            string baseCurrency,
            string currency,
            string expectedCurrency,
            string fromDate,
            string toDate)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new TopCustomerMetaModel
                {
                    CurrencyId = 1,
                    FromDate = Convert.ToDateTime(fromDate),
                    ToDate = Convert.ToDateTime(toDate),
                    IsExportExcel = true,
                    BaseCurrency = baseCurrency,
                    CurrencyName = currency
                };

                _productService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.BA }, new Product { ProductCode = Product.SB } }));

                var excel = Substitute.ForPartsOf<TopCustomerExcel>(string.Empty, null, new DataTable(), string.Empty);
                excel.When(x => x.Export()).DoNotCallBase();
                controller.CreateExcel(string.Empty, null, new DataTable(), string.Empty).ReturnsForAnyArgs(excel);

                var expectedTitle = $"{ViewResources.Top} {metaModel.TopRow} {ViewResources.Customer} {ViewResources.From} {fromDate} --> {toDate}"
                    + $" - {string.Format(ViewResources.Viewin, expectedCurrency)}";

                var excelData = new DataTable();
                excelData.Rows.Add(excelData.NewRow());
                _topCustomerDataAccess.GetDataForExcel(metaModel).Returns(excelData);
                var expectedDataRows = metaModel.IsValid ? excelData.Rows.Count : 0;

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received().CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<ShowingOption>(x => x.BAVisible && !x.RBVisible && !x.NGVisible && x.SBVisible && x.CustomerCommVisible == DefaultLicenseeSession.IsAllowedCustomerComm),
                        Arg.Is<DataTable>(x => x.Rows.Count == expectedDataRows),
                        expectedTitle);
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private TopCustomerWareHouseController CreateController()
        {
            return Substitute.ForPartsOf<TopCustomerWareHouseController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                _balancUpDataAccess,
                _productService,
                _sportTypeDataAccess,
                _topCustomerDataAccess);
        }
    }
}