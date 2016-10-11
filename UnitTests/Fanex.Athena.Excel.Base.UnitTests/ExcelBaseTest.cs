namespace Fanex.Athena.Excel.Base.UnitTests
{
    using Base.Extension;
    using ClassTest;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ExcelBaseTest
    {
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

            var excelBase = new FakeExcelBase();

            // Action
            excelBase.HideRow(sheet, rowIndexs);

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

            var excelBase = new FakeExcelBase();

            // Action
            excelBase.HideRow(sheet, rowIndexs);

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
            FakeExcelBase.HideColumn(sheet, columnIndexs);

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
            FakeExcelBase.HideColumn(sheet, columnIndexs);

            // Assert
            sheet.Received(times).SetColumnHidden(Arg.Any<int>(), true);
        }
    }
}