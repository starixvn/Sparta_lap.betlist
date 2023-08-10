using Fanex.Athena.Result.Controller.ResultPopup;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Result.Core.SAGaming;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup
{
    [TestFixture]
    public class SAGamingResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var transDesc = FakeHelper.Create<string>();
            var matchCode = FakeHelper.Create<string>();
            var betType = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();
            var result3RdApi = Substitute.For<ISAGamingResultBuilder>();

            using (var controller = new SAGamingResultPopupController(result3RdApi))
            {
                // Action
                controller.Index(transDesc, matchCode, betType, languageCode);

                // Assert
                result3RdApi.Received().Render(transDesc, matchCode, betType, languageCode);
            }
        }
    }
}
