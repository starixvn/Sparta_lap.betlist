// --------------------------------------------------------------------------
// <copyright file="CustomerTurnoverExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Report.DTO;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CustomerTurnoverExcel : ReportBase
    {
        private const int ColumnDate = 1;
        private const int ColumnUserName = 2;
        private const int ColumnCurrency = 3;
        private const int ColumnSport = 4;
        private const int ColumnBetType = 5;
        private const int ColumnEffectiveBettingValue = 6;
        private const int ColumnTotalTransaction = 7;
        private const int ColumnTurnover = 8;
        private const int ColumnTotalTurnover = 9;
        private const int ColumnCustomerWl = 10;
        private const int ColumnCustomerComm = 11;
        private const int ColumnCustomerTotal = 12;

        private const int ColumnTemplateCount = 12;

        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private const int DefaultPageSize = 1500;

        private readonly IEnumerable<CustTurnover> _dataList;
        private readonly string _formatDateTime;
        private readonly CustTurnover _total;
        private readonly string _reportTitle;

        protected internal CustomerTurnoverExcel()
            : base(string.Empty, 0, null)
        {
        }

        public CustomerTurnoverExcel(
                                     string templatePath,
                                     ShowingOption showingOption,
                                     IEnumerable<CustTurnover> dataList,
                                     CustTurnover total,
                                     string formatDateTime,
                                     string reportTitle)
            : base(templatePath, dataList.Count(), showingOption)
        {
            _dataList = dataList;
            _total = total;
            _formatDateTime = formatDateTime;
            _reportTitle = reportTitle;
            PageSize = DefaultPageSize;
            IsMultiSheet = true;
        }

        protected override int NumberOfSheets
        {
            get
            {
                return GetNumberOfSheets(_dataList.Last().Index, PageSize);
            }
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);
            var headerTextList = new[]
            {
                "#",
                ViewResources.Date,
                ViewResources.Username,
                ViewResources.Currency,
                ViewResources.Sport,
                ViewResources.BetType,
                ViewResources.EffectiveBettingValue,
                ViewResources.TotalTransaction,
                ViewResources.Turnover,
                ViewResources.Total + " " + ViewResources.Turnover,
                ViewResources.CustomerW_L,
            };
            SetTextRowHeader(row, headerTextList);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet, int pageIndex)
        {
            var rowIndex = 0;
            var minGroup = checked(pageIndex * PageSize);
            var maxGroup = Math.Min(_dataList.Last().Index, checked((pageIndex + 1) * PageSize));
            var dataInRange = _dataList.Where(x => x.Index > minGroup && x.Index <= maxGroup).ToList();

            foreach (var customerTurnover in dataInRange)
            {
                var realIndex = rowIndex + RowContentTemplate;
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, realIndex);
                if (customerTurnover.RowSpan > -1)
                {
                    row.Cells[ColumnIndex].SetCellValue(customerTurnover.Index);
                    row.Cells[ColumnDate].SetCellValue(customerTurnover.ReportBy.ToString(_formatDateTime));
                    row.Cells[ColumnUserName].SetCellValue(customerTurnover.UserName);
                    row.Cells[ColumnCurrency].SetCellValue(customerTurnover.Currency);
                    row.Cells[ColumnTotalTurnover].SetCellValue(customerTurnover.TotalTurnover);
                }

                if (customerTurnover.RowSpan2 > -1)
                {
                    row.Cells[ColumnSport].SetCellValue(customerTurnover.SportType);
                }

                if (customerTurnover.RowSpan > 0)
                {
                    sheet.MergeRows(realIndex, (customerTurnover.RowSpan - 1) + realIndex, ColumnIndex);
                    sheet.MergeRows(realIndex, (customerTurnover.RowSpan - 1) + realIndex, ColumnDate);
                    sheet.MergeRows(realIndex, (customerTurnover.RowSpan - 1) + realIndex, ColumnUserName);
                    sheet.MergeRows(realIndex, (customerTurnover.RowSpan - 1) + realIndex, ColumnCurrency);
                    sheet.MergeRows(realIndex, (customerTurnover.RowSpan - 1) + realIndex, ColumnTotalTurnover);
                }

                if (customerTurnover.RowSpan2 > 0)
                {
                    sheet.MergeRows(realIndex, (customerTurnover.RowSpan2 - 1) + realIndex, ColumnSport);
                }

                row.Cells[ColumnBetType].SetCellValue(customerTurnover.Bettype);
                row.Cells[ColumnEffectiveBettingValue].SetCellValue(customerTurnover.EffectiveBettingValue);
                row.Cells[ColumnTotalTransaction].SetCellValue(customerTurnover.TotalTransactions);
                row.Cells[ColumnTurnover].SetCellValue(customerTurnover.Turnover);
                row.Cells[ColumnCustomerWl].SetCellValue(customerTurnover.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].SetCellValue(customerTurnover.CustomerComm);
                row.Cells[ColumnCustomerTotal].SetCellValue(customerTurnover.CustomerWinLoss + customerTurnover.CustomerComm);
                rowIndex++;
            }

            AssignTotalRow(sheet, rowIndex + RowContentTemplate);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, ColumnTemplateCount, _reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, RowContentTemplate);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerComm);
                sheet.HideRow(RowTableHeaderTemplate + 1);
            }
        }

        /// <summary>
        /// Assign data to total row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="totalRow">The total value.</param>
        protected internal virtual void AssignTotalRow(ISheet sheet, int totalRow)
        {
            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);
            row.Cells[ColumnCurrency].SetCellValue(string.Empty);
            row.Cells[ColumnBetType].SetCellValue(string.Empty);
            row.Cells[ColumnSport].SetCellValue(string.Empty);
            row.Cells[ColumnUserName].SetCellValue(string.Empty);
            row.Cells[ColumnEffectiveBettingValue].SetCellValue(_total.EffectiveBettingValue);
            row.Cells[ColumnTotalTransaction].SetCellValue(_total.TotalTransactions);
            row.Cells[ColumnTurnover].SetCellValue(_total.Turnover);
            row.Cells[ColumnTotalTurnover].SetCellValue(_total.Turnover);
            row.Cells[ColumnCustomerWl].SetCellValue(_total.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(_total.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_total.CustomerWinLoss + _total.CustomerComm);

            sheet.MergeCols(ColumnIndex, ColumnBetType, totalRow);
        }
    }
}