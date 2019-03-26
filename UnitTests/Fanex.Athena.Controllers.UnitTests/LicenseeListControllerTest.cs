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
    public class LicenseeListControllerTest : CultureBaseControllerHelperTest
    {
        private ILicenseeDataAccess _licenseeDataAccess;

        [SetUp]
        public void SetUp()
        {
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();

            SetUpDefaultFakeData();
        }

        [TearDown]
        public void TearDown()
        {
            _licenseeDataAccess = null;
        }

        [Test]
        public void Index_InvalidUserSession_RedirectToSignIn()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubUserSessionData(null);

                // Action
                var result = controller.Index(0) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
                });
            }
        }

        [Test]
        public void Index_ValidLicenseeId_RedirectToHome()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int LicenseeId = 1;

                // Action
                var result = controller.Index(LicenseeId) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    SessionService.Received().UpdateLiceseeSession(LicenseeId);
                    Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
                });
            }
        }

        [Test]
        public void Index_UserHasNoLicensee_RedirectToSignIn()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubLicenseeDataAccessGetByUser(DefaultUserSession.UserId, new List<Licensee>());

                // Action
                var result = controller.Index(0) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("SignIn"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Authorization"));
                });
            }
        }

        [Test]
        public void Index_UserHasOneLicensee_RedirectToHome()
        {
            using (var controller = CreateController())
            {
                // Arrange
                StubLicenseeDataAccessGetByUser(DefaultUserSession.UserId, new List<Licensee> { new Licensee() });

                // Action
                var result = controller.Index(null) as RedirectToRouteResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
                    Assert.That(result.RouteValues["controller"], Is.EqualTo("Home"));
                });
            }
        }

        [Test]
        public void Index_UserHasManyLicensees_ReturnIndexView()
        {
            using (var controller = CreateController())
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
                var result = controller.Index(0) as ViewResult;

                // Assert
                StringAssert.Contains("LicenseeList/Index.cshtml", result.ViewName);
            }
        }

        private LicenseeListController CreateController()
        {
            return new LicenseeListController(LangService, SessionService, AppSettings, _licenseeDataAccess);
        }

        private void StubLicenseeDataAccessGetByUser(int userId, List<Licensee> fakeResult)
        {
            _licenseeDataAccess.GetByUser(userId).Returns(fakeResult);
        }
    }
}