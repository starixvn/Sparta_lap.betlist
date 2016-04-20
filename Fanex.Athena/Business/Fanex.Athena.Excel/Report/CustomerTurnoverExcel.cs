// --------------------------------------------------------------------------
// <copyright file="CustomerTurnoverExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Models.DTO.Report;
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
        private const int ColumnTemplateCount = 12;
        private const int ColumnCustomerTotal = 12;

        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly IList<CustTurnover> _customerTurnoverList;
        private readonly string _formatDateTime;
        private readonly CustTurnover _total;

        public CustomerTurnoverExcel(
                                     string templatePath,
                                     string exportingFileName,
                                     ShowingOption showingOption,
                                     IList<CustTurnover> customerTurnoverList,
                                     CustTurnover total,
                                     string formatDateTime)
            : base(templatePath, exportingFileName, showingOption, new ConvertingOption())
        {
            _customerTurnoverList = customerTurnoverList;
            _total = total;
            _formatDateTime = formatDateTime;
            HeaderTextList = new List<string>
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
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _customerTurnoverList.Count;
            ISheet sheet;

            if (numOfDataRow == 0)
            {
                sheet = Workbook.CloneSheet("Template", "Data");
                AssignTitle(sheet);
                AssignHeader(sheet);
                AssignNoInfoRow(sheet);
                ProcessUI(sheet);
            }
            else
            {
                PageSize = 1500;
                var numOfSheet = GetNumberOfSheets(_customerTurnoverList.Last().Index, PageSize);
                for (int i = 1; i <= numOfSheet; i++)
                {
                    sheet = Workbook.CloneSheet("Template", "Data" + i.ToString());
                    AssignTitle(sheet);
                    AssignHeader(sheet);
                    AssignSheetData(sheet, i);
                    ProcessUI(sheet);
                }
            }
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

            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        /// <summary>
        /// Assign "no info" to sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="pageIndex">Start index.</param>
        /// <returns>Row index.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        public override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowIndex = 0;
            int minGroup = checked((pageIndex - 1) * PageSize);
            int maxGroup = Math.Min(_customerTurnoverList.Last().Index, checked(pageIndex * PageSize));
            var dataInRange = _customerTurnoverList.Where(x => x.Index > minGroup && x.Index <= maxGroup).ToList();

            for (int index = 0; index < dataInRange.Count(); index++)
            {
                CustTurnover customerTurnover = dataInRange[index];
                var realIndex = rowIndex + RowContentTemplate;
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, realIndex);
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
                    sheet.MergeCells(realIndex, ColumnIndex, (customerTurnover.RowSpan - 1) + realIndex, ColumnIndex);
                    sheet.MergeCells(realIndex, ColumnDate, (customerTurnover.RowSpan - 1) + realIndex, ColumnDate);
                    sheet.MergeCells(realIndex, ColumnUserName, (customerTurnover.RowSpan - 1) + realIndex, ColumnUserName);
                    sheet.MergeCells(realIndex, ColumnCurrency, (customerTurnover.RowSpan - 1) + realIndex, ColumnCurrency);
                    sheet.MergeCells(realIndex, ColumnTotalTurnover, (customerTurnover.RowSpan - 1) + realIndex, ColumnTotalTurnover);
                }

                if (customerTurnover.RowSpan2 > 0)
                {
                    sheet.MergeCells(realIndex, ColumnSport, (customerTurnover.RowSpan2 - 1) + realIndex, ColumnSport);
                }

                row.Cells[ColumnBetType].SetCellValue(customerTurnover.Bettype);
                row.Cells[ColumnEffectiveBettingValue].SetCellValue(customerTurnover.EffectiveBettingValue);
                row.Cells[ColumnTotalTransaction].SetCellValue(customerTurnover.TotalTransactions);
                row.Cells[ColumnTurnover].SetCellValue(customerTurnover.Turnover);
                row.Cells[ColumnCustomerWl].SetCellValue(customerTurnover.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].SetCellValue(customerTurnover.CustomerComm);
                row.Cells[ColumnCustomerTotal].SetCellValue(customerTurnover.CustomerWinLoss +
                        customerTurnover.CustomerComm);

                rowIndex++;
            }

            AssignTotalRow(sheet, rowIndex + RowContentTemplate);
            return rowIndex;
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                HideRow(sheet, RowTableHeaderTemplate + 1);
            }
        }

        /// <summary>
        /// Assign data to total row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="totalRow">The total value.</param>
        protected void AssignTotalRow(ISheet sheet, int totalRow)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);
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

            sheet.MergeCells(totalRow, 0, totalRow, 1);
        }
    }
}