namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
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

    public class AgCasinoBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IAgCasinoBetSettingService _betSettingService;

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

                _betSettingService.GetViewModel(DefaultLicenseeSession.LicenseeId, metaModel.CustId, DefaultLicenseeSession.LicenseeName)
                    .Returns(x => { return null; });

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
                    .Returns(new AgCasinoBetSettingViewModel());

                // Action
                var result = controller.Index(metaModel) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties((BetSettingBaseViewModel)result.Model, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/AgCasino.cshtml"));
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.AgCasino } }));

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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.AgCasino } }));

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
                var result = controller.UpdateBetSetting(string.Empty, string.Empty) as JsonResult;
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.AgCasino } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateAgCasinoBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Any<string>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateBetSetting(string.Empty, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateAgCasinoBetSettingSuccessMsg, string.Empty)));
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductID = ProductTypes.AgCasino } }));

                SetUpForBetSettingEditable(true);

                _betSettingService.UpdateAgCasinoBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Any<string>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateBetSetting(string.Empty, string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        private AgCasinoBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<IAgCasinoBetSettingService>();

            var controller = new AgCasinoBetSettingController(
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

            controller.HttpContext.Request["custid"].Returns("1");

            return controller;
        }
    }
}