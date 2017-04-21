// --------------------------------------------------------------------------
// <copyright file="TopCustomer.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Warehouse
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Base;
    using Base.Entities;
    using Common;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Top customer - Export excel.
    /// </summary>
    public class TopCustomer : ReportBase
    {
        private const int ColumnBaCommission = 9;
        private const int ColumnCustomerComm = 6;
        private const int ColumnCustomerTotal = 7;
        private const int ColumnCustomerWl = 5;
        private const int ColumnLicenseeTotal = 10;
        private const int ColumnLicenseeWl = 8;
        private const int ColumnTemplateCount = 12;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        public TopCustomer(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                DataTable exportData)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            ExportData = exportData;

            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.Customer,
                ViewResources.Currency,
                ViewResources.Turnover,
                ViewResources.BetCount,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.LastActivityDate,
            };
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        protected override void AssignData()
        {
            ISheet sheet = Workbook.CloneSheet("Template", "Data");
            string errorMessage = GetErrorMessage();

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, errorMessage);
            }
            else
            {
                AssignSheetData(sheet);
            }

            // Hide unnecessary column
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Commission);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            int rowCount = ExportData.Rows.Count;

            IList<string> columnNameList = GetColumnNameList();
            for (int index = 0; index < rowCount; index++)
            {
                DataRow dataRow = ExportData.Rows[index];
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                row.Cells[0].SetCellValue(index + 1);
                AssignRowContent(row, dataRow, columnNameList, 1);
            }

            // Total row
            AssignTotalData(sheet, RowContentTemplate + rowCount);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignTitle(ISheet sheet)
        {
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Only show LicenseeWL if licensee doesn't have NG
            if (!ShowingOption.NGVisible)
            {
                sheet.GetCell(RowTableHeaderTemplate, ColumnLicenseeWl).SetCellValue(ViewResources.LicenseeWL);
            }

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
            HideRow(sheet, RowTableHeaderTemplate + 1);
        }

        /// <summary>
        /// Get column name in data table.
        /// </summary>
        /// <returns>The list of column name in data table.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        protected IList<string> GetColumnNameList()
        {
            return new List<string>
            {
                "UserName",
                "currency",
                "Turnover",
                "betcount",
                "MemberWinlost",
                "MemberWinlost",
                "MemberWinlost",
                "LicenseeWL",
                "lastactivedate"
            };
        }

        /// <summary>
        /// Get column name in data table to get total data.
        /// </summary>
        /// <returns>The list of column name in data table.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        protected IList<string> GetTotalColumnNameList()
        {
            return new List<string>
            {
                "Turnover",
                "betcount",
                "MemberWinlost",
                "MemberWinlost",
                "MemberWinlost",
                "LicenseeWL",
            };
        }

        /// <summary>
        /// Assign data to total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        protected void AssignTotalData(ISheet sheet, int currentRow)
        {
            int cellIndex = 3;
            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);
            IList<string> columnNameList = GetTotalColumnNameList();
            foreach (string columnName in columnNameList)
            {
                object sumValue = ExportData.Compute("Sum(" + columnName + ")", string.Empty);
                row.Cells[cellIndex].SetCellValue(Converter.ToDecimal(sumValue));
                cellIndex++;
            }

            row.Cells[cellIndex].SetCellValue("-");

            sheet.MergeCells(currentRow, 0, currentRow, 2);
        }

        /// <summary>
        ///     Check data has error or hasn't error.
        /// </summary>
        /// <returns>Error message.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        protected string GetErrorMessage()
        {
            if (ExportData.Rows.Count == 0)
            {
                return ViewResources.NoInfo;
            }

            return ExportData.Columns.Contains("message")
                   ? Converter.ToString2(ExportData.Rows[0]["message"])
                   : string.Empty;
        }
    }
}