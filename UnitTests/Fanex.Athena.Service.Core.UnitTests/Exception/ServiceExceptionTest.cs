namespace Fanex.Athena.Service.Core.UnitTests.Exception
{
    using Core.Exception;
    using NUnit.Framework;

    [TestFixture]
    public class ServiceExceptionTest
    {
        [Test]
        public void Message_WhenCall_ReturnCorrectValue()
        {
            // Arrange
            string serviceName = "ServiceName", message = "Test Message";
            var serviceException = new ServiceException(message, serviceName);
            var expectedMessage = $"Exception when call Service: {serviceName}. Exception Message: {message}";

            // Action
            var result = serviceException.Message;

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }
    }
}