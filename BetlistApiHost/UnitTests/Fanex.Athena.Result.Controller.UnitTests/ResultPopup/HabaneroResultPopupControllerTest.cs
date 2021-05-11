namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Result.Core.Habanero;
    using NSubstitute;
    using NUnit.Framework;
    using ResultPopup;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class HabaneroResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var custId = FakeHelper.Create<string>();
            var transId3rd = FakeHelper.Create<string>();
            var gameId3rd = FakeHelper.Create<string>();
            var languageCode = FakeHelper.Create<string>();
            var siteId = FakeHelper.Create<string>();

            var resultApi = Substitute.For<IHabaneroResult>();

            using (var controller = new HabaneroResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(custId, transId3rd, gameId3rd, siteId, languageCode);

                // Assert
                resultApi.Received().GetResult(custId, transId3rd, gameId3rd, siteId, languageCode);
            }
        }
    }
}