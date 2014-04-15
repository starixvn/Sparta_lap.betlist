// --------------------------------------------------------------------------
// <copyright file="WinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Base;
    using Common;
    using Common.Extensions;
    using Entities;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Models.ViewModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win Loss Report - Export excel.
    /// </summary>
    public class WinLoss : ReportBase
    {
        private const int ColumnBaCommission = 13;
        private const int ColumnCustomerComm = 9;
        private const int ColumnCustomerTotal = 10;
        private const int ColumnCustomerWinLoss = 8;
        private const int ColumnLicenseeTotal = 14;
        private const int ColumnLicenseeWl = 12;
        private const int ColumnMargin = 11;
        private const int ColumnTemplateCount = 15;
        private const int ColumnTurnover = 6;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private static readonly IDictionary<int, string> GroupColumnNameMap = new Dictionary<int, string>
        {
            { (int)WarehouseReportBy.Day, "date" },
            { (int)WarehouseReportBy.Month, "month" },
            { (int)WarehouseReportBy.Currency, "currency" },
            { (int)WarehouseReportBy.Bettype, "bettype" },
            { (int)WarehouseReportBy.League, "league" },
         };

        private static readonly IDictionary<int, int> GroupNameIndexMap = new Dictionary<int, int>
        {
            { (int)WarehouseReportBy.Day, (int)WarehouseReportBy.Day },
            { (int)WarehouseReportBy.Month, (int)WarehouseReportBy.Month },
            { (int)WarehouseReportBy.Currency, (int)WarehouseReportBy.Currency },
            { (int)WarehouseReportBy.Bettype, (int)WarehouseReportBy.Bettype },
            { (int)WarehouseReportBy.League, (int)WarehouseReportBy.League },
         };

        public WinLoss(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                DataTable exportData)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            ExportData = exportData;

            AddExtraColumns();

            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.Date,
                ViewResources.Month,
                ViewResources.Currency,
                ViewResources.BetType,
                ViewResources.League,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.Margin,
                ViewResources.LicenseeW_L + " (" + ViewResources.SB + " + " + ViewResources.NG + ")",
             };
        }

        public int GroupingId
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Commission);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            var rowCount = ExportData.Rows.Count;
            var columnNameList = GetColumnNameList();
            var groupNameIndex = GetGroupNameIndex();
            var groupColumnName = GetGroupColumnName();

            for (int index = 0; index < rowCount; index++)
            {
                DataRow dataRow = ExportData.Rows[index];
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                row.Cells[0].SetCellValue(index + 1);
                row.Cells[groupNameIndex].SetCellValue(dataRow[groupColumnName]);
                AssignRowContent(row, dataRow, columnNameList, ColumnTurnover);
            }

            AssignTotalData(sheet, ExportData.Rows.Count + RowContentTemplate);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Only show LicenseeWL if licensee doesn't have NG
            if (!ShowingOption.NgVisible)
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
        public override void ProcessUi(ISheet sheet)
        {
            HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
            HideRow(sheet, RowTableHeaderTemplate + 1);

            HideColumn(sheet, ColumnBaCommission);
            HideColumn(sheet, ColumnLicenseeTotal);

            int groupNameIndex = GetGroupNameIndex();
            for (int i = 1; i <= 5; i++)
            {
                if (i != groupNameIndex)
                {
                    HideColumn(sheet, i);
                }
            }
        }

        // / <summary>
        // / Add columns margin, licenseewinloss, licenseetotal
        // / </summary>
        protected void AddExtraColumns()
        {
            const string Expression = "IIF(Turnover > 0, MemberWinLost/Turnover, 0)";
            ExportData.Columns.Add("margin", typeof(decimal), Expression);
        }

        /// <summary>
        /// Assign data to total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        protected void AssignTotalData(ISheet sheet, int currentRow)
        {
            int cellIndex = ColumnTurnover;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);
            List<string> columnNameList = GetColumnNameList();
            foreach (string columnName in columnNameList)
            {
                object sumValue = ExportData.Compute("Sum(" + columnName + ")", string.Empty);
                row.Cells[cellIndex].SetCellValue(Converter.ToDecimal(sumValue));
                cellIndex++;
            }

            decimal winlossTotal = Converter.ToDecimal(ExportData.Compute("Sum(MemberWinlost)", string.Empty));
            decimal turnoverTotal = Converter.ToDecimal(ExportData.Compute("Sum(Turnover)", string.Empty));
            decimal totalMargin = turnoverTotal != 0 ? winlossTotal / turnoverTotal : 0;
            row.Cells[ColumnMargin].SetCellValue(totalMargin);

            sheet.MergeCells(currentRow, 0, currentRow, 5);
        }

        /// <summary>
        /// Get column name in data table.
        /// </summary>
        /// <returns>The list of column name in data table.</returns>
        protected List<string> GetColumnNameList()
        {
            return new List<string>
            {
                "Turnover",
                "betcount",
                "MemberWinlost",
                "MemberWinlost",
                "MemberWinlost",
                "margin",
                "LicenseeWL",
             };
        }

        protected string GetGroupColumnName()
        {
            return GroupColumnNameMap.ContainsKey(GroupingId)
                   ? GroupColumnNameMap[GroupingId]
                   : "currency";
        }

        protected int GetGroupNameIndex()
        {
            return GroupNameIndexMap.ContainsKey(GroupingId)
                   ? GroupNameIndexMap[GroupingId]
                   : 3;
        }
    }
}