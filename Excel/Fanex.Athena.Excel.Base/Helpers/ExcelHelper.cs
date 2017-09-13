namespace Fanex.Athena.Excel.Base.Helpers
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.SS.UserModel;

    public static class ExcelHelper
    {
        public static int AssignRowContent(IRow row, DataRow data, IEnumerable<string> columnNameList, int columnStart = 0)
        {
            foreach (string columnName in columnNameList)
            {
                if (data.Table.Columns.Contains(columnName))
                {
                    row.Cells[columnStart].SetCellValue(data[columnName]);
                }

                columnStart++;
            }

            return columnStart;
        }
    }
}