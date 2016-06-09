// --------------------------------------------------------------------------
// <copyright file="AgCasinoBalanceExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Ag Casino Win Loss - Export excel.
    /// </summary>
    public class AgCasinoBalanceExcel : ReportBase
    {
        private const int ColumnTemplateCount = 13;

        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnOpeningBalance = 5;
        private const int ColumnTransferIn = 6;
        private const int ColumnTransferOut = 7;
        private const int ColumnTurnover = 8;
        private const int ColumnTotalTransaction = 9;
        private const int ColumnCustomerWl = 10;
        private const int ColumnLicenseeWl = 11;
        private const int ColumnClosingBalance = 12;

        private const int RowContentTemplate = 2;
        private const int RowTotalNoSolidBorder = 4;
        private const int RowTotalSolidBorder = 3;
        private const int RowGrantTotal = 5;
        private const int RowNoInfoTemplate = 6;

        // This is drill down of report
        private readonly IList<AgCasinoBalance> _casinoList;

        private readonly AgCasinoBalanceMetaModel _metaModel;
        private readonly IDictionary<string, AgCasinoBalance> _summaryTotal;

        public AgCasinoBalanceExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                AgCasinoBalanceMetaModel metaModel,
                AgCasinoBalanceViewModel viewModel)
            : base(templatePath, exportingFileName, null, convertingOption)
        {
            _metaModel = metaModel;
            _casinoList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
            PageSize = metaModel.DrillDownLevel == 2 ? metaModel.PageSize : metaModel.PageSizeExcel;
        }

        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _casinoList.Count;

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
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnOpeningBalance].SetCellValue(ViewResources.OpeningBalance);
            row.Cells[ColumnTransferIn].SetCellValue(ViewResources.TransferIn);
            row.Cells[ColumnTransferOut].SetCellValue(ViewResources.TransferOut);
            row.Cells[ColumnTurnover].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            row.Cells[ColumnLicenseeWl].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnClosingBalance].SetCellValue(ViewResources.ClosingBalance);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <returns>The row index.</returns>
        public override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowCount = Math.Min(_casinoList.Count, checked((pageIndex + 1) * PageSize));
            var rowIndex = 0;
            int index;
            int currentIndex = 1;

            for (index = pageIndex * PageSize; index < rowCount; index++)
            {
                var casinoBalance = _casinoList[index];
                var isDrillDownLevel = casinoBalance.RowSpan == 0;

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);

                var reportDateString = casinoBalance.Date.Value.ToString("MM/dd/yyyy");

                row.Cells[ColumnExternalId].SetCellValue(casinoBalance.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(casinoBalance.SystemId);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                row.Cells[ColumnCurrency].SetCellValue(casinoBalance.CurrencyName);
                AssignCommonData(row, casinoBalance);
                rowIndex++;

                if (isDrillDownLevel)
                {
                    row.Cells[ColumnIndex].SetCellValue(index + 1);
                }
                else
                {
                    row.Cells[ColumnIndex].SetCellValue(currentIndex);
                }

                if (casinoBalance.RowSpan > 1)
                {
                    var currentRow = index + RowContentTemplate;
                    sheet.MergeCells(currentRow, ColumnIndex, currentRow + casinoBalance.RowSpan - 1, ColumnIndex);
                    sheet.MergeCells(currentRow, ColumnDate, currentRow + casinoBalance.RowSpan - 1, ColumnDate);
                    currentIndex++;
                }
            }

            rowIndex = AssignTotalData(sheet, rowIndex + 2);
            AssignGrandTotalData(sheet, rowIndex);

            return rowIndex;
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = _metaModel.DrillDownLevel > 0 && _metaModel.CurrencyId == 0
                ? GetExactTitleCurrencyName(_metaModel.DrillDownCurrencyId)
                : GetExactTitleCurrencyName(_metaModel.CurrencyId);

            ReportTitle = string.Format(ViewResources.AGCasinoBalance + ", {0} --> {1} - {2}", fromdate, todate, title);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUI(ISheet sheet)
        {
            switch (Level)
            {
                case 0:
                    HideColumn(sheet, ColumnExternalId, ColumnSystemId);
                    break;

                case 1:
                    HideColumn(sheet, ColumnExternalId, ColumnDate);
                    break;

                default:
                    HideColumn(sheet, ColumnDate);
                    break;
            }
        }

        /// <summary>
        /// Assign common data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="data">Licensee win loss data.</param>
        /// <param name="isTotalData">Total data or not.</param>
        protected void AssignCommonData(IRow row, AgCasinoBalance data, bool isTotalData = false)
        {
            row.Cells[ColumnTransferIn].SetCellValue(data.TransferIn);
            row.Cells[ColumnTransferOut].SetCellValue(data.TransferOut);
            row.Cells[ColumnTurnover].SetCellValue(data.TurnOver);
            row.Cells[ColumnTotalTransaction].SetCellValue(data.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(data.CustomerWinLoss);
            row.Cells[ColumnLicenseeWl].SetCellValue(data.LicenseeWL);

            if (isTotalData)
            {
                row.Cells[ColumnOpeningBalance].SetCellValue("-");
                row.Cells[ColumnClosingBalance].SetCellValue("-");
            }
            else
            {
                row.Cells[ColumnOpeningBalance].SetCellValue(data.OpeningBalance);
                row.Cells[ColumnClosingBalance].SetCellValue(data.ClosingBalance);
            }
        }

        /// <summary>
        /// Assign grand total data of EUR.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <returns>The last row index after assign data.</returns>
        protected int AssignGrandTotalData(ISheet sheet, int rowIndex)
        {
            var currencyList = _summaryTotal.Keys.ToList();
            currencyList.Reverse();

            foreach (string currency in currencyList)
            {
                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowGrantTotal, rowIndex);
                AssignCommonData(row, _summaryTotal[currency], true);
                row.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + currency.ToUpper());

                // Merge first two column
                sheet.MergeCells(rowIndex, ColumnIndex, rowIndex, ColumnCurrency);
                checked
                {
                    rowIndex++;
                }
            }

            return rowIndex;
        }

        /// <summary>
        /// Assign grand total data.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <returns>The last row index after assign data.</returns>
        protected int AssignTotalData(ISheet sheet, int rowIndex)
        {
            int beginTotalRow = rowIndex;
            List<AgCasinoBalance> totalCurrencyList = GetTotalCurrency(_casinoList);

            foreach (AgCasinoBalance totalCurrency in totalCurrencyList)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowTotalNoSolidBorder, rowIndex);

                if (rowIndex == beginTotalRow)
                {
                    row = sheet.CopyRow(TemplateSheet, RowTotalSolidBorder, rowIndex);
                    row.Cells[0].SetCellValue(ViewResources.Total);
                }

                row.Cells[ColumnCurrency].SetCellValue(totalCurrency.CurrencyName);

                AssignCommonData(row, totalCurrency, true);

                rowIndex++;
            }

            // Merge first two column
            sheet.MergeCells(rowIndex - totalCurrencyList.Count, 0, rowIndex - 1, 3);

            return rowIndex;
        }

        /// <summary>
        /// Get total currency data.
        /// </summary>
        /// <param name="dataList">Report data.</param>
        /// <returns>List of AgCasinoBalance entity.</returns>
        private static List<AgCasinoBalance> GetTotalCurrency(IEnumerable<AgCasinoBalance> dataList)
        {
            var listTotalCurrency = dataList.GroupBy(x => new { x.CurrencyName })
                    .Select(group => new AgCasinoBalance
                    {
                        CurrencyName = group.Key.CurrencyName,
                        OpeningBalance = group.Sum(a => a.OpeningBalance),
                        TransferIn = group.Sum(a => a.TransferIn),
                        TransferOut = group.Sum(a => a.TransferOut),
                        TurnOver = group.Sum(a => a.TurnOver),
                        TotalTransaction = group.Sum(a => a.TotalTransaction),
                        CustomerWinLoss = group.Sum(a => a.CustomerWinLoss),
                        LicenseeWL = group.Sum(a => a.LicenseeWL),
                        ClosingBalance = group.Sum(a => a.ClosingBalance),
                    }).ToList();

            return listTotalCurrency;
        }
    }
}