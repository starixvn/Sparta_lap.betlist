namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class HomeControllerTest : BaseControllerHelperTest
    {
        private HomeController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            StubGetValueAppSettings("IsEnabledMoveBetNotification", "0");
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new HomeController(LangService, SessionService, AppSettings)
            {
                ControllerContext = ControllerContext
            };

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Index_InvalidSessionUser_RedirectToLocationRestriction()
        {
            // Arrange
            StubUserSessionData(new Model.Core.DTO.Users.UserSession
            {
                UserId = 0
            });

            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

            // Action
            var result = _controller.Index() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
            });
        }

        [Test]
        public void Index_IsNotRequestBeingRedirected_ReturnIndexView()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

            // Action
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/Index.cshtml"));
        }

        [Test]
        public void Index_ValidSessionUser_ReturnIndexView()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

            // Action
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/Index.cshtml"));
        }

        [TestCase(true, "1")]
        [TestCase(false, "0")]
        public void Index_AssignViewBagData_AssignCorrectIsInternalData(bool isInternal, string expectedValue)
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

            AppSettings.LocalTest.Returns(true);

            StubUserSessionData(new Model.Core.DTO.Users.UserSession
            {
                UserId = 0,
                IsInternal = isInternal
            });

            // Action
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result.ViewBag.IsInternal, Is.EqualTo(expectedValue));
        }

        [TestCase(true, "0")]
        [TestCase(false, "1")]
        public void Index_AssignViewBagData_AssignCorrectHasRecheckInData(bool localTest, string expectedValue)
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

            AppSettings.LocalTest.Returns(localTest);

            // Action
            var result = _controller.Index() as ViewResult;

            // Assert
            Assert.That(result.ViewBag.HasRecheckIn, Is.EqualTo(expectedValue));
        }

        [Test]
        public void IndexIpad_InvalidSessionUser_RedirectToLocationRestriction()
        {
            // Arrange
            StubUserSessionData(new Model.Core.DTO.Users.UserSession
            {
                UserId = 0
            });

            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

            // Action
            var result = _controller.IndexIpad() as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
            });
        }

        [Test]
        public void IndexIpad_ValidSessionUser_ReturnIndexIpadView()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(true);

            // Action
            var result = _controller.IndexIpad() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/IndexIpad.cshtml"));
        }

        [Test]
        public void SwitchView_WhenCall_AssignViewTypeCorrectly()
        {
            // Arrange
            SessionService.UserSession.ViewType = Model.Core.DataDomain.ClientViewType.None;

            // Action
            var result = _controller.SwitchView(Model.Core.DataDomain.ClientViewType.Desktop) as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(SessionService.UserSession.ViewType, Is.EqualTo(Model.Core.DataDomain.ClientViewType.Desktop));
                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
            });
        }
    }
}