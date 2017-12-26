namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Base.UnitTests;
    using Fanex.Athena.Resources;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;

    public class UtilityControllerTest : BaseControllerHelperTest
    {
        private IMaintenanceService _maintenanceService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();

            StubGetValueAppSettings("InternalSubDomain", "internal.");
            StubGetValueAppSettings("LapSubDomain", "lap.");
        }

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
            using (var controller = CreateController())
            {
                // Action
                var result = controller.SessionIsAlive() as JsonResult;

                // Assert
                Assert.That(result.Data, Is.EqualTo(controller.HttpContext.User.Identity.IsAuthenticated));
            }
        }

        [Test]
        public void AccessDenied_WhenCall_ReturnCorrectViewName()
        {
            using (var controller = CreateController())
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
        [TestCase(500)]
        public void OneTokenRestriction_InvalidCode_ReturnDefaultMsg(int code)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var expectedMsg = ViewResources.OneTokenRestrictionDefaultMsg;

                // Action
                var result = controller.OneTokenRestriction(code) as ViewResult;

                // Assert
                Assert.That(result.ViewBag.Message, Is.EqualTo(expectedMsg));
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
            using (var controller = CreateController())
            {
                // Action
                var result = controller.OneTokenRestriction(code) as ViewResult;

                // Assert
                Assert.That(result.ViewBag.Message, Is.EqualTo(msg));
            }
        }

        [Test]
        public void UnderMaintenance_UnderMaintenanceState_ReturnRightUMTimeInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var maintenanceInfo = new UnderMaintenanceTime(true, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(true) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.UmTimeFrom, Is.EqualTo(maintenanceInfo.From));
                    Assert.That(result.ViewBag.UmTimeTo, Is.EqualTo(maintenanceInfo.To));
                });
            }
        }

        [Test]
        public void UnderMaintenance_UnderMaintenanceStateAndMaintenanceTimeIsEmpty_ReturnUMTimeFromQueryString()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.GetUnderMaintenanceTime().Returns(new UnderMaintenanceTime(true, string.Empty, string.Empty));

                const string QueryFrom = "01-01-2016 05:00:00";
                const string QueryTo = "01-15-2016 05:00:00";
                controller.ControllerContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString($"from={QueryFrom}&to={QueryTo}"));

                // Action
                var result = controller.UnderMaintenance(true) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.UmTimeFrom, Is.EqualTo(QueryFrom));
                    Assert.That(result.ViewBag.UmTimeTo, Is.EqualTo(QueryTo));
                });
            }
        }

        [Test]
        public void UnderMaintenance_UnderMaintenanceStateWithMaintenanceTimeAndQueryStringIsNull_ReturnEmptyUMInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _maintenanceService.GetUnderMaintenanceTime().Returns(new UnderMaintenanceTime(true, null, null));

                controller.ControllerContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString(string.Empty));

                // Action
                var result = controller.UnderMaintenance(true) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.UmTimeFrom, Is.EqualTo(string.Empty));
                    Assert.That(result.ViewBag.UmTimeTo, Is.EqualTo(string.Empty));
                });
            }
        }

        [Test]
        public void UnderMaintenance_NotUnderMaintenanceState_ShowEmptyUMInfo()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var maintenanceInfo = new UnderMaintenanceTime(false, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(true) as ViewResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewBag.UmTimeFrom, Is.Empty);
                    Assert.That(result.ViewBag.UmTimeTo, Is.Empty);
                });
            }
        }

        [Test]
        public void UnderMaintenance_NotMaintenanceStateAndReload_RedirectToLogin()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var maintenanceInfo = new UnderMaintenanceTime(false, "11-14-2016 05:00:00", "11-14-2016 07:00:00");
                _maintenanceService.GetUnderMaintenanceTime().Returns(x => maintenanceInfo);

                // Action
                var result = controller.UnderMaintenance(false) as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo(FormsAuthentication.LoginUrl));
            }
        }

        [TestCase(null, "InvalidRequestMsg")]
        [TestCase("Invalid", "InvalidRequestMsg")]
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
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString(code == null ? string.Empty : $"code={code}"));

                // Action
                var result = controller.SSODenyAccess() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.Message, Is.EqualTo(ViewResources.ResourceManager.GetString(returnedMessageResourceKey)));
            }
        }

        [Test]
        public void LocationRestriction_IsInternalUrl_RedirectToSignInPage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Request.Url.Returns(new Uri("http://internal.com"));

                // Action
                var result = controller.LocationRestriction() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
                });
            }
        }

        [Test]
        public void LocationRestriction_IsLapDomain_RedirectToRestrictionPage()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Request.Url.Returns(new Uri("http://lap.com"));

                // Action
                var result = controller.LocationRestriction() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("OneTokenRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                    Assert.That(result.RouteValues["code"], Is.EqualTo(100));
                });
            }
        }

        [Test]
        public void LocationRestriction_NotInternalAndLapDomain_ReturnViewLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
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
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Request.QueryString.Returns(HttpUtility.ParseQueryString($"err={code}"));

                // Action
                var result = controller.Error() as ViewResult;

                // Assert
                Assert.That(result.Model, Is.EqualTo(ViewResources.ResourceManager.GetString(returnedMessageResourceKey)));
            }
        }

        private UtilityController CreateController()
        {
            return new UtilityController(AppSettings, LangService, _maintenanceService)
            {
                ControllerContext = ControllerContext
            };
        }
    }
}