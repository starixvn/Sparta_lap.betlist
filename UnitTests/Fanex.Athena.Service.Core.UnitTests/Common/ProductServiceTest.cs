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
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 1 }
            };

            const int LicenseeId = 1;
            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetProductList(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result[0].ProductID, Is.EqualTo(expectedResult[0].ProductID));
        }

        [Test]
        public void GetProductAccess_WhenCall_ReturnAllowedProducts()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB" },
                new Product { ProductID = 200, ProductCode = "ABC" },
            };

            const string XmlContent = "<Root><Group Name=\"Default\"><Function Name = \"_Default\" Product=\"SB,DB\"></Function></Group></Root>";

            const int LicenseeId = 1;
            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));

            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccess(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [Test]
        public void GetProductAccessForActiveCustomerSummary_WhenCall_ReturnAllowedProducts()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB", IsOfferedSingleWallet = true },
                new Product { ProductID = 200, ProductCode = "ABC", IsOfferedSingleWallet = true },
            };

            const int LicenseeId = 1;
            const string XmlContent = "<Root><Group Name=\"Default\"><Function Name = \"ActiveCustomerSummary\" Product=\"SB,DB\"></Function></Group></Root>";

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));

            _productDataAccess.GetProductAccessList(LicenseeId, true).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccessForActiveCustomerSummary(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public void IsAllowedSaToteCurrency_WhenCall_ReturnCorrectValue(int custId, bool expected)
        {
            // Arrange
            _productDataAccess.IsAllowedSaToteCurrency(custId).Returns(expected);

            // Action
            var result = _productService.IsAllowedSaToteCurrency(custId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}