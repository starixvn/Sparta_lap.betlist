using Fanex.Athena.Result.Controller.ResultPopup;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Result.Core.SabaClub;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup
{
    [TestFixture]
    public class SabaClubResultPopupControllerTest
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
            var languageCode = FakeHelper.Create<string>();
            var result3RdApi = Substitute.For<ISabaClubResult>();

            using (var controller = new SabaClubResultPopupController(result3RdApi))
            {
                // Action
                controller.Index(custId, gameRoundId, gameId, siteId, sportType, languageCode);

                // Assert
                result3RdApi.Received().GetResult(custId, gameRoundId, gameId, siteId, sportType, languageCode);
            }
        }
    }
}