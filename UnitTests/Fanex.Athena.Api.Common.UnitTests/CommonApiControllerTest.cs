namespace Fanex.Athena.Api.Common.UnitTests
{
    using Base;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;

    public class CommonApiControllerTest
    {
        [Test]
        public void GetMoveBetNotification_WhenCall_ReturnCorrectResult()
        {
            var moveBetService = Substitute.For<IMoveBetService>();
            var languageService = Substitute.For<ILanguageService>();

            using (var controller = new CommonApiController(moveBetService, languageService))
            {
                // Arrange
                const string Language = "en";
                const string Notification = "Test notification api";

                languageService.GetCurrentLanguage().Returns(Language);
                moveBetService.GetMoveBetNotificationAsync(Language).Returns(Notification);

                // Action
                var result = controller.GetMoveBetNotification();

                // Assert
                Assert.That(result, Is.EqualTo(Notification));
            }
        }
    }
}