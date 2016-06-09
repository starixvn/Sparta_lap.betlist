// --------------------------------------------------------------------------
// <copyright file="ExcelBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Base
{
    using System.IO;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// The excel base.
    /// </summary>
    public abstract class ExcelBase
    {
        protected const int ColumnIndex = 0;
        protected const string FormatDate = "MM/dd/yyyy";

        public ShowingOption ShowingOption
        {
            get;
            set;
        }

        /// <summary>
        /// Export empty file.
        /// </summary>
        /// <returns>A byte array.</returns>
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

        /// <summary>
        /// Hide rows.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The row index.</param>
        public virtual void HideRow(ISheet sheet, params int[] rowIndex)
        {
            // Clear content
            foreach (int i in rowIndex)
            {
                for (int j = 0; j < 255; j++)
                {
                    ICell cell = sheet.GetCell(i, j);
                    if (cell != null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                }
            }

            // Hide row
            foreach (int i in rowIndex)
            {
                IRow row = sheet.GetRow(i);
                if (row != null)
                {
                    row.ZeroHeight = true;
                }
            }
        }

        /// <summary>
        ///     Method used hidden column with parameter is position of column.
        /// </summary>
        /// <param name="sheet">The excel sheet.</param>
        /// <param name="columnIndex">Column Index of column will be hidden.</param>
        protected static void HideColumn(ISheet sheet, params int[] columnIndex)
        {
            // Clear content
            int firstRowNum = sheet.FirstRowNum;
            int lastRowNum = sheet.LastRowNum;

            for (int i = firstRowNum; i <= lastRowNum; i++)
            {
                foreach (int j in columnIndex)
                {
                    ICell cell = sheet.GetCell(i, j);
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