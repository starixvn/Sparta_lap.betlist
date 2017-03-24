namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public partial class UserGuideControllerTest : BaseControllerTest
    {
        private IProductService _productService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _productService = Substitute.For<IProductService>();
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            // Arrange
            const string ResourceName = "CustomerList";
            using (var controller = CreateController())
            {
                controller.Url = this.Url;
                controller.GetFileContent(Arg.Any<string>()).Returns("{rooturl}content-here");

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/UserGuide/Index.cshtml"));
                Assert.That(result.Model, Is.EqualTo("/content-here"));
            }
        }

        [TestCase]
        public void Index_ContentHasCasinoProductList_ReturnCorrectContent()
        {
            // Arrange
            const string ResourceName = "LicenseeWL";
            _productService.GetProductList(Arg.Any<int>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Product>
            {
                new Product { ProductID = ProductTypes.AllBet, ProductName = "AllBet" }
            });

            using (var controller = CreateController())
            {
                controller.Url = this.Url;
                controller.GetFileContent(Arg.Any<string>()).Returns("{casino-productlist-section}{casino-productlist} content-here{/casino-productlist-section}");

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/UserGuide/Index.cshtml"));
                Assert.That(result.Model, Is.EqualTo("AllBet content-here"));
            }
        }

        [TestCase]
        public void Index_ProductListNotHaveCasinoProduct_ReturnCorrectContent()
        {
            // Arrange
            const string ResourceName = "LicenseeWL";
            _productService.GetProductList(Arg.Any<int>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Product>
            {
                new Product { ProductID = ProductTypes.Sportsbook, ProductName = "Sportsbook" }
            });

            using (var controller = CreateController())
            {
                controller.Url = this.Url;
                controller.GetFileContent(Arg.Any<string>()).Returns("{casino-productlist-section}{casino-productlist} content-here{/casino-productlist-section}ABC");

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/UserGuide/Index.cshtml"));
                Assert.That(result.Model, Is.EqualTo("ABC"));
            }
        }

        [Test]
        public void DownloadCustomerListPdf_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var result = controller.DownloadCustomerListPdf() as FilePathResult;

                // Assert
                Assert.That(result.FileName, Is.EqualTo("~/App_Data/UserGuide/CustomerList/pdf/en-Us/Odds Group Feature_Licensee.pdf"));
            }
        }

        private UserGuideController CreateController()
        {
            var controller = Substitute.ForPartsOf<UserGuideController>(
                LangService,
                SessionService,
                AppSettings,
                _productService);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}