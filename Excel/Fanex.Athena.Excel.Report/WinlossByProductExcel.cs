// --------------------------------------------------------------------------
// <copyright file="WinlossByProductExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Fanex.Athena.Excel.Base.Extension;
    using Helpers;
    using Model.Report.DTO;
    using Models;
    using NPOI.SS.UserModel;
    using Resources;

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
        private const int RowNoInfoTemplate = 12;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalNoSolidBorder = 9;
        private const int RowTotalSolidBorder = 8;

        private readonly WinLossByProductExcelModel _excelModel;

        public WinlossByProductExcel(string templatePath, WinLossByProductExcelModel excelModel)
            : base(templatePath)
        {
            _excelModel = excelModel;

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

        protected override void AssignData()
        {
            var numOfDataRow = _excelModel.DataList.Count();

            var sheet = Workbook.CloneSheet("Template", "Data");

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

        protected void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected void AssignNoInfoRow(ISheet sheet)
        {
            var message = string.IsNullOrWhiteSpace(_excelModel.ErrorMessage) ? ViewResources.NoInfo : _excelModel.ErrorMessage;
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTitleTemplate, ColumnTemplateCount, message);
        }

        protected void AssignSheetData(ISheet sheet)
        {
            var indexReport = 1;

            var currentRow = 3;

            var rowCount = _excelModel.DataList.Count();

            var date = DateTime.MinValue.ToShortDateString();

            var endRowSpan = 2;

            for (int index = 0; index < rowCount; index++)
            {
                var winlossByProduct = _excelModel.DataList.ElementAt(index);

                indexReport = SetDateWinLoss(sheet, winlossByProduct, indexReport, ref date, ref currentRow);

                var rowContent = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

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

        protected void AssignTitle(ISheet sheet)
        {
            var fromdate = _excelModel.MetaModel.Level == 0
                              ? _excelModel.MetaModel.FromDate.ToString(FormatDate)
                              : _excelModel.MetaModel.DrillDownFromDate.ToString(FormatDate);

            var todate = _excelModel.MetaModel.Level == 0
                            ? _excelModel.MetaModel.ToDate.ToString(FormatDate)
                            : _excelModel.MetaModel.DrillDownToDate.ToString(FormatDate);

            var currencyId = _excelModel.MetaModel.Level > 0 && _excelModel.MetaModel.CurrencyId == 0 ?
                _excelModel.MetaModel.DrillDownCurrencyId : _excelModel.MetaModel.CurrencyId;

            var currencyName = ReportExcelHelper.GetExactTitleCurrencyName(_excelModel.ConvertingOption, currencyId);
            var productsTitle = ReportExcelHelper.GetLicenseeWLProduct(_excelModel.MetaModel.ProductOptions, _excelModel.SystemId);
            var reportTitle = string.Format(ViewResources.WinLossByProductTitle, fromdate, todate, currencyName + productsTitle);

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected void ProcessUI(ISheet sheet)
        {
            if (!_excelModel.SystemIdVisible)
            {
                sheet.HideColumn(ColumnSystemId);
            }

            if (!_excelModel.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerComm);
                sheet.HideRow(RowTableHeaderTemplate + 1);
            }

            if (!_excelModel.BAVisible && !_excelModel.RBVisible)
            {
                sheet.HideColumn(ColumnComm);
            }

            if (_excelModel.MetaModel.Level == 0)
            {
                sheet.HideColumn(ColumnExternalId, ColumnSystemId);
            }

            if (_excelModel.MetaModel.Level == 1)
            {
                sheet.HideColumn(ColumnExternalId);
            }
        }

        private static void AssignCommonData(IRow row, WinLossByProduct winlossByProduct)
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
        private void AssignSummaryTotal(ISheet sheet, int currentRow)
        {
            var currencyList = _excelModel.SummaryTotal.Keys.ToList();
            currencyList.Reverse();

            var temp = 0;

            foreach (string currency in currencyList)
            {
                var rowBegin = currentRow;
                var productSummaryList = _excelModel.SummaryTotal[currency];

                foreach (WinLossByProduct productSummary in productSummaryList)
                {
                    var row = sheet.CopyRow(TemplateSheet, RowGrandTotal + temp, currentRow);
                    row.Cells[ColumnProduct].SetCellValue(productSummary.ProductName);
                    row.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + currency.ToUpperInvariant());
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
        private int AssignTotalData(ISheet sheet, int currentRow)
        {
            var rowBegin = currentRow;
            var currencyList = _excelModel.TotalData.GroupBy(a => a.CurrencyName).ToList();
            var isFirstRowTotal = true;

            foreach (var currency in currencyList)
            {
                var data = _excelModel.TotalData.Where(a => a.CurrencyName == currency.Key).ToList();
                sheet.MergeCells(currentRow, ColumnCurrency, currentRow + data.Count - 1, ColumnCurrency);
                foreach (var item in data)
                {
                    var cellIndex = ColumnCurrency;
                    IRow row;
                    if (isFirstRowTotal)
                    {
                        row = sheet.CopyRow(TemplateSheet, RowTotalSolidBorder, currentRow);
                        row.Cells[0].SetCellValue(ViewResources.Total);
                        isFirstRowTotal = false;
                    }
                    else
                    {
                        row = sheet.CopyRow(TemplateSheet, RowTotalNoSolidBorder, currentRow);
                    }

                    row.Cells[cellIndex++].SetCellValue(item.CurrencyName);
                    row.Cells[cellIndex].SetCellValue(item.ProductName);
                    AssignCommonData(row, item);
                    currentRow++;
                }
            }

            MergeCellTotal(sheet, rowBegin, currentRow);

            return currentRow;
        }

        private int CalculateReportRowSpan(WinLossByProduct item)
        {
            int rowSpan;

            if (_excelModel.MetaModel.Level == 0)
            {
                rowSpan = _excelModel.DataList.Count(a => a.Date == item.Date && a.CurrencyId == item.CurrencyId);
            }
            else if (_excelModel.MetaModel.Level == 1)
            {
                rowSpan = _excelModel.DataList.Count(a => a.SystemId == item.SystemId);
            }
            else
            {
                rowSpan = _excelModel.DataList.Count(a => a.ExternalId == item.ExternalId &&
                                   a.SystemId == item.SystemId &&
                                   a.CurrencyId == item.CurrencyId);
            }

            return rowSpan;
        }

        private void MergeCellTotal(ISheet sheet, int rowBegin, int currentRow)
        {
            const int CellIndex = ColumnCurrency;

            sheet.MergeCells(rowBegin, ColumnIndex, currentRow - 1, ColumnCurrency - 1);

            if (_excelModel.MetaModel.Level != 0)
            {
                sheet.MergeCells(rowBegin, CellIndex, currentRow - 1, CellIndex);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        private int SetDateWinLoss(
            ISheet sheet,
            WinLossByProduct winlossByProduct,
            int indexReport,
            ref string date,
            ref int currentRow)
        {
            if (_excelModel.MetaModel.Level == 0)
            {
                var dateGetDb = winlossByProduct.Date;

                if (date != dateGetDb.ToShortDateString())
                {
                    var reportDateString = _excelModel.MetaModel.ReportBy == 1
                                              ? dateGetDb.ToShortDateString()
                                              : dateGetDb.ToString("MM/yyyy");
                    sheet.SetTextRowValue(TemplateSheet, RowContentTitleTemplate, currentRow, ColumnTemplateCount, reportDateString);
                    date = dateGetDb.ToShortDateString();
                    currentRow++;
                }
            }

            return indexReport;
        }
    }
}