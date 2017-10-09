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
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.IsUnderMaintenance().Returns(true);
                _maintenanceService.GetUnderMaintenanceTime().Returns(new UnderMaintenanceTime
                {
                    From = "Today",
                    To = "NextWeek"
                });

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.UnderMaintenance));
            }
        }

        [Test]
        public void RecheckIn_NotAuthenticated_ReturnAccessDeniedResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.IsUnderMaintenance().Returns(false);

                controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(false);

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.AccessDenied));
            }
        }

        [Test]
        public void RecheckIn_OnIdNotSameSessionOnId_ReturnSessionOverrideResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.IsUnderMaintenance().Returns(false);

                controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
                controller.ControllerContext.HttpContext.Request["onid"].Returns("123");

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.SessionOverride));
            }
        }

        [Test]
        public void RecheckIn_Kickout_SignOut()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.IsUnderMaintenance().Returns(false);
                _userService.RecheckIn(DefaultUserSession.OnUserId).Returns(true);

                controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
                controller.ControllerContext.HttpContext.Request["onid"].Returns(DefaultUserSession.OnUserId.ToString());

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                _formAuthenticationService.Received().SignOut();
            }
        }

        [Test]
        public void RecheckIn_ReCheckInSuccess_ReturnSuccessResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.IsUnderMaintenance().Returns(false);
                _userService.RecheckIn(DefaultUserSession.OnUserId).Returns(false);

                controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
                controller.ControllerContext.HttpContext.Request["onid"].Returns(DefaultUserSession.OnUserId.ToString());

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.Success));
            }
        }
    }
}