// --------------------------------------------------------------------------
// <copyright file="WinLossByBetTypeExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using Base;
    using Common.Extensions;
    using Entities;
    using Models.DTO.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win loss by bet type report - Export excel.
    /// </summary>
    public class WinLossByBetTypeExcel : ReportBase
    {
        private const int ColumnAmtDueToBa = 9;
        private const int ColumnAmtDueToRacing = 10;
        private const int ColumnBaCommission = 7;
        private const int ColumnCustomerComm = 4;
        private const int ColumnCustomerTotal = 5;
        private const int ColumnCustomerWinLoss = 3;
        private const int ColumnGameTypeName = 1;
        private const int ColumnLicenseeWl = 6;
        private const int ColumnRacingRevenue = 8;
        private const int ColumnTemplateCount = 11;
        private const int ColumnTurnover = 2;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly WinLossByBetType _total;
        private readonly WinLossByBetTypeViewModel _viewModel;
        private readonly IList<WinLossByBetType> _winlossByBetTypeList;

        public WinLossByBetTypeExcel(
                                     string templatePath,
                                     string exportingFileName,
                                     ConvertingOption convertingOption,
                                     ShowingOption showingOption,
                                     WinLossByBetTypeViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _viewModel = viewModel;
            _winlossByBetTypeList = viewModel.MainResult;
            _total = viewModel.Total;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.GameTypeName,
                ViewResources.Turnover,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.RacingRevenue,
                ViewResources.AmtDueToBA,
                ViewResources.AmtDueToRacing
            };
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _winlossByBetTypeList.Count;

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (numOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUi(sheet);
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
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUi(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int rowCount = _winlossByBetTypeList.Count;

            for (int index = 0; index < rowCount; index++)
            {
                WinLossByBetType winlossByBetType = _winlossByBetTypeList[index];

                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnGameTypeName].SetCellValue(winlossByBetType.BetTypeName);
                AssignCommonData(row, winlossByBetType);
            }

            AssignSumData(sheet);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                                        ViewResources.WinLossByBetTypeTitle,
                                        _viewModel.Param.FromDate.ToString(FormatDate),
                                        _viewModel.Param.ToDate.ToString(FormatDate),
                                        GetTitleCurrencyName());

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
        public override void ProcessUi(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                HideRow(sheet, RowTableHeaderTemplate + 1);
            }

            if (!ShowingOption.BaVisible)
            {
                HideColumn(sheet, ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RbVisible)
            {
                HideColumn(sheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
            }
        }

        /// <summary>
        /// Assign data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="winlossByBetType">Win Loss By Bet Type data.</param>
        protected void AssignCommonData(IRow row, WinLossByBetType winlossByBetType)
        {
            row.Cells[ColumnTurnover].SetCellValue(winlossByBetType.Turnover);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(winlossByBetType.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(winlossByBetType.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(winlossByBetType.CustomerTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(winlossByBetType.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(winlossByBetType.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(winlossByBetType.RacingWinLoss);
            row.Cells[ColumnAmtDueToBa].SetCellValue(winlossByBetType.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(winlossByBetType.AmtDueToRacing);
        }

        /// <summary>
        /// Assign data to summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected void AssignSumData(ISheet sheet)
        {
            int totalRow = _winlossByBetTypeList.Count + RowContentTemplate;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            sheet.MergeCells(totalRow, ColumnIndex, totalRow, 1);

            row.Cells[ColumnIndex].SetCellValue(ViewResources.Total);
            row.Cells[ColumnTurnover].SetCellValue(_total.TotalTurnover);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(_total.TotalCustomerWl);
            row.Cells[ColumnCustomerComm].SetCellValue(_total.TotalCustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_total.TotalCustomerWl + _total.TotalCustomerComm);
            row.Cells[ColumnLicenseeWl].SetCellValue(_total.TotalLicenseeWl);
            row.Cells[ColumnBaCommission].SetCellValue(_total.TotalBaCommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(_total.TotalRacingWinLoss);
            row.Cells[ColumnAmtDueToBa].SetCellValue(_total.TotalAmtDueToBa);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(_total.TotalAmtDueToRacing);
        }
    }
}