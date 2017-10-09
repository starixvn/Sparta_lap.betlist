namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ProductServiceTest
    {
        private const int LicenseeId = 1;

        private IAppSettings _appSettings;
        private IProductDataAccess _productDataAccess;
        private IXDocumentWrapper _xmlWrapper;
        private IProductService _productService;
        private IMemoryCacheService _cache;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _productDataAccess = Substitute.For<IProductDataAccess>();
            _xmlWrapper = Substitute.For<IXDocumentWrapper>();
            _cache = Substitute.For<IMemoryCacheService>();

            _productService = new ProductService(_appSettings, _productDataAccess, _xmlWrapper, _cache);

            _appSettings.GetValue("FunctionAllowedProductFile").Returns(@"xml_file_path");
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _productDataAccess = null;
            _xmlWrapper = null;
            _productService = null;
        }

        [Test]
        public void GetProductList_CacheNotContainsData_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 1 }
            };

            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns((IList<Product>)null);

            // Action
            var result = _productService.GetProductList(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetProductList_GetDataFromCache_ReturnCachedData()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 1 }
            };

            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);
            _productService.GetProductList(LicenseeId, Arg.Any<string>());
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _productService.GetProductList(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetProductList_GetEmptyListFromDataAccess_ReturnNull()
        {
            // Arrange
            var expectedResult = new List<Product>();
            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);
            _productService.GetProductList(LicenseeId, Arg.Any<string>());
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _productService.GetProductList(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}