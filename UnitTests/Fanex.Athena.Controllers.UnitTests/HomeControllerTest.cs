namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;

    [TestFixture]
    public partial class HomeControllerTest : BaseControllerTest
    {
        private IMoveBetService _moveBetService;

        [SetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _moveBetService = Substitute.For<IMoveBetService>();
        }

        [TearDown]
        public new void FixtureTearDown()
        {
            _moveBetService = null;
        }

        [Test]
        public void Index_InvalidSessionUser_RedirectToLocationRestriction()
        {
            // Arrange
            SessionService.UserSession.UserId = 0;
            using (var controller = CreateController())
            {
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as RedirectToRouteResult;

                // Assert
                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
            }
        }

        [Test]
        public void Index_ValidSessionUser_ReturnCorrectView()
        {
            // Arrange
            _moveBetService.GetMoveBet().Returns(new MoveBet());
            using (var controller = CreateController())
            {
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Home/Index.cshtml"));
            }
        }

        private HomeController CreateController()
        {
            var controller = new HomeController(
                LangService,
                SessionService,
                AppSettings,
                _moveBetService);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}