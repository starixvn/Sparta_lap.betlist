using System.Collections.Specialized;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.Web.ActionFilter;
using NUnit.Framework;

namespace Fanex.Athena.Web.UnitTests.ActionFilter
{
    public class AjaxRequestAttributeTest
    {
        [TestCase(true)]
        [TestCase(false)]
        public void IsValidForRequest_WhenCall_ReturnCorrect(bool isAjax)
        {
            // Arrange
            var header = new NameValueCollection();
            header["X-Requested-With"] = "XMLHttpRequest";
            var controllerContext = MvcHelper.CreateControllerContext(isAjax ? header : null);
            var attribute = new AjaxRequestAttribute();

            // Action
            var result = attribute.IsValidForRequest(controllerContext, null);

            // Assert
            Assert.That(result, Is.EqualTo(isAjax));
        }
    }
}