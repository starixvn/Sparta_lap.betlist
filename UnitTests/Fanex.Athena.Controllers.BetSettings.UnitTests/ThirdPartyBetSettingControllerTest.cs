namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetSettings.Interfaces;

    public class ThirdPartyBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IThirdPartyBetSettingService _betSettingService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeBetSettingData();
        }

        [Test]
        public void Index_NoPermission_RedirectToAccessDeniedPage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                _betSettingService.GetViewModel(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(x => { return null; });

                // Action
                var result = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        [TestCase(ProductTypes.Macau, "~/Views/Customer/BetSetting/Macau.cshtml")]
        [TestCase(ProductTypes.MicroGamingLiveCasino, "~/Views/Customer/BetSetting/MicroGamingLiveCasino.cshtml")]
        [TestCase(ProductTypes.MicrogamingRNGCasino, "~/Views/Customer/BetSetting/MicroGamingRNGCasino.cshtml")]
        [TestCase(ProductTypes.BetTrade, "~/Views/Customer/BetSetting/BetTrade.cshtml")]
        [TestCase(ProductTypes.ColossusBets, "~/Views/Customer/BetSetting/ColossusBets.cshtml")]
        [TestCase(ProductTypes.CashOut, "~/Views/Customer/BetSetting/CashOut.cshtml")]
        public void Index_HasPermission_ReturnCorrectViewResult(int productId, string viewName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { ProductId = productId };

                _betSettingService.GetViewModel(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(new ThirdPartyBetSettingViewModel());

                // Action
                var result = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties((BetSettingBaseViewModel)result.Model, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo(viewName));
                });
            }
        }

        [TestCase(0)]
        [TestCase(16)]
        [TestCase(ProductTypes.BA)]
        [TestCase(ProductTypes.RngCasino)]
        public void Index_InvalidProduct_ThrowException(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { ProductId = productId };

                _betSettingService.GetViewModel(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(new ThirdPartyBetSettingViewModel());

                // Action
                TestDelegate testDelegate = () => controller.Index(metaModel);

                // Assert
                Assert.Throws<ArgumentException>(testDelegate, "Not a thirdparty product.");
            }
        }

        [Test]
        public void UpdateMacauStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateMacauStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateMacauStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.Macau } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateMacauStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [Test]
        public void UpdateMacauStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.Macau } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateMacauStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateMicroGamingLiveCasinoStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateMicroGamingLiveCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateMicroGamingLiveCasinoStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.MicroGamingLiveCasino } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateMicroGamingLiveCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [Test]
        public void UpdateMicroGamingLiveCasinoStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.MicroGamingLiveCasino } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateMicroGamingLiveCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateMicroGamingRNGCasinoStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateMicroGamingRNGCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateMicroGamingRNGCasinoStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.MicrogamingRNGCasino } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateMicroGamingRNGCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [Test]
        public void UpdateMicroGamingRNGCasinoStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.MicrogamingRNGCasino } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateMicroGamingRNGCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateBetTradeStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateMicroGamingRNGCasinoStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateBetTradeStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.BetTrade } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateBetTradeStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [Test]
        public void UpdateBetTradeStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.BetTrade } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateBetTradeStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateColossusBetsStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateColossusBetsStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateColossusBetsStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.ColossusBets } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateColossusBetsStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [Test]
        public void UpdateColossusBetsStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.ColossusBets } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateColossusBetsStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateCashOutStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateCashOutStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void UpdateCashOutStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.CashOut } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateCashOutStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [Test]
        public void UpdateCashOutStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.CashOut } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateCashOutStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        private ThirdPartyBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<IThirdPartyBetSettingService>();

            var controller = new ThirdPartyBetSettingController(
                LangService,
                SessionService,
                AppSettings,
                AuthorizationService,
                CoreBetSettingDataAccess,
                CustomerService,
                ProductService,
                _betSettingService)
            {
                ControllerContext = ControllerContext
            };
            return controller;
        }
    }
}