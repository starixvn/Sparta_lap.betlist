// --------------------------------------------------------------------------
// <copyright file="ExcelBase.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base
{
    using System.IO;
    using Entities;
    using Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public abstract class ExcelBase
    {
        protected const int ColumnIndex = 0;
        protected const string FormatDate = "MM/dd/yyyy";

        protected ShowingOption ShowingOption
        {
            get;
            set;
        }

        public static byte[] ExportEmptyFile()
        {
            // Create a new workbook
            var templateWorkbook = new HSSFWorkbook();
            templateWorkbook.CreateSheet();

            var ms = new MemoryStream();

            // Writing the workbook content to the MemoryStream...
            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        protected internal virtual void HideRow(ISheet sheet, params int[] rowIndex)
        {
            // Clear content
            foreach (int i in rowIndex)
            {
                for (int j = 0; j < 255; j++)
                {
                    var cell = sheet.GetCell(i, j);
                    if (cell != null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                }
            }

            // Hide row
            foreach (int i in rowIndex)
            {
                var row = sheet.GetRow(i);
                if (row != null)
                {
                    row.ZeroHeight = true;
                }
            }
        }

        protected internal virtual void HideColumn(ISheet sheet, params int[] columnIndex)
        {
            // Clear content
            var firstRowNum = sheet.FirstRowNum;
            var lastRowNum = sheet.LastRowNum;

            for (int i = firstRowNum; i <= lastRowNum; i++)
            {
                foreach (int j in columnIndex)
                {
                    var cell = sheet.GetCell(i, j);
                    if (cell != null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                }
            }

            // Hide column
            foreach (int i in columnIndex)
            {
                sheet.SetColumnWidth(i, 0);
                sheet.SetColumnHidden(i, true);
            }
        }
    }
}