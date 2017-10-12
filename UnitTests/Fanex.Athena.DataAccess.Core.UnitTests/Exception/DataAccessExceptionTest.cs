using Fanex.Athena.DataAccess.Core.Exception;
using NUnit.Framework;

namespace Fanex.Athena.DataAccess.Core.UnitTests.Exception
{
    [TestFixture]
    public class DataAccessExceptionTest
    {
        [Test]
        public void Message_WhenCall_ReturnCorrectly()
        {
            // Arrange
            const string InputMessage = "Hello";
            const string UspName = "SP_Hello";
            const string ExpectedMessage = "Exception when call usp SP_Hello. \nException: Hello";

            // Action
            var exception = new DataAccessException(InputMessage, UspName);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(ExpectedMessage));
        }

        [Test]
        public void JsonParam_WhenCall_ReturnCorrectly()
        {
            // Arrange
            const string ExpectedJson = "{\"Id\":1,\"Name\":\"A\"}";
            var exception = new DataAccessException(
                string.Empty,
                string.Empty,
                new
                {
                    Id = 1,
                    Name = "A"
                });

            // Action

            // Assert
            Assert.That(exception.JsonParam, Is.EqualTo(ExpectedJson));
        }
    }
}