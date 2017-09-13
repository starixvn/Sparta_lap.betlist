// --------------------------------------------------------------------------
// <copyright file="TopWinLoss.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Warehouse
{
    using System.Collections.Generic;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Warehouse.DTO;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Win Loss Report - Export excel.
    /// </summary>
    public class TopWinLoss : ReportBase
    {
        private const int ColumnCustomerComm = 9;
        private const int ColumnCustomerTotal = 10;
        private const int ColumnCustomerWinLoss = 8;
        private const int ColumnLicenseeWl = 12;
        private const int ColumnMargin = 11;
        private const int ColumnTemplateCount = 15;
        private const int ColumnTurnover = 6;
        private const int ColumnTotalTrans = 7;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private static readonly IDictionary<int, int> GroupNameIndexMap = new Dictionary<int, int>
        {
            { (int)WarehouseReportBy.Day, (int)WarehouseReportBy.Day },
            { (int)WarehouseReportBy.Month, (int)WarehouseReportBy.Month },
            { (int)WarehouseReportBy.Currency, (int)WarehouseReportBy.Currency },
            { (int)WarehouseReportBy.Bettype, (int)WarehouseReportBy.Bettype },
            { (int)WarehouseReportBy.League, (int)WarehouseReportBy.League },
         };

        private readonly IList<WinLoss> _dataList;

        private readonly IDictionary<string, decimal> _totalValues;

        public TopWinLoss(
                string templatePath,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                IList<Fanex.Athena.Model.Warehouse.DTO.WinLoss> dataList,
                IDictionary<string, decimal> totalValues)
            : base(templatePath, showingOption, convertingOption)
        {
            _dataList = dataList;

            _totalValues = totalValues;

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
                ViewResources.LicenseeW_L,
             };
        }

        public int GroupingId { get; set; }

        protected override void AssignData()
        {
            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);

            AssignTitle(sheet);

            if (_dataList.Count == 0)
            {
                AssignNoInfoRow(sheet);
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

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Commission);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            var rowCount = _dataList.Count;

            for (int index = 0; index < rowCount; index++)
            {
                var item = _dataList[index];
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                row.Cells[0].SetCellValue(index + 1);
                row.Cells[GroupingId].SetCellValue(item.GroupByString);
                row.Cells[ColumnTurnover].SetCellValue(item.Turnover);
                row.Cells[ColumnTotalTrans].SetCellValue(item.BetCount);
                row.Cells[ColumnCustomerWinLoss].SetCellValue(item.MemberWinLoss);
                row.Cells[ColumnMargin].SetCellValue(item.Margin / 100);
                row.Cells[ColumnLicenseeWl].SetCellValue(item.LicenseeWinloss);
            }

            AssignTotalData(sheet, _dataList.Count + RowContentTemplate);
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

            int groupNameIndex = GetGroupNameIndex();
            for (int i = 1; i <= 5; i++)
            {
                if (i != groupNameIndex)
                {
                    HideColumn(sheet, i);
                }
            }
        }

        /// <summary>
        /// Assign data to total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        protected void AssignTotalData(ISheet sheet, int currentRow)
        {
            var marginTotal = _totalValues["turnover"] != 0 ? (_totalValues["memberwinloss"] / _totalValues["turnover"]) : 0;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);
            row.Cells[ColumnTurnover].SetCellValue(_totalValues["turnover"]);
            row.Cells[ColumnTotalTrans].SetCellValue(_totalValues["betcount"]);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(_totalValues["memberwinloss"]);
            row.Cells[ColumnMargin].SetCellValue(marginTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(_totalValues["licenseewinloss"]);
            sheet.MergeCells(currentRow, 0, currentRow, 5);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        protected int GetGroupNameIndex()
        {
            return GroupNameIndexMap[GroupingId];
        }
    }
}