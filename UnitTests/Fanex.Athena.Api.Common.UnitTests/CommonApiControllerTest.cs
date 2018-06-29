namespace Fanex.Athena.Api.Common.UnitTests
{
    using Base;
    using DataAccess.Core.Common.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;

    public class CommonApiControllerTest
    {
        [Test]
        public void GetMoveBetNotification_WhenCall_ReturnCorrectResult()
        {
            var moveBetService = Substitute.For<IMoveBetService>();
            var languageService = Substitute.For<ILanguageService>();
            var sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();
            var sessionService = Substitute.For<ISessionService>();

            using (var controller = new CommonApiController(moveBetService, languageService, sportTypeDataAccess, sessionService))
            {
                // Arrange
                const string Language = "en";
                const string Notification = "Test notification api";

                languageService.GetCurrentLanguage().Returns(Language);
                moveBetService.GetMoveBetNotification(Language).Returns(Notification);

                // Action
                var result = controller.GetMoveBetNotification();

                // Assert
                Assert.That(result, Is.EqualTo(Notification));
            }
        }
    }
}