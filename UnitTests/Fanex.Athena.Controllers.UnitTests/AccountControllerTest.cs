namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Account;
    using Base.UnitTests;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Reviewed.")]
    [TestFixture]
    public partial class AccountControllerTest : BaseControllerTest
    {
        private IFunctionService _functionService;
        private IUserService _userService;
        private ICurrencyService _currencyService;

        private AccountController _controller;

        [OneTimeSetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _functionService = Substitute.For<IFunctionService>();
            _userService = Substitute.For<IUserService>();
            _currencyService = Substitute.For<ICurrencyService>();
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _functionService = null;
            _userService = null;
            _currencyService = null;
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new AccountController(
                LangService,
                SessionService,
                AppSettings,
                _functionService,
                _userService,
                _currencyService);

            _controller.ControllerContext = this.ControllerContext;
        }

        [TearDown]
        public void TearDown()
        {
            if (_controller != null)
            {
                _controller.Dispose();
            }
        }

        [Test]
        public void Manage_WhenCall_ReturnCorrectViewName()
        {
            // Arrange
            const int LicenseeId = 1;
            _userService.GetByLicensee(LicenseeId, Arg.Any<bool>()).Returns(new List<User>());

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
                new User { UserID = 1 },
                new User { UserID = 2 }
            };
            _userService.GetByLicensee(SessionService.LicenseeSession.LicenseeId, Arg.Any<bool>()).Returns(userList);

            // Action
            var result = (_controller.Manage() as ViewResult).Model as List<User>;

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void PermissionPage_UserIsExists_ReturnCorrectViewAndModel()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", "111" } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(new User
            {
                Username = "Superman"
            });

            // Action
            var viewResult = _controller.PermissionPage() as ViewResult;

            // Assert
            Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Account/PermissionPage.cshtml"));
            Assert.That((viewResult.Model as User).Username, Is.EqualTo("Superman"));
        }

        [Test]
        public void PermissionPage_UserIsNotExists_RedirectToAccessDeniedView()
        {
            // Arrange
            var requestParams = new NameValueCollection { { "UserId", "111" } };
            _controller.ControllerContext.HttpContext.Request.Params.Returns(requestParams);

            _userService.GetUserInfo(Arg.Any<int>(), Arg.Any<int>()).Returns(x => null);

            // Action
            var result = _controller.PermissionPage() as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
        }
    }
}