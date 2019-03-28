namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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
        public async Task Index_NoPermission_RedirectToAccessDeniedPage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { ProductId = ProductTypes.Macau, CustId = 100 };
                
                _betSettingService.GetViewModelAsync(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(Task.FromResult<ThirdPartyBetSettingViewModel>(null));

                // Action
                var result = await controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        [TestCase(ProductTypes.Macau, "~/Views/Desktop/Customer/BetSetting/Macau.cshtml")]
        [TestCase(ProductTypes.BetTrade, "~/Views/Desktop/Customer/BetSetting/BetTrade.cshtml")]
        [TestCase(ProductTypes.ColossusBets, "~/Views/Desktop/Customer/BetSetting/ColossusBets.cshtml")]
        [TestCase(ProductTypes.CashOut, "~/Views/Desktop/Customer/BetSetting/CashOut.cshtml")]
        [TestCase(ProductTypes.GoldDeluxe, "~/Views/Desktop/Customer/BetSetting/GoldDeluxe.cshtml")]
        public async Task Index_HasPermission_ReturnCorrectViewResult(int productId, string viewName)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { ProductId = productId };

                _betSettingService.GetViewModelAsync(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(Task.FromResult(new ThirdPartyBetSettingViewModel()));

                // Action
                var result = await controller.Index(metaModel) as ViewResult;

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

                _betSettingService.GetViewModelAsync(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                    .Returns(Task.FromResult(new ThirdPartyBetSettingViewModel()));

                // Action
                AsyncTestDelegate testDelegate = () => controller.Index(metaModel);

                // Assert
                Assert.ThrowsAsync<ArgumentException>(testDelegate, "Not a thirdparty product.");
            }
        }

        [Test]
        public async Task UpdateMacauStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product>())));

                // Action
                var result = await controller.UpdateMacauStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateMacauStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.Macau } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                var result = await controller.UpdateMacauStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateMacauStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.Macau } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 1, Message = "Error" }));

                // Action
                var result = await controller.UpdateMacauStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public async Task UpdateBetTradeStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.BetTrade } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                var result = await controller.UpdateBetTradeStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateBetTradeStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.BetTrade } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 1, Message = "Error" }));

                // Action
                var result = await controller.UpdateBetTradeStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public async Task UpdateColossusBetsStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product>())));

                // Action
                var result = await controller.UpdateColossusBetsStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateColossusBetsStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.ColossusBets } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                var result = await controller.UpdateColossusBetsStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateColossusBetsStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.ColossusBets } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 1, Message = "Error" }));

                // Action
                var result = await controller.UpdateColossusBetsStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public async Task UpdateCashOutStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product>())));

                // Action
                var result = await controller.UpdateCashOutStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateCashOutStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.CashOut } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                var result = await controller.UpdateCashOutStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateCashOutStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.CashOut } })));

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(
                   DefaultLicenseeSession.LicenseeId,
                   Arg.Any<int>(),
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 1, Message = "Error" }));

                // Action
                var result = await controller.UpdateCashOutStatus(0, string.Empty, 0) as JsonResult;
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
                CoreBetSettingService,
                ProductService,
                _betSettingService)
            {
                ControllerContext = ControllerContext
            };
            return controller;
        }
    }
}