using Fanex.Athena.Resources;
using NUnit.Framework;
using System.Web.Mvc;

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
            controller.ControllerContext = this.ControllerContext;
            var expectedMsg = ViewResources.OneTokenRestrictionDefaultMsg;

            // Action
            var result = controller.OneTokenRestriction(code) as ViewResult;

            // Assert
            Assert.AreEqual(expectedMsg, result.ViewBag.Msg);
        }
    }
}