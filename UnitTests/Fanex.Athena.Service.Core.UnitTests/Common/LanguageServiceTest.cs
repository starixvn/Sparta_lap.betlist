namespace Fanex.Athena.Service.Core.UnitTests.Common
{
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

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _languageService = new LanguageService(_sessionService);
        }

        [Test]
        public void GetCurrentLanguage_WhenCall_ReturnLanguageFromUserSession()
        {
            // Arrange
            const string expectedLanguage = "vi-VN";
            _sessionService.UserSession.Returns(new UserSession
            {
                Language = expectedLanguage
            });

            // Action
            var result = _languageService.GetCurrentLanguage();

            // Assert
            Assert.That(result, Is.EqualTo(expectedLanguage));
        }

        [Test]
        public void ChangeLanguage_WhenCall_CallAndPassCorrectParamToSessionService()
        {
            // Arrange
            const string languageCode = "vi-VN";

            // Action
            var result = _languageService.ChangeLanguage(languageCode);

            // Assert
            _sessionService.Received().UpdateLanguage(languageCode);
        }
    }
}