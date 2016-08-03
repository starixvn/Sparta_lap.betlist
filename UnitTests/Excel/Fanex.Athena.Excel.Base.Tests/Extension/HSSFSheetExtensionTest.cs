namespace Fanex.Athena.Excel.Base.Tests.Extension
{
    using Excel.Base.Extension;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HSSFSheetExtensionTest
    {
        [TestCase(0, 2)]
        [TestCase(2, 0)]
        public void CopyRow(int sourceRowIndex, int destRowIndex)
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var sourceSheet = Substitute.For<ISheet>();

            // Action
            var row = sheet.CopyRow(sourceSheet, sourceRowIndex, destRowIndex);

            // Assert
            Assert.That(row, Is.Not.Null);
        }

        [TestCase(CellType.Blank)]
        [TestCase(CellType.Boolean)]
        [TestCase(CellType.Error)]
        [TestCase(CellType.Formula)]
        [TestCase(CellType.Numeric)]
        [TestCase(CellType.String)]
        [TestCase(CellType.Unknown)]
        public void CopyCell_SourceCellIsNotNull_CellCopied(CellType cellType)
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var source = Substitute.For<ICell>();
            source.SetCellType(cellType);
            var destination = Substitute.For<ICell>();

            // Action
            var cell = sheet.CopyCell(source, destination);

            // Assert
            Assert.That(cell, Is.Not.Null);
            Assert.That(cell.CellType, Is.EqualTo(source.CellType));
        }

        [Test]
        public void CopyCell_SourceCellIsNull_ReturnNull()
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var destination = Substitute.For<ICell>();

            // Action
            var cell = sheet.CopyCell(null, destination);

            // Assert
            Assert.That(cell, Is.Null);
        }

        [TestCase(1, 0)]
        [TestCase(1, 5)]
        [TestCase(1, 9)]
        public void GetCell_WhenCall_ReturnCorrectCellByIndex(int rowIndex, int cellIndex)
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var row = Substitute.For<IRow>();
            var cell = Substitute.For<ICell>();
            sheet.GetRow(rowIndex).Returns(row);
            row.GetCell(cellIndex).Returns(cell);

            // Action
            var result = sheet.GetCell(rowIndex, cellIndex);

            // Assert
            Assert.AreEqual(cell, result);
        }

        [Test]
        public void MergeCells_WhenCall_ReturnCorrectValue()
        {
            // Arrange
            int firstRow = 0, firstCol = 0, lastRow = 1, lastCol = 4;
            var sheet = Substitute.For<ISheet>();

            // Action
            var result = sheet.MergeCells(firstRow, firstCol, lastRow, lastCol);

            // Assert
            sheet.ReceivedWithAnyArgs().AddMergedRegion(null);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(10)]
        public void DeleteRow_WhenCall_RemoveRowMethodIsCalled(int rowIndex)
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var row = Substitute.For<IRow>();
            sheet.GetRow(rowIndex).Returns(row);

            // Action
            sheet.DeleteRow(rowIndex);

            // Assert
            sheet.Received().RemoveRow(row);
        }
    }
}