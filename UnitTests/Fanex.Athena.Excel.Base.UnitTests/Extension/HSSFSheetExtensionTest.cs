namespace Fanex.Athena.Excel.Base.UnitTests.Extension
{
    using Excel.Base.Extension;
    using NPOI.SS.UserModel;
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

        [Test]
        public void CopyCell_SourceCellIsNotNull_SetCellTypeMethodIsCalled()
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var source = Substitute.For<ICell>();
            var destination = Substitute.For<ICell>();

            // Action
            var cell = sheet.CopyCell(source, destination);

            // Assert
            destination.Received().SetCellType(source.CellType);
        }

        [TestCase(CellType.Blank)]
        [TestCase(CellType.Unknown)]
        public void CopyCell_SourceCellTypeIsBlankOrUnknown_SetCellValueMethodIsCalledWithCorrectParameter(CellType cellType)
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var source = Substitute.For<ICell>();
            source.CellType.Returns(cellType);
            var cellValue = "StringValue";
            source.StringCellValue.Returns(cellValue);
            var destination = Substitute.For<ICell>();

            // Action
            var cell = sheet.CopyCell(source, destination);

            // Assert
            cell.Received().SetCellValue(cellValue);
        }

        [Test]
        public void CopyCell_SourceCellTypeIsBoolean_SetCellValueMethodIsCalledWithCorrectParameter()
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var source = Substitute.For<ICell>();
            source.CellType.Returns(CellType.Boolean);
            var cellValue = true;
            source.BooleanCellValue.Returns(cellValue);
            var destination = Substitute.For<ICell>();

            // Action
            var cell = sheet.CopyCell(source, destination);

            // Assert
            cell.Received().SetCellValue(cellValue);
        }

        [Test]
        public void CopyCell_SourceCellTypeIsError_SetCellValueMethodIsCalledWithCorrectParameter()
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var source = Substitute.For<ICell>();
            source.CellType.Returns(CellType.Error);
            byte cellValue = 4;
            source.ErrorCellValue.Returns(cellValue);
            var destination = Substitute.For<ICell>();

            // Action
            var cell = sheet.CopyCell(source, destination);

            // Assert
            cell.Received().SetCellErrorValue(cellValue);
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
            Assert.That(result, Is.EqualTo(cell));
        }

        [Test]
        public void MergeCells_WhenCall_AddMergedRegionMethodIsCalled()
        {
            // Arrange
            int firstRow = 0, firstCol = 0, lastRow = 1, lastCol = 4;
            var sheet = Substitute.For<ISheet>();

            // Action
            sheet.MergeCells(firstRow, firstCol, lastRow, lastCol);

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

        [Test]
        public void HideRow_WhenCall_SetCellValueMethodCalled()
        {
            // Arrange
            var rowIndexs = new int[] { 0, 1, 5 };
            var times = rowIndexs.Length;
            var sheet = Substitute.For<ISheet>();
            var cell = Substitute.For<ICell>();
            foreach (var item in rowIndexs)
            {
                sheet.GetCell(item, 0).Returns(cell);
            }

            // Action
            sheet.HideRow(rowIndexs);

            // Assert
            cell.Received(times).SetCellValue(string.Empty);
        }

        [Test]
        public void HideRow_WhenCall_ZeroHeightIsTrue()
        {
            // Arrange
            var rowIndexs = new int[] { 0, 1, 5, 8 };
            var times = rowIndexs.Length;
            var sheet = Substitute.For<ISheet>();
            var row = Substitute.For<IRow>();
            foreach (var item in rowIndexs)
            {
                sheet.GetRow(item).Returns(row);
            }

            // Action
            sheet.HideRow(rowIndexs);

            // Assert
            row.Received(times).ZeroHeight = true;
        }

        [Test]
        public void HideColumn_WhenCall_SetCellValueMethodCalled()
        {
            // Arrange
            var columnIndexs = new int[] { 0, 1, 5 };
            var times = columnIndexs.Length;
            var sheet = Substitute.For<ISheet>();
            sheet.FirstRowNum.Returns(1);
            sheet.LastRowNum.Returns(3);

            var cell = Substitute.For<ICell>();
            foreach (var item in columnIndexs)
            {
                sheet.GetCell(2, item).Returns(cell);
            }

            // Action
            sheet.HideColumn(columnIndexs);

            // Assert
            cell.Received(times).SetCellValue(string.Empty);
        }

        [Test]
        public void HideColumn_WhenCall_SetColumnHiddenMethodCalledWithCorrectParam()
        {
            // Arrange
            var columnIndexs = new int[] { 0, 1, 5 };
            var times = columnIndexs.Length;
            var sheet = Substitute.For<ISheet>();
            sheet.FirstRowNum.Returns(1);
            sheet.LastRowNum.Returns(3);

            // Action
            sheet.HideColumn(columnIndexs);

            // Assert
            sheet.Received(times).SetColumnHidden(Arg.Any<int>(), true);
        }
    }
}