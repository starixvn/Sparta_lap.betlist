namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Authorization;
    using NSubstitute;
    using NUnit.Framework;

    public partial class AuthorizationControllerTest
    {
        [Test]
        public void SignIn_WhenGet_NotInternalDomain_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("InternalSubDomain", "fakeurl");

                // Action
                var result = controller.SignIn() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void SignIn_WhenGet_IsInternalDomain_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("InternalSubDomain", TestingRequestDomain);

                // Action
                var result = controller.SignIn() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
            }
        }

        [Test]
        public void SignIn_WhenGet_IsInternalDomain_ReturnCorrectViewBag()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("InternalSubDomain", TestingRequestDomain);

                // Action
                var result = controller.SignIn() as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.LogInToken, Is.Not.Null);
                    Assert.That(result.ViewBag.errCode, Is.EqualTo(0));
                    Assert.That(result.ViewBag.errMsg, Is.Empty);
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_InvalidCaptcha_ReturnSignInViewWithError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "0");
                SessionService.CaptchaCode.Returns("OriginalCaptcha");

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "anything",
                    PassWord = "anything",
                    Captcha = "WrongCaptcha"
                }) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(SessionService.CaptchaCode, Is.Not.EqualTo("OriginalCaptcha"));
                    Assert.That(result.ViewBag.errCode, Is.EqualTo(101));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_UserNameIsEmpty_ReturnSignInViewWithError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "1");
                SessionService.CaptchaCode.Returns("OriginalCaptcha");

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = string.Empty
                }) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.errCode, Is.EqualTo(102));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_PassWordIsEmpty_ReturnSignInViewWithError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "1");
                SessionService.CaptchaCode.Returns("OriginalCaptcha");

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "User name",
                    PassWord = string.Empty
                }) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.errCode, Is.EqualTo(103));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_LoginFail_ReturnSignInViewWithError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                IResultInfo resultInfo;
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "1");
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

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "xxx",
                    PassWord = "xxx"
                }) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.errCode, Is.EqualTo(999));
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/Authorization/SignIn.cshtml"));
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_LoginWithSecretCaptcha_Successfully()
        {
            using (var controller = CreateController())
            {
                // Arrange
                IResultInfo resultInfo;
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "0");
                SessionService.CaptchaCode.Returns("OriginalCaptcha");
                StubGetValueAppSettings("SecretCaptcha", "SecretCaptcha");
                StubGetValueAppSettings("CompanyIP", controller.ControllerContext.HttpContext.Request.UserHostAddress);

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

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "xxx",
                    PassWord = "xxx",
                    Captcha = "SecretCaptcha"
                }) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("LicenseeList"));
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_AdminUser_RedirectToLicenseeList()
        {
            using (var controller = CreateController())
            {
                // Arrange
                IResultInfo resultInfo;
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "1");
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

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "xxx",
                    PassWord = "xxx"
                }) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("LicenseeList"));
                });
            }
        }

        [Test]
        public void SignIn_WhenPost_ValidUser_CreateNewSession()
        {
            using (var controller = CreateController())
            {
                // Arrange
                IResultInfo resultInfo;
                StubGetValueAppSettings("AllowAutomationCaptchaCode", "1");
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
                            Role = UserRole.Licensee.ToString(),
                            Licensee = new Licensee()
                        };
                    });

                // Action
                var result = controller.SignIn(new LoginMetalModel
                {
                    UserName = "xxx",
                    PassWord = "xxx"
                }) as RedirectToRouteResult;

                // Assert
                SessionService.Received().CreateNewSession(111, Arg.Any<int>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<int>());
            }
        }
    }
}