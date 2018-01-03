namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
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
    public class TurnoverReportControllerTest : BaseControllerHelperTest
    {
        private const int CurrencyId = 999;

        private const string AgentListXml = "<abc>";

        private ITurnoverService _turnoverService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _turnoverService = Substitute.For<ITurnoverService>();

            base.FixtureSetUp();

            SetUpDefaultFakeData();

            SessionService.GenerateAgentListXml(CurrencyId).Returns(AgentListXml);
        }

        [TearDown]
        public void TearDown()
        {
            _turnoverService.ClearReceivedCalls();
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController("SB,BA"))
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as TurnoverViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/Turnover/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.UserId, Is.EqualTo(SessionService.UserSession.UserId));
                    Assert.That(viewModel.Param.ProductOptions.Any(x => x.Code == Product.BA), Is.False);
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        [TestCase(true, " External ID ", null)]
        [TestCase(true, "abcd", "abcd")]
        [TestCase(false, "  External ID or System ID ", null)]
        [TestCase(false, "External ID", "External ID")]
        public void Excel_WhenCall_AssignCorrectUsername(bool isFakeLicensee, string inputUserName, string expectedUserName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                MockCreatingExcel(controller);
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                AppSettings.FakeLicenseeList.Returns(new[] { isFakeLicensee ? licenseeId : -1 });
                var metaModel = new TurnoverMetaModel
                {
                    Username = inputUserName
                };

                // Action
                controller.Excel(metaModel);

                // Assert
                Assert.That(metaModel.Username, Is.EqualTo(expectedUserName));
            }
        }

        [Test]
        public void Excel_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController("BA"))
            {
                // Arrange
                const string CurrencyName = "SSU";
                MockCreatingExcel(controller);
                var metaModel = new TurnoverMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = CurrencyName,
                    CurrencyId = CurrencyId
                };
                var expectedData = new List<TurnoverDTO>
                    {
                        new TurnoverDTO()
                    };
                var expectedTotalData = new TurnoverDTO();
                _turnoverService.GetDataList(metaModel).Returns(expectedData);
                _turnoverService.SumTotalData(expectedData).Returns(expectedTotalData);

                // Action
                var result = controller.Excel(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<TurnoverViewModel>(x =>
                            x.DataList == expectedData &&
                            x.TotalData == expectedTotalData),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == CurrencyName),
                        Arg.Is<ShowingOption>(x => x.BAVisible),
                        SessionService.LicenseeSession.LicenseeId);
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private static void MockCreatingExcel(TurnoverReportController controller)
        {
            var excel = Substitute.ForPartsOf<TurnoverExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, null, null, null, 0).ReturnsForAnyArgs(excel);
        }

        private TurnoverReportController CreateController(string productCodeList = null)
        {
            var productList = string.IsNullOrEmpty(productCodeList) ?
                new List<Product>() :
                productCodeList.Split(',').Select(x => new Product
                {
                    ProductCode = x
                });
            var productService = Substitute.For<IProductService>();
            productService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.Turnover).Returns(new ProductAccess(productList.ToList()));
            var controller = Substitute.ForPartsOf<TurnoverReportController>(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _turnoverService);
            controller.WhenForAnyArgs(x => x.CreateExcel(string.Empty, null, null, null, 0)).DoNotCallBase();
            return controller;
        }
    }
}