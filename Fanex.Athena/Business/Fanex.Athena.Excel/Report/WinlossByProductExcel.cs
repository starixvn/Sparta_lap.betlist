// --------------------------------------------------------------------------
// <copyright file="WinlossByProductExcel.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using Base;
    using Common.Extensions;
    using Entities;
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win loss by product - Export excel.
    /// </summary>
    public class WinlossByProductExcel : ReportBase
    {
        private const int ColumnAmtDueTo = 12;
        private const int ColumnComm = 11;
        private const int ColumnCurrency = 3;
        private const int ColumnCustomerComm = 8;
        private const int ColumnCustomerTotal = 9;
        private const int ColumnCustomerWinLoss = 7;
        private const int ColumnExternalId = 1;
        private const int ColumnLicenseeWl = 10;
        private const int ColumnProduct = 4;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 13;
        private const int ColumnTotalTransaction = 6;
        private const int ColumnTurnover = 5;
        private const int RowContentTemplate = 4;
        private const int RowContentTitleTemplate = 3;
        private const int RowGrandTotal = 10;
        private const int RowNoInfoTemplate = 11;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalNoSolidBorder = 9;
        private const int RowTotalSolidBorder = 8;

        private readonly WinLossByProductMetaModel _metaModel;
        private readonly IDictionary<string, IList<WinLossByProduct>> _summaryTotal;
        private readonly IList<WinLossByProduct> _winlossByProductList;

        public WinlossByProductExcel(
                                     string templatePath,
                                     string exportingFileName,
                                     ConvertingOption convertingOption,
                                     ShowingOption showingOption,
                                     WinLossByProductMetaModel metaModel,
                                     WinLossByProductViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _metaModel = metaModel;
            _winlossByProductList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Currency,
                ViewResources.Product,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.Comm,
                ViewResources.AmountDue,
            };
        }

        public byte EnableReportByMonth
        {
            get;
            set;
        }

        public int FilterCurrencyId
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public bool ReportByMonthly
        {
            get;
            set;
        }

        public string TitleReport
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _winlossByProductList.Count;

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
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTitleTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int indexReport = 1;

            int currentRow = 3;

            int rowCount = _winlossByProductList.Count;

            string date = DateTime.MinValue.ToShortDateString();

            int endRowSpan = 2;

            for (int index = 0; index < rowCount; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTitleTemplate, currentRow);

                WinLossByProduct winlossByProduct = _winlossByProductList[index];

                indexReport = SetDateWinLoss(sheet, row, winlossByProduct, indexReport, ref date, ref currentRow);

                IRow rowContent = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

                rowContent.Cells[ColumnIndex].SetCellValue(indexReport);
                rowContent.Cells[ColumnExternalId].SetCellValue(winlossByProduct.ExternalId);
                rowContent.Cells[ColumnSystemId].SetCellValue(winlossByProduct.SystemId);
                rowContent.Cells[ColumnCurrency].SetCellValue(winlossByProduct.CurrencyName);
                rowContent.Cells[ColumnProduct].SetCellValue(winlossByProduct.ProductName);

                var rowSpan = CalculateReportRowSpan(winlossByProduct);

                if (currentRow == 2 || endRowSpan < currentRow)
                {
                    endRowSpan = currentRow + rowSpan - 1;

                    sheet.MergeCells(currentRow, ColumnIndex, endRowSpan, ColumnIndex);
                    sheet.MergeCells(currentRow, ColumnCurrency, endRowSpan, ColumnCurrency);
                    sheet.MergeCells(currentRow, ColumnSystemId, endRowSpan, ColumnSystemId);
                    sheet.MergeCells(currentRow, ColumnExternalId, endRowSpan, ColumnExternalId);
                    indexReport++;
                }

                AssignCommonData(rowContent, winlossByProduct);

                currentRow++;
            }

            currentRow = AssignTotalData(sheet, currentRow);

            AssignSummaryTotal(sheet, currentRow);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            string fromdate = _metaModel.Level == 0
                              ? _metaModel.FromDate.ToString(FormatDate)
                              : _metaModel.DrillDownFromDate.ToString(FormatDate);

            string todate = _metaModel.Level == 0
                            ? _metaModel.ToDate.ToString(FormatDate)
                            : _metaModel.DrillDownToDate.ToString(FormatDate);
            if (_metaModel.Level > 0 && _metaModel.CurrencyId == 0)
            {
                ReportTitle = string.Format(
                                    ViewResources.WinLossByProductTitle,
                                    fromdate,
                                    todate,
                                    GetExactTitleCurrencyName(_metaModel.DrillDownCurrencyId)) + GetLicenseeWlProduct();
            }
            else
            {
                ReportTitle = string.Format(
                                        ViewResources.WinLossByProductTitle,
                                        fromdate,
                                        todate,
                                        GetExactTitleCurrencyName(_metaModel.CurrencyId)) + GetLicenseeWlProduct();
            }

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

            if (ShowingOption.BaVisible == false && ShowingOption.RbVisible == false)
            {
                HideColumn(sheet, ColumnComm);
            }

            switch (Level)
            {
                case 0:
                    HideColumn(sheet, ColumnExternalId, ColumnSystemId);
                    break;

                case 1:
                    HideColumn(sheet, ColumnExternalId);
                    break;
            }
        }

        protected void AssignCommonData(IRow row, WinLossByProduct winlossByProduct)
        {
            row.Cells[ColumnTurnover].SetCellValue(winlossByProduct.TurnOver);
            row.Cells[ColumnTotalTransaction].SetCellValue(winlossByProduct.TotalTransaction);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(winlossByProduct.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(winlossByProduct.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(winlossByProduct.CustomerTotal);
            row.Cells[ColumnLicenseeWl].SetCellValue(winlossByProduct.LicenseeWL);
            row.Cells[ColumnComm].SetCellValue(winlossByProduct.LicenseeWLComm);
            row.Cells[ColumnAmtDueTo].SetCellValue(winlossByProduct.AmountDue);
        }

        /// <summary>
        /// Assign data to summary total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        protected void AssignSummaryTotal(ISheet sheet, int currentRow)
        {
            List<string> currencyList = _summaryTotal.Keys.ToList();
            currencyList.Reverse();

            int temp = 0;

            foreach (string currency in currencyList)
            {
                int rowBegin = currentRow;
                IList<WinLossByProduct> productSummaryList = _summaryTotal[currency];

                foreach (WinLossByProduct productSummary in productSummaryList)
                {
                    IRow row = sheet.CopyRow(TemplateSheet, RowGrandTotal + temp, currentRow);
                    row.Cells[ColumnProduct].SetCellValue(productSummary.ProductName);
                    row.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + currency.ToUpper());
                    AssignCommonData(row, productSummary);
                    currentRow++;
                }

                sheet.MergeCells(rowBegin, ColumnIndex, currentRow - 1, ColumnCurrency);
                temp++;
            }
        }

        /// <summary>
        /// Assign data to Total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <returns>The row index after assign data.</returns>
        protected int AssignTotalData(ISheet sheet, int currentRow)
        {
            int rowBegin = currentRow;
            var totalCurrencyList = GetTotalCurrencyList();
            var currencyList = totalCurrencyList.OrderBy(i => i.CurrencyName).GroupBy(a => a.CurrencyName).ToList();
            var isFirstRowTotal = true;

            foreach (var currency in currencyList)
            {
                var data = totalCurrencyList.Where(a => a.CurrencyName == currency.Key).ToList();
                sheet.MergeCells(currentRow, ColumnCurrency, currentRow + data.Count() - 1, ColumnCurrency);
                foreach (var item in data)
                {
                    int cellIndex = ColumnCurrency;
                    int rowTemplate = RowTotalNoSolidBorder;
                    if (isFirstRowTotal)
                    {
                        rowTemplate = RowTotalSolidBorder;
                        isFirstRowTotal = false;
                    }

                    IRow row = sheet.CopyRow(TemplateSheet, rowTemplate, currentRow);
                    row.Cells[cellIndex++].SetCellValue(item.CurrencyName);
                    row.Cells[cellIndex].SetCellValue(item.ProductName);
                    AssignCommonData(row, item);
                    currentRow++;
                }
            }

            MergeCellTotal(sheet, rowBegin, currentRow);

            return currentRow;
        }

        /// <summary>
        ///     Get row template.
        /// </summary>
        /// <param name="productName">Product name.</param>
        /// <param name="nameOfRow">Name of row.</param>
        /// <returns>Row number.</returns>
        protected int GetNumberRow(string productName, string nameOfRow)
        {
            int[] rowList = GetRowNumberFromTemplate(nameOfRow);

            int rowNumber;
            if (Level == 0 && (productName == "Sportsbook" || productName == "Number Game"))
            {
                rowNumber = rowList[0];
            }
            else
            {
                rowNumber = rowList[3];

                if (productName == "Sportsbook" || productName == "Number Game")
                {
                    rowNumber = rowList[2];
                }
            }

            return rowNumber;
        }

        /// <summary>
        ///     Get array template rows.
        /// </summary>
        /// <param name="nameOfRow">Name of row.</param>
        /// <returns>Row number list.</returns>
        protected int[] GetRowNumberFromTemplate(string nameOfRow)
        {
            var numberRowList = new[] { 4, 5, 6, 7 };
            switch (nameOfRow)
            {
                case "RowTotalHeader":
                    return new[] { 8, 9, 10, 11 };
                case "RowTotal":
                    return new[] { 12, 13, 14, 15 };
                case "RowTotalInEURHeader":
                    return new[] { 16, 17, 18, 19 };
                case "RowTotalInEUR":

                    return new[] { 20, 21, 22, 23 };
            }

            return numberRowList;
        }

        /// <summary>
        /// Calculate row span for a group of currency and product.
        /// </summary>
        /// <param name="winlossByProduct">The full data list.</param>
        /// <returns>Row span count. </returns>
        protected int CalculateReportRowSpan(WinLossByProduct winlossByProduct)
        {
            var rowSameValueList = _winlossByProductList
                                .Where(a => a.CurrencyId == winlossByProduct.CurrencyId);

            if (Level == 0)
            {
                rowSameValueList =
                    _winlossByProductList.Where(
                        a => a.Date == winlossByProduct.Date && a.CurrencyId == winlossByProduct.CurrencyId);
            }
            else if (Level == 1)
            {
                rowSameValueList =
                    _winlossByProductList.Where(
                        a => a.SystemId == winlossByProduct.SystemId);
            }
            else if (Level == 2)
            {
                rowSameValueList = _winlossByProductList.Where(a => a.ExternalId == winlossByProduct.ExternalId &&
                                   a.SystemId == winlossByProduct.SystemId &&
                                   a.CurrencyId == winlossByProduct.CurrencyId);
            }

            return rowSameValueList.Count();
        }

        protected void MergeCellTotal(ISheet sheet, int rowBegin, int currentRow)
        {
            const int CellIndex = ColumnCurrency;

            sheet.MergeCells(rowBegin, ColumnIndex, currentRow - 1, ColumnCurrency - 1);

            if (Level != 0)
            {
                sheet.MergeCells(rowBegin, CellIndex, currentRow - 1, CellIndex);
            }
        }

        /// <summary>
        /// Set win loss date for report row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="row">Excel row.</param>
        /// <param name="winlossByProduct">Win loss by product data.</param>
        /// <param name="indexReport">Report index value.</param>
        /// <param name="date">Win loss date.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <returns>The row index after assign data.</returns>
        protected int SetDateWinLoss(
            ISheet sheet,
            IRow row,
            WinLossByProduct winlossByProduct,
            int indexReport,
            ref string date,
            ref int currentRow)
        {
            if (Level == 0)
            {
                DateTime dateGetDb = winlossByProduct.Date;

                if (date != dateGetDb.ToShortDateString())
                {
                    // Report Date
                    string reportDateString = EnableReportByMonth == 1
                                              ? dateGetDb.ToShortDateString()
                                              : dateGetDb.ToString("MM/yyyy");
                    SetTextRowValue(sheet, RowContentTitleTemplate, currentRow, ColumnTemplateCount, reportDateString);
                    date = dateGetDb.ToShortDateString();
                    currentRow++;
                }
            }

            return indexReport;
        }

        /// <summary>
        /// Get data for summary data by currency.
        /// </summary>
        /// <returns>Summary data list.</returns>
        private List<WinLossByProduct> GetTotalCurrencyList()
        {
            List<WinLossByProduct> listTotalCurrency =
                _winlossByProductList.GroupBy(x => new { x.CurrencyId, x.CurrencyName, x.ProductCode })
                .Select(group => new WinLossByProduct
            {
                CurrencyId = group.Key.CurrencyId,
                CurrencyName = group.Key.CurrencyName,
                ProductCode = group.Key.ProductCode,
                TurnOver = group.Sum(a => a.TurnOver),
                TotalTransaction = group.Sum(a => a.TotalTransaction),
                CustomerWinLoss = group.Sum(a => a.CustomerWinLoss),
                CustomerComm = group.Sum(a => a.CustomerComm),
                LicenseeWL = group.Sum(a => a.LicenseeWL),
                LicenseeWLComm = group.Sum(a => a.LicenseeWLComm),
                AmountDue = group.Sum(a => a.AmountDue)
            }).ToList();

            return listTotalCurrency;
        }
    }
}