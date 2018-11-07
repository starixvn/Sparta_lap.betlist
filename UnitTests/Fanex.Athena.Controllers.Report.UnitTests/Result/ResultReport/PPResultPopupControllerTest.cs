namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using Base.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using Report.Result.ResultPopup;
    using Result.Core.PP;
    using Session.Model;

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
            StubUserSessionData(new UserSession
            {
                Language = "enus"
            });
            var resultApi = Substitute.For<IPPResult>();

            resultApi.GetPPResult(CustId, GamePlayId, SessionService.UserSession.Language)
                .Returns(ExpectedResult);
            using (var controller = new PPResultPopupController(LangService, SessionService, AppSettings, resultApi))
            {
                // Action
                var result = controller.Index(CustId, GamePlayId);

                // Assert
                Assert.That(result, Is.EqualTo(ExpectedResult));
            }
        }
    }
}