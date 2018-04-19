namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetSettings.Interfaces;

    public class DcsBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IDcsBetSettingService _betSettingService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betSettingService = Substitute.For<IDcsBetSettingService>();

            SetUpDefaultFakeData();
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeBetSettingData();
        }

        [TestCase(1)]
        [TestCase(100)]
        public void Index_InvalidProduct_ThrowArgumentException(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { ProductId = productId };

                // Action
                TestDelegate testDelegate = () => controller.Index(metaModel);

                // Assert
                Assert.Throws<ArgumentException>(testDelegate, "Invalid product id: " + productId);
            }
        }

        [TestCase(ProductTypes.AllBet, "AllBet", true, false)]
        [TestCase(ProductTypes.Voidbridge, "Voidbridge", true, false)]
        [TestCase(ProductTypes.FishingWorld, "FishingWorld", true, false)]
        [TestCase(ProductTypes.VirtualSports, "VirtualSports", false, true)]
        [TestCase(ProductTypes.VirtualSports2, "VirtualSports2", false, true)]
        [TestCase(ProductTypes.Lottery, "Lottery", true, true)]
        [TestCase(ProductTypes.Exchange, "Exchange", true, false)]
        [TestCase(ProductTypes.PP, "PP", true, false)]
        public void Index_ValidProduct_ReturnCorrectViewResult(int productId, string viewName, bool hasStatus, bool hasBetLimit)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { ProductId = productId };

                bool? expectedStatus = null;
                var expectedNumberOfMainData = 0;

                if (hasStatus)
                {
                    expectedStatus = true;
                    _betSettingService.GetStatus(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId).Returns(expectedStatus);
                }

                if (hasBetLimit)
                {
                    expectedNumberOfMainData = 1;
                    _betSettingService.GetBetSettings(DefaultLicenseeSession.LicenseeId, metaModel.CustId, metaModel.ProductId)
                        .Returns(new List<BetSetting> { new BetSetting() });
                }

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (BetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo($"~/Views/Customer/BetSetting/{viewName}.cshtml"));
                    Assert.That(viewModel.IsEnabled, Is.EqualTo(expectedStatus));
                    Assert.That(viewModel.MainData.Count, Is.EqualTo(expectedNumberOfMainData));
                });
            }
        }

        [TestCase(ProductTypes.AllBet)]
        [TestCase(ProductTypes.Voidbridge)]
        [TestCase(ProductTypes.FishingWorld)]
        [TestCase(ProductTypes.VirtualSports)]
        [TestCase(ProductTypes.VirtualSports2)]
        [TestCase(ProductTypes.Lottery)]
        [TestCase(ProductTypes.Exchange)]
        [TestCase(ProductTypes.PP)]
        public void UpdateStatus_NoPermission_ReturnAccessDeniedResult(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateStatus(0, string.Empty, 0, productId) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [TestCase(ProductTypes.AllBet)]
        [TestCase(ProductTypes.Voidbridge)]
        [TestCase(ProductTypes.FishingWorld)]
        [TestCase(ProductTypes.VirtualSports)]
        [TestCase(ProductTypes.VirtualSports2)]
        [TestCase(ProductTypes.Lottery)]
        [TestCase(ProductTypes.Exchange)]
        [TestCase(ProductTypes.PP)]
        public void UpdateStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = productId } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<bool>(),
                   productId).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateStatus(0, string.Empty, 0, productId) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSuccess, string.Empty)));
                });
            }
        }

        [TestCase(ProductTypes.AllBet)]
        [TestCase(ProductTypes.Voidbridge)]
        [TestCase(ProductTypes.FishingWorld)]
        [TestCase(ProductTypes.VirtualSports)]
        [TestCase(ProductTypes.VirtualSports2)]
        [TestCase(ProductTypes.Lottery)]
        [TestCase(ProductTypes.Exchange)]
        [TestCase(ProductTypes.PP)]
        public void UpdateStatus_UpdateFailed_ReturnFailedResultInfo(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = productId } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<bool>(),
                   productId).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateStatus(0, string.Empty, 0, productId) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [TestCase(ProductTypes.AllBet)]
        [TestCase(ProductTypes.Voidbridge)]
        [TestCase(ProductTypes.FishingWorld)]
        [TestCase(ProductTypes.VirtualSports)]
        [TestCase(ProductTypes.VirtualSports2)]
        [TestCase(ProductTypes.Lottery)]
        [TestCase(ProductTypes.Exchange)]
        [TestCase(ProductTypes.PP)]
        public void UpdateBetSetting_NoPermission_ReturnAccessDeniedResult(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateBetSetting(productId, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [TestCase(ProductTypes.VirtualSports, "180,181,182,183,184,185,186")]
        [TestCase(ProductTypes.VirtualSports2, "190,191,192,193,199")]
        [TestCase(ProductTypes.Lottery, "220")]
        public void UpdateBetSetting_ProductHaveBetLimit_GetCorrectParam(int productId, string expectedBetLimits)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = productId } }));

                SetUpForBetSettingEditable(true);

                IEnumerable<DcsBetSetting> param = null;
                _betSettingService.UpdateBetSetting(
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Do<IEnumerable<DcsBetSetting>>(x => param = x)).Returns(new ResultInfo { Code = 0 });

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                // Action
                controller.UpdateBetSetting(productId, string.Empty);

                // Assert
                Assert.That(string.Join(",", param.Select(x => x.SportId)), Is.EqualTo(expectedBetLimits));
            }
        }

        [TestCase(ProductTypes.AllBet)]
        [TestCase(ProductTypes.Voidbridge)]
        [TestCase(ProductTypes.FishingWorld)]
        [TestCase(ProductTypes.Exchange)]
        public void UpdateBetSetting_ProductDoesNotHaveBetLimit_ReturnInvalidParamsResult(int productId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = productId } }));

                SetUpForBetSettingEditable(true);

                // Action
                var result = controller.UpdateBetSetting(productId, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void UpdateBetSetting_InvalidParams_ReturnInvalidParamsResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.VirtualSports } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("0");

                // Action
                var result = controller.UpdateBetSetting(ProductTypes.VirtualSports, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.InvalidParams));
                });
            }
        }

        [Test]
        public void UpdateBetSetting_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.Lottery } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSetting(
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<IEnumerable<DcsBetSetting>>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateBetSetting(ProductTypes.Lottery, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.IsSuccessful, Is.True);
                });
            }
        }

        [Test]
        public void UpdateBetSetting_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.Lottery } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSetting(
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<IEnumerable<DcsBetSetting>>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateBetSetting(ProductTypes.Lottery, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        private DcsBetSettingController CreateController()
        {
            var controller = new DcsBetSettingController(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
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