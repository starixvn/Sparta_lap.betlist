// --------------------------------------------------------------------------
// <copyright file="MobileWinLossExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Fanex.Athena.Models.DataDomain;
    using Models.DTO.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// The mobile win-loss.
    /// </summary>
    public class MobileWinLossExcel : ReportBase
    {
        private const int ColumnAmtDueToBa = 12;
        private const int ColumnAmtDueToRacing = 13;
        private const int ColumnBaCommission = 10;
        private const int ColumnCurrency = 3;
        private const int ColumnCustomerCommission = 7;
        private const int ColumnCustomerTotal = 8;
        private const int ColumnCustomerWl = 6;
        private const int ColumnExternalId = 1;
        private const int ColumnLicenseeWl = 9;
        private const int ColumnRacingRevenue = 11;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 14;
        private const int ColumnTotalTrans = 5;
        private const int ColumnTurnover = 4;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 6;
        private const int RowTotalEurTemp = 5;
        private const int RowTotalTemp = 4;

        private readonly IList<MobileWinLoss> _exportData;
        private readonly MobileWinLossViewModel _model;
        private readonly IDictionary<string, MobileWinLoss> _summaryTotal;
        private readonly IList<MobileWinLoss> _totalList;

        public MobileWinLossExcel(
                        string templatePath,
                                  string exportingFileName,
                                  MobileWinLossViewModel model,
                                  ShowingOption showingOption,
                                  ConvertingOption convertingOption)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _model = model;
            _exportData = model.MainResult;
            _totalList = model.TotalResult;
            _summaryTotal = model.SummaryTotal;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Currency,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
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
            int numOfDataRow = _exportData.Count;
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

            ProcessUI(sheet);
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
            row.Cells[ColumnCustomerCommission].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        /// <summary>
        /// Assign no info row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int rowIndex = 0;
            int index = 1;
            foreach (MobileWinLoss mobile in _exportData)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);
                row.Cells[ColumnIndex].SetCellValue(index);
                row.Cells[ColumnCurrency].SetCellValue(mobile.CurrencyName);
                AssignCellData(row, mobile);
                rowIndex++;
                index++;
            }

            int totalRowEur = AssignTotal(sheet, rowIndex + RowContentTemplate);

            AssignSummaryTotal(sheet, totalRowEur);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            string siteText = ViewResources.Site + ": " +
                              (_model.Param.Site == (int)SiteType.Wap
                               ? ViewResources.Wap
                               : _model.Param.Site == (int)SiteType.SmartPhone ? ViewResources.SmartPhone : ViewResources.Others);

            ReportTitle = string.Format(
                                        ViewResources.MobileWinLossTitle,
                                        _model.Param.FromDate.ToString(FormatDate),
                                        _model.Param.ToDate.ToString(FormatDate),
                                        GetExactTitleCurrencyName(_model.Param.CurrencyId),
                                        siteText) + GetLicenseeWLProduct();

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
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerCommission);
                HideRow(sheet, RowHeaderTemplate + 1);
            }

            if (!ShowingOption.BAVisible)
            {
                HideColumn(sheet, ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RBVisible)
            {
                HideColumn(sheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
            }
        }

        /// <summary>
        /// Assign data cell.
        /// </summary>
        /// <param name="row">The current row.</param>
        /// <param name="mobileWinLoss">The mobile win-loss.</param>
        protected static void AssignCellData(IRow row, MobileWinLoss mobileWinLoss)
        {
            row.Cells[ColumnExternalId].SetCellValue(mobileWinLoss.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(mobileWinLoss.SystemId);
            row.Cells[ColumnTurnover].SetCellValue(mobileWinLoss.Turnover);
            row.Cells[ColumnTotalTrans].SetCellValue(mobileWinLoss.TotalTransactions);
            row.Cells[ColumnCustomerWl].SetCellValue(mobileWinLoss.CustomerWinLoss);
            row.Cells[ColumnCustomerCommission].SetCellValue(mobileWinLoss.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(mobileWinLoss.CustomerTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(mobileWinLoss.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(mobileWinLoss.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(mobileWinLoss.RacingRevenue);
            row.Cells[ColumnAmtDueToBa].SetCellValue(mobileWinLoss.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(mobileWinLoss.AmtDueToRacing);
        }

        /// <summary>
        /// Assign summary total row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The row index.</param>
        protected void AssignSummaryTotal(ISheet sheet, int rowIndex)
        {
            List<string> currencyList = _summaryTotal.Keys.ToList();
            currencyList.Reverse();
            foreach (string currency in currencyList)
            {
                // Copy template
                IRow row = sheet.CopyRow(TemplateSheet, RowTotalEurTemp, rowIndex);
                AssignCellData(row, _summaryTotal[currency]);
                row.Cells[0].SetCellValue(ViewResources.Totalin + " " + currency.ToUpper());

                // Merge first two column
                sheet.MergeCells(rowIndex, ColumnIndex, rowIndex, ColumnCurrency);
                rowIndex++;
            }
        }

        /// <summary>
        /// Assign total row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowTotal">The index of total row.</param>
        /// <returns>Return the index row.</returns>
        protected int AssignTotal(ISheet sheet, int rowTotal)
        {
            int beginRowTotal = rowTotal;
            foreach (MobileWinLoss t in _totalList)
            {
                int rowContentTemp = beginRowTotal == rowTotal ? RowTotalTemp : RowContentTemplate;
                IRow totalRow = sheet.CopyRow(TemplateSheet, rowContentTemp, rowTotal);
                MobileWinLoss data = t;

                totalRow.Cells[ColumnIndex].SetCellValue(ViewResources.Total);
                totalRow.Cells[ColumnCurrency].SetCellValue(data.CurrencyName);
                AssignCellData(totalRow, data);
                rowTotal++;
            }

            sheet.MergeCells(beginRowTotal, ColumnIndex, rowTotal - 1, ColumnSystemId);
            sheet.MergeCells(rowTotal, ColumnIndex, rowTotal, ColumnSystemId);

            return rowTotal;
        }
    }
}