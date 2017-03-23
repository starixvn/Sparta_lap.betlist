namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Models;

    [TestFixture]
    public partial class AuthorizationControllerTest : BaseControllerTest
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

            using (var controller = CreateController())
            {
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
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);

            using (var controller = CreateController())
            {
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
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);

            using (var controller = CreateController())
            {
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
        public void RecheckIn_Kickout_SignOutAndReturnKickedOut()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);
            _userService.RecheckIn(SessionOnUserId).Returns(true);
            using (var controller = CreateController())
            {
                controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
                controller.ControllerContext.HttpContext.Request["onid"].Returns(SessionOnUserId.ToString());

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                _formAuthenticationService.Received().SignOut();
                Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.KickedOut));
            }
        }

        [Test]
        public void RecheckIn_ReCheckInSuccess_ReturnSuccessResult()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(false);
            _userService.RecheckIn(SessionOnUserId).Returns(false);
            using (var controller = CreateController())
            {
                controller.ControllerContext.HttpContext.Request.IsAuthenticated.Returns(true);
                controller.ControllerContext.HttpContext.Request["onid"].Returns(SessionOnUserId.ToString());

                // Action
                var result = controller.RecheckIn() as JsonResult;
                var data = result.Data as dynamic;

                // Assert
                Assert.That(data.ErrCode, Is.EqualTo((int)ErrorType.Success));
            }
        }
    }
}