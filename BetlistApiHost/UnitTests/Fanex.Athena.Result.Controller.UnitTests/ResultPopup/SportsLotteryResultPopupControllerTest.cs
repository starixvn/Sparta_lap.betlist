namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Athena.Result.Controller.ResultPopup;
    using Fanex.Result.Core.SportsLottery;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SportsLotteryResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var custId = FakeHelper.Create<string>();
            var transId3rd = FakeHelper.Create<string>();
            var platform = "d";
            var languageCode = FakeHelper.Create<string>();
            var isDesktop = true;

            var resultApi = Substitute.For<ISportsLotteryResult>();

            using (var controller = new SportsLotteryResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(custId, transId3rd, platform, languageCode);

                // Assert
                resultApi.Received().GetResultDetail(custId, transId3rd, isDesktop, languageCode);
            }
        }

        [Test]
        public void ResultPage_WhenCall_ReturnResultPageFromApi()
        {
            // Arrange
            var platform = "d";
            var languageCode = FakeHelper.Create<string>();
            var isDesktop = true;

            var resultApi = Substitute.For<ISportsLotteryResult>();

            using (var controller = new SportsLotteryResultPopupController(resultApi))
            {
                // Action
                var result = controller.ResultPage(platform, languageCode);

                // Assert
                resultApi.Received().GetResultPage(isDesktop, languageCode);
            }
        }
    }
}