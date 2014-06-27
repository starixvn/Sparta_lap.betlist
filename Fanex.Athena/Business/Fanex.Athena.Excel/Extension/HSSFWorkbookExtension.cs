// <copyright file="HSSFWorkbookExtension.cs" company="Nexcel Solutions Vietnam" >
//  Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// <history>
//  <change who="jacob.phan" date="2012.5.8">Created</change>
// </history>

namespace Fanex.Athena.Excel.Extension
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// HSSFWorkbookExtension class, define some extension method for HSSFWorkbook such as CloneSheet, RemoveSheet, ..
    /// </summary>
    internal static class HSSFWorkbookExtension
    {
        /// <summary>
        /// CloneSheet method is used to copy a sheet to new sheet
        /// </summary>
        /// <param name="workbook">Target workbook</param>
        /// <param name="sourceSheetName">Template sheet name</param>
        /// <param name="newSheetName">New sheet name</param>
        /// <returns>Return the copied sheet</returns>
        public static ISheet CloneSheet(this IWorkbook workbook, string sourceSheetName, string newSheetName)
        {
            var sheetIndex = workbook.GetSheetIndex(sourceSheetName);
            var newSheet = workbook.CloneSheet(sheetIndex);
            var newSheetIndex = workbook.GetSheetIndex(newSheet);
            workbook.SetSheetName(newSheetIndex, newSheetName);
            return newSheet;
        }

        /// <summary>
        /// Remove the sheet by sheet name
        /// </summary>
        /// <param name="workbook">Target workbook</param>
        /// <param name="sheetName">The sheet name</param>
        public static void RemoveSheet(this IWorkbook workbook, string sheetName)
        {
            var sheet = workbook.GetSheet("Template");

            if (sheet != null)
            {
                var sheetIndex = workbook.GetSheetIndex(sheet);
                workbook.RemoveSheetAt(sheetIndex);
            }
        }
    }
}