// --------------------------------------------------------------------------
// <copyright file="RacingCommissionExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
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
    /// Racing commission report - Export excel.
    /// </summary>
    public class RacingCommissionExcel : ReportBase
    {
        private const int ColumnCustomerCommission = 7;
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 8;
        private const int ColumnTotePlace = 4;
        private const int ColumnToteTotal = 6;
        private const int ColumnToteWin = 3;
        private const int ColumnToteWinPlace = 5;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTotalTemplate = 4;

        private readonly IList<RacingCommission> _racingCommissionList;
        private readonly RacingCommission _total;
        private readonly RacingCommissionViewModel _viewModel;

        private readonly ConvertingOption _convertingOption;

        protected internal RacingCommissionExcel()
            : base(string.Empty, 0)
        {
        }

        public RacingCommissionExcel(
                                     string templatePath,
                                     ConvertingOption convertingOption,
                                     ShowingOption showingOption,
                                     RacingCommissionViewModel viewModel)
            : base(templatePath, viewModel.RacingCommissionList.Count, showingOption)
        {
            _convertingOption = convertingOption;
            _viewModel = viewModel;
            _racingCommissionList = viewModel.RacingCommissionList;
            _total = viewModel.Total;
            IsMultiSheet = true;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnToteWin].SetCellValue(ViewResources.Tote);
            row.Cells[ColumnCustomerCommission].SetCellValue(ViewResources.Customer + " " + ViewResources.Comm);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            row.Cells[ColumnToteWin].SetCellValue(ViewResources.Win);
            row.Cells[ColumnTotePlace].SetCellValue(ViewResources.Place);
            row.Cells[ColumnToteWinPlace].SetCellValue(ViewResources.Win + " " + ViewResources.Place);
            row.Cells[ColumnToteTotal].SetCellValue(ViewResources.Total);
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
        /// <param name="pageIndex">Start index.</param>
        /// <returns>Row index.</returns>
        protected override void AssignSheetData(ISheet sheet, int pageIndex)
        {
            var rowCount = Math.Min(_racingCommissionList.Count, checked((pageIndex + 1) * PageSize));
            var rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);
                var racingCommission = _racingCommissionList[index];

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(racingCommission.ExternalID);
                row.Cells[ColumnSystemId].SetCellValue(racingCommission.SystemID);
                row.Cells[ColumnToteWin].SetCellValue(racingCommission.ToteWin);
                row.Cells[ColumnTotePlace].SetCellValue(racingCommission.TotePlace);
                row.Cells[ColumnToteWinPlace].SetCellValue(racingCommission.ToteWinPlace);
                row.Cells[ColumnToteTotal].SetCellValue(racingCommission.ToteTotal);
                row.Cells[ColumnCustomerCommission].SetCellValue(racingCommission.CustomerCommission);

                rowIndex++;
            }

            AssignSumData(sheet, rowIndex);

            ProcessUI(sheet);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(
                                        ViewResources.RacingCommissionTitle,
                                        _viewModel.Param.FromDate.ToString(FormatDate),
                                        _viewModel.Param.ToDate.ToString(FormatDate),
                                        ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);

            // Freeze title and header
            sheet.CreateFreezePane(0, 3);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerCommission);
            }
        }

        /// <summary>
        /// Assign data to summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">Row index.</param>
        protected internal virtual void AssignSumData(ISheet sheet, int rowIndex)
        {
            var totalRow = rowIndex + RowContentTemplate;

            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);
            row.Cells[0].SetCellValue(ViewResources.Totalin + " " + _convertingOption.BaseCurrency.ToUpperInvariant());
            sheet.MergeCells(totalRow, ColumnIndex, totalRow, 1);

            row.Cells[ColumnToteWin].SetCellValue(_total.ToteWin);
            row.Cells[ColumnTotePlace].SetCellValue(_total.TotePlace);
            row.Cells[ColumnToteWinPlace].SetCellValue(_total.ToteWinPlace);
            row.Cells[ColumnToteTotal].SetCellValue(_total.ToteTotal);
            row.Cells[ColumnCustomerCommission].SetCellValue(_total.CustomerCommission);
        }
    }
}