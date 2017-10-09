namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ProductServiceTest
    {
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

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));

            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccess(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [Test]
        public void GetProductAccess_GetDataFromCache_ReturnCachedData()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB" },
                new Product { ProductID = 200, ProductCode = "ABC" },
            };

            const string XmlContent = "<Root><Group Name=\"Default\"><Function Name = \"_Default\" Product=\"SB,DB\"></Function></Group></Root>";

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));
            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);
            _productService.GetProductAccess(LicenseeId, Arg.Any<string>());

            // Action
            var result = _productService.GetProductAccess(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [Test]
        public void GetProductAccess_AllowedProductIsEmptyArray_ReturnOriginalProductList()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "123" },
                new Product { ProductID = 200, ProductCode = "ABC" },
            };

            const string XmlContent = "<Root></Root>";

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));
            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccess(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.True);
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

            const string XmlContent = "<Root><Group Name=\"Default\"><Function Name = \"ActiveCustomerSummary\" Product=\"SB,DB\"></Function></Group></Root>";

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(x => null);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));

            _productDataAccess.GetProductAccessList(LicenseeId, true).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccessForActiveCustomerSummary(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [Test]
        public void GetProductAccessForActiveCustomerSummary_GetDataFromCache_ReturnCachedData()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB", IsOfferedSingleWallet = true },
                new Product { ProductID = 200, ProductCode = "ABC", IsOfferedSingleWallet = true },
            };

            var expectedCacheResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB", IsOfferedSingleWallet = true }
            };

            const string XmlContent = "<Root><Group Name=\"Default\"><Function Name = \"ActiveCustomerSummary\" Product=\"SB,ABC\"></Function></Group></Root>";

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedCacheResult);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));
            _productDataAccess.GetProductAccessList(LicenseeId, true).Returns(expectedResult);
            _productService.GetProductAccessForActiveCustomerSummary(LicenseeId, Arg.Any<string>());

            // Action
            var result = _productService.GetProductAccessForActiveCustomerSummary(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.False);
        }

        [Test]
        public void GetProductAccessForActiveCustomerSummary_AllowedProductIsEmptyList_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB", IsOfferedSingleWallet = false },
                new Product { ProductID = 200, ProductCode = "ABC", IsOfferedSingleWallet = false },
            };

            const string XmlContent = "<Root></Root>";

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));
            _productDataAccess.GetProductAccessList(LicenseeId, true).Returns(expectedResult);

            // Action
            var result = _productService.GetProductAccessForActiveCustomerSummary(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.That(result.IsAllow(100), Is.True);
            Assert.That(result.IsAllow(200), Is.True);
        }

        [Test]
        public void GetSingleWalletProductAccess_WhenCall_ReturnAllowedProducts()
        {
            // Arrange
            var expectedResult = new List<Product>
            {
                new Product { ProductID = 100, ProductCode = "SB", IsOfferedSingleWallet = true },
                new Product { ProductID = 200, ProductCode = "BA" },
                new Product { ProductID = 300, ProductCode = "ABC" },
            };

            const string XmlContent = "<Root><Group Name=\"Default\"><Function Name = \"_Default\" Product=\"SB,BA\"></Function></Group></Root>";

            _cache.Get<IEnumerable<XElement>>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<IEnumerable<XElement>>())).DoNotCallBase();
            _cache.Get<IList<Product>>(Arg.Any<string>()).Returns(expectedResult);

            _xmlWrapper.Load(Arg.Any<string>()).Returns(XDocument.Parse(XmlContent));

            _productDataAccess.GetProductAccessList(LicenseeId).Returns(expectedResult);

            // Action
            var result = _productService.GetSingleWalletProductAccess(LicenseeId, Arg.Any<string>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsAllow(100), Is.True);
                Assert.That(result.IsAllow(200), Is.False);
                Assert.That(result.IsAllow(300), Is.False);
            });
        }
    }
}