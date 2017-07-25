namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Authorization;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class AuthorizationControllerTest : BaseControllerTest
    {
        [Test]
        public void SignIn_NotInternalDomain_RedirectToLocationRestriction()
        {
            // Arrange
            AppSettings.GetValue("internaldomain").Returns("fakeurl");

            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignIn() as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
            }
        }

        [Test]
        public void SignIn_IsInternalDomain_ReturnCorrectView()
        {
            // Arrange
            AppSettings.GetValue("internaldomain").Returns(TestingRequestDomain);

            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignIn() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.LogInToken, Is.Not.Null);
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
            }
        }

        [Test]
        public void SignInPost_InvalidCaptcha_ReturnSignInViewWithError()
        {
            // Arrange
            AppSettings.GetValue("AllowAutomationCaptchaCode").Returns("0");
            SessionService.CaptchaCode.Returns("OriginalCaptcha");
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    Captcha = "WrongCaptcha"
                }) as ViewResult;

                // Assert
                Assert.That(SessionService.CaptchaCode, Is.Not.EqualTo("OriginalCaptcha"));
                Assert.That(result.ViewBag.errCode, Is.EqualTo(101));
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
            }
        }

        [Test]
        public void SignInPost_LoginFail_ReturnSignInViewWithError()
        {
            // Arrange
            IResultInfo resultInfo;
            AppSettings.GetValue("AllowAutomationCaptchaCode").Returns("1");
            SessionService.CaptchaCode.Returns("OriginalCaptcha");
            _userService.ValidateUserForLrfSystem(Arg.Any<User>(), Arg.Any<string>(), out resultInfo)
                .Returns(x =>
                {
                    x[2] = new ResultInfo
                    {
                        Code = 999,
                        Message = "Error999"
                    };
                    return null;
                });
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "xxx",
                    PassWord = "xxx"
                }) as ViewResult;

                // Assert
                Assert.That(result.ViewBag.errCode, Is.EqualTo(999));
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
            }
        }

        [Test]
        public void SignInPost_LoginSuccess_CreateSessionAndRedirectToLicenseeList()
        {
            // Arrange
            IResultInfo resultInfo;
            AppSettings.GetValue("AllowAutomationCaptchaCode").Returns("1");
            SessionService.CaptchaCode.Returns("OriginalCaptcha");
            _userService.ValidateUserForLrfSystem(Arg.Any<User>(), Arg.Any<string>(), out resultInfo)
                .Returns(x =>
                {
                    x[2] = new ResultInfo
                    {
                        Code = 0
                    };
                    return new User
                    {
                        UserID = 111,
                        Role = UserRole.Admin.ToString(),
                        Licensee = new Licensee()
                    };
                });
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "xxx",
                    PassWord = "xxx"
                }) as RedirectToRouteResult;

                // Assert
                SessionService.Received().CreateNewSession(111, Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<int>());
                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("LicenseeList"));
            }
        }
    }
}