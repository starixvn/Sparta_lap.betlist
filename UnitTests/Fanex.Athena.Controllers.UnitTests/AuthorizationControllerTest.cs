namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
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
    public partial class AuthorizationControllerTest : BaseControllerTest
    {
        private IMaintenanceService _maintenanceService;
        private IUserService _userService;
        private IFormsAuthenticationService _formAuthenticationService;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _maintenanceService = Substitute.For<IMaintenanceService>();
            _userService = Substitute.For<IUserService>();
            _formAuthenticationService = Substitute.For<IFormsAuthenticationService>();
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _maintenanceService = null;
            _userService = null;
            _formAuthenticationService = null;
        }

        [Test]
        public void Captcha_WhenCall_ReturnFileContentAndAssignCaptchToSession()
        {
            // Arrange
            SessionService.CaptchaCode = null;
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Captcha() as FileContentResult;

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(SessionService.CaptchaCode.Length, Is.EqualTo(4));
            }
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

            using (var controller = CreateController())
            {
                // Action
                var result = controller.GetUserSalt(UserName) as string;

                // Assert
                Assert.That(result, Is.EqualTo("xxx"));
            }
        }

        [Test]
        public void SignOut_WhenCall_CallSignOutFromServiceAndRedirectToLocationRestriction()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignOut() as RedirectToRouteResult;

                // Assert
                _userService.Received().Logout(SessionOnUserId);
                _formAuthenticationService.Received().SignOut();

                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
            }
        }

        [Test]
        public void InternalSignOut_WhenCall_CallSignOutFromServiceAndRedirectToOneTokenRestriction()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var result = controller.InternalSignOut() as RedirectToRouteResult;

                // Assert
                _userService.Received().Logout(SessionOnUserId);
                _formAuthenticationService.Received().SignOut();

                Assert.That(result.RouteValues["action"], Is.EqualTo("OneTokenRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                Assert.That(result.RouteValues["code"], Is.EqualTo(100));
            }
        }

        [Test]
        public void ChangePassword_WhenCall_ReturnCorrectViewNameAndViewBag()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Utility/ChangePass.cshtml"));
                Assert.That(result.ViewBag.PageData as Dictionary<string, string>, Is.Not.Null);
            }
        }

        [Test]
        public void ChangePasswordPost_InvalidNewPassword_ReturnErrorResult()
        {
            // Arrange
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword("oldpassword", "abc") as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.errCode, Is.EqualTo(ErrorType.Unknown));
                Assert.That(data.errMsg, Is.EqualTo(ViewResources.PasswordNoteMsg));
            }
        }

        [Test]
        public void ChangePasswordPost_IsInternalUser_ReturnAccessDenied()
        {
            // Arrange
            SessionService.UserSession.IsInternal = true;
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword("oldpassword", "1234aa") as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.errCode, Is.EqualTo(ErrorType.AccessDenied));
                Assert.That(data.errMsg, Is.EqualTo(ViewResources.AccessDeniedMsg));
            }
        }

        [Test]
        public void ChangePasswordPost_WhenCall_CallServiceCorrectly()
        {
            // Arrange
            const string OldPassword = "oldpassword";
            const string NewPassword = "1234aa";
            const int ErrorCode = 999;
            SessionService.UserSession.IsInternal = false;
            _userService.ChangePassword(SesssionUserName, OldPassword, NewPassword, Arg.Any<string>())
                .Returns(new ResultInfo
                {
                    Code = ErrorCode
                });
            using (var controller = CreateController())
            {
                // Action
                var result = controller.ChangePassword(OldPassword, NewPassword) as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                _userService.Received().ChangePassword(SesssionUserName, OldPassword, NewPassword, string.Empty);
                Assert.That(data.errCode, Is.EqualTo(ErrorCode));
            }
        }

        private AuthorizationController CreateController()
        {
            var controller = new AuthorizationController(
                LangService,
                SessionService,
                AppSettings,
                _maintenanceService,
                _userService,
                _formAuthenticationService);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}