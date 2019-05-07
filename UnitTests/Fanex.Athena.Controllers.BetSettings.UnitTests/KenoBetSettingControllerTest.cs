namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

    public class KenoBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IKenoBetSettingService _betSettingService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _betSettingService = Substitute.For<IKenoBetSettingService>();

            SetUpDefaultFakeData();
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeBetSettingData();
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                _betSettingService.GetViewModelAsync(SessionService.LicenseeSession.LicenseeId, metaModel.CustId).Returns(Task.FromResult(new BetSettingViewModel()));

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var viewModel = (BetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/Keno.cshtml"));
                });
            }
        }

        [Test]
        public async Task UpdateBetSetting_NoPermission_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product>()));

                // Action
                var result = await controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(-1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [TestCase("0", "2")]
        [TestCase("1", "0")]
        [TestCase("0", "0")]
        public async Task UpdateBetSetting_InvalidParam_ReturnInvalidParamsResult(string id, string custid)
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.KN, ProductID = ProductTypes.Keno } }));

                CoreBetSettingService.IsBetSettingEditable(DefaultUserSession).Returns(true);

                controller.HttpContext.Request["id"].Returns(id);
                controller.HttpContext.Request["custid"].Returns(custid);

                // Action
                var result = await controller.UpdateBetSetting(string.Empty) as JsonResult;
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
        public async Task UpdateBetSetting_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.KN, ProductID = ProductTypes.Keno } }));

                CoreBetSettingService.IsBetSettingEditable(DefaultUserSession).Returns(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSettingAsync(
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<string>(),
                   Arg.Any<KenoBetSetting>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                var result = await controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(0));
                    Assert.That(resultInfo.Message, Is.EqualTo(string.Format(ViewResources.UpdateKenoBetSettingSuccessMsg, string.Empty)));
                });
            }
        }

        [Test]
        public async Task UpdateBetSetting_WhenCall_PassCorrectParam()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.KN, ProductID = ProductTypes.Keno } }));

                CoreBetSettingService.IsBetSettingEditable(DefaultUserSession).Returns(true);

                const string Id = "1";
                const string CustId = "2";
                const decimal MinBet = 2.5m;
                const decimal MaxBet = 3.552m;
                const decimal MaxPerMatch = 12.586m;

                controller.HttpContext.Request["id"].Returns(Id);
                controller.HttpContext.Request["custid"].Returns(CustId);
                controller.HttpContext.Request["maxbet1"].Returns(MaxBet.ToString());
                controller.HttpContext.Request["minbet1"].Returns(MinBet.ToString());
                controller.HttpContext.Request["maxpermatch1"].Returns(MaxPerMatch.ToString());

                KenoBetSetting param = null;
                _betSettingService.UpdateBetSettingAsync(
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<string>(),
                   Arg.Do<KenoBetSetting>(x => param = x)).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                await controller.UpdateBetSetting(string.Empty);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(param.MaxBet, Is.EqualTo(MaxBet));
                    Assert.That(param.MinBet, Is.EqualTo(MinBet));
                    Assert.That(param.MaxPerMatch, Is.EqualTo(MaxPerMatch));
                    Assert.That(param.GameTypeID, Is.EqualTo(Id));
                });
            }
        }

        [Test]
        public async Task UpdateBetSetting_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.KN, ProductID = ProductTypes.Keno } }));

                CoreBetSettingService.IsBetSettingEditable(DefaultUserSession).Returns(true);

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                _betSettingService.UpdateBetSettingAsync(
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<int>(),
                   Arg.Any<string>(),
                   Arg.Any<string>(),
                   Arg.Any<KenoBetSetting>()).Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 1, Message = "Error" }));

                // Action
                var result = await controller.UpdateBetSetting(string.Empty) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        private KenoBetSettingController CreateController()
        {
            var controller = new KenoBetSettingController(
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