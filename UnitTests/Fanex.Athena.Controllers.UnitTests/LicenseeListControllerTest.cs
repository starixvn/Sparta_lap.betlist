namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeListControllerTest : BaseControllerHelperTest
    {
        private ILicenseeDataAccess _licenseeDataAccess;

        private LicenseeListController _controller;

        [SetUp]
        public void SetUp()
        {
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();

            SetUpDefaultFakeData();

            _controller = new LicenseeListController(LangService, SessionService, AppSettings, _licenseeDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _licenseeDataAccess = null;

            _controller.Dispose();
        }

        [Test]
        public void Index_InvalidUserSession_RedirectToSignIn()
        {
            // Arrange
            StubUserSessionData(null);

            // Action
            var result = _controller.Index(0) as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
            });
        }

        [Test]
        public void Index_ValidLicenseeId_RedirectToHome()
        {
            // Arrange
            const int LicenseeId = 1;

            // Action
            var result = _controller.Index(LicenseeId) as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                SessionService.Received().UpdateLiceseeSession(LicenseeId);
                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
            });
        }

        [Test]
        public void Index_UserHasNoLicensee_RedirectToSignIn()
        {
            // Arrange
            StubLicenseeDataAccessGetByUser(DefaultUserSession.UserId, new List<Licensee>());

            // Action
            var result = _controller.Index(0) as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
            });
        }

        [Test]
        public void Index_UserHasOneLicensee_RedirectToHome()
        {
            // Arrange
            StubLicenseeDataAccessGetByUser(DefaultUserSession.UserId, new List<Licensee> { new Licensee() });

            // Action
            var result = _controller.Index(null) as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
            });
        }

        [Test]
        public void Index_UserHasManyLicensees_ReturnIndexView()
        {
            // Arrange
            StubLicenseeDataAccessGetByUser(
                DefaultUserSession.UserId,
                new List<Licensee>
                {
                    new Licensee(),
                    new Licensee()
                });

            // Action
            var result = _controller.Index(0) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("~/Views/LicenseeList/Index.cshtml"));
        }

        private void StubLicenseeDataAccessGetByUser(int userId, List<Licensee> fakeResult)
        {
            _licenseeDataAccess.GetByUser(userId).Returns(fakeResult);
        }
    }
}