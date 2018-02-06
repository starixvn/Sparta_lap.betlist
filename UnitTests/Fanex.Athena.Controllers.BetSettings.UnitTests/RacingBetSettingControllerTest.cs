namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetSettings.Interfaces;

    public class RacingBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IRacingBetSettingService _betSettingService;

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
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Customer/BetSetting/Main.cshtml"));
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

        [TestCase("1", "Error")]
        [TestCase("", "Test")]
        public void UpdateBetSetting_HasPermission_ReturnCorrectResultInfo(string code, string message)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.RB } }));

                SetUpForBetSettingEditable(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                var expectedResultInfo = new Dictionary<string, string> { { "Code", code }, { "Message", message } };

                _betSettingService.UpdateBetSetting(
                   Arg.Any<int>(),
                   Arg.Any<IDictionary<string, RacingBetSetting>>()).Returns(expectedResultInfo);

                var expectedMessage = code.Length == 0 ? string.Format(ViewResources.UpdateRacingBetSettingSuccessMsg, string.Empty) : message;

                // Action
                var result = controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (Dictionary<string, string>)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo["Code"], Is.EqualTo(expectedResultInfo["Code"]));
                    Assert.That(resultInfo["Message"], Is.EqualTo(expectedMessage));
                });
            }
        }

        [Test]
        public void UpdateBetSetting_WhenCall_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.RB } }));

                SetUpForBetSettingEditable(true);

                const decimal MinBet151_31 = 2.5m;
                const decimal MaxBet151_31 = 3.552m;
                const decimal MaxPerMatch151_31 = 12.586m;

                controller.HttpContext.Request[Arg.Any<string>()].Returns(string.Empty);
                controller.HttpContext.Request["custid"].Returns("1");
                controller.HttpContext.Request["minbet151_31"].Returns(MinBet151_31.ToString());
                controller.HttpContext.Request["maxbet151_31"].Returns(MaxBet151_31.ToString());
                controller.HttpContext.Request["maxpermatch151_31"].Returns(MaxPerMatch151_31.ToString());

                var expectedResultInfo = new Dictionary<string, string> { { "Code", "1" }, { "Message", "Test" } };

                IDictionary<string, RacingBetSetting> param = null;
                _betSettingService.UpdateBetSetting(
                   Arg.Any<int>(),
                   Arg.Do<IDictionary<string, RacingBetSetting>>(x => param = x)).Returns(expectedResultInfo);

                // Action
                controller.UpdateBetSetting(string.Empty);
                var betSetting151_31 = param.Single(x => x.Key == "151_31");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(betSetting151_31.Value.IsDataChange, Is.True);
                    Assert.That(betSetting151_31.Value.MinBet, Is.EqualTo(MinBet151_31));
                    Assert.That(betSetting151_31.Value.MaxBet, Is.EqualTo(MaxBet151_31));
                    Assert.That(betSetting151_31.Value.MaxPerMatch, Is.EqualTo(MaxPerMatch151_31));

                    Assert.That(param.Where(x => x.Key != betSetting151_31.Key && x.Value.IsDataChange).Any(), Is.False);
                });
            }
        }

        private RacingBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<IRacingBetSettingService>();

            var controller = new RacingBetSettingController(
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