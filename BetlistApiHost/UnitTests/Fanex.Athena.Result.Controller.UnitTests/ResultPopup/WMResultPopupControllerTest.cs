namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Athena.Result.Controller.ResultPopup;
    using Fanex.Result.Core.WM;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class WMResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var transDesc = FakeHelper.Create<string>();
            var betCheck = FakeHelper.Create<string>();
            var betType = FakeHelper.Create<int>();
            var languageCode = FakeHelper.Create<string>();

            var resultApi = Substitute.For<IWMResultBuilder>();

            using (var controller = new WMResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(transDesc, betCheck, betType, languageCode);

                // Assert
                resultApi.Received().Render(transDesc, betCheck, betType, languageCode);
            }
        }
    }
}