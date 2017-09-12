namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Fanex.Athena.Resources;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;

    public class UtilityControllerTest : BaseControllerTest
    {
        private IMaintenanceService _maintenanceService;

        [SetUp]
        public void SetUp()
        {
            _maintenanceService = Substitute.For<IMaintenanceService>();
        }

        [TearDown]
        public void TearDown()
        {
            _maintenanceService = null;
        }

        [Test]
        public void SessionIsAlive_WhenCall_ReturnCorrectData()
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;

                // Action
                var result = controller.SessionIsAlive() as JsonResult;

                // Assert
                Assert.That(result.Data, Is.True);
            }
        }

        [Test]
        public void AccessDenied_WhenCall_ReturnCorrectViewName()
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                // Action
                var viewResult = controller.AccessDenied() as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Utility/AccessDenied.cshtml"));
            }
        }

        [TestCase(-1)]
        [TestCase(null)]
        [TestCase(20)]
        public void OneTokenRestriction_InvalidCode_ReturnDefaultMsg(int code)
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;
                var expectedMsg = ViewResources.OneTokenRestrictionDefaultMsg;

                // Action
                var result = controller.OneTokenRestriction(code) as ViewResult;

                // Assert
                Assert.AreEqual(expectedMsg, result.ViewBag.Message);
            }
        }

        [TestCase(100, "Token was expired")]
        [TestCase(200, "Not found licensee")]
        [TestCase(400, "Direct Access to this website is restricted. Please contact Administrator for further enquiries.")]
        [TestCase(600, "You do not have any currency permissions to view this report, please contact your administrator account.")]
        [TestCase(300, "Token is invalid")]
        [TestCase(700, "You are not allowed to access this page, please contact your administrator account.")]
        [TestCase(800, "Not found user")]
        public void OneTokenRestriction_ValidCode_ReturnMsgByCode(int code, string msg)
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                // Action
                var result = controller.OneTokenRestriction(code) as ViewResult;

                // Assert
                Assert.AreEqual(msg, result.ViewBag.Message);
            }
        }

        [Test]
        public void UnderMaintenance_UnderMaintenanceState_ReturnRightUMTimeInfo()
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;
                var maintenanceInfo = new UnderMaintenanceTime(true, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(Arg.Any<bool>()) as ViewResult;

                // Assert
                Assert.AreEqual(maintenanceInfo.From, result.ViewBag.UmTimeFrom);
                Assert.AreEqual(maintenanceInfo.To, result.ViewBag.UmTimeTo);
            }
        }

        [Test]
        public void UnderMaintenance_NotUnderMaintenanceState_ShowEmptyUMInfo()
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = ControllerContext;
                var maintenanceInfo = new UnderMaintenanceTime(false, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(true) as ViewResult;

                // Assert
                Assert.AreEqual(string.Empty, result.ViewBag.UmTimeFrom);
                Assert.AreEqual(string.Empty, result.ViewBag.UmTimeTo);
            }
        }

        [TestCase(null, "InvalidRequestMsg")]
        [TestCase("0", "InvalidRequestMsg")]
        [TestCase("1", "SSOLinkIncorrectMsg")]
        [TestCase("2", "DisableSSOMsg")]
        [TestCase("100", "SSOLinkExpiredMsg")]
        [TestCase("101", "WrongUserNamePassword")]
        [TestCase("102", "UserNotActive")]
        [TestCase("103", "UserTooManyLicensees")]
        [TestCase("104", "UserNoLicensee")]
        [TestCase("999", "InvalidRequestMsg")]
        public void SSODenyAccess_WhenCall_ReturnCorrectMessageAccordingtoCode(string code, string returnedMessageResourceKey)
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = ControllerContext;
                controller.ControllerContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString($"code={code}"));

                // Action
                var result = controller.SSODenyAccess() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.Message, Is.EqualTo(ViewResources.ResourceManager.GetString(returnedMessageResourceKey)));
            }
        }

        [Test]
        public void LocationRestriction_IsInternalUrl_RedirectToSignInPage()
        {
            // Arrange
            FakeBaseControllerDependency();
            AppSettings.GetValue("InternalSubDomain").Returns("internal");
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;
                controller.ControllerContext.HttpContext.Request.Url.Returns(new Uri("http://internal.com"));

                // Action
                var result = controller.LocationRestriction() as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
            }
        }

        [Test]
        public void LocationRestriction_NotInternalAndLapDomain_ReturnViewLocationRestriction()
        {
            // Arrange
            FakeBaseControllerDependency();
            AppSettings.GetValue("InternalSubDomain").Returns("internal.");
            AppSettings.GetValue("LapSubDomain").Returns("lap.");
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = this.ControllerContext;
                controller.ControllerContext.HttpContext.Request.Url.Returns(new Uri("http://abc.com"));

                // Action
                var result = controller.LocationRestriction() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/Utility/LocationRestriction.cshtml"));
            }
        }

        [TestCase(null, "HttpError_Default")]
        [TestCase("400", "InvalidRequestMsg")]
        [TestCase("403", "HttpError_403")]
        [TestCase("404", "HttpError_404")]
        [TestCase("999", "HttpError_Default")]
        public void Error_WhenCall_ReturnCorrectMessageAccordingtoCode(string code, string returnedMessageResourceKey)
        {
            // Arrange
            FakeBaseControllerDependency();
            using (var controller = new UtilityController(AppSettings, LangService, _maintenanceService))
            {
                controller.ControllerContext = ControllerContext;
                controller.ControllerContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString($"err={code}"));

                // Action
                var result = controller.Error() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.Message, Is.EqualTo(ViewResources.ResourceManager.GetString(returnedMessageResourceKey)));
            }
        }
    }
}