namespace Fanex.Athena.Excel.Base.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ExcelExceptionTest
    {
        [Test]
        public void Message_WhenGetProperty_ReturnCorrectFormat()
        {
            // Arrang
            string message = "This is a message", excelFile = "TestFile.xls";
            var excelException = new ExcelException(message, excelFile);
            var expected = "Exception when export excel TestFile.xls. Exception Message: This is a message";

            // Action
            var result = excelException.Message;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}