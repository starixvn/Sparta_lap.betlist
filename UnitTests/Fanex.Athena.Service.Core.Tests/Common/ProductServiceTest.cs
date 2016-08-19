namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System.Collections.Generic;
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
        private IProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _productDataAccess = Substitute.For<IProductDataAccess>();
            _productService = new ProductService(_appSettings, _productDataAccess);
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
    }
}