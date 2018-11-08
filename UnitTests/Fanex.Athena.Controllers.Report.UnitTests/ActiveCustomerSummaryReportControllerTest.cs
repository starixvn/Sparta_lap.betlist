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

    [TestFixture]
    public class ActiveCustomerSummaryReportControllerTest : BaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
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
                var viewModel = viewResult.Model as ActiveCustomerViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    StringAssert.Contains("Report/ActiveCustomerSummary/Main.cshtml", viewResult.ViewName);
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                });
            }
        }

        private ActiveCustomerSummaryReportController CreateController()
        {
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccessForActiveCustomerSummary(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language)
                .Returns(new ProductAccess(new List<Product>()));
            return new ActiveCustomerSummaryReportController(
                LangService,
                SessionService,
                AppSettings,
                productService);
        }
    }
}