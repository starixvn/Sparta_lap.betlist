namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Session.Enum;
    using Session.Model;

    public class BetSettingBaseControllerTest : BetSettingBaseControllerHelperTest
    {
        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeData();
        }

        [Test]
        public void AccessDenied_WhenCall_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.AccessDenied() as JsonResult;
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
        public void GetCustomerInfo_WhenCall_CallService()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int LicenseeId = 1;
                const int CustId = 2;

                // Action
                controller.GetCustomerInfo(LicenseeId, CustId);

                // Assert
                CustomerService.Received().GetCustomerInfo(LicenseeId, CustId);
            }
        }

        [Test]
        public void AssignRequiredProperties_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SetUpDefaultFakeBetSettingData();
                var viewModel = new BetSettingBaseViewModel();
                var metaModel = new BetSettingMetaModel();

                // Action
                controller.AssignRequiredProperties(viewModel, metaModel);

                // Assert
                AssertRequiredProperties(viewModel, metaModel);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AssignRequiredProperties_IsNotSyncCasino_DisableRngCasinoProduct(bool isEnableRngCasino)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var viewModel = new BetSettingBaseViewModel();
                var metaModel = new BetSettingMetaModel();

                CustomerService.GetCustomerInfo(DefaultLicenseeSession.LicenseeId, metaModel.CustId)
                    .Returns(new CustomerInfo { SyncCasino = false });

                CoreBetSettingDataAccess.GetBetSettingProducts(DefaultLicenseeSession.LicenseeId)
                    .Returns(new List<BetSettingProduct> { new BetSettingProduct { ProductId = ProductTypes.RngCasino, IsEnable = isEnableRngCasino } });

                // Action
                controller.AssignRequiredProperties(viewModel, metaModel);

                // Assert
                Assert.That(viewModel.BetSettingProducts.Any(x => x.ProductId == ProductTypes.RngCasino), Is.False);
            }
        }

        [Test]
        public void GetBetSettingCommonParam_WhenCall_ReturnCorrectData()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int CustId = 1;
                controller.HttpContext.Request["custid"].Returns(CustId.ToString());

                const string UserHostAddress = "UserHostAddress";
                controller.HttpContext.Request.UserHostAddress.Returns(UserHostAddress);

                // Action
                var result = controller.GetBetSettingCommonParam();

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(result.Ip, Is.EqualTo(UserHostAddress));
                    Assert.That(result.Language, Is.EqualTo(DefaultUserSession.Language));
                    Assert.That(result.UserId, Is.EqualTo(DefaultUserSession.UserId));
                    Assert.That(result.CustId, Is.EqualTo(CustId));
                });
            }
        }

        [Test]
        public void IsBetSettingEditable_IsParasiticMode_GetPermissionFromPortalFunctionList()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(new UserSession
                {
                    ApplicationMode = AppMode.Parasitic,
                    UserId = DefaultUserSession.UserId,
                    PortalFunctionList = DefaultUserSession.PortalFunctionList
                });

                // Action
                controller.IsBetSettingEditable();

                // Assert
                AuthorizationService.Received().HasPermissions(DefaultUserSession.PortalFunctionList, PermissionKeys.CustomerInfoEditBetSetting);
            }
        }

        [Test]
        public void IsBetSettingEditable_IsNotParasiticMode_GetPermissionFromFunctionIdList()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(new UserSession
                {
                    ApplicationMode = AppMode.Self,
                    UserId = DefaultUserSession.UserId,
                    PortalFunctionList = DefaultUserSession.PortalFunctionList
                });

                // Action
                controller.IsBetSettingEditable();

                // Assert
                AuthorizationService.Received().HasPermissions(DefaultUserSession.FunctionIdList, PermissionKeys.CustomerInfoEditBetSetting);
            }
        }

        [Test]
        public void GetDecimalRequest_NameIsNotExist_ReturnDefaultValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Name = "Test";
                const decimal DefaultValue = 5m;
                controller.HttpContext.Request["Test"].Returns(x => { return null; });

                // Action
                var result = controller.GetDecimalRequest(Name, DefaultValue);

                // Assert
                Assert.That(result, Is.EqualTo(DefaultValue));
            }
        }

        [Test]
        public void GetDecimalRequest_ValueIsIncorrectFormat_ReturnDefaultValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Name = "Test";
                const decimal DefaultValue = 5m;
                controller.HttpContext.Request["Test"].Returns("Invalid");

                // Action
                var result = controller.GetDecimalRequest(Name, DefaultValue);

                // Assert
                Assert.That(result, Is.EqualTo(DefaultValue));
            }
        }

        [Test]
        public void GetDecimalRequest_ValidRequestValue_ReturnRequestValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Name = "Test";
                const decimal Value = 5m;
                controller.HttpContext.Request["Test"].Returns(Value.ToString());

                // Action
                var result = controller.GetDecimalRequest(Name, 0);

                // Assert
                Assert.That(result, Is.EqualTo(Value));
            }
        }

        [Test]
        public void GetIntRequest_NameIsNotExist_ReturnDefaultValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Name = "TestInt";
                const int DefaultValue = 5;
                controller.HttpContext.Request["TestInt"].Returns(x => { return null; });

                // Action
                var result = controller.GetIntRequest(Name, DefaultValue);

                // Assert
                Assert.That(result, Is.EqualTo(DefaultValue));
            }
        }

        [Test]
        public void GetIntRequest_ValueIsIncorrectFormat_ReturnDefaultValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Name = "TestInt";
                const int DefaultValue = 5;
                controller.HttpContext.Request["TestInt"].Returns("Invalid");

                // Action
                var result = controller.GetIntRequest(Name, DefaultValue);

                // Assert
                Assert.That(result, Is.EqualTo(DefaultValue));
            }
        }

        [Test]
        public void GetIntRequest_ValidRequestValue_ReturnRequestValue()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string Name = "TestInt";
                const int Value = 5;
                controller.HttpContext.Request["TestInt"].Returns(Value.ToString());

                // Action
                var result = controller.GetIntRequest(Name, 0);

                // Assert
                Assert.That(result, Is.EqualTo(Value));
            }
        }

        private FakeBetSettingBaseController CreateController()
        {
            var controller = new FakeBetSettingBaseController(
                LangService,
                SessionService,
                AppSettings,
                AuthorizationService,
                CoreBetSettingDataAccess,
                CustomerService,
                ProductService)
            {
                ControllerContext = ControllerContext
            };

            return controller;
        }
    }
}