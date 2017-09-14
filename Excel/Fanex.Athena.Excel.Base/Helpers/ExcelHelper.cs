namespace Fanex.Athena.Excel.Base.Helpers
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Core.MetaModel.Product;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Resources;

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

        public static string GetLicenseeWLProduct(IEnumerable<ProductOption> productOptions, string caption = null)
        {
            var productCodeList = productOptions.Where(p => p.Status)
                            .Aggregate(string.Empty, (current, p) => current + (string.IsNullOrEmpty(current) ? p.Code : ", " + p.Code));

            return string.IsNullOrEmpty(productCodeList)
                ? string.IsNullOrWhiteSpace(caption) ? string.Empty : ", " + caption
                      : string.Format(", {0} ({1})", string.IsNullOrWhiteSpace(caption) ? ViewResources.LicenseeW_L : caption, productCodeList);
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