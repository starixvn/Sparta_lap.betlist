using System.Web.Mvc;
using Fanex.Athena.Web.ActionFilter;
using NUnit.Framework;

namespace Fanex.Athena.Web.UnitTests.ActionFilter
{
    public class HandleAntiForgeryErrorAttributeTest
    {
        [Test]
        public void OnException_ExceptionIsNull_CorrectFilterContextResult()
        {
            // Arrange
            var context = new ExceptionContext();
            var attribute = new HandleAntiForgeryErrorAttribute();

            // Action
            attribute.OnException(context);

            // Assert
            Assert.That(context.Result, Is.InstanceOf<EmptyResult>());
        }

        [Test]
        public void OnException_HttpAntiForgeryException_CorrectFilterContextResult()
        {
            // Arrange
            var context = new ExceptionContext
            {
                Exception = new HttpAntiForgeryException()
            };
            var attribute = new HandleAntiForgeryErrorAttribute();

            // Action
            attribute.OnException(context);
            var result = context.Result as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo("/default.aspx"));
        }
    }
}