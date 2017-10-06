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

    [TestFixture]
    public partial class AuthorizationControllerTest : BaseControllerHelperTest
    {
        private IMaintenanceService _maintenanceService;
        private IUserService _userService;
        private IFormsAuthenticationService _formAuthenticationService;
        private AuthorizationController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _maintenanceService = Substitute.For<IMaintenanceService>();
            _userService = Substitute.For<IUserService>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
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

            _controller = new AuthorizationController(
               LangService,
               SessionService,
               AppSettings,
               _maintenanceService,
               _userService,
               _formAuthenticationService)
            {
                ControllerContext = ControllerContext
            };
        }

        [TearDown]
        public void TearDown()
        {
            AppSettings.ClearReceivedCalls();
            _userService.ClearReceivedCalls();

            _controller.Dispose();
        }

        [Test]
        public void Captcha_WhenCall_ReturnFileContent()
        {
            // Arrange
            SessionService.CaptchaCode = null;

            // Action
            var result = _controller.Captcha() as FileContentResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Captcha_WhenCall_AssignCaptchaToSession()
        {
            // Arrange
            SessionService.CaptchaCode = null;
            StubUserSessionData(new UserSession { ViewType = ClientViewType.Desktop });

            // Action
            _controller.Captcha();

            // Assert
            Assert.That(SessionService.CaptchaCode.Length, Is.EqualTo(4));
        }

        [Test]
        public void Captcha_NoneView_CreateCaptchaBaseOnAppSettingViewType()
        {
            // Arrange
            SessionService.CaptchaCode = null;
            StubUserSessionData(null);
            StubGetValueAppSettings(nameof(ClientViewType), ClientViewType.Default.ToString("d"));

            // Action
            _controller.Captcha();

            // Assert
            AppSettings.Received().GetValue(nameof(ClientViewType));
        }

        [Test]
        public void Captcha_UserSessionIsNull_CreateCaptchaBaseOnAppSettingViewType()
        {
            // Arrange
            SessionService.CaptchaCode = null;
            StubUserSessionData(new UserSession { ViewType = ClientViewType.None });
            StubGetValueAppSettings(nameof(ClientViewType), ClientViewType.Default.ToString("d"));

            // Action
            _controller.Captcha();

            // Assert
            AppSettings.Received().GetValue(nameof(ClientViewType));
        }

        [Test]
        public void GetUserSalt_WhenCall_ReturnSaltFromService()
        {
            // Arrange
            const string UserName = "abc";
            _userService.GetByUsername(UserName).Returns(new User
            {
                UserSalt = "xxx"
            });

            // Action
            var result = _controller.GetUserSalt(UserName) as string;

            // Assert
            Assert.That(result, Is.EqualTo("xxx"));
        }

        [Test]
        public void GetUserSalt_UserDoesNotExist_ReturnEmptyString()
        {
            // Arrange
            const string UserName = "abc";
            _userService.GetByUsername(UserName).Returns(x => null);

            // Action
            var result = _controller.GetUserSalt(UserName) as string;

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void SignOut_WhenCall_CallSignOutFromService()
        {
            // Action
            var result = _controller.SignOut() as RedirectToRouteResult;

            // Assert
            _userService.Received().Logout(DefaultUserSession.OnUserId);
            _formAuthenticationService.Received().SignOut();
        }

        [Test]
        public void SignOut_UserDoesNotExist_NotCallLogoutFromUserService()
        {
            // Arrange
            StubUserSessionData(null);

            // Action
            var result = _controller.SignOut() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                _userService.DidNotReceive().Logout(Arg.Any<int>());
                _formAuthenticationService.Received().SignOut();
            });
        }

        [Test]
        public void SignOut_WhenCall_RedirectToLocationRestriction()
        {
            // Action
            var result = _controller.SignOut() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
            });
        }

        [Test]
        public void SignOut_WhenCall_ClearCookies()
        {
            // Arrange
            const string CookieName = "cookie1";
            _controller.ControllerContext.HttpContext.Request.Cookies.Add(new System.Web.HttpCookie(CookieName));

            // Action
            var result = _controller.SignOut() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(_controller.ControllerContext.HttpContext.Response.Cookies[0].Name, Is.EqualTo(CookieName));
                Assert.That(_controller.ControllerContext.HttpContext.Response.Cookies[0].Expires, Is.LessThan(DateTime.Now));
            });
        }

        [Test]
        public void SignOut_WhenCall_ClearSession()
        {
            // Action
            var result = _controller.SignOut() as RedirectToRouteResult;

            // Assert
            _controller.Session.Received().Clear();
        }

        [Test]
        public void SignOut_WhenCall_ClearCache()
        {
            // Action
            var result = _controller.SignOut() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                _controller.Response.Cache.Received().SetNoStore();
                _controller.Response.Cache.Received().SetExpires(Arg.Any<DateTime>());
                _controller.Response.Cache.Received().SetCacheability(HttpCacheability.NoCache);
            });
        }

        [Test]
        public void InternalSignOut_WhenCall_CallSignOutFromService()
        {
            // Action
            var result = _controller.InternalSignOut() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                _userService.Received().Logout(DefaultUserSession.OnUserId);
                _formAuthenticationService.Received().SignOut();
            });
        }

        [Test]
        public void InternalSignOut_WhenCall_RedirectToOneTokenRestriction()
        {
            // Action
            var result = _controller.InternalSignOut() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("OneTokenRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                Assert.That(result.RouteValues["code"], Is.EqualTo(100));
            });
        }

        [Test]
        public void ChangePassword_WhenGet_ReturnCorrectViewNam()
        {
            // Action
            var result = _controller.ChangePassword() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/ChangePass.cshtml"));
        }

        [Test]
        public void ChangePassword_WhenGet_ReturnCorrectViewBag()
        {
            // Action
            var result = _controller.ChangePassword() as ViewResult;

            // Assert
            Assert.That(result.ViewBag.PageData as Dictionary<string, string>, Is.Not.Null);
        }

        [Test]
        public void ChangePassword_WhenPost_InvalidNewPassword_ReturnErrorResult()
        {
            // Action
            var result = _controller.ChangePassword("oldpassword", "abc") as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(data.errCode, Is.EqualTo(ErrorType.Unknown));
                Assert.That(data.errMsg, Is.EqualTo(ViewResources.PasswordNoteMsg));
            });
        }

        [Test]
        public void ChangePassword_WhenPost_IsInternalUser_ReturnAccessDenied()
        {
            // Arrange
            SessionService.UserSession.IsInternal = true;

            // Action
            var result = _controller.ChangePassword("oldpassword", "1234aa") as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(data.errCode, Is.EqualTo(ErrorType.AccessDenied));
                Assert.That(data.errMsg, Is.EqualTo(ViewResources.AccessDeniedMsg));
            });
        }

        [Test]
        public void ChangePassword_WhenPost_ValidData_CallServiceCorrectly()
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
            var result = _controller.ChangePassword(OldPassword, NewPassword) as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.Multiple(() =>
            {
                _userService.Received().ChangePassword(DefaultUserSession.UserName, OldPassword, NewPassword, string.Empty);
                Assert.That(data.errCode, Is.EqualTo(ErrorCode));
            });
        }
    }
}