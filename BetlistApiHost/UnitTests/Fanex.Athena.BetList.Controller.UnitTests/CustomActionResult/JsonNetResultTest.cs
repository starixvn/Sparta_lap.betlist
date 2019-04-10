namespace Fanex.Athena.BetList.Controller.UnitTests.CustomActionResult
{
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class JsonNetResultTest
    {
        [Test]
        public void JsonNetResult_ContextIsNull_ThrowArgumentException()
        {
            using (var controller = new FakeCustomerActionResultController(null))
            {
                // Action
                TestDelegate testDelegate = () => controller.TestJsonNetResult(FakeHelper.Create<string>()).ExecuteResult(controller.ControllerContext);

                // Assert
                Assert.That(testDelegate, Throws.TypeOf<ArgumentNullException>());
            }
        }

        [Test]
        public void JsonNetResult_ContentTypeIsEmpty_UpdateContentTypeToJson()
        {
            using (var controller = new FakeCustomerActionResultController(MvcHelper.CreateControllerContext()))
            {
                // Arrange
                controller.HttpContext.Response.ContentType = string.Empty;

                // Action
                controller.TestJsonNetResult(FakeHelper.Create<string>()).ExecuteResult(controller.ControllerContext);

                // Assert
                Assert.That(controller.HttpContext.Response.ContentType, Is.EqualTo("application/json"));
            }
        }

        [Test]
        public void JsonNetResult_ContentEncodingIsNotNull_UpdateResponseContentEncoding()
        {
            using (var controller = new FakeCustomerActionResultController(MvcHelper.CreateControllerContext()))
            {
                // Arrange
                var contentEncoding = Encoding.ASCII;

                // Action
                controller.TestJsonNetResult(FakeHelper.Create<string>(), contentEncoding).ExecuteResult(controller.ControllerContext);

                // Assert
                Assert.That(controller.HttpContext.Response.ContentEncoding, Is.EqualTo(contentEncoding));
            }
        }

        [Test]
        public void JsonNetResult_WhenCall_WriteSerializeObjectToOuputStream()
        {
            using (var controller = new FakeCustomerActionResultController(MvcHelper.CreateControllerContext()))
            {
                // Arrange
                var data = FakeHelper.CreateMany<int>(2);

                // Action
                controller.TestJsonNetResult(data).ExecuteResult(controller.ControllerContext);

                // Assert
                controller.HttpContext.Response.Received(1).Write(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
        }
    }
}