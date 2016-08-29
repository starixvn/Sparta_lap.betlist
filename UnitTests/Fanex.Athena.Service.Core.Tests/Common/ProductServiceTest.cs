namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductServiceTest
    {
        private IAppSettings _appSettings;
        private IProductDataAccess _productDataAccess;
        private IServerPathMapper _serverPathMapper;
        private IProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _productDataAccess = Substitute.For<IProductDataAccess>();
            _serverPathMapper = Substitute.For<IServerPathMapper>();

            _productService = new ProductService(_appSettings, _productDataAccess, _serverPathMapper);
            CacheService.Delete();
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
            var licenseeId = 1;
            _appSettings.GetValue("FunctionAllowedProductFile").Returns(@"AppData\FunctionAllowedProduct.xml");
            _serverPathMapper.MapPath(Arg.Any<string>())
                .Returns(x => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (string)x[0]));

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
                new Product() { ProductID = 100, ProductCode = "SB" },
                new Product() { ProductID = 200, ProductCode = "ABC" },
            };
            var licenseeId = 1;
            _appSettings.GetValue("FunctionAllowedProductFile").Returns(@"AppData\FunctionAllowedProduct.xml");
            _serverPathMapper.MapPath(Arg.Any<string>())
                .Returns(x => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (string)x[0]));

            _productDataAccess.GetProductAccessList(licenseeId, true).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccessForActiveCustomerSummary(licenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }
    }
}