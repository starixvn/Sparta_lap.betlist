namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Account;
    using Base.UnitTests;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public class AccountControllerTest : BaseControllerTest
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
            _controller = new AccountController(
                LangService, 
                SessionService, 
                AppSettings, 
                _functionService, 
                _userService, 
                _currencyService);
        }

        [OneTimeTearDown]
        public new void FixtureTearDown()
        {
            _userService = null;
            if (_controller != null)
            {
                _controller.Dispose();
                _controller = null;
            }
        }

        [Test]
        public void Manage_WhenCall_Return()
        {
            // Arrange
            const int LicenseeId = 1;
            var userList = new List<User>
            {
                new User()
            };
            _userService.GetByLicensee(LicenseeId, Arg.Any<bool>()).Returns(userList);

            // Action
            var result = _controller.Manage() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Account/Manage.cshtml"));
        }
    }
}