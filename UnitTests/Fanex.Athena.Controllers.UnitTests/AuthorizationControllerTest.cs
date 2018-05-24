namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Services;
    using Resources;
    using Session.Enum;
    using Session.Model;

    [TestFixture]
    public partial class AuthorizationControllerTest : BaseControllerHelperTest
    {
        private IMaintenanceService _maintenanceService;
        private IUserService _userService;
        private IAuthService _authService;
        private IFormsAuthenticationService _formAuthenticationService;
        private IFileSystem _fileSystem;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _maintenanceService = Substitute.For<IMaintenanceService>();
            _userService = Substitute.For<IUserService>();
            _authService = Substitute.For<IAuthService>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _fileSystem = Substitute.For<IFileSystem>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _maintenanceService = null;
            _userService = null;
            _formAuthenticationService = null;
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            AppSettings.ClearReceivedCalls();
            _userService.ClearReceivedCalls();
            _authService.ClearReceivedCalls();
        }

        [Test]
        public void Captcha_WhenCall_ReturnFileContent()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.CaptchaCode = null;

                // Action
                var result = controller.Captcha() as FileContentResult;

                // Assert
                Assert.That(result, Is.Not.Null);
            }
        }

        [Test]
        public void Captcha_WhenCall_AssignCaptchaToSession()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.CaptchaCode = null;
                StubUserSessionData(new UserSession { ViewType = ClientViewType.Desktop });

                // Action
                controller.Captcha();

                // Assert
                Assert.That(SessionService.CaptchaCode.Length, Is.EqualTo(4));
            }
        }

        [Test]
        public void Captcha_NoneView_CreateCaptchaBaseOnAppSettingViewType()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.CaptchaCode = null;
                StubUserSessionData(null);
                StubGetValueAppSettings(nameof(ClientViewType), ClientViewType.Default.ToString("d"));

                // Action
                controller.Captcha();

                // Assert
                AppSettings.Received().GetValue(nameof(ClientViewType));
            }
        }

        [Test]
        public void Captcha_UserSessionIsNull_CreateCaptchaBaseOnAppSettingViewType()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.CaptchaCode = null;
                StubUserSessionData(new UserSession { ViewType = ClientViewType.None });
                StubGetValueAppSettings(nameof(ClientViewType), ClientViewType.Default.ToString("d"));

                // Action
                controller.Captcha();

                // Assert
                AppSettings.Received().GetValue(nameof(ClientViewType));
            }
        }

        [Test]
        public void GetUserSalt_WhenCall_ReturnSaltFromService()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string UserName = "abc";
                _userService.GetByUsername(UserName, false).Returns(new User
                {
                    UserSalt = "xxx"
                });

                // Action
                var result = controller.GetUserSalt(UserName) as string;

                // Assert
                Assert.That(result, Is.EqualTo("xxx"));
            }
        }

        [Test]
        public void GetUserSalt_UserDoesNotExist_ReturnEmptyString()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string UserName = "abc";
                _userService.GetByUsername(UserName, false).Returns(x => null);

                // Action
                var result = controller.GetUserSalt(UserName) as string;

                // Assert
                Assert.That(result, Is.Empty);
            }
        }

        [Test]
        public void SignOut_WhenCall_CallSignOutFromService()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                _authService.Received().Logout(DefaultUserSession.OnUserId);
                _formAuthenticationService.Received().SignOut();
            }
        }

        [Test]
        public void SignOut_UserDoesNotExist_NotCallLogoutFromUserService()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(null);

                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _authService.DidNotReceive().Logout(Arg.Any<int>());
                    _formAuthenticationService.Received().SignOut();
                });
            }
        }

        [Test]
        public void SignOut_WhenCall_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void SignOut_WhenCall_ClearCookies()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string CookieName = "cookie1";
                controller.ControllerContext.HttpContext.Request.Cookies.Add(new HttpCookie(CookieName));

                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(controller.ControllerContext.HttpContext.Response.Cookies[0].Name, Is.EqualTo(CookieName));
                    Assert.That(controller.ControllerContext.HttpContext.Response.Cookies[0].Expires, Is.LessThan(DateTime.Now));
                });
            }
        }

        [Test]
        public void SignOut_WhenCall_ClearSession()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                controller.Session.Received().Clear();
            }
        }

        [Test]
        public void SignOut_WhenCall_ClearCache()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Response.Cache.Received().SetNoStore();
                    controller.Response.Cache.Received().SetExpires(Arg.Any<DateTime>());
                    controller.Response.Cache.Received().SetCacheability(HttpCacheability.NoCache);
                });
            }
        }

        [Test]
        public void InternalSignOut_WhenCall_CallSignOutFromService()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.InternalSignOut() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    _authService.Received().Logout(DefaultUserSession.OnUserId);
                    _formAuthenticationService.Received().SignOut();
                });
            }
        }

        [Test]
        public void InternalSignOut_WhenCall_RedirectToOneTokenRestriction()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.InternalSignOut() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("OneTokenRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                    Assert.That(result.RouteValues["code"], Is.EqualTo(100));
                });
            }
        }

        [Test]
        public void ChangePassword_WhenGet_ReturnCorrectViewNam()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword() as ViewResult;

                // Assert
                StringAssert.Contains("Authorization/ChangePass.cshtml", result.ViewName);
            }
        }

        [Test]
        public void ChangePassword_WhenGet_ReturnCorrectViewBag()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.PageData as Dictionary<string, string>, Is.Not.Null);
            }
        }

        [Test]
        public void ChangePassword_WhenPost_InvalidNewPassword_ReturnErrorResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword("oldpassword", "abc") as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data.errCode, Is.EqualTo(ErrorType.Unknown));
                    Assert.That(data.errMsg, Is.EqualTo(ViewResources.PasswordNoteMsg));
                });
            }
        }

        [Test]
        public void ChangePassword_WhenPost_IsInternalUser_ReturnAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.UserSession.IsInternal = true;

                // Action
                var result = controller.ChangePassword("oldpassword", "1234aa") as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data.errCode, Is.EqualTo(ErrorType.AccessDenied));
                    Assert.That(data.errMsg, Is.EqualTo(ViewResources.AccessDeniedMsg));
                });
            }
        }

        [Test]
        public void ChangePassword_WhenPost_ValidData_CallServiceCorrectly()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string OldPassword = "oldpassword";
                const string NewPassword = "1234aa";
                const int ErrorCode = 999;

                SessionService.UserSession.IsInternal = false;

                _userService.ChangePassword(DefaultUserSession.UserName, OldPassword, NewPassword, Arg.Any<string>())
                    .Returns(new ResultInfo
                    {
                        Code = ErrorCode
                    });

                // Action
                var result = controller.ChangePassword(OldPassword, NewPassword) as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.Multiple(() =>
                {
                    _userService.Received().ChangePassword(DefaultUserSession.UserName, OldPassword, NewPassword, string.Empty);
                    Assert.That(data.errCode, Is.EqualTo(ErrorCode));
                });
            }
        }

        private AuthorizationController CreateController()
        {
            return new AuthorizationController(
               LangService,
               SessionService,
               AppSettings,
               ClientViewHelper,
               _maintenanceService,
               _userService,
               _authService,
               _formAuthenticationService)
            {
                ControllerContext = ControllerContext
            };
        }
    }
}