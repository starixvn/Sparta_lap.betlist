namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;

    [TestFixture]
    public partial class LicenseeListControllerTest : BaseControllerTest
    {
        private ILicenseeService _licenseeService;

        [SetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _licenseeService = Substitute.For<ILicenseeService>();
        }

        [TearDown]
        public new void FixtureTearDown()
        {
            _licenseeService = null;
        }

        [Test]
        public void Index_InvalidUserSession_RedirectToSignIn()
        {
            // Arrange
            SessionService.UserSession.Returns(x => null);
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index(0) as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
            }
        }

        [Test]
        public void Index_ValidLicenseeId_UpdateSessionAndRedirectToHome()
        {
            // Arrange
            const int LicenseeId = 1;
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index(LicenseeId) as RedirectToRouteResult;

                // Assert
                SessionService.Received().UpdateLiceseeSession(LicenseeId);
                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
            }
        }

        [Test]
        public void Index_UserHasNoLicensee_RedirectToSignIn()
        {
            // Arrange
            _licenseeService.GetByUser(SessionUserId).Returns(new List<Licensee>());
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index(0) as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
            }
        }

        [Test]
        public void Index_UserHasOneLicensee_RedirectToHome()
        {
            // Arrange
            _licenseeService.GetByUser(SessionUserId).Returns(new List<Licensee>
            {
                new Licensee()
            });
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index(0) as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
            }
        }

        [Test]
        public void Index_UserHasManyLicensees_ReturnCorrectView()
        {
            // Arrange
            _licenseeService.GetByUser(SessionUserId).Returns(new List<Licensee>
            {
                new Licensee(),
                new Licensee()
            });
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index(0) as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Utility/SelectLicensee.cshtml"));
            }
        }

        private LicenseeListController CreateController()
        {
            var controller = new LicenseeListController(
                LangService,
                SessionService,
                AppSettings,
                _licenseeService);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}