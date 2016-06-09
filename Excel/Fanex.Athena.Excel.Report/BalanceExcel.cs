// --------------------------------------------------------------------------
// <copyright file="BalanceExcel.cs" company="Nexcel Solutions Vietnam">
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
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
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

        public BalanceExcel(
                            string templatePath,
                            string exportingFileName,
                            ShowingOption showingOption,
                            ConvertingOption convertingOption,
                            Balance balance,
                            string date)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _balance = balance;
            _date = date;
        }

        public string SelectedMonth
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _balance.BalanceList.Count;

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignTitle(sheet);

            if (numOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign no info row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignNoInfoRow(ISheet sheet)
        {
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            for (int index = 0; index < _balance.BalanceList.Count; index++)
            {
                BalanceDetails balanceDetails = _balance.BalanceList[index];

                IRow row = sheet.CopyRow(sheet, RowMonth, index + 2);

                string timeText = index == 0 ? _date : balanceDetails.BalanceDateString;

                AssignCommonData(row, balanceDetails, timeText);
            }

            AssignCustomerInformation(sheet);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(ViewResources.BalanceTitle, GetTitleCurrencyName()) + GetLicenseeWLProduct();

            // Set for Balance, report title
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Get row copy from template
            var columIndex = ColumnLicenseeWl;
            IRow row = sheet.CopyRow(sheet, RowLicensee, RowLicensee);
            row.Cells[columIndex++].SetCellValue(ViewResources.LicenseeWinLoss);
            row.Cells[columIndex++].SetCellValue(ViewResources.BetTradeCommission);
            row.Cells[columIndex++].SetCellValue(ViewResources.BACommission);
            row.Cells[columIndex++].SetCellValue(ViewResources.RacingRevenue);
            row.Cells[columIndex++].SetCellValue(ViewResources.AmtDueToBetTrade);
            row.Cells[columIndex++].SetCellValue(ViewResources.AmtDueToBA);
            row.Cells[columIndex++].SetCellValue(ViewResources.AmtDueToRacing);
            row.Cells[columIndex++].SetCellValue(ViewResources.CustomerCommission);

            // Set for customer information
            SetTextRowValue(sheet, RowTitleCustomer, RowTitleCustomer, ColumnTemplateCount, ViewResources.CustomerInfo);

            row = sheet.CopyRow(TemplateSheet, RowCustomerPendingFund, RowCustomerPendingFund);
            row.Cells[0].SetCellValue(ViewResources.CustomerPedingFunds);

            row = sheet.CopyRow(TemplateSheet, RowTotal, RowTotal);
            row.Cells[0].SetCellValue(ViewResources.TotalCustomerActiveClosedSuspended);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.BAVisible)
            {
                HideColumn(sheet, ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RBVisible)
            {
                HideColumn(sheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
            }

            if (!ShowingOption.BTVisible)
            {
                HideColumn(sheet, ColumnBetTradeCommission, ColumnAmtDueToBetTrade);
            }

            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerCommission);
            }
        }

        /// <summary>
        /// Assign data to common cell.
        /// </summary>
        /// <param name="row">The row of sheet.</param>
        /// <param name="balanceDetails">The detail balance.</param>
        /// <param name="timeText">The time text.</param>
        protected static void AssignCommonData(IRow row, BalanceDetails balanceDetails, string timeText)
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
        protected void AssignCustomerInformation(ISheet sheet)
        {
            const string FormatTotal = "{0} / {1} / {2}";
            int lastColumnIndex = GetLastIndex();

            IRow row = sheet.CopyRow(sheet, RowCustomerPendingFund, RowCustomerPendingFund);
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
        protected int GetLastIndex()
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