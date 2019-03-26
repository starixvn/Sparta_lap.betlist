namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class CultureBaseControllerTest : CultureBaseControllerHelperTest
    {
        private FakeCultureBaseController _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new FakeCultureBaseController(LangService, SessionService, AppSettings);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Initialize_WhenCall_SetPageCulture()
        {
            // Arrange
            const string Language = "en";
            LangService.GetCurrentLanguage().Returns(Language);
            var controllerContext = MvcHelper.CreateControllerContext();
            var url = MvcHelper.CreateUrlHelper(controllerContext.HttpContext);

            // Action
            _controller.Initialize(url.RequestContext);

            // Assert
            LangService.Received().SetPageCulture(Language);
        }
    }
}