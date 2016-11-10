using Fanex.Athena.Resources;
using NUnit.Framework;
using System.Web.Mvc;

namespace Fanex.Athena.Controllers.UnitTests
{
    public class UtilityControllerTest : BaseControllerTest
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("20 0")]
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

        [TestCase("100", "Token was expired")]
        [TestCase("200", "Licensee is not existed")]
        public void OneTokenRestriction_ValidCode_ReturnMsgByCode(string code, string msg)
        {
            // Arrange
            FakeBaseControllerDependency();
            var controller = new UtilityController(LangService, SessionService, AppSettings);
            controller.ControllerContext = this.ControllerContext;

            // Action
            var result = controller.OneTokenRestriction(code) as ViewResult;

            // Assert
            Assert.AreEqual(msg, result.ViewBag.Msg);
        }
    }
}