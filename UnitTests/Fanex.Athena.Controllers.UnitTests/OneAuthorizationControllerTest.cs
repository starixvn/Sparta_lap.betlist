using Fanex.Athena.Service.Core.Users.Interfaces;
using NSubstitute;
using NUnit.Framework;
using ONEbook.UM.Services;

namespace Fanex.Athena.Controllers.UnitTests
{
    [TestFixture]
    public class OneAuthorizationControllerTest : BaseControllerTest
    {
        private IUserService _userService;
        private IMaintenanceService _maintenanceService;

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        [TestCase("rtXOPRoiuUy1ZwlOy96GYQ ")]
        [TestCase("rtXOPRo iuU y1ZwlOy96GYQ ")]
        public void CreateSession_InvalidToken_ReturnTokenRestrictionPage(string token)
        {
            // Arrange
            var controller = FakeController();
            var expectedUrl = "Utility/OneTokenRestriction";
            // Action
            var result = controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        private OneAuthorizationController FakeController()
        {
            _userService = Substitute.For<IUserService>();
            _maintenanceService = Substitute.For<IMaintenanceService>();

            var controller = new OneAuthorizationController(_userService, _maintenanceService);
            controller.ControllerContext = ControllerContext;

            controller.Url = this.Url;

            return controller;
        }
    }
}