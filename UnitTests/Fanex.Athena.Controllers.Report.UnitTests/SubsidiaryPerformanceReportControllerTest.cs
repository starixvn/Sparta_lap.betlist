namespace Fanex.Athena.Controllers.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Fanex.Athena.Controllers.Base.UnitTests;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Enum;

    [TestFixture]
    public class SubsidiaryPerformanceReportControllerTest : BaseControllerHelperTest
    {
        private const string FakeProductCode = "TestingCode";
        private const string FakeProductName = "Testing Product Name";

        private static readonly ReadOnlyCollection<string> BaseCurrencyList = new ReadOnlyCollection<string>(
            new List<string> { "EUR", "USD", "GBP", "MYR", "CNY", "THB", "IDR" });

        private ISubsidiaryService _subsidiaryService;
        private ISubsidiaryPerformanceService _service;
        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
            SessionService.UserSession.ViewType = ClientViewType.Desktop;

            _subsidiaryService = Substitute.For<ISubsidiaryService>();
            _service = Substitute.For<ISubsidiaryPerformanceService>();
            _serializer = new JavaScriptSerializer();
        }

        [Test]
        public void Index_SubsidiaryListHasData_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var operatorList = new List<string> { "New York", "India", "Irag", "China" };
                _subsidiaryService.GetSubsidiaryNames().Returns(operatorList);

                var expectedViewModelData = new SubsidiaryPerformanceViewModel
                {
                    ProductOptionList = new List<ProductOption>
                    {
                        new ProductOption { Code = FakeProductCode, Name = FakeProductName, Status = true }
                    },
                    SubsidiaryList = operatorList.Select(x => new CheckedItem { Name = x, Checked = true })
                };

                var expectedModel = _serializer.Serialize(expectedViewModelData);
                var expectedViewName = "~/Views/Report/SubsidiaryPerformance/Main.cshtml";
                HttpContext.Current = GetHttpContext();

                // Action
                var viewResult = controller.Index() as ViewResult;
                var modelResult = _serializer.Serialize(viewResult.Model);

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo(expectedViewName));
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
                ClientViewHelper,
                productService,
                _subsidiaryService);
        }
    }
}