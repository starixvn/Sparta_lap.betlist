namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Extension.Utility.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Helpers;
    using Session.Enum;
    using Session.Model;

    public class BaseControllerViewTest : BaseControllerHelperTest
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
                new HttpRequest(null, TestingRequestDomain, null),
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
        public void View_Desktop_NewUIIsAvailable_ReturnNewUIView()
        {
            // Arrange
            const string ViewPath = "Views/NewUI";
            const string ExpectedViewPath = "Views/Desktop/NewUI";

            StubUserSessionData(new UserSession { ViewType = ClientViewType.Desktop });
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

            StubUserSessionData(new UserSession { ViewType = ClientViewType.Desktop });
            _fileSystem.Exists(Arg.Any<string>()).Returns(false);

            // Action
            var result = _controller.View(ViewPath, string.Empty, null) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo(ViewPath));
        }

        [Test]
        public void View_Default_ViewIsExisting_ReturnRequestView()
        {
            // Arrange
            const string ViewPath = "Views/OldUI";

            StubUserSessionData(new UserSession { ViewType = ClientViewType.Default });
            _fileSystem.Exists(Arg.Any<string>()).Returns(false, true);

            // Action
            var result = _controller.View(ViewPath, string.Empty, null) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo(ViewPath));
        }

        [Test]
        public void View_Default_OldUIIsNotExisting_NewUIIsAvailable_ReturnLinkToNewUIView()
        {
            // Arrange
            const string ViewPath = "Views/OldUI";
            const string LinkToNewUI = "~/Views/Shared/FunctionNotAvailableOldUI.cshtml";
            const string ReturnController = "Home";
            const string ReturnAction = "Index";
            var expectedReturnPath = $"{ReturnController}/{ReturnAction}";

            StubUserSessionData(new UserSession { ViewType = ClientViewType.Default });
            _fileSystem.Exists(Arg.Any<string>()).Returns(true, false);

            var routeData = new RouteData();
            routeData.Values.Add("Controller", ReturnController);
            routeData.Values.Add("Action", ReturnAction);
            _controller.ControllerContext.RouteData = routeData;

            // Action
            var result = _controller.View(ViewPath, string.Empty, null) as ViewResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ViewName, Is.EqualTo(LinkToNewUI));
                Assert.That(result.Model.ToString(), Is.EqualTo(expectedReturnPath));
            });
        }
    }
}