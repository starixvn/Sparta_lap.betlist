namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;

    [TestFixture]
    public class SettledMatchCountReportControllerTest : BaseControllerHelperTest
    {
        private ISettledMatchCountService _settledMatchCountService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _settledMatchCountService = Substitute.For<ISettledMatchCountService>();
            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expectedData = new List<SettledMatchCountViewModel>
                {
                    new SettledMatchCountViewModel()
                };

                _settledMatchCountService.GetDataList(
                    Arg.Any<bool>(),
                    SessionService.LicenseeSession.LicenseeId,
                    Arg.Any<ProductAccess>()).Returns(expectedData);

                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as IList<SettledMatchCountViewModel>;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/SettledMatchCount/Main.cshtml"));
                    Assert.That(viewModel, Is.EqualTo(expectedData));
                });
            }
        }

        private SettledMatchCountReportController CreateController()
        {
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(new List<Product>()));
            return new SettledMatchCountReportController(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _settledMatchCountService);
        }
    }
}