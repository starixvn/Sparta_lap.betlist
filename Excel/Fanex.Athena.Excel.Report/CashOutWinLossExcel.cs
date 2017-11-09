// --------------------------------------------------------------------------
// <copyright file="CashOutWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Core.DataDomain;
    using Helpers;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    public class CashOutWinLossExcel : ReportBase
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;

        private const int ColumnCashOutTimes = 5;
        private const int ColumnCashOutTickets = 6;
        private const int ColumnCashOutValue = 7;

        private const int ColumnBuyBackAmountSportsbook = 8;
        private const int ColumnBuyBackAmountBA = 9;
        private const int ColumnBuyBackAmountTotal = 10;

        private const int ColumnTicketPayout = 11;
        private const int ColumnCashOutCommmission = 12;

        private const int ColumnSettleAmountTicketPayout = 13;
        private const int ColumnSettleAmountBBAmtComm = 14;
        private const int ColumnSettleAmountTotal = 15;

        private const int ColumnTemplateCount = 16;

        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowTotalSolidBorder = 4;

        private readonly bool _isAllowBA;

        private readonly IList<CashOutWinLoss> _dataList;

        private readonly ThirdPartyWinLossMetaModel _metaModel;
        private readonly IDictionary<string, CashOutWinLoss> _summaryTotal;
        private readonly IEnumerable<CashOutWinLoss> _totalData;

        private readonly ConvertingOption _convertingOption;

        public CashOutWinLossExcel(
                string templatePath,
                ConvertingOption convertingOption,
                ThirdPartyWinLossMetaModel metaModel,
                CashOutWinLossViewModel viewModel)
            : base(templatePath, viewModel.MainResult.Count())
        {
            _convertingOption = convertingOption;
            _metaModel = metaModel;
            _dataList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
            _isAllowBA = viewModel.IsLicenseeOfferBA;
            _totalData = viewModel.TotalData;
        }

        public int Level
        {
            get;
            set;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        protected override void AssignHeader(ISheet sheet)
        {
            // row 1
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnCashOutTimes].SetCellValue(ViewResources.CashOutTimes);
            row.Cells[ColumnCashOutTickets].SetCellValue(ViewResources.CashOutTickets);
            row.Cells[ColumnCashOutValue].SetCellValue(ViewResources.CashOutValue);
            row.Cells[ColumnBuyBackAmountSportsbook].SetCellValue(ViewResources.BuyBackAmount);
            row.Cells[ColumnTicketPayout].SetCellValue(ViewResources.TicketPayout);
            row.Cells[ColumnCashOutCommmission].SetCellValue(ViewResources.CashOutCommission);
            row.Cells[ColumnSettleAmountTicketPayout].SetCellValue(ViewResources.SettleAmount);

            // row 2
            var row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row2.Cells[ColumnBuyBackAmountSportsbook].SetCellValue(ViewResources.ProductSB);
            row2.Cells[ColumnBuyBackAmountBA].SetCellValue(ViewResources.ProductBA);
            row2.Cells[ColumnBuyBackAmountTotal].SetCellValue(ViewResources.Total);

            row2.Cells[ColumnSettleAmountTicketPayout].SetCellValue(ViewResources.TicketPayout);
            row2.Cells[ColumnSettleAmountBBAmtComm].SetCellValue(ViewResources.BBAmtComm);
            row2.Cells[ColumnSettleAmountTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet)
        {
            var rowSpan = 1;
            var rowIndex = 1;
            var indexDetail = 1;
            int index;
            var rowCount = _dataList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var item = _dataList[index];

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                var reportDateString = _metaModel.ReportBy == ReportByType.Monthly
                                          ? item.Date.ToString("MM/yyyy")
                                          : item.Date.ToShortDateString();

                // Merge first two columns
                var needMerge = index >= rowCount - 1 ||
                                 item.Date.Date != _dataList[index + 1].Date.Date;

                // Merge cell
                if (needMerge && rowSpan > 1)
                {
                    MergeCells(sheet, rowSpan, index);
                }

                rowSpan = needMerge ? 1 : rowSpan + 1;

                var indexid = Level == 0 ? rowIndex : indexDetail++;
                row.Cells[ColumnIndex].SetCellValue(indexid);
                rowIndex = needMerge ? rowIndex + 1 : rowIndex;

                row.Cells[ColumnExternalId].SetCellValue(item.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(item.SystemId);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                row.Cells[ColumnCurrency].SetCellValue(item.CurrencyName);
                AssignCommonData(row, item);
            }

            rowIndex = AssignTotalData(sheet, index + 3);
            AssignGrandTotalInEurData(sheet, rowIndex);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = ReportExcelHelper.GenerateReportTitle(ViewResources.CashOutWinLoss, _convertingOption, _metaModel);
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (!_isAllowBA)
            {
                sheet.HideColumn(ColumnBuyBackAmountBA, ColumnBuyBackAmountTotal);
                sheet.MergeCells(RowHeaderTemplate, ColumnBuyBackAmountSportsbook, RowHeaderTemplate + 1, ColumnBuyBackAmountSportsbook);
            }

            if (Level == 0)
            {
                sheet.HideColumn(ColumnExternalId, ColumnSystemId);
            }
            else
            {
                sheet.HideColumn(ColumnDate);
            }
        }

        private static void AssignCommonData(IRow row, CashOutWinLoss data)
        {
            row.Cells[ColumnCashOutTimes].SetCellValue(data.CashOutTimes);
            row.Cells[ColumnCashOutTickets].SetCellValue(data.CashOutTickets);
            row.Cells[ColumnCashOutValue].SetCellValue(data.CashOutValue);

            row.Cells[ColumnBuyBackAmountSportsbook].SetCellValue(data.BuyBackAmountSB);
            row.Cells[ColumnBuyBackAmountBA].SetCellValue(data.BuyBackAmountBA);
            row.Cells[ColumnBuyBackAmountTotal].SetCellValue(data.TotalBuyBackAmount);

            row.Cells[ColumnTicketPayout].SetCellValue(data.TicketPayout);
            row.Cells[ColumnCashOutCommmission].SetCellValue(data.CashOutCommission);

            row.Cells[ColumnSettleAmountTicketPayout].SetCellValue(data.SettleAmountTicketPayout);
            row.Cells[ColumnSettleAmountBBAmtComm].SetCellValue(data.SettleAmountBuyBackComm);
            row.Cells[ColumnSettleAmountTotal].SetCellValue(data.SettleAmountTotal);
        }

        private void AssignGrandTotalInEurData(ISheet sheet, int rowIndex)
        {
            var currencies = _summaryTotal.Keys.Reverse();

            foreach (string currency in currencies)
            {
                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowGrantTotal, rowIndex);
                AssignCommonData(row, _summaryTotal[currency]);
                row.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + currency.ToUpperInvariant());

                // Merge first two column
                sheet.MergeCells(rowIndex, ColumnIndex, rowIndex, ColumnCurrency);
                rowIndex++;
            }
        }

        /// <summary>
        /// Assign grand total data.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <returns>The last row index after assign data.</returns>
        private int AssignTotalData(ISheet sheet, int rowIndex)
        {
            var beginTotalRow = rowIndex;

            foreach (var totalCurrency in _totalData)
            {
                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowTotalNoSolidBorder, rowIndex);

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
            sheet.MergeCells(rowIndex - _totalData.Count(), 0, rowIndex - 1, 3);

            return rowIndex;
        }

        private void MergeCells(ISheet sheet, int rowSpan, int index)
        {
            if (Level == 0)
            {
                sheet.MergeCells(index + RowContentTemplate - rowSpan + 1, ColumnIndex, index + RowContentTemplate, ColumnIndex);
            }

            sheet.MergeCells(index + RowContentTemplate - rowSpan + 1, ColumnDate, index + RowContentTemplate, ColumnDate);
        }
    }
}