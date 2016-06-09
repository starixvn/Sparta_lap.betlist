// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeWinLossExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
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
    /// Gold deluxe Win Loss - Export excel.
    /// </summary>
    public class GoldDeluxeWinLossExcel : ReportBase
    {
        private const int ColumnTemplateCount = 9;

        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnover = 5;
        private const int ColumnTotalTransaction = 6;
        private const int ColumnCustomerWl = 7;
        private const int ColumnLicenseeWl = 8;

        private const int RowContentTemplate = 2;
        private const int RowTotalNoSolidBorder = 4;
        private const int RowTotalSolidBorder = 3;
        private const int RowGrantTotal = 5;
        private const int RowNoInfoTemplate = 6;

        // This is drill down of report
        private readonly IList<GoldDeluxeWinLoss> _winLossList;

        private readonly GoldDeluxeWinLossMetaModel _metaModel;
        private readonly IDictionary<string, GoldDeluxeWinLoss> _summaryTotal;

        public GoldDeluxeWinLossExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                GoldDeluxeWinLossMetaModel metaModel,
                GoldDeluxeWinLossViewModel viewModel)
            : base(templatePath, exportingFileName, null, convertingOption)
        {
            _metaModel = metaModel;
            _winLossList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
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
            int numOfDataRow = _winLossList.Count;

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
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnover].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            row.Cells[ColumnLicenseeWl].SetCellValue(ViewResources.LicenseeW_L);
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
        public override void AssignSheetData(ISheet sheet)
        {
            var rowIndex = 1;
            int index;
            var rowCount = _winLossList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var licenseeWinLoss = _winLossList[index];

                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                // Report Date
                var dateStringFormat = _metaModel.ReportBy == 1 ? "MM/dd/yyyy" : "MM/yyyy";
                var reportDateString = licenseeWinLoss.Date.Value.ToString(dateStringFormat);

                row.Cells[ColumnIndex].SetCellValue(licenseeWinLoss.Index);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                row.Cells[ColumnExternalId].SetCellValue(licenseeWinLoss.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(licenseeWinLoss.SystemId);
                row.Cells[ColumnCurrency].SetCellValue(licenseeWinLoss.CurrencyName);
                AssignCommonData(row, licenseeWinLoss);
                rowIndex++;

                if (licenseeWinLoss.RowSpan > 1)
                {
                    var currentRow = index + RowContentTemplate;
                    sheet.MergeCells(currentRow, ColumnIndex, currentRow + licenseeWinLoss.RowSpan - 1, ColumnIndex);
                    sheet.MergeCells(currentRow, ColumnDate, currentRow + licenseeWinLoss.RowSpan - 1, ColumnDate);
                    continue;
                }
            }

            rowIndex = AssignTotalData(sheet, index + 2);
            AssignGrandTotalInEurData(sheet, rowIndex);
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

            ReportTitle = string.Format(ViewResources.GoldDeluxeWinLoss + ", {0} --> {1} - {2}", fromdate, todate, title);
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
        protected void AssignCommonData(IRow row, GoldDeluxeWinLoss data)
        {
            row.Cells[ColumnTurnover].SetCellValue(data.TurnOver);
            row.Cells[ColumnTotalTransaction].SetCellValue(data.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(data.CustomerWinLoss);
            row.Cells[ColumnLicenseeWl].SetCellValue(data.LicenseeWL);
        }

        /// <summary>
        /// Assign grand total data of EUR.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <returns>The last row index after assign data.</returns>
        protected int AssignGrandTotalInEurData(ISheet sheet, int rowIndex)
        {
            var currencyList = _summaryTotal.Keys.ToList();
            currencyList.Reverse();

            foreach (string currency in currencyList)
            {
                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowGrantTotal, rowIndex);
                AssignCommonData(row, _summaryTotal[currency]);
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
            List<GoldDeluxeWinLoss> totalCurrencyList = GetTotalCurrency(_winLossList);

            foreach (GoldDeluxeWinLoss totalCurrency in totalCurrencyList)
            {
                // Copy template
                IRow row = sheet.CopyRow(TemplateSheet, RowTotalNoSolidBorder, rowIndex);

                if (rowIndex == beginTotalRow)
                {
                    row = sheet.CopyRow(TemplateSheet, RowTotalSolidBorder, rowIndex);
                    row.Cells[0].SetCellValue(ViewResources.Total);
                }

                row.Cells[ColumnCurrency].SetCellValue(totalCurrency.CurrencyName);

                AssignCommonData(row, totalCurrency);

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
        /// <returns>List of GoldDeluxeWinLoss entity.</returns>
        private static List<GoldDeluxeWinLoss> GetTotalCurrency(IEnumerable<GoldDeluxeWinLoss> dataList)
        {
            var listTotalCurrency = dataList.GroupBy(x => new { x.CurrencyName })
                    .Select(group => new GoldDeluxeWinLoss
                    {
                        CurrencyName = group.Key.CurrencyName,
                        TurnOver = group.Sum(a => a.TurnOver),
                        TotalTransaction = group.Sum(a => a.TotalTransaction),
                        CustomerWinLoss = group.Sum(a => a.CustomerWinLoss),
                        LicenseeWL = group.Sum(a => a.LicenseeWL),
                    }).ToList();

            return listTotalCurrency;
        }
    }
}