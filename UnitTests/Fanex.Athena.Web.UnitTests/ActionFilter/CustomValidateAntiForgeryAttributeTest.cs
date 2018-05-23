using System.Collections.Specialized;
using System.Web.Mvc;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.Web.ActionFilter;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Web.UnitTests.ActionFilter
{
    public class CustomValidateAntiForgeryAttributeTest
    {
        private CustomValidateAntiForgeryAttribute _attribute;

        [SetUp]
        public void Setup()
        {
            _attribute = new CustomValidateAntiForgeryAttribute();
        }

        [Test]
        public void OnAuthorization_AjaxClientTokenNull_ThrowException()
        {
            // Arrange
            var header = new NameValueCollection();
            header["X-Requested-With"] = "XMLHttpRequest";
            var controllerContext = MvcHelper.CreateControllerContext(header);
            var filterContext = new AuthorizationContext(controllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            // Action
            TestDelegate testDelegate = () => _attribute.OnAuthorization(filterContext);

            // Assert
            Assert.That(
                testDelegate,
                Throws.TypeOf<HttpAntiForgeryException>().With.Message.EqualTo($"Header/Form does not contain __RequestVerificationToken"));
        }

        [Test]
        public void OnAuthorization_ClientTokenNull_ThrowException()
        {
            // Arrange
            var controllerContext = MvcHelper.CreateControllerContext();
            controllerContext.RequestContext.HttpContext.Request.Form.Returns(new NameValueCollection());
            var filterContext = new AuthorizationContext(controllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            // Action
            TestDelegate testDelegate = () => _attribute.OnAuthorization(filterContext);

            // Assert
            Assert.That(
                testDelegate,
                Throws.TypeOf<HttpAntiForgeryException>().With.Message.EqualTo($"Header/Form does not contain __RequestVerificationToken"));
        }

        [Test]
        public void OnAuthorization_ServerTokenCookieNull_ThrowException()
        {
            // Arrange
            var formData = new NameValueCollection();
            formData["__RequestVerificationToken"] = "abc";
            var controllerContext = MvcHelper.CreateControllerContext();
            controllerContext.RequestContext.HttpContext.Request.Form.Returns(formData);
            var filterContext = new AuthorizationContext(controllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            // Action
            TestDelegate testDelegate = () => _attribute.OnAuthorization(filterContext);

            // Assert
            Assert.That(
                testDelegate,
                Throws.TypeOf<HttpAntiForgeryException>().With.Message.EqualTo($"Cookies does not contain __RequestVerificationToken"));
        }
    }
}