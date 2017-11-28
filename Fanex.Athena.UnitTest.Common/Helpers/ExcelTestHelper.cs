using System.Collections.Generic;
using NPOI.SS.UserModel;
using NSubstitute;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class ExcelTestHelper
    {
        public static ICell MockGetRow(ISheet sheet, int columnCount)
        {
            var row = Substitute.For<IRow>();
            var cell = Substitute.For<ICell>();
            cell.Row.Returns(row);
            var cellList = new List<ICell>();
            for (int i = 0; i < columnCount; i++)
            {
                cellList.Add(cell);
            }

            row.Cells.ReturnsForAnyArgs(cellList);
            sheet.GetRow(0).ReturnsForAnyArgs(row);
            return cell;
        }
    }
}