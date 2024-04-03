using Fanex.Athena.Result.Controller.ResultPopup;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Result.Core.VGaming;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup
{
    [TestFixture]
    public class VGamingResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var custId = FakeHelper.Create<string>();
            var gameRoundId = FakeHelper.Create<string>();
            var gameId = FakeHelper.Create<string>();
            var siteId = FakeHelper.Create<string>();
            var sportType = FakeHelper.Create<int>();
            var currencyName = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();
            var result3RdApi = Substitute.For<IVGamingResult>();

            using (var controller = new VGamingResultPopupController(result3RdApi))
            {
                // Action
                controller.Index(custId, gameRoundId, gameId, siteId, sportType, currencyName, languageCode);

                // Assert
                result3RdApi.Received()
                            .GetResult(custId, gameRoundId, gameId, siteId, sportType, currencyName, languageCode);
            }
        }
    }
}
