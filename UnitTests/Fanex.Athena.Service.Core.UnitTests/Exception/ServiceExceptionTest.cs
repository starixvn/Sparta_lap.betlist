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
            const string ServiceName = "ServiceName";
            const string Message = "Test Message";

            var serviceException = new ServiceException(Message, ServiceName);
            var expectedMessage = $"Exception when call Service: {ServiceName}. Exception Message: {Message}";

            // Action
            var result = serviceException.Message;

            // Assert
            Assert.That(result, Is.EqualTo(expectedMessage));
        }
    }
}