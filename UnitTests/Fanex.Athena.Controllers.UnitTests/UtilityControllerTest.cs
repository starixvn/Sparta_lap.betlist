using System.Web.Mvc;
using Fanex.Athena.Resources;
using NUnit.Framework;
using NSubstitute;
using Fanex.Athena.Service.Core.Users.Interfaces;

namespace Fanex.Athena.Controllers.UnitTests
{
    public class UtilityControllerTest : BaseControllerTest
    {
        [TestCase("")]
        [TestCase(null)]
        public void OneTokenRestriction_InvalidCode_ReturnDefaultMsg(string code)
        {

            // Arrange
            FakeBaseControllerDependency();
            var controller = new UtilityController(LangService, SessionService, AppSettings);
            controller.ControllerContext = this.controllerContext;
            var expectedMsg = ViewResources.OneTokenRestrictionDefaultMsg;

            // Action
            var result = controller.OneTokenRestriction(code) as ViewResult;

            // Assert
            Assert.AreEqual(expectedMsg, result.ViewBag.Msg);
        }
    }
}
