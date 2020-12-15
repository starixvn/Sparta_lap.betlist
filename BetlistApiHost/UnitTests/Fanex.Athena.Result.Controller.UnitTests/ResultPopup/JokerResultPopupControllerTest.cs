namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Athena.Result.Controller.ResultPopup;
    using Fanex.Result.Core.Joker;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class JokerResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var custId = FakeHelper.Create<string>();
            var gamePlayId = FakeHelper.Create<string>();
            var gameCode = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();

            var resultApi = Substitute.For<IJokerResult>();

            using (var controller = new JokerResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(custId, gamePlayId, gameCode, languageCode);

                // Assert
                resultApi.Received().GetJokerResult(custId, gamePlayId, gameCode, languageCode);
            }
        }
    }
}
