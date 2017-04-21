// --------------------------------------------------------------------------
// <copyright file="TurnoverExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using Base;
    using Base.Entities;
    using Common.Extensions;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Report.DTO;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Turnover report - Export excel.
    /// </summary>
    public class TurnoverExcel : ReportBase
    {
        private const int ColumnDay = 1;
        private const int ColumnOpeningBalance = 2;
        private const int ColumnDeposit = 3;
        private const int ColumnWithdrawal = 4;
        private const int ColumnBonus = 5;
        private const int ColumnAdjustment = 6;
        private const int ColumnTurnOver = 7;
        private const int ColumnDrawGame = 8;
        private const int ColumnNetTurnOver = 9;
        private const int ColumnPendingFunds = 10;
        private const int ColumnCustomerWinLoss = 11;
        private const int ColumnCustomerComm = 12;
        private const int ColumnCustomerTotal = 13;
        private const int ColumnClosingBalance = 14;
        private const int ColumnAffiliateTransfer = 15;
        private const int ColumnCasinoTransfer = 16;
        private const int ColumnTurnoverBa = 17;
        private const int ColumnCommBa = 18;
        private const int ColumnAmountDueToBa = 19;
        private const int ColumnTemplateCount = 20;

        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemp = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly TurnoverDTO _total;
        private readonly IList<TurnoverDTO> _turnoverList;
        private readonly TurnoverViewModel _viewModel;

        public TurnoverExcel(
                             string templatePath,
                             string exportingFileName,
                             ConvertingOption convertingOption,
                             ShowingOption showingOption,
                             TurnoverViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _viewModel = viewModel;
            _turnoverList = viewModel.DataList;
            _total = viewModel.TotalData;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.Date,
                ViewResources.OpeningBalance,
                ViewResources.Deposit,
                ViewResources.Withdrawal,
                ViewResources.Bonus,
                ViewResources.Adjustment,
                ViewResources.Turnover,
                ViewResources.DrawGame,
                ViewResources.NetTurnover,
                ViewResources.PendingFunds,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.ClosingBalance,
                ViewResources.AffiliateTransfer,
                ViewResources.CasinoTransfer,
                ViewResources.ProductBA
            };
        }

        public int LicenseeId
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        protected override void AssignData()
        {
            int numOfDataRow = _turnoverList.Count;

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
        protected override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            IRow row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row2.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row2.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row2.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);

            row2.Cells[ColumnTurnoverBa].SetCellValue(ViewResources.Turnover);
            row2.Cells[ColumnCommBa].SetCellValue(ViewResources.Comm);
            row2.Cells[ColumnAmountDueToBa].SetCellValue(ViewResources.AmtDueTo);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemp, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="pageIndex">Start index.</param>
        /// <returns>Row index.</returns>
        protected override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowCount = Math.Min(_turnoverList.Count, checked((pageIndex + 1) * PageSize));
            var currentRow = 3;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var turnover = _turnoverList[index];

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnDay].SetCellValue(turnover.Date);
                row.Cells[ColumnOpeningBalance].SetCellValue(turnover.OpeningBalance);
                row.Cells[ColumnClosingBalance].SetCellValue(turnover.ClosingBalance);

                AssignCommonData(row, turnover);

                currentRow++;
            }

            AssignTotalCellData(sheet, currentRow);
            return currentRow;
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignTitle(ISheet sheet)
        {
            string titleNoUserName = string.Format(
                                                    ViewResources.TurnoverTitleNoUser,
                                                   _viewModel.Param.FromDate.ToString(FormatDate),
                                                   _viewModel.Param.ToDate.ToString(FormatDate),
                                                   GetExactTitleCurrencyName(_viewModel.Param.CurrencyId));
            string title = string.Format(
                                        ViewResources.TurnoverTitle,
                                         _viewModel.Param.FromDate.ToString(FormatDate),
                                         _viewModel.Param.ToDate.ToString(FormatDate),
                                         ViewResources.Customer,
                                         _viewModel.Param.Username,
                                         _viewModel.Param.CurrencyName);
            ReportTitle = _viewModel.Param.Username.IsNullOrTrimEmpty() ? titleNoUserName : title;
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
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);

                // If row isn't hide (in case BA visible), then merge cell and set value for header
                sheet.MergeCells(RowTableHeaderTemplate, ColumnCustomerWinLoss, RowTableHeaderTemplate + 1, ColumnCustomerWinLoss);
                sheet.GetCell(RowTableHeaderTemplate, ColumnCustomerWinLoss).SetCellValue(ViewResources.CustomerW_L);
            }

            if (!ShowingOption.BAVisible)
            {
                HideColumn(sheet, ColumnTurnoverBa, ColumnCommBa, ColumnAmountDueToBa);
            }

            if (!ShowingOption.CustomerCommVisible && !ShowingOption.BAVisible)
            {
                HideRow(sheet, RowTableHeaderTemplate + 1);
            }

            if (LicenseeId != 1)
            {
                HideColumn(sheet, ColumnCasinoTransfer);
            }
        }

        /// <summary>
        /// Assign common data to excel row .
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="turnover">Turnover data.</param>
        protected void AssignCommonData(IRow row, TurnoverDTO turnover)
        {
            row.Cells[ColumnDeposit].SetCellValue(turnover.Deposit);
            row.Cells[ColumnWithdrawal].SetCellValue(turnover.Withdrawal);
            row.Cells[ColumnBonus].SetCellValue(turnover.Bonus);
            row.Cells[ColumnAdjustment].SetCellValue(turnover.Adjustment);
            row.Cells[ColumnTurnOver].SetCellValue(turnover.Turnover);
            row.Cells[ColumnDrawGame].SetCellValue(turnover.DrawGame);
            row.Cells[ColumnNetTurnOver].SetCellValue(turnover.NetTurnover);
            row.Cells[ColumnPendingFunds].SetCellValue(turnover.PendingFunds);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(turnover.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(turnover.PlayerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(turnover.WinLoss + turnover.PlayerComm);
            row.Cells[ColumnAffiliateTransfer].SetCellValue(turnover.AffiliateTransfer);
            row.Cells[ColumnCasinoTransfer].SetCellValue(turnover.Transfer3rd);
            row.Cells[ColumnTurnoverBa].SetCellValue(turnover.TurnoverBA);
            row.Cells[ColumnCommBa].SetCellValue(turnover.CommBA);
            row.Cells[ColumnAmountDueToBa].SetCellValue(turnover.AmountDueToBA);
        }

        protected void AssignTotalCellData(ISheet sheet, int currentRow)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);
            row.Cells[0].SetCellValue(ViewResources.Total);
            row.Cells[ColumnOpeningBalance].SetCellValue("-");
            row.Cells[ColumnClosingBalance].SetCellValue("-");

            AssignCommonData(row, _total);

            sheet.MergeCells(currentRow, ColumnIndex, currentRow, ColumnDay);
        }
    }
}