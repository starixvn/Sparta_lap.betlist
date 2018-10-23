namespace Fanex.Athena.Excel.Base.Helpers
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public static class ExcelHelper
    {
        /// <summary>
        /// This method is obsolete, please try to remove it for me.
        /// </summary>
        public static int AssignRowContent(IRow row, DataRow data, IEnumerable<string> columnNameList, int columnStart = 0)
        {
            var tableColumns = data.Table.Columns;
            foreach (string columnName in columnNameList)
            {
                if (tableColumns.Contains(columnName))
                {
                    row.Cells[columnStart].SetCellValue(data[columnName]);
                }

                columnStart++;
            }

            return columnStart;
        }

        public static byte[] ExportEmptyFile()
        {
            var templateWorkbook = new HSSFWorkbook();
            templateWorkbook.CreateSheet();
            var ms = new MemoryStream();
            templateWorkbook.Write(ms);

            return ms.ToArray();
        }
    }
}