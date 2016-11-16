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
    }
}