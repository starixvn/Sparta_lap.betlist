namespace Fanex.Athena.Controllers.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DataAccess.Core.Users.Interfaces;
    using Excel.Customer;
    using Fanex.Athena.Controllers.Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.ResultModel;
    using Model.Customer.DTO;
    using Model.Customer.MetaModel;
    using Model.Customer.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Customer.Interfaces;
    using Service.Report.Interfaces;
    using Session.Model;

    public partial class CustomerControllerTest : CultureBaseControllerHelperTest
    {
        private ICustomerListService _customerListService;
        private IGroupComService _groupComService;
        private ILicenseeDataAccess _licenseeDataAccess;
        private IProductService _productService;
        private ISubsidiaryService _subsidiaryService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            SetUpDefaultFakeData();
            _customerListService = Substitute.For<ICustomerListService>();
            _groupComService = Substitute.For<IGroupComService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
            _productService = Substitute.For<IProductService>();
            _subsidiaryService = Substitute.For<ISubsidiaryService>();

            AppSettings.GetValue(Arg.Any<string>()).Returns(string.Empty);
            SessionService.IsAllowedEditCommission(Arg.Any<UserSession>()).Returns(false);
            SessionService.IsAllowedSpecialBA(Arg.Any<UserSession>()).Returns(true);

            _groupComService.GetAll(Arg.Any<int>()).Returns(new List<GroupCommMetaModel>());
            _groupComService.GetById(Arg.Any<int>()).Returns(new GroupCommMetaModel());
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();
            _customerListService = null;
            _groupComService = null;
            _licenseeDataAccess = null;
            _productService = null;
        }

        [TearDown]
        public void TearDown()
        {
            _groupComService.ClearReceivedCalls();
            _customerListService.ClearReceivedCalls();
            _licenseeDataAccess.ClearReceivedCalls();
        }

        [Test]
        public void LogInfo_WhenCall_CallServiceToGetData()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const int CustId = 1;

                StubLicenseeSessionData(DefaultLicenseeSession);
                _customerListService.GetCustomerLoginInfo(CustId, DefaultLicenseeSession.LicenseeId, DefaultLicenseeSession.LicenseeName).Returns(new LogInfo());

                // Action
                var result = controller.LogInfo(CustId);

                // Assert
                _customerListService.Received().GetCustomerLoginInfo(CustId, DefaultLicenseeSession.LicenseeId, DefaultLicenseeSession.LicenseeName);
            }
        }

        [Test]
        public void LogInfo_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const int CustId = 2;
                const string ExpectedViewName = "Customer/CustomerDialog/LogInfo.cshtml";

                StubLicenseeSessionData(DefaultLicenseeSession);

                var productAccess = new ProductAccess(new List<Product> { new Product { ProductCode = Product.SB } });
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(productAccess);

                var logInfo = new LogInfo { CustId = CustId };
                _customerListService.GetCustomerLoginInfo(CustId, DefaultLicenseeSession.LicenseeId, DefaultLicenseeSession.LicenseeName).Returns(logInfo);

                // Action
                var result = controller.LogInfo(CustId) as ViewResult;
                var model = (LogInfoViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    StringAssert.Contains(ExpectedViewName, result.ViewName);
                    Assert.That(model.LogInfo, Is.EqualTo(logInfo));
                    Assert.That(model.ProductAccess, Is.EqualTo(productAccess));
                });
            }
        }

        [Test]
        public void ExportExcel_ParameterIsNull_ReturnNull()
        {
            using (var controller = CreateContoller())
            {
                // Action
                var result = controller.ExportExcel(null);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public void ExportExcel_GridColumnIsEmpty_ReturnEmptyFile()
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string ExcelFileName = "Empty.xls";
                const string ExcelParam = "{'GridColumns': []}";

                // Action
                var result = controller.ExportExcel(ExcelParam) as FileContentResult;

                // Assert
                Assert.That(result.FileDownloadName, Is.EqualTo(ExcelFileName));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ExportExcel_IsConvertIsFalse_ExportWithCorrectReportTitle(bool isConvert)
        {
            using (var controller = CreateContoller())
            {
                // Arrange
                const string CurrencyName = "Currency";
                const string BaseCurrency = "BaseCurrency";
                const string Path = "";
                var excelParam = "{'CurrencyName': 'Currency', " + (isConvert ? "'BaseCurrency': 'BaseCurrency' ," : string.Empty)
                    + "'GridColumns': [{'Name': 'Column1', 'Checked': 'true'}]}";

                controller.Server.MapPath(Arg.Any<string>()).Returns(Path);
                StubLicenseeSessionData(DefaultLicenseeSession);
                _productService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>()).Returns(new ProductAccess(new List<Product>()));
                SessionService.IsAllowedEditCommission(DefaultUserSession).Returns(false);
                SessionService.IsAllowedSpecialBA(DefaultUserSession).Returns(false);
                _customerListService.GetActiveCustomers(Arg.Any<CustomerListMetaModel>()).Returns(new ReportResultModel<CustomerDTO>
                {
                    DataList = new List<CustomerDTO>(),
                    TotalRows = 0
                });

                var actualReportTitle = string.Empty;
                var excel = Substitute.ForPartsOf<CustomerListExcel>(
                   Arg.Any<string>(),
                   Arg.Any<string>(),
                   new CustomerListViewModel { DataList = new List<CustomerDTO>(), Param = new CustomerListMetaModel { GridColumns = new List<CheckedItem>() } });
                excel.When(x => x.Export()).DoNotCallBase();

                var reportTitle = Resources.ViewResources.CustomerList + " - "
                        + string.Format(Resources.ViewResources.Viewin, isConvert ? BaseCurrency.ToUpperInvariant() : CurrencyName);
                var path = string.Empty;
                controller.CreateExcel(
                   path,
                   reportTitle,
                   new CustomerListViewModel { DataList = new List<CustomerDTO>(), Param = new CustomerListMetaModel { GridColumns = new List<CheckedItem>() } })
                   .ReturnsForAnyArgs(excel);
                controller.When(x => x.CreateExcel(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CustomerListViewModel>())).Do(x => actualReportTitle = (string)x[1]);

                // Action
                var result = controller.ExportExcel(excelParam);

                // Assert
                Assert.Multiple(() =>
                {
                    excel.Received().Export();
                    Assert.That(actualReportTitle, Is.EqualTo(reportTitle));
                });
            }
        }

        private CustomerController CreateContoller()
        {
            var controller = Substitute.ForPartsOf<CustomerController>(
                LangService,
                SessionService,
                AppSettings,
                _customerListService,
                _groupComService,
                _licenseeDataAccess,
                _productService,
                _subsidiaryService);
            controller.ControllerContext = ControllerContext;

            return controller;
        }
    }
}