// --------------------------------------------------------------------------
// <copyright file="RacingCommissionExcel.cs" company="Nexcel Solutions Vietnam">
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
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Models.DTO.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

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

        public RacingCommissionExcel(
                                     string templatePath,
                                     string exportingFileName,
                                     ConvertingOption convertingOption,
                                     ShowingOption showingOption,
                                     RacingCommissionViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _viewModel = viewModel;
            _racingCommissionList = viewModel.RacingCommissionList;
            _total = viewModel.Total;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _racingCommissionList.Count;

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
                var numOfSheet = GetNumberOfSheets(numOfDataRow, PageSize);
                for (int i = 0; i < numOfSheet; i++)
                {
                    sheet = Workbook.CloneSheet("Template", "Data" + (i + 1).ToString());
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
        public override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowCount = Math.Min(_racingCommissionList.Count, checked((pageIndex + 1) * PageSize));
            int rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);
                RacingCommission racingCommission = _racingCommissionList[index];

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
            return rowIndex;
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                                        ViewResources.RacingCommissionTitle,
                                        _viewModel.Param.FromDate.ToString(FormatDate),
                                        _viewModel.Param.ToDate.ToString(FormatDate),
                                        GetTitleCurrencyName());

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);

            // Freeze title and header
            sheet.CreateFreezePane(0, 3);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerCommission);
            }
        }

        /// <summary>
        /// Assign data to summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">Row index.</param>
        protected void AssignSumData(ISheet sheet, int rowIndex)
        {
            int totalRow = rowIndex + RowContentTemplate;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            sheet.MergeCells(totalRow, ColumnIndex, totalRow, 1);

            row.Cells[ColumnToteWin].SetCellValue(_total.ToteWin);
            row.Cells[ColumnTotePlace].SetCellValue(_total.TotePlace);
            row.Cells[ColumnToteWinPlace].SetCellValue(_total.ToteWinPlace);
            row.Cells[ColumnToteTotal].SetCellValue(_total.ToteTotal);
            row.Cells[ColumnCustomerCommission].SetCellValue(_total.CustomerCommission);
        }
    }
}