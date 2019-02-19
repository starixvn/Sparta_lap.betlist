namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using Base.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using Report.Result.ResultPopup;
    using Service.Core.Utility.Interfaces;
    using UnitTest.Common.Extensions;

    [TestFixture]
    public partial class PPResultPopupControllerTest : BaseControllerHelperTest
    {
        [Test]
        public void Index_WhenCall_RedirectToErrorPage()
        {
            // Arrange
            const string CustId = "123";
            const string GamePlayId = "345";
            const string ExpectedResult = "aaaaaaaaaaaa";
            const string Url = "/PPResultPopup/Index";
            const string Language = "vi-VN";
            StubUserSessionData(new Session.Model.UserSession
            {
                Language = Language
            });
            var betListApiClient = Substitute.For<IBetListApiClient>();

            betListApiClient.PostAsyncString(Url, Arg.Any<object>()).Returns(ExpectedResult);
            using (var controller = new PPResultPopupController(LangService, SessionService, AppSettings, betListApiClient))
            {
                // Action
                var result = controller.Index(CustId, GamePlayId).Result;

                // Assert
                Assert.Multiple(() =>
                {
                    betListApiClient.Received(1).PostAsyncString(
                        Url, 
                        Arg.Is<object>(
                            x => (string)x.GetValue("custId") == CustId && 
                                (string)x.GetValue("gamePlayId") == GamePlayId &&
                                (string)x.GetValue("languageCode") == Language));
                    Assert.That(result, Is.EqualTo(ExpectedResult));
                });
            }
        }
    }
}