namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Extension.Utility.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public partial class UserGuideControllerTest : BaseControllerHelperTest
    {
        private const string DefaultLanguage = "en-US";

        private IProductService _productService;

        private IFileSystem _fileSystem;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [SetUp]
        public void SetUp()
        {
            _productService = Substitute.For<IProductService>();

            _fileSystem = Substitute.For<IFileSystem>();
        }

        [TearDown]
        public void TearDown()
        {
            _productService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "CustomerList";

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(true);
                _fileSystem.ReadFile(Arg.Any<string>()).Returns("{rooturl}");

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/UserGuide/Index.cshtml"));
                });
            }
        }

        [Test]
        public void Index_FileDoesNotExist_ReturnDefaultFilePath()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "CustomerList";

                var expectedFilePath = $"~/App_Data/UserGuide/{ResourceName}/en-US.htm";
                var actualFilePath = string.Empty;

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Do<string>(x => actualFilePath = x)).Returns("{rooturl}");

                // Action
                controller.Index(ResourceName);

                // Assert
                Assert.That(actualFilePath, Is.EqualTo(expectedFilePath));
            }
        }

        [Test]
        public void Index_FileExist_ReturnFilePathBaseOnSessionInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "CustomerList";

                var expectedFilePath = $"~/App_Data/UserGuide/{ResourceName}/{DefaultUserSession.Language}.htm";
                var actualFileName = string.Empty;

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(true);
                _fileSystem.ReadFile(Arg.Do<string>(x => actualFileName = x)).Returns("{rooturl}");

                // Action
                controller.Index(ResourceName);

                // Assert
                Assert.That(actualFileName, Is.EqualTo(expectedFilePath));
            }
        }

        [Test]
        public void Index_UserUsesDefaultLanguage_ReturnDefaultFilePath()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "CustomerList";

                var expectedFilePath = $"~/App_Data/UserGuide/{ResourceName}/en-US.htm";
                var actualFileName = string.Empty;

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Do<string>(x => actualFileName = x)).Returns("{rooturl}");

                // Action
                controller.Index(ResourceName);

                // Assert
                Assert.That(actualFileName, Is.EqualTo(expectedFilePath));
            }
        }

        [Test]
        public void Index_CustomerListReport_ReplaceRootUrl()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "CustomerList";
                const string ExpectedContent = "/";

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Any<string>()).Returns("{rooturl}");

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(ExpectedContent));
            }
        }

        [TestCase]
        public void Index_ContentHasCasinoProductList_ReturnCorrectContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "LicenseeWL";
                const string Content = "{casino-productlist-section}{casino-productlist} content-here{/casino-productlist-section}";
                const string ExpectedContent = "AllBet content-here";

                _productService.GetProductList(Arg.Any<int>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Product>
            {
                new Product { ProductID = ProductTypes.AllBet, ProductName = "AllBet" }
            });

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Any<string>()).Returns(Content);

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(ExpectedContent));
            }
        }

        [TestCase]
        public void Index_ProductListNotHaveCasinoProduct_ReturnCorrectContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "LicenseeWL";
                const string Content = "{casino-productlist-section}{casino-productlist} content-here{/casino-productlist-section}ABC";
                const string ExpectedContent = "ABC";

                _productService.GetProductList(Arg.Any<int>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Product>
            {
                new Product { ProductID = ProductTypes.Sportsbook, ProductName = "Sportsbook" }
            });

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Any<string>()).Returns(Content);

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(ExpectedContent));
            }
        }

        [TestCase]
        public void Index_ProductListHaveVirtualProduct_ReturnCorrectContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "LicenseeWL";
                const string Content = "{VS-productlist-section}content VS{/VS-productlist-section}"
                                       + "{VS2-productlist-section} content VS2{/VS2-productlist-section} other content";
                const string ExpectedContent = "content VS content VS2 other content";

                _productService.GetProductList(Arg.Any<int>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Product>
            {
                new Product { ProductID = ProductTypes.VirtualSports },
                new Product { ProductID = ProductTypes.VirtualSports2 }
            });

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Any<string>()).Returns(Content);

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(ExpectedContent));
            }
        }

        [TestCase]
        public void Index_ProductListNotHaveVirtualProduct_ReturnCorrectContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ResourceName = "LicenseeWL";
                const string Content = "{VS-productlist-section}content VS{/VS-productlist-section}"
                                       + "{VS2-productlist-section} content VS2{/VS2-productlist-section} other content";
                const string ExpectedContent = " other content";

                _productService.GetProductList(Arg.Any<int>(), Arg.Any<string>()).ReturnsForAnyArgs(new List<Product>
            {
                new Product { ProductID = ProductTypes.Sportsbook }
            });

                controller.Url = Url;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);
                _fileSystem.ReadFile(Arg.Any<string>()).Returns(Content);

                // Action
                var result = controller.Index(ResourceName) as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(ExpectedContent));
            }
        }

        [Test]
        public void DownloadCustomerListPdf_IsEnableBA_ReturnCorrectFileName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.IsEnableBA = true;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);

                // Action
                var result = controller.DownloadCustomerListPdf();

                // Assert
                Assert.That(result.FileName, Does.Contain("Odds Group Feature_Licensee_BA.pdf"));
            }
        }

        [Test]
        public void DownloadCustomerListPdf_IsNotEnableBA_ReturnCorrectFileName()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.IsEnableBA = false;

                _fileSystem.Exists(Arg.Any<string>()).Returns(false);

                // Action
                var result = controller.DownloadCustomerListPdf();

                // Assert
                Assert.That(result.FileName, Does.Contain("Odds Group Feature_Licensee.pdf"));
            }
        }

        private UserGuideController CreateController()
        {
            var controller = new UserGuideController(LangService, SessionService, AppSettings, _fileSystem, _productService)
            {
                ControllerContext = ControllerContext
            };

            controller.Server.MapPath(Arg.Any<string>()).Returns(x => x.ArgAt<string>(0));

            return controller;
        }
    }
}