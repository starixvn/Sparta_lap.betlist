namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public class MicrogamingWinlossReportControllerTest : BaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_MicroGamingIsNotAvaiable_ReturnAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index() as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        [TestCase(Product.MGCRNG)]
        [TestCase(Product.MGCLC)]
        public void Index_MicroGamingIsAvaiable_ReturnCorrectViewResult(string productCode)
        {
            // Arrange
            var product = new Product
            {
                ProductCode = productCode
            };
            using (var controller = CreateController(product))
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as MicroGamingWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/MicrogamingWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        private MicrogamingWinlossReportController CreateController(params Product[] products)
        {
            var productList = products == null ? new List<Product>() : products.ToList();
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(productList));
            return new MicrogamingWinlossReportController(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                productService);
        }
    }
}