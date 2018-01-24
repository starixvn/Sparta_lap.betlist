namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Common.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DTO.Common;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class CustomerTurnoverReportControllerTest : BaseControllerHelperTest
    {
        private ISportTypeDataAccess _sportTypeDataAccess;

        private ICustomerTurnoverService _customerTurnoverService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();
            _customerTurnoverService = Substitute.For<ICustomerTurnoverService>();

            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as CustomerTurnoverViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/CustomerTurnover/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        [Test]
        public void GetSportsAndProducts_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expectedData = new[]
                {
                    new SportProductItem { ID = 1 },
                    new SportProductItem { ID = 2 }
                };
                _sportTypeDataAccess.GetSportsAndProducts(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language).Returns(expectedData);

                // Action
                var result = controller.GetSportsAndProducts() as JsonResult;

                // Assert
                Assert.That(result.Data, Is.EqualTo(expectedData));
            }
        }

        [Test]
        public void Excel_EmptyExcelParam_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                MockCreatingExcel(controller);

                // Action
                var result = controller.Excel(string.Empty);

                // Assert
                Assert.That(result, Is.Null);
            }
        }

        [Test]
        public void Excel_ModelIsNotValid_ReturnCorrectRedirectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var excelParam = "{\"FromDate\": \"2017/2/2\", \"ToDate\": \"2017/1/1\"}";
                MockCreatingExcel(controller);

                // Action
                var result = controller.Excel(excelParam) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/Error?errCode=400"));
            }
        }

        [TestCase(1, "MM/dd/yyyy")]
        [TestCase(2, "MM/yyyy")]
        [TestCase(0, "MM/yyyy")]
        public void Excel_ModelIsValid_ReturnCorrectRedictResult(int reportBy, string expectedFormatDate)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var excelParam = @"{""fromdate"":""11/01/2017"",""todate"":""11/30/2017"",""reportby"":" + reportBy + @",""showwin"":true,
                                ""SportProducts"":[{""Checked"":true}],""bettypes"":[{""Checked"":true}]}";
                MockCreatingExcel(controller);
                var resultModel = new ReportResultModel<CustTurnover>();
                _customerTurnoverService.GetDataList(
                    Arg.Any<CustomerTurnoverMetaModel>(),
                    SessionService.UserSession.Language).Returns(resultModel);

                // Action
                var result = controller.Excel(excelParam) as ExcelResult;

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        expectedFormatDate,
                        Arg.Any<ShowingOption>(),
                        resultModel,
                        "Customer Turnover, 11/1/2017 --> 11/30/2017 - Currency:  - Minimum odds: 0 ");
                    Assert.That(result, Is.Not.Null);
                });
            }
        }

        private static void MockCreatingExcel(CustomerTurnoverReportController controller)
        {
            var excel = Substitute.ForPartsOf<CustomerTurnoverExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, string.Empty, null, null, string.Empty).ReturnsForAnyArgs(excel);
        }

        private CustomerTurnoverReportController CreateController()
        {
            var controller = Substitute.ForPartsOf<CustomerTurnoverReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                Substitute.For<IProductService>(),
                _sportTypeDataAccess,
                _customerTurnoverService);
            controller.When(x => x.CreateExcel(string.Empty, string.Empty, null, null, string.Empty)).DoNotCallBase();
            return controller;
        }
    }
}