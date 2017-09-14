// --------------------------------------------------------------------------
// <copyright file="HSSFSheetExtension.cs" company="" >
//  Copyright (c) . All rights reserved.
// </copyright>

namespace Fanex.Athena.Excel.Base.Extension
{
    #region Using

    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    #endregion Using

    /// <summary>
    /// HSSFSheetExtensions class, define some extension method for HSSFSheet such as Copy Row, Copy Cell, ..
    /// </summary>
    public static class HSSFSheetExtension
    {
        public static IRow CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowIndex, int destRowIndex)
        {
            if (sheet.LastRowNum < destRowIndex)
            {
                for (var i = sheet.LastRowNum + 1; i <= destRowIndex; i++)
                {
                    sheet.CreateRow(i);
                }
            }

            // Get the source / new row
            var sourceRow = sourceSheet.GetRow(sourceRowIndex);
            if (sourceRow == null)
            {
                return null;
            }

            var newRow = sheet.GetRow(destRowIndex);
            if (newRow == null)
            {
                newRow = sheet.CreateRow(destRowIndex);
            }

            // Loop through source columns to add to new row
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                var oldCell = sourceRow.GetCell(i);
                var newCell = newRow.CreateCell(i);

                sheet.CopyCell(oldCell, newCell);
            }

            return newRow;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Reviewed.")]
        public static ICell CopyCell(this ISheet sheet, ICell sourceCell, ICell destCell)
        {
            if (sourceCell == null)
            {
                destCell = null;
                return null;
            }

            destCell.CellStyle = sourceCell.CellStyle;

            if (destCell.CellComment != null)
            {
                destCell.CellComment = sourceCell.CellComment;
            }

            if (sourceCell.Hyperlink != null)
            {
                destCell.Hyperlink = sourceCell.Hyperlink;
            }

            destCell.SetCellType(sourceCell.CellType);

            switch (sourceCell.CellType)
            {
                case CellType.Blank:
                    destCell.SetCellValue(sourceCell.StringCellValue);
                    break;

                case CellType.Boolean:
                    destCell.SetCellValue(sourceCell.BooleanCellValue);
                    break;

                case CellType.Error:
                    destCell.SetCellErrorValue(sourceCell.ErrorCellValue);
                    break;

                case CellType.Numeric:
                    destCell.SetCellValue(sourceCell.NumericCellValue);
                    break;

                case CellType.String:
                    destCell.SetCellValue(sourceCell.RichStringCellValue);
                    break;

                case CellType.Unknown:
                    destCell.SetCellValue(sourceCell.StringCellValue);
                    break;
            }

            return (ICell)destCell;
        }

        public static ICell GetCell(this ISheet sheet, int rowIndex, int colIndex)
        {
            return sheet.GetRow(rowIndex).GetCell(colIndex);
        }

        public static int MergeCells(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            var rangeAddress = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            return sheet.AddMergedRegion(rangeAddress);
        }

        public static void DeleteRow(this ISheet sheet, int rowIndex)
        {
            sheet.RemoveRow(sheet.GetRow(rowIndex));
        }

        public static void HideRow(this ISheet sheet, params int[] rowIndex)
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

        public static void HideColumn(this ISheet sheet, params int[] columnIndex)
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