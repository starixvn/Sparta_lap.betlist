namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
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

    public class GoldDeluxeBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IGoldDeluxeBetSettingService _betSettingService;

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

                AuthorizationService.HasPermissionOnData(DefaultLicenseeSession.LicenseeId, metaModel.CustId, 0).Returns(false);

                // Action
                var result = controller.Index(metaModel) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        [Test]
        public void Index_HasPermission_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                AuthorizationService.HasPermissionOnData(DefaultLicenseeSession.LicenseeId, metaModel.CustId, 0).Returns(true);
                _betSettingService.GetViewModel(DefaultLicenseeSession.LicenseeId, metaModel.CustId, DefaultLicenseeSession.LicenseeName)
                    .Returns(new GoldDeluxeBetSettingViewModel());

                // Action
                var result = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties((BetSettingBaseViewModel)result.Model, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/GoldDeluxe.cshtml"));
                });
            }
        }

        [Test]
        public void UpdateStatus_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
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
        public void UpdateStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.GoldDeluxe } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
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
        public void UpdateStatus_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.GoldDeluxe } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateStatus(
                   DefaultLicenseeSession.LicenseeId,
                   DefaultUserSession.UserId,
                   Arg.Any<int>(),
                   DefaultUserSession.Language,
                   Arg.Any<string>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateBetSetting_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = controller.UpdateBetSetting(0, string.Empty, string.Empty) as JsonResult;
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
        public void UpdateBetSetting_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.GoldDeluxe } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateBetSetting(
                   DefaultLicenseeSession.LicenseeId,
                   DefaultLicenseeSession.LicenseeName,
                   0,
                   string.Empty).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateBetSetting(0, string.Empty, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateGoldDeluxeBetSettingSuccessMsg, string.Empty)));
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.GoldDeluxe } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateBetSetting(
                   DefaultLicenseeSession.LicenseeId,
                   DefaultLicenseeSession.LicenseeName,
                   0,
                   string.Empty).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateBetSetting(0, string.Empty, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        private GoldDeluxeBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<IGoldDeluxeBetSettingService>();

            var controller = new GoldDeluxeBetSettingController(
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