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
    public class GoldDeluxeWinLossReportControllerTest : BaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_GoldDeluxeIsNotAvaiable_ReturnAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index() as RedirectResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
                });
            }
        }

        [Test]
        public void Index_GoldDeluxeIsAvaiable_ReturnCorrectViewResult()
        {
            // Arrange 
            var product = new Product
            {
                ProductCode = Product.GD,
                ProductID = 111,
                ProductName = "Golden Donkey"
            };
            using (var controller = CreateController(product))
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as ThirdPartyWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/GoldDeluxeWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param.ProductId, Is.EqualTo(product.ProductID));
                    Assert.That(viewModel.Param.ProductCode, Is.EqualTo(product.ProductCode));
                    Assert.That(viewModel.Param.ProductName, Is.EqualTo(product.ProductName));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        private GoldDeluxeWinLossReportController CreateController(params Product[] products)
        {
            var productList = products == null ? new List<Product>() : products.ToList();
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(productList));
            return new GoldDeluxeWinLossReportController(
                LangService,
                SessionService,
                AppSettings,
                productService);
        }
    }
}