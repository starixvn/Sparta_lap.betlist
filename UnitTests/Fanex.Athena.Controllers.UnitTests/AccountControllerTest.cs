namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Account;
    using Base.UnitTests;
    using DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public partial class AccountControllerTest : BaseControllerHelperTest
    {
        private const string AccessDeniedUrl = "~/Utility/AccessDenied";

        private IFunctionService _functionService;
        private IUserService _userService;
        private ICurrencyDataAccess _currencyDataAccess;

        private AccountController _controller;

        [SetUp]
        public void SetUp()
        {
            _functionService = Substitute.For<IFunctionService>();
            _userService = Substitute.For<IUserService>();
            _currencyDataAccess = Substitute.For<ICurrencyDataAccess>();

            _controller = new AccountController(
            LangService,
            SessionService,
            AppSettings,
            _functionService,
            _userService,
            _currencyDataAccess)
            {
                ControllerContext = ControllerContext
            };

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _functionService = null;
            _userService = null;
            _currencyDataAccess = null;

            _controller.Dispose();
        }

        [Test]
        public void Manage_WhenCall_ReturnManageView()
        {
            // Arrange
            _userService.GetByLicensee(DefaultLicenseeSession.LicenseeId, Arg.Any<bool>()).Returns(new List<User>());

            // Action
            var result = _controller.Manage() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Account/Manage.cshtml"));
        }

        [Test]
        public void Manage_WhenCall_ReturnCorrectViewModel()
        {
            // Arrange
            var userList = new List<User>
            {
                new User { UserID = DefaultUserSession.UserId },
                new User { UserID = 2 }
            };
            _userService.GetByLicensee(DefaultLicenseeSession.LicenseeId, Arg.Any<bool>()).Returns(userList);

            // Action
            var result = (_controller.Manage() as ViewResult).Model as List<User>;

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void PermissionPage_InvalidUserRequestParam_AccessDenied()
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", "invalid" } });

            // Action
            var result = _controller.PermissionPage() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void PermissionPage_IsCurrentUser_AccessDenied()
        {
            // Arrange
            SetUpUserRequestParameter(new NameValueCollection { { "UserId", DefaultUserSession.UserId.ToString() } });

            // Action
            var result = _controller.PermissionPage() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void PermissionPage_ValidatePermissionRestriction_UserDoesNotExist_AccessDenied()
        {
            // Arrange
            SetUpUserRequestParameter();

            StubUserSessionData(new UserSession
            {
                UserId = DefaultUserSession.UserId,
                IsInternal = true
            });

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);

            // Action
            var result = _controller.PermissionPage() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void PermissionPage_UserDoesNotExist_AccessDenied()
        {
            // Arrange
            SetUpUserRequestParameter();

            StubUserSessionData(new UserSession
            {
                UserId = DefaultUserSession.UserId,
                IsInternal = true
            });

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => new User { UserID = DefaultUserSession.UserId, IsInternal = true }, x => null);

            // Action
            var result = _controller.PermissionPage() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void PermissionPage_IsInternalUser_RedirectToAccessDeniedView()
        {
            // Arrange
            SetUpUserRequestParameter();

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => new User { UserID = 111 });

            AppSettings.EnableAssignCurrency.Returns(false);

            StubUserSessionData(new UserSession
            {
                UserId = DefaultUserSession.UserId,
                IsInternal = true
            });

            // Action
            var result = _controller.PermissionPage() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo(AccessDeniedUrl));
        }

        [Test]
        public void PermissionPage_ValidUser_ReturnPermissionPageView()
        {
            // Arrange
            SetUpUserRequestParameter();

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => new User { UserID = 111, IsInternal = true });

            AppSettings.EnableAssignCurrency.Returns(false);

            StubUserSessionData(new UserSession
            {
                UserId = DefaultUserSession.UserId,
                IsInternal = false
            });

            // Action
            var viewResult = _controller.PermissionPage() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/PermissionPage.cshtml"));
        }

        private void SetUpUserRequestParameter(NameValueCollection requestParam = null)
        {
            if (requestParam == null)
            {
                requestParam = new NameValueCollection { { "UserId", "111" } };
            }

            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParam);
        }
    }
}