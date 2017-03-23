namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Threading;
    using System.Web;
    using Core.Common;
    using Core.Common.Interfaces;
    using Core.Users.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LanguageServiceTest
    {
        private ISessionService _sessionService;
        private ILanguageService _languageService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _languageService = new LanguageService(_sessionService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _sessionService = null;
            _languageService = null;
        }

        [Test]
        public void GetCurrentLanguage_WhenCall_ReturnLanguageFromUserSession()
        {
            // Arrange
            const string ExpectedLanguage = "vi-VN";
            _sessionService.UserSession.Returns(new UserSession
            {
                Language = ExpectedLanguage
            });

            // Action
            var result = _languageService.GetCurrentLanguage();

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedLanguage));
        }

        [Test]
        public void ChangeLanguage_WhenCall_CallAndPassCorrectParamToSessionService()
        {
            // Arrange
            const string LanguageCode = "vi-VN";

            // Action
            var result = _languageService.ChangeLanguage(LanguageCode);

            // Assert
            _sessionService.Received().UpdateLanguage(LanguageCode);
        }

        [Test]
        public void GetCurrentLanguage_WhenCall_ReturnLanguageFromCookie()
        {
            // Arrange
            const string CookieName = "risk_forcast_system";
            const string ExpectedLanguage = "vi-VN";
            HttpContext.Current = GetHttpContext();
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(CookieName, ExpectedLanguage));

            _sessionService.UserSession.Returns(new UserSession
            {
                Language = string.Empty
            });

            // Action
            var result = _languageService.GetCurrentLanguage();

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedLanguage));
        }

        [Test]
        public void GetCurrentLanguage_WhenCall_ReturnDefaultLanguage()
        {
            // Arrange
            const string ExpectedLanguage = "en-US";
            HttpContext.Current = GetHttpContext();

            _sessionService.UserSession.Returns(new UserSession
            {
                Language = string.Empty
            });

            // Action
            var result = _languageService.GetCurrentLanguage();

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedLanguage));
        }

        [Test]
        public void SetPageCulture_WhenCall_LanguageCodeSetCorrectly()
        {
            // Arrange
            const string LanguageCode = "vi-VN";

            // Action
            _languageService.SetPageCulture(LanguageCode);
            var result = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            // Assert
            Assert.That(result, Is.EqualTo(LanguageCode));
        }

        [Test]
        public void ChangeLanguage_WhenCall_LanguageIsChangedAndReturnTrue()
        {
            // Arrange
            const string CookieName = "risk_forcast_system";
            const string LanguageCode = "vi-VN";
            HttpContext.Current = GetHttpContext();
            HttpContext.Current.Request.Cookies.Add(new HttpCookie(CookieName, LanguageCode));

            _sessionService.UserSession.Returns(new UserSession
            {
                Language = LanguageCode
            });
            _sessionService.UpdateLanguage(LanguageCode).Returns(true);

            // Action
            var result = _languageService.ChangeLanguage(LanguageCode);
            var currentLanguage = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            var cookiValue = HttpContext.Current.Response.Cookies[CookieName].Value;

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(currentLanguage, Is.EqualTo(LanguageCode));
            Assert.That(cookiValue, Is.EqualTo(LanguageCode));
        }

        [Test]
        public void ChangeLanguage_LanguageCodeIsEmpty_ReturnFalse()
        {
            // Arrange
            const string LanguageCode = "";

            // Action
            var result = _languageService.ChangeLanguage(LanguageCode);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        private HttpContext GetHttpContext()
        {
            return new HttpContext(
                new HttpRequest(null, "http://tempuri.org", null),
                new HttpResponse(null));
        }
    }
}