// --------------------------------------------------------------------------
// <copyright file="WinLossByBetTypeExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Helpers;
    using Model.Report.DTO;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Win loss by bet type report - Export excel.
    /// </summary>
    public class WinLossByBetTypeExcel : ReportBase
    {
        private const int ColumnAmtDueToBa = 8;
        private const int ColumnBaCommission = 7;
        private const int ColumnCustomerComm = 4;
        private const int ColumnCustomerTotal = 5;
        private const int ColumnCustomerWinLoss = 3;
        private const int ColumnGameTypeName = 1;
        private const int ColumnLicenseeWl = 6;
        private const int ColumnTemplateCount = 9;
        private const int ColumnTurnover = 2;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly WinLossByBetType _total;
        private readonly WinLossByBetTypeViewModel _viewModel;
        private readonly IList<WinLossByBetType> _dataList;

        private readonly ConvertingOption _convertingOption;

        protected internal WinLossByBetTypeExcel()
            : base(string.Empty, 0)
        {
        }

        public WinLossByBetTypeExcel(
                                     string templatePath,
                                     ConvertingOption convertingOption,
                                     ShowingOption showingOption,
                                     WinLossByBetTypeViewModel viewModel)
            : base(templatePath, viewModel.MainResult.Count, showingOption)
        {
            _convertingOption = convertingOption;
            _viewModel = viewModel;
            _dataList = viewModel.MainResult;
            _total = viewModel.Total;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);
            var headerTextList = new[]
            {
                "#",
                ViewResources.GameTypeName,
                ViewResources.Turnover,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmtDueToBA
            };
            SetTextRowHeader(row, headerTextList);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
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
                var dataItem = _dataList[index];

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnGameTypeName].SetCellValue(dataItem.BetTypeName);
                AssignCommonData(row, dataItem);
            }

            AssignSumData(sheet);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(
                                        ViewResources.WinLossByBetTypeTitle,
                                        _viewModel.Param.FromDate.ToString(FormatDate),
                                        _viewModel.Param.ToDate.ToString(FormatDate),
                                        ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, ColumnTemplateCount, reportTitle);

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

            if (!ShowingOption.BAVisible)
            {
                sheet.HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
            }
        }

        private static void AssignCommonData(IRow row, WinLossByBetType dataItem)
        {
            row.Cells[ColumnTurnover].SetCellValue(dataItem.Turnover);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(dataItem.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(dataItem.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(dataItem.CustomerTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(dataItem.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(dataItem.BACommission);
            row.Cells[ColumnAmtDueToBa].SetCellValue(dataItem.AmtDueToBA);
        }

        /// <summary>
        /// Assign data to summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected internal virtual void AssignSumData(ISheet sheet)
        {
            var totalRow = _dataList.Count + RowContentTemplate;

            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            sheet.MergeCols(ColumnIndex, ColumnGameTypeName, totalRow);

            row.Cells[ColumnIndex].SetCellValue(ViewResources.Total);
            row.Cells[ColumnTurnover].SetCellValue(_total.Turnover);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(_total.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(_total.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_total.CustomerTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(_total.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(_total.BACommission);
            row.Cells[ColumnAmtDueToBa].SetCellValue(_total.AmtDueToBA);
        }
    }
}