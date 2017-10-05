namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Models;

    public partial class AuthorizationControllerTest
    {
        [Test]
        public void RecheckIn_IsUMTime_ReturnUMResult()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(true);
            _maintenanceService.GetUnderMaintenanceTime().Returns(new UnderMaintenanceTime
            {
                From = "Today",
                To = "NextWeek"
            });

            // Action
            var result = _controller.RecheckIn() as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.UnderMaintenance));
        }

        [Test]
        public void RecheckIn_NotAuthenticated_ReturnAccessDeniedResult()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);

            _controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(false);

            // Action
            var result = _controller.RecheckIn() as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.AccessDenied));
        }

        [Test]
        public void RecheckIn_OnIdNotSameSessionOnId_ReturnSessionOverrideResult()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);

            _controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
            _controller.ControllerContext.HttpContext.Request["onid"].Returns("123");

            // Action
            var result = _controller.RecheckIn() as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.SessionOverride));
        }

        [Test]
        public void RecheckIn_Kickout_SignOut()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);
            _userService.RecheckIn(DefaultUserSession.OnUserId).Returns(true);

            _controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
            _controller.ControllerContext.HttpContext.Request["onid"].Returns(DefaultUserSession.OnUserId.ToString());

            // Action
            var result = _controller.RecheckIn() as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            _formAuthenticationService.Received().SignOut();
        }

        [Test]
        public void RecheckIn_ReCheckInSuccess_ReturnSuccessResult()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);
            _userService.RecheckIn(DefaultUserSession.OnUserId).Returns(false);

            _controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
            _controller.ControllerContext.HttpContext.Request["onid"].Returns(DefaultUserSession.OnUserId.ToString());

            // Action
            var result = _controller.RecheckIn() as JsonResult;
            var data = result.Data as dynamic;

            // Assert
            Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.Success));
        }
    }
}