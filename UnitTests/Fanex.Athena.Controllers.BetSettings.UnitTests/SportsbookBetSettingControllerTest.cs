namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
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

    public class SportsbookBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private ISportsbookBetSettingService _betSettingService;

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

                _betSettingService.GetViewModel(DefaultLicenseeSession.LicenseeId, metaModel.CustId).Returns(new BetSettingViewModel());

                // Action
                var result = controller.Index(metaModel) as ViewResult;
                var viewModel = (BetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);

                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/Main.cshtml"));
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.SB } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Any<IDictionary<string, object>>()).Returns(new ResultInfo { Code = 0 });

                // Action
                var result = controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateSportsbookBetSettingSuccessMsg, string.Empty)));
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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.SB } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Any<IDictionary<string, object>>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

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
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.SB } }));

                SetUpForBetSettingEditable(true);

                const decimal MinBet1 = 2.5m;
                const decimal MaxBet1 = 3.552m;
                const decimal MaxPerMatch1 = 12.586m;

                controller.HttpContext.Request[Arg.Any<string>()].Returns(string.Empty);
                controller.HttpContext.Request["custid"].Returns("1");
                controller.HttpContext.Request["minbet1"].Returns(MinBet1.ToString());
                controller.HttpContext.Request["maxbet1"].Returns(MaxBet1.ToString());
                controller.HttpContext.Request["maxpermatch1"].Returns(MaxPerMatch1.ToString());
                controller.HttpContext.Request["update1"].Returns("true");
                controller.HttpContext.Request["update2"].Returns("on");
                controller.HttpContext.Request["update3"].Returns("1");

                IDictionary<string, object> param = null;
                _betSettingService.UpdateBetSetting(
                   Arg.Any<CommonBetSetting>(),
                   Arg.Do<IDictionary<string, object>>(x => param = x)).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                controller.UpdateBetSetting(string.Empty);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(param["minbet"], Is.EqualTo(MinBet1));
                    Assert.That(param["maxbet"], Is.EqualTo(MaxBet1));
                    Assert.That(param["maxpermatch"], Is.EqualTo(MaxPerMatch1));
                    Assert.That(param["updateminmaxbet"], Is.True);

                    Assert.That(param["minbetv2"], Is.EqualTo(0));
                    Assert.That(param["maxbetv2"], Is.EqualTo(0));
                    Assert.That(param["maxpermatchv2"], Is.EqualTo(0));
                    Assert.That(param["updateminmaxbetv2"], Is.True);

                    Assert.That(param["updateminmaxbetv3"], Is.True);
                    Assert.That(param["updateminmaxbetv5"], Is.False);
                });
            }
        }

        private SportsbookBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<ISportsbookBetSettingService>();

            var controller = new SportsbookBetSettingController(
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