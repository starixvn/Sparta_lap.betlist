namespace Fanex.Athena.Controllers.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public partial class MenuControllerTest : BaseControllerTest
    {
        private IFunctionService _functionService;

        [SetUp]
        public new void FixtureSetUp()
        {
            FakeBaseControllerDependency();
            _functionService = Substitute.For<IFunctionService>();
        }

        [TearDown]
        public new void FixtureTearDown()
        {
            _functionService = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectView()
        {
            // Arrange
            SessionService.UserSession.FunctionIdList = new List<int> { 1, 2, 3 };
            _functionService.Functions.Returns(new List<Function>());
            using (var controller = CreateController())
            {
                controller.ControllerContext.HttpContext.Response.IsRequestBeingRedirected.Returns(false);

                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Menu/Index.cshtml"));
            }
        }

        private MenuController CreateController()
        {
            var controller = new MenuController(
                LangService,
                SessionService,
                AppSettings,
                _functionService);

            controller.ControllerContext = this.ControllerContext;

            return controller;
        }
    }
}