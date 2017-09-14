// --------------------------------------------------------------------------
// <copyright file="TopCustomer.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Base;
    using Base.Entities;
    using Base.Helpers;
    using Common;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.SS.UserModel;
    using Resources;

    public class TopCustomer : ReportBase
    {
        private const int ColumnCustomerComm = 6;
        private const int ColumnCustomerTotal = 7;
        private const int ColumnCustomerWl = 5;
        private const int ColumnLicenseeWl = 8;
        private const int ColumnTemplateCount = 12;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly DataTable _exportData;

        public TopCustomer(
                string templatePath,
                ShowingOption showingOption,
                DataTable exportData)
            : base(templatePath, showingOption)
        {
            _exportData = exportData;

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
                sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, errorMessage);
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

        protected override void AssignSheetData(ISheet sheet)
        {
            int rowCount = _exportData.Rows.Count;

            var columnNameList = GetColumnNameList();
            for (int index = 0; index < rowCount; index++)
            {
                var dataRow = _exportData.Rows[index];
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                row.Cells[0].SetCellValue(index + 1);
                ExcelHelper.AssignRowContent(row, dataRow, columnNameList, 1);
            }

            AssignTotalData(sheet, RowContentTemplate + rowCount);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

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

        protected override void ProcessUI(ISheet sheet)
        {
            sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerComm);
            sheet.HideRow(RowTableHeaderTemplate + 1);
        }

        private static IEnumerable<string> GetColumnNameList()
        {
            return new string[]
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

        private static IEnumerable<string> GetTotalColumnNameList()
        {
            return new string[]
            {
                "Turnover",
                "betcount",
                "MemberWinlost",
                "MemberWinlost",
                "MemberWinlost",
                "LicenseeWL",
            };
        }

        private void AssignTotalData(ISheet sheet, int currentRow)
        {
            int cellIndex = 3;
            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);
            var columnNameList = GetTotalColumnNameList();
            foreach (string columnName in columnNameList)
            {
                object sumValue = _exportData.Compute("Sum(" + columnName + ")", string.Empty);
                row.Cells[cellIndex].SetCellValue(Converter.ToDecimal(sumValue));
                cellIndex++;
            }

            row.Cells[cellIndex].SetCellValue("-");

            sheet.MergeCells(currentRow, 0, currentRow, 2);
        }

        private string GetErrorMessage()
        {
            if (_exportData.Rows.Count == 0)
            {
                return ViewResources.NoInfo;
            }

            return _exportData.Columns.Contains("message")
                   ? Converter.ToString2(_exportData.Rows[0]["message"])
                   : string.Empty;
        }
    }
}