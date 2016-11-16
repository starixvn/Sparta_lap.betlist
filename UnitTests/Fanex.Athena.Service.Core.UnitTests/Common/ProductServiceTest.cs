namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductServiceTest
    {
        private IAppSettings _appSettings;
        private IProductDataAccess _productDataAccess;
        private IXDocumentWrapper _xmlWrapper;
        private IProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _productDataAccess = Substitute.For<IProductDataAccess>();
            _xmlWrapper = Substitute.For<IXDocumentWrapper>();

            _productService = new ProductService(_appSettings, _productDataAccess, _xmlWrapper);

            _appSettings.GetValue("FunctionAllowedProductFile").Returns(@"xml_file_path");
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _productDataAccess = null;
            _xmlWrapper = null;
            _productService = null;
            CacheService.Clear();
        }

        [Test]
        public void GetProductList_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Product>()
            {
                new Product() { ProductID = 1 }
            };
            var licenseeId = 1;
            _productDataAccess.GetProductAccessList(licenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetProductList(licenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result[0].ProductID, Is.EqualTo(expectedResult[0].ProductID));
        }

        [Test]
        public void GetProductAccess_WhenCall_ReturnAllowedProducts()
        {
            // Arrange
            var expectedResult = new List<Product>()
            {
                new Product() { ProductID = 100, ProductCode = "SB" },
                new Product() { ProductID = 200, ProductCode = "ABC" },
            };
            var xmlContent = "<Root><Group Name=\"Default\"><Function Name = \"_Default\" Product=\"SB,DB\"></Function></Group></Root>";

            var licenseeId = 1;
            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(xmlContent));

            _productDataAccess.GetProductAccessList(licenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccess(licenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [Test]
        public void GetProductAccessForActiveCustomerSummary_WhenCall_ReturnAllowedProducts()
        {
            // Arrange
            var expectedResult = new List<Product>()
            {
                new Product { ProductID = 100, ProductCode = "SB" },
                new Product { ProductID = 200, ProductCode = "ABC" },
            };
            var licenseeId = 1;
            var xmlContent = "<Root><Group Name=\"Default\"><Function Name = \"ActiveCustomerSummary\" Product=\"SB,DB\"></Function></Group></Root>";

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(xmlContent));

            _productDataAccess.GetProductAccessList(licenseeId, true).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccessForActiveCustomerSummary(licenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }
    }
}