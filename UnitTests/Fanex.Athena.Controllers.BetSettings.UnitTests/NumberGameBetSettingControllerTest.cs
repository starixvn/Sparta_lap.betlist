namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetSettings.Interfaces;

    public class NumberGameBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private INumberGameBetSettingService _betSettingService;

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
        public void Index_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                _betSettingService.GetViewModel(SessionService.LicenseeSession.LicenseeId, metaModel.CustId).Returns(new BetSettingViewModel());

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (BetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Customer/BetSetting/NumberGame.cshtml"));
                });
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
                var result = controller.UpdateBetSetting(string.Empty) as JsonResult;
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.NG } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Any<Hashtable>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateNumberGameBetSettingSuccessMsg, string.Empty)));
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.NG } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Any<Hashtable>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                var result = controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        [Test]
        public void UpdateBetSetting_WhenCall_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.NG } }));

                SetUpForBetSettingEditable(true);

                const decimal MinBet = 2.5m;
                const decimal MaxBet = 3.552m;
                const decimal MaxPerMatch = 12.586m;
                const decimal MaxPerBall = 7.256m;

                controller.HttpContext.Request["minbet161"].Returns(MinBet.ToString());
                controller.HttpContext.Request["maxbet161"].Returns(MaxBet.ToString());
                controller.HttpContext.Request["maxpermatch161"].Returns(MaxPerMatch.ToString());
                controller.HttpContext.Request["maxperball"].Returns(MaxPerBall.ToString());

                Hashtable param = null;
                _betSettingService.UpdateBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Do<Hashtable>(x => param = x)).Returns(new ResultInfo { Code = 0 });

                // Action
                controller.UpdateBetSetting(string.Empty);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(param["minbet161"], Is.EqualTo(MinBet));
                    Assert.That(param["maxbet161"], Is.EqualTo(MaxBet));
                    Assert.That(param["maxpermatch161"], Is.EqualTo(MaxPerMatch));
                    Assert.That(param["maxperball"], Is.EqualTo(MaxPerBall));
                });
            }
        }

        private NumberGameBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<INumberGameBetSettingService>();

            var controller = new NumberGameBetSettingController(
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