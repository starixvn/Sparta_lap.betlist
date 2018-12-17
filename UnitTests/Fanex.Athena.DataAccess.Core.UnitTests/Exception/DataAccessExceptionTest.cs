namespace Fanex.Athena.DataAccess.Core.UnitTests.Exception
{
    using Fanex.Athena.DataAccess.Core.Exception;
    using NUnit.Framework;

    [TestFixture]
    public class DataAccessExceptionTest
    {
        [Test]
        public void JsonParam_WhenCall_ReturnCorrectly()
        {
            // Arrange
            const string ExpectedJson = "{\"Id\":1,\"Name\":\"A\"}";

            // Action
            var exception = new DataAccessException(
                new System.Exception(),
                string.Empty,
                new
                {
                    Id = 1,
                    Name = "A"
                });

            // Assert
            Assert.That(exception.JsonParam, Is.EqualTo(ExpectedJson));
        }
    }
}