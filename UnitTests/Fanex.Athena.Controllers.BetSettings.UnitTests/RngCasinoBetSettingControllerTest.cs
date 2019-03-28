namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.BetSettings.Interfaces;
    using UnitTest.Common.Helpers;

    internal class RngCasinoBetSettingControllerTest : BetSettingBaseControllerHelperTest
    {
        private IRngCasinoBetSettingService _betSettingService;

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
        public async Task Index_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetSettingMetaModel { CustId = 100 };

                var customer = FakeHelper.Create<CustomerInfo>();
                CoreBetSettingService.GetCustomerInfo(metaModel.CustId).Returns(customer);

                _betSettingService.GetViewModel(metaModel.CustId, customer, DefaultUserSession.Language).Returns(new RngCasinoBetSettingViewModel());

                // Action
                var result = await controller.Index(metaModel) as ViewResult;
                var viewModel = (BetSettingViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertRequiredProperties(viewModel, metaModel);

                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Customer/BetSetting/RngCasino.cshtml"));
                });
            }
        }

        [Test]
        public async Task UpdateBetSetting_IsNotAllowCS_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product>())));

                // Action
                var result = await controller.UpdateBetSetting(0, string.Empty) as JsonResult;
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
        public async Task UpdateBetSetting_IsNotSyncCasino_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = false });

                // Action
                var result = await controller.UpdateBetSetting(0, string.Empty) as JsonResult;
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
        public async Task UpdateBetSetting_WhenCallService_ReturnCorrectResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = true });

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                var expectedResultInfo = new ResultInfo { Code = 0, Message = "Error" };
                _betSettingService.UpdateSetting(
                   Arg.Any<RngCasinoBetSettingMetaModel>(),
                   Arg.Any<bool>(),
                   Arg.Any<bool>()).Returns(expectedResultInfo);

                // Action
                var result = await controller.UpdateBetSetting(0, "1") as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(expectedResultInfo.Code));
                    Assert.That(resultInfo.Message, Is.EqualTo(expectedResultInfo.Message));
                });
            }
        }

        [Test]
        public async Task UpdateBetSetting_WhenThrowException_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = true });

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                controller.HttpContext.Request[Arg.Any<string>()].Returns("1");

                const string ErrorMessage = "Failed";
                _betSettingService.UpdateSetting(
                   Arg.Any<RngCasinoBetSettingMetaModel>(),
                   Arg.Any<bool>(),
                   Arg.Any<bool>()).Returns(x => { throw new System.Exception(ErrorMessage); });

                // Action
                var result = await controller.UpdateBetSetting(0, "1") as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(resultInfo.Code, Is.EqualTo(1));
                    Assert.That(resultInfo.Message, Is.EqualTo(ErrorMessage));
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
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = true });

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                const int CustId = 1;
                var gameTypes = new[] { 1, 2 };
                const string Stake1 = "3";
                const string Stake2 = "4";

                controller.HttpContext.Request["stake1"].Returns(Stake1);
                controller.HttpContext.Request["stake2"].Returns(Stake2);

                RngCasinoBetSettingMetaModel param = null;
                _betSettingService.UpdateSetting(
                   Arg.Do<RngCasinoBetSettingMetaModel>(x => param = x),
                   Arg.Any<bool>(),
                   Arg.Any<bool>()).Returns(new ResultInfo { Code = 1, Message = "Error" });

                // Action
                await controller.UpdateBetSetting(CustId, string.Join(",", gameTypes));
                var game1 = param.TableParam.Rows[0];
                var game2 = param.TableParam.Rows[1];

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(game1["GameTypeId"], Is.EqualTo(gameTypes[0]));
                    Assert.That(game2["GameTypeId"], Is.EqualTo(gameTypes[1]));

                    Assert.That(game1["StakeLevelList"], Is.EqualTo(Stake1));
                    Assert.That(game2["StakeLevelList"], Is.EqualTo(Stake2));
                });
            }
        }

        [Test]
        public async Task UpdateStatus_IsNotAllowCS_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product>())));

                // Action
                var result = await controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateStatus_IsNotSyncCasino_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = false });

                // Action
                var result = await controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateStatus_UpdateSuccessfully_ReturnSuccessfullResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = true });

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(Arg.Any<BetSettingCasinoStatusMetaModel>())
                    .Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 0 }));

                // Action
                var result = await controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
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
        public async Task UpdateBetSetting_UpdateFailed_ReturnFailedResultInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                ProductService.GetProductAccess(DefaultLicenseeSession.LicenseeId, DefaultUserSession.Language)
                    .Returns(Task.FromResult(new ProductAccess(new List<Product> { new Product { ProductCode = Product.CS } })));

                CoreBetSettingService.GetCustomerInfo(0).Returns(new CustomerInfo { SyncCasino = true });

                CoreBetSettingService.IsBetSettingEditableAsync().Returns(Task.FromResult(true));

                _betSettingService.UpdateStatusAsync(Arg.Any<BetSettingCasinoStatusMetaModel>())
                    .Returns(Task.FromResult<IResultInfo>(new ResultInfo { Code = 1, Message = "Error" }));

                // Action
                var result = await controller.UpdateStatus(0, string.Empty, 0) as JsonResult;
                var resultInfo = (ResultInfo)result.Data;

                // Assert
                Assert.That(resultInfo.IsSuccessful, Is.False);
            }
        }

        private RngCasinoBetSettingController CreateController()
        {
            _betSettingService = Substitute.For<IRngCasinoBetSettingService>();

            var controller = new RngCasinoBetSettingController(
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