namespace Fanex.Athena.Excel.Base.Tests.Extension
{
    using Base.Extension;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HSSFWorkbookExtensionTest
    {
        [Test]
        public void CloneSheet_WhenCall_ReturnNewSheet()
        {
            // Arrange
            string sourceSheetName = "Source", newSheetName = "NewSheet";
            var workbook = Substitute.For<IWorkbook>();
            var sourceSheetIndex = 0;
            workbook.GetSheetIndex(sourceSheetName).Returns(sourceSheetIndex);

            var newSheet = Substitute.For<ISheet>();
            var newSheetIndex = 1;
            workbook.CloneSheet(sourceSheetIndex).Returns(newSheet);
            workbook.GetSheetIndex(newSheet).Returns(newSheetIndex);

            // Action
            var result = workbook.CloneSheet(sourceSheetName, newSheetName);

            // Assert
            Assert.That(result, Is.SameAs(newSheet));
        }

        [Test]
        public void RemoveSheet_WhenCall_RemoveSheetAtMethodIsCalled()
        {
            // Arrange
            var sheetName = "Sheet";
            var workbook = Substitute.For<IWorkbook>();
            var sheet = Substitute.For<ISheet>();
            workbook.GetSheet(sheetName).Returns(sheet);

            var sheetIndex = 0;
            workbook.GetSheetIndex(sheet).Returns(sheetIndex);

            // Action
            workbook.RemoveSheet(sheetName);

            // Assert
            workbook.Received().RemoveSheetAt(sheetIndex);
        }
    }
}