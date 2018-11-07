namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System.Web;
    using System.Web.Mvc;
    using Extension.Utility.Interfaces;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Helpers;
    using UnitTest.Common.Helpers;

    public class BaseControllerTest_View : BaseControllerHelperTest
    {
        private FakeBaseController _controller;

        private ClientViewHelper _clientViewHelper;

        private IFileSystem _fileSystem;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _fileSystem = Substitute.For<IFileSystem>();
            _clientViewHelper = new ClientViewHelper(_fileSystem);

            var fakeHttpContext = new HttpContext(
                new HttpRequest(null, MvcHelper.TestingRequestDomain, null),
                new HttpResponse(null));

            HttpContext.Current = fakeHttpContext;
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _fileSystem = null;
            _clientViewHelper = null;
            base.FixtureTearDown();
        }

        [SetUp]
        public void SetUp()
        {
            _controller = new FakeBaseController(LangService, SessionService, AppSettings, _clientViewHelper)
            {
                ControllerContext = ControllerContext
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void View_NewUIIsAvailable_ReturnNewUIView()
        {
            // Arrange
            const string ViewPath = "Views/NewUI";
            const string ExpectedViewPath = "Views/Desktop/NewUI";

            _fileSystem.Exists(Arg.Any<string>()).Returns(true);

            // Action
            var result = _controller.View(ViewPath, string.Empty, null) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo(ExpectedViewPath));
        }

        [Test]
        public void View_Desktop_NewUIIsNotAvailable_ReturnRequestView()
        {
            // Arrange
            const string ViewPath = "Views/UI";

            _fileSystem.Exists(Arg.Any<string>()).Returns(false);

            // Action
            var result = _controller.View(ViewPath, string.Empty, null) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo(ViewPath));
        }
    }
}