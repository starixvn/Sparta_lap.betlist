using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Fanex.Athena.Controllers.Base.UnitTests;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Service.Core.Common.Interfaces;
using Fanex.Athena.Service.Report.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Controllers.Report.UnitTests
{
    [TestFixture]
    public class SubsidiaryPerformanceReportControllerTest : BaseControllerHelperTest
    {
        private const string FakeProductCode = "TestingCode";
        private const string FakeProductName = "Testing Product Name";

        private ISubsidiaryPerformanceService _service;
        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
            SessionService.UserSession.ViewType = ClientViewType.Desktop;

            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _serializer = new JavaScriptSerializer();
        }

        [Test]
        public void Index_SubsidiaryListHasData_ReturnCorrectViewResult()
        {
            var expectedOperatorList = new List<Operator>
            {
                new Operator { AgentId = 1, OperatorName = "New York" },
                new Operator { AgentId = 2, OperatorName = "New York" },
                new Operator { AgentId = 3, OperatorName = "India" }
            };
            var expectedViewModelData = new SubsidiaryPerformanceViewModel
            {
                ProductOptionList = GetExpectedProductOptionList(),
                SubsidiaryList = GetExpectedSubsidiaryList()
            };

            TestIndexViewModel(expectedOperatorList, expectedViewModelData);
        }

        [Test]
        public void Index_SubsidiaryListHasNoData_ReturnCorrectViewResult()
        {
            var expectedOperatorList = new List<Operator>();
            var expectedViewModelData = new SubsidiaryPerformanceViewModel
            {
                ProductOptionList = GetExpectedProductOptionList(),
                SubsidiaryList = new List<CheckedItem>()
            };

            TestIndexViewModel(expectedOperatorList, expectedViewModelData);
        }

        private static IEnumerable<ProductOption> GetExpectedProductOptionList()
        {
            return new List<ProductOption>
            {
                new ProductOption { Code = FakeProductCode, Name = FakeProductName, Status = true }
            };
        }

        private static IEnumerable<CheckedItem> GetExpectedSubsidiaryList()
        {
            return new List<CheckedItem>
            {
                new CheckedItem { Name = "New York", Checked = true },
                new CheckedItem { Name = "India", Checked = true }
            };
        }

        private void TestIndexViewModel(IEnumerable<Operator> expectedOperatorList, SubsidiaryPerformanceViewModel viewModelData)
        {
            using (var controller = CreateController())
            {
                // Arrange
                _service.GetOperatorList(SessionService.LicenseeSession.LicenseeName).Returns(expectedOperatorList);

                var expectedModel = _serializer.Serialize(viewModelData);
                HttpContext.Current = GetHttpContext();

                // Action
                var viewResult = controller.Index() as ViewResult;
                var modelResult = _serializer.Serialize(viewResult.Model);

                // Assert
                Assert.That(modelResult, Is.EqualTo(expectedModel));
            }
        }

        private static HttpContext GetHttpContext()
        {
            return new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }

        private SubsidiaryPerformanceReportController CreateController()
        {
            var productService = Substitute.For<IProductService>();
            productService.GetSingleWalletProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.SubsidiaryPerformance)
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = FakeProductCode, ProductName = FakeProductName } }));

            return new SubsidiaryPerformanceReportController(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _service);
        }
    }
}