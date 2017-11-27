// --------------------------------------------------------------------------
// <copyright file="BalanceExcel.cs" company="">
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
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// The balance excel report.
    /// </summary>
    [System.CLSCompliant(false)]
    public class BalanceExcel : ReportBase
    {
        private const int ColumnDate = 0;
        private const int ColumnLicenseeWl = 1;
        private const int ColumnBetTradeCommission = 2;
        private const int ColumnBaCommission = 3;
        private const int ColumnRacingRevenue = 4;
        private const int ColumnAmtDueToBetTrade = 5;
        private const int ColumnAmtDueToBa = 6;
        private const int ColumnAmtDueToRacing = 7;
        private const int ColumnCustomerCommission = 8;

        private const int ColumnTemplateCount = 8;
        private const int RowCustomerPendingFund = 6;
        private const int RowLicensee = 1;
        private const int RowMonth = 2;
        private const int RowTitleCustomer = 5;
        private const int RowTotal = 7;

        private readonly Balance _balance;
        private readonly string _date;

        private readonly ConvertingOption _convertingOption;

        public BalanceExcel(
                            string templatePath,
                            ShowingOption showingOption,
                            ConvertingOption convertingOption,
                            Balance balance,
                            string date)
            : base(templatePath, 0, showingOption)
        {
            _convertingOption = convertingOption;
            _balance = balance;
            _date = date;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        protected override void AssignData()
        {
            var sheet = Workbook.CloneSheet("Template", "Data");

            AssignTitle(sheet);

            AssignSheetData(sheet);

            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            for (int index = 0; index < _balance.BalanceList.Count; index++)
            {
                var balanceDetails = _balance.BalanceList[index];

                var row = sheet.CopyRow(sheet, RowMonth, index + 2);

                var timeText = index == 0 ? _date : balanceDetails.BalanceDateString;

                AssignCommonData(row, balanceDetails, timeText);
            }

            AssignCustomerInformation(sheet);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(ViewResources.BalanceTitle, ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            // Set for Balance, report title
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);
        }

        protected override void AssignHeader(ISheet sheet)
        {
            // Get row copy from template
            var columIndex = ColumnLicenseeWl;
            var row = sheet.CopyRow(sheet, RowLicensee, RowLicensee);
            row.Cells[columIndex++].SetCellValue(ViewResources.LicenseeWinLoss);
            row.Cells[columIndex++].SetCellValue(ViewResources.BetTradeCommission);
            row.Cells[columIndex++].SetCellValue(ViewResources.BACommission);
            row.Cells[columIndex++].SetCellValue(ViewResources.RacingRevenue);
            row.Cells[columIndex++].SetCellValue(ViewResources.AmtDueToBetTrade);
            row.Cells[columIndex++].SetCellValue(ViewResources.AmtDueToBA);
            row.Cells[columIndex++].SetCellValue(ViewResources.AmtDueToRacing);
            row.Cells[columIndex].SetCellValue(ViewResources.CustomerCommission);

            // Set for customer information
            sheet.SetTextRowValue(TemplateSheet, RowTitleCustomer, RowTitleCustomer, ColumnTemplateCount, ViewResources.CustomerInfo);

            row = sheet.CopyRow(TemplateSheet, RowCustomerPendingFund, RowCustomerPendingFund);
            row.Cells[0].SetCellValue(ViewResources.CustomerPedingFunds);

            row = sheet.CopyRow(TemplateSheet, RowTotal, RowTotal);
            row.Cells[0].SetCellValue(ViewResources.TotalCustomerActiveClosedSuspended);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.BAVisible)
            {
                sheet.HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RBVisible)
            {
                sheet.HideColumn(ColumnRacingRevenue, ColumnAmtDueToRacing);
            }

            if (!ShowingOption.BTVisible)
            {
                sheet.HideColumn(ColumnBetTradeCommission, ColumnAmtDueToBetTrade);
            }

            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerCommission);
            }
        }

        private static void AssignCommonData(IRow row, BalanceDetails balanceDetails, string timeText)
        {
            row.Cells[ColumnDate].SetCellValue(timeText);
            row.Cells[ColumnLicenseeWl].SetCellValue(balanceDetails.LicWinLoss);
            row.Cells[ColumnBetTradeCommission].SetCellValue(balanceDetails.BetTradeCommission);
            row.Cells[ColumnBaCommission].SetCellValue(balanceDetails.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(balanceDetails.RacingWinLoss);
            row.Cells[ColumnAmtDueToBetTrade].SetCellValue(balanceDetails.AmtDueToBetTrade);
            row.Cells[ColumnAmtDueToBa].SetCellValue(balanceDetails.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(balanceDetails.AmtDueToRacing);
            row.Cells[ColumnCustomerCommission].SetCellValue(balanceDetails.CustomerCommission);
        }

        /// <summary>
        /// Assign customer information.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        private void AssignCustomerInformation(ISheet sheet)
        {
            const string FormatTotal = "{0} / {1} / {2}";
            var lastColumnIndex = GetLastIndex();

            var row = sheet.CopyRow(sheet, RowCustomerPendingFund, RowCustomerPendingFund);
            row.Cells[0].SetCellValue(ViewResources.CustomerPedingFunds);
            row.Cells[lastColumnIndex].SetCellValue(_balance.CustomerPendingFunds);
            sheet.MergeCells(RowCustomerPendingFund, ColumnIndex, RowCustomerPendingFund, lastColumnIndex - 1);

            row = sheet.CopyRow(sheet, RowTotal, RowTotal);
            row.Cells[0].SetCellValue(ViewResources.TotalCustomerActiveClosedSuspended);
            var customerTotal = string.Format(
                        FormatTotal,
                        _balance.TotalCustomerActive,
                        _balance.TotalCustomerClosed,
                        _balance.TotalCustomerSuspended);
            row.Cells[lastColumnIndex].SetCellValue(customerTotal);
            sheet.MergeCells(RowTotal, ColumnIndex, RowTotal, lastColumnIndex - 1);
        }

        /// <summary>
        /// Get last column index.
        /// </summary>
        /// <returns>The integer value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        private int GetLastIndex()
        {
            var listColumn = new List<int>
            {
                ColumnDate,
                ColumnLicenseeWl,
                ColumnBetTradeCommission,
                ColumnBaCommission,
                ColumnRacingRevenue,
                ColumnAmtDueToBetTrade,
                ColumnAmtDueToBa,
                ColumnAmtDueToRacing,
                ColumnCustomerCommission
            };

            if (!ShowingOption.BAVisible)
            {
                listColumn.Remove(ColumnBaCommission);
                listColumn.Remove(ColumnAmtDueToBa);
            }

            if (!ShowingOption.RBVisible)
            {
                listColumn.Remove(ColumnRacingRevenue);
                listColumn.Remove(ColumnAmtDueToRacing);
            }

            if (!ShowingOption.RBVisible)
            {
                listColumn.Remove(ColumnBetTradeCommission);
                listColumn.Remove(ColumnAmtDueToBetTrade);
            }

            if (!ShowingOption.CustomerCommVisible)
            {
                listColumn.Remove(ColumnCustomerCommission);
            }

            // Get last item
            return listColumn[listColumn.Count - 1];
        }
    }
}