namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.SessionState;
    using Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Users;
    using Users.Interfaces;

    [TestFixture]
    public class SessionServiceTest
    {
        private const int DefaultUserId = 1;

        private IFunctionService _functionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;
        private ISessionService _sessionService;

        [SetUp]
        public void SetUp()
        {
            _functionService = Substitute.For<IFunctionService>();
            _licenseeService = Substitute.For<ILicenseeService>();
            _currencyService = Substitute.For<ICurrencyService>();
            _sessionService = new SessionService(_functionService, _licenseeService, _currencyService);
        }

        [Test]
        public void CreateNewSession_WhenCall_UserSessionIsCreatedProperly()
        {
            // Action
            CreateDefaultNewSession();

            // Assert
            Assert.That(_sessionService.UserSession.UserId, Is.EqualTo(DefaultUserId));
        }

        [Test]
        public void CreateNewSession_WhenCall_LicenseeSessionIsCreatedProperly()
        {
            // Arrange
            var licenseeId = 1;
            var licensees = new List<Licensee>()
            {
                new Licensee() { Id = 1, Name = "spondemo" }
            };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);

            // Action
            CreateDefaultNewSession(licenseeId);

            // Assert
            Assert.That(_sessionService.LicenseeSession.LicenseeId, Is.EqualTo(licenseeId));
            Assert.That(_sessionService.LicenseeSession.LicenseeName, Is.EqualTo("spondemo"));
        }

        [Test]
        public void UpdateLiceseeSession_WhenCall_LicenseeSessionIsCreatedProperly()
        {
            // Arrange
            var licensees = new List<Licensee>()
            {
                new Licensee() { Id = 1, Name = "spondemo" },
                new Licensee() { Id = 2, Name = "12bet" },
            };
            _licenseeService.GetByUser(DefaultUserId).Returns(licensees);
            var oldLicenseeId = 1;
            var newLicenseeId = 2;
            CreateDefaultNewSession(oldLicenseeId);

            // Action
            _sessionService.UpdateLiceseeSession(newLicenseeId);

            // Assert
            Assert.That(_sessionService.LicenseeSession.LicenseeId, Is.EqualTo(newLicenseeId));
            Assert.That(_sessionService.LicenseeSession.LicenseeName, Is.EqualTo("12bet"));
        }

        [Test]
        public void UpdateLanguage()
        {
            // Arrange
            var languageCode = "vi-VN";
            CreateDefaultNewSession();

            // Action
            _sessionService.UpdateLanguage(languageCode);

            // Assert
            Assert.That(_sessionService.UserSession.Language, Is.EqualTo(languageCode));
        }

        private static void MockHttpContext()
        {
            var httpContext = new HttpContext(
                            new HttpRequest(string.Empty, "http://tempuri.org", string.Empty),
                            new HttpResponse(new StringWriter()));

            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc, 
                false);
            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            HttpContext.Current = httpContext;
        }

        private void CreateDefaultNewSession(int licenseeId = 0)
        {
            MockHttpContext();
            var userId = 1;
            var onUserId = 2;
            var userName = "user1";
            var isInternal = false;

            _sessionService.CreateNewSession(userId, onUserId, userName, isInternal, licenseeId);
        }
    }
}