namespace Fanex.Athena.Excel.Base.UnitTests.Extension
{
    using Excel.Base.Extension;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HSSFCellExtensionTest
    {
        [TestCase(true)]
        [TestCase("string")]
        [TestCase(10.2)]
        [TestCase(10)]
        public void SetCellValue_ParamIsObject_SetCellValueMethodIsCalledCorrectly(object value)
        {
            // Arrange
            var cell = Substitute.For<ICell>();

            // Action
            cell.SetCellValue(value);

            // Assert
            cell.Received().SetCellValue(value);
        }
    }
}