namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Account;
    using Base.UnitTests;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class AccountControllerTest : BaseControllerHelperTest
    {
        private const string AccessDeniedUrl = "~/Utility/AccessDenied";

        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userService = Substitute.For<IUserService>();

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _userService = null;
        }

        [Test]
        public void Manage_WhenCall_ReturnManageView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _userService.GetByLicensee(DefaultLicenseeSession.LicenseeId, Arg.Any<bool>()).Returns(new List<User>());

                // Action
                var result = controller.Manage() as ViewResult;

                // Assert
                StringAssert.Contains("Account/Manage.cshtml", result.ViewName);
            }
        }

        [Test]
        public void Manage_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var userList = new List<User>
            {
                new User { UserID = DefaultUserSession.UserId },
                new User { UserID = 2 }
            };
                _userService.GetByLicensee(DefaultLicenseeSession.LicenseeId, Arg.Any<bool>()).Returns(userList);

                // Action
                var result = (controller.Manage() as ViewResult).Model as List<User>;

                // Assert
                Assert.That(result.Count, Is.EqualTo(1));
            }
        }

        private AccountController CreateController()
        {
            return new AccountController(
            LangService,
            SessionService,
            AppSettings,
            _userService)
            {
                ControllerContext = ControllerContext
            };
        }

        private void SetUpUserRequestParameter(NameValueCollection requestParam = null)
        {
            if (requestParam == null)
            {
                requestParam = new NameValueCollection { { "UserId", "111" } };
            }

            ControllerContext.HttpContext.Request.Params.Returns(requestParam);
        }
    }
}