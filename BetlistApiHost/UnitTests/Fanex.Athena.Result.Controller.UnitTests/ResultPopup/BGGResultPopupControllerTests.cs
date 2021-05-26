namespace Fanex.Athena.Result.Controller.UnitTests.ResultPopup
{
    using Fanex.Athena.Result.Controller.ResultPopup;
    using Fanex.Result.Core.BG;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BGGResultPopupControllerTests
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            const string custId = "123";
            const string transId3rd = "345";
            const string currencyId = "20";
            const string siteId = "1";
            const string languageCode = "en-US";
            const string ExpectedResult = "SGSGSGSGS";

            var resultApi = Substitute.For<IBigGamingResult>();

            resultApi.GetResult(custId, transId3rd, currencyId, siteId, languageCode).Returns(ExpectedResult);
            using (var controller = new BGGResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(custId, transId3rd, currencyId, siteId, languageCode);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedResult));
            }
        }
    }
}
