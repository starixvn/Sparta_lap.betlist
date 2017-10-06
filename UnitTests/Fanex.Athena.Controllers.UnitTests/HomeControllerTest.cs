namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class HomeControllerTest : BaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            StubGetValueAppSettings("IsEnabledMoveBetNotification", "0");
        }

        [SetUp]
        public void SetUp()
        {
            SetUpDefaultFakeData();
        }

        [Test]
        public void Index_InvalidSessionUser_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(new Model.Core.DTO.Users.UserSession
                {
                    UserId = 0
                });

                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void Index_UserDoesNotExist_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.UserSession.Returns(x => DefaultUserSession, x => null);

                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void Index_InvalidSessionUserName_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(new Model.Core.DTO.Users.UserSession
                {
                    UserId = 1,
                    UserName = string.Empty
                });

                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void Index_InvalidLicenseeSession_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubLicenseeSessionData(null);

                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void Index_IsNotRequestBeingRedirected_ReturnIndexView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/Index.cshtml"));
            }
        }

        [Test]
        public void Index_ValidSessionUser_ReturnIndexView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/Index.cshtml"));
            }
        }

        [TestCase(true, "1")]
        [TestCase(false, "0")]
        public void Index_AssignViewBagData_AssignCorrectIsInternalData(bool isInternal, string expectedValue)
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

                AppSettings.LocalTest.Returns(true);

                StubUserSessionData(new Model.Core.DTO.Users.UserSession
                {
                    UserId = 0,
                    IsInternal = isInternal
                });

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.IsInternal, Is.EqualTo(expectedValue));
            }
        }

        [TestCase(true, "0")]
        [TestCase(false, "1")]
        public void Index_AssignViewBagData_AssignCorrectHasRecheckInData(bool localTest, string expectedValue)
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

                AppSettings.LocalTest.Returns(localTest);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewBag.HasRecheckIn, Is.EqualTo(expectedValue));
            }
        }

        [Test]
        public void IndexIpad_InvalidSessionUser_RedirectToLocationRestriction()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(new Model.Core.DTO.Users.UserSession
                {
                    UserId = 0
                });

                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.IndexIpad() as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                });
            }
        }

        [Test]
        public void IndexIpad_ValidSessionUser_ReturnIndexIpadView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

                // Action
                var result = controller.IndexIpad() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/IndexIpad.cshtml"));
            }
        }

        [Test]
        public void SwitchView_WhenCall_AssignViewTypeCorrectly()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.UserSession.ViewType = Model.Core.DataDomain.ClientViewType.None;

                // Action
                var result = controller.SwitchView(Model.Core.DataDomain.ClientViewType.Desktop) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(SessionService.UserSession.ViewType, Is.EqualTo(Model.Core.DataDomain.ClientViewType.Desktop));
                    Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
                });
            }
        }

        private HomeController CreateController()
        {
            return new HomeController(LangService, SessionService, AppSettings)
            {
                ControllerContext = ControllerContext
            };
        }
    }
}