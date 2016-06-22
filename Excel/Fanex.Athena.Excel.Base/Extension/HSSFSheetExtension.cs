// <copyright file="HSSFSheetExtension.cs" company="" >
//  Copyright (c) . All rights reserved.
// </copyright>
//
// <history>
//  <change who="jacob.phan" date="2012.5.8">Created</change>
// </history>

namespace Fanex.Athena.Excel.Base.Extension
{
    #region Using

    using System.Collections.Generic;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    #endregion Using

    /// <summary>
    /// HSSFSheetExtensions class, define some extension method for HSSFSheet such as Copy Row, Copy Cell, ..
    /// </summary>
    public static class HSSFSheetExtension
    {
        /// <summary>
        /// CopyRow method is used to copy one row from a template to another template.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="sourceSheet">Template sheet.</param>
        /// <param name="sourceRowIndex">Index of source row in template sheet, base by zero.</param>
        /// <param name="destRowIndex">Index of destination row in main sheet, base by zero.</param>
        /// <returns>Return the copied row.</returns>
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

        /// <summary>
        /// CopyRow method is used to copy one row from a template to another template.
        /// Only copy the cell in cellIndexList.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="sourceSheet">Template sheet.</param>
        /// <param name="sourceRowIndex">Index of source row in template sheet, base by zero.</param>
        /// <param name="destRowIndex">Index of destination row in main sheet, base by zero.</param>
        /// <param name="cellIndexList">Cell index list.</param>
        /// <returns>Return the copied row.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed.")]
        public static IRow CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowIndex, int destRowIndex, IList<int> cellIndexList)
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
            var startIndex = 0;
            foreach (var cellIndex in cellIndexList)
            {
                // Grab a copy of the old/new cell
                var oldCell = sourceRow.GetCell(cellIndex);
                var newCell = newRow.CreateCell(startIndex);
                sheet.CopyCell(oldCell, newCell);
                startIndex++;
            }

            return newRow;
        }

        /// <summary>
        /// CopyRow method is used to copy one row from a template to another template.
        /// Only copy the cell in cellIndexList.
        /// Set default style the rest cell.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="sourceSheet">Template sheet.</param>
        /// <param name="sourceRowIndex">Index of source row in template sheet, base by zero.</param>
        /// <param name="destRowIndex">Index of destination row in main sheet, base by zero.</param>
        /// <param name="cellIndexList">Cell index list.</param>
        /// <param name="defaultStyle">Default style.</param>
        /// <returns>Return the copied row.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed.")]
        public static IRow CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowIndex, int destRowIndex, IList<int> cellIndexList, ICellStyle defaultStyle)
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
            var startIndex = 0;
            foreach (var cellIndex in cellIndexList)
            {
                // Grab a copy of the old/new cell
                var oldCell = sourceRow.GetCell(cellIndex);
                var newCell = newRow.CreateCell(startIndex);
                sheet.CopyCell(oldCell, newCell);
                startIndex++;
            }

            for (int i = cellIndexList.Count; i < newRow.Cells.Count; i++)
            {
                var cell = newRow.Cells[i];
                cell.CellStyle = defaultStyle;
                cell.SetCellValue(string.Empty);
            }

            return newRow;
        }

        /// <summary>
        /// CopyRow method is used to copy one row to another row.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="rowNum">Inserting row index, base by zero.</param>
        /// <param name="sourceRowNum">Index of source row, base by zero.</param>
        /// <returns>Return the copied row.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Reviewed.")]
        public static IRow InsertCopy(this ISheet sheet, int rowNum, int sourceRowNum)
        {
            sheet.ShiftRows(rowNum, sheet.LastRowNum, 1);

            // Get the source / new row
            var newRow = sheet.CreateRow(rowNum);
            var sourceRow = sheet.GetRow(sourceRowNum + 1);

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

        /// <summary>
        /// CopyCell method is used to copy one cell to another cell.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="sourceCell">Source cell.</param>
        /// <param name="destCell">Destination cell.</param>
        /// <returns>Returns copied cell.</returns>
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

        /// <summary>
        /// GetCell method is used to get one cell in a sheet by row and column index.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="rowIndex">Row index, base by zero.</param>
        /// <param name="colIndex">Column index, base by zero.</param>
        /// <returns>Return matched cell.</returns>
        public static ICell GetCell(this ISheet sheet, int rowIndex, int colIndex)
        {
            return sheet.GetRow(rowIndex).GetCell(colIndex);
        }

        /// <summary>
        /// MergeCells method is used to Merge a region.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="firstRow">First row index, base by zero.</param>
        /// <param name="firstCol">First column index, , base by zero.</param>
        /// <param name="lastRow">Last row index, base by zero.</param>
        /// <param name="lastCol">Last column Index.</param>
        /// <returns>Return affected cell count.</returns>
        public static int MergeCells(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            var rangeAddress = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            return sheet.AddMergedRegion(rangeAddress);
        }

        /// <summary>
        /// Delete row in sheet.
        /// </summary>
        /// <param name="sheet">Main sheet object.</param>
        /// <param name="rowIndex">Deleting row index.</param>
        public static void DeleteRow(this ISheet sheet, int rowIndex)
        {
            sheet.RemoveRow(sheet.GetRow(rowIndex));
        }
    }
}