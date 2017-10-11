namespace Fanex.Athena.Excel.Base.UnitTests
{
    using NUnit.Framework;

    [TestFixture]
    public class ExcelExceptionTest
    {
        [Test]
        public void Message_WhenGetProperty_ReturnCorrectFormat()
        {
            // Arrang
            const string Message = "This is a message";
            const string ExcelFile = "TestFile.xls";
            const string Expected = "Exception when export excel TestFile.xls. Exception Message: This is a message";

            var excelException = new ExcelException(Message, ExcelFile);

            // Action
            var result = excelException.Message;

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }
    }
}