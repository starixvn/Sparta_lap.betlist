// -----------------------------------------------------------------------
// <copyright file="ColossusBetsWinLossExcel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Helpers;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    public class ColossusBetsWinLossExcel : ReportBase
    {
        private const int ColumnTemplateCount = 13;

        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnover = 5;
        private const int ColumnTotalTransaction = 6;
        private const int ColumnMemberWinning = 7;
        private const int ColumnCashOutAmt = 8;
        private const int ColumnCashOutShare = 9;
        private const int ColumnCommissionShare = 10;
        private const int ColumnAmountDueToCB = 11;
        private const int ColumnGrandPrize = 12;

        private const int RowContentTemplate = 3;
        private const int RowTotalSolidBorder = 4;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;

        // This is drill down of report
        private readonly IList<ColossusBetWinLoss> _winLossList;

        private readonly ThirdPartyWinLossMetaModel _metaModel;
        private readonly IDictionary<string, ColossusBetWinLoss> _summaryTotal;

        private readonly ConvertingOption _convertingOption;

        public ColossusBetsWinLossExcel(
                string templatePath,
                ConvertingOption convertingOption,
                ThirdPartyWinLossMetaModel metaModel,
                ColossusBetWinLossViewModel viewModel)
            : base(templatePath, null)
        {
            _convertingOption = convertingOption;
            _metaModel = metaModel;
            _winLossList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
        }

        public int Level
        {
            get;
            set;
        }

        protected override void AssignData()
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

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnover].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnMemberWinning].SetCellValue(ViewResources.AmtLessThan30kGBP);
            row.Cells[ColumnCashOutShare].SetCellValue(ViewResources.CashOutShare);
            row.Cells[ColumnCommissionShare].SetCellValue(ViewResources.CommissionShare);
            row.Cells[ColumnAmountDueToCB].SetCellValue(ViewResources.AmountDueToCB);
            row.Cells[ColumnGrandPrize].SetCellValue(ViewResources.AmtGreaterThan30kGBP);

            var row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            row2.Cells[ColumnMemberWinning].SetCellValue(ViewResources.CustomerWinnings);
            row2.Cells[ColumnCashOutAmt].SetCellValue(ViewResources.CashOutAmt);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet)
        {
            var rowIndex = 1;
            int index;
            var rowCount = _winLossList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var item = _winLossList[index];

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                var dateStringFormat = _metaModel.ReportBy == 1 ? "MM/dd/yyyy" : "MM/yyyy";
                var reportDateString = item.Date.ToString(dateStringFormat);

                row.Cells[ColumnIndex].SetCellValue(item.Index);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                row.Cells[ColumnExternalId].SetCellValue(item.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(item.SystemId);
                row.Cells[ColumnCurrency].SetCellValue(item.CurrencyName);
                AssignCommonData(row, item);
                rowIndex++;

                if (item.RowSpan > 1)
                {
                    var currentRow = index + RowContentTemplate;
                    sheet.MergeCells(currentRow, ColumnDate, currentRow + item.RowSpan - 1, ColumnDate);
                }
            }

            rowIndex = AssignTotalData(sheet, index + RowContentTemplate);
            AssignGrandTotal(sheet, rowIndex);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = _metaModel.DrillDownLevel > 0 && _metaModel.CurrencyId == 0
                ? ReportExcelHelper.GetExactTitleCurrencyName(_convertingOption, _metaModel.DrillDownCurrencyId)
                : ReportExcelHelper.GetExactTitleCurrencyName(_convertingOption, _metaModel.CurrencyId);

            ReportTitle = GenerateProductReportTitle(fromdate, todate, title);
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (Level == 0)
            {
                sheet.HideColumn(ColumnExternalId, ColumnSystemId);
            }
            else
            {
                sheet.HideColumn(ColumnDate);
            }
        }

        protected void AssignCommonData(IRow row, ColossusBetWinLoss data)
        {
            row.Cells[ColumnTurnover].SetCellValue(data.TurnOver);
            row.Cells[ColumnTotalTransaction].SetCellValue(data.TotalTransaction);
            row.Cells[ColumnMemberWinning].SetCellValue(data.MemberWinning);
            row.Cells[ColumnCashOutAmt].SetCellValue(data.CashOut);
            row.Cells[ColumnCashOutShare].SetCellValue(data.CashOutShare);
            row.Cells[ColumnCommissionShare].SetCellValue(data.CommissionShare);
            row.Cells[ColumnAmountDueToCB].SetCellValue(data.AmountDueToCB);
            row.Cells[ColumnGrandPrize].SetCellValue(data.GrandPrize);
        }

        protected int AssignTotalData(ISheet sheet, int rowIndex)
        {
            int beginTotalRow = rowIndex;
            var totalCurrencyList = GetTotalCurrency(_winLossList);

            foreach (ColossusBetWinLoss totalCurrency in totalCurrencyList)
            {
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

            sheet.MergeCells(rowIndex - totalCurrencyList.Count, 0, rowIndex - 1, 3);

            return rowIndex;
        }

        protected int AssignGrandTotal(ISheet sheet, int rowIndex)
        {
            var currencyList = _summaryTotal.Keys.ToList();
            currencyList.Reverse();

            foreach (string currency in currencyList)
            {
                var row = sheet.CopyRow(TemplateSheet, RowGrantTotal, rowIndex);
                AssignCommonData(row, _summaryTotal[currency]);
                row.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + currency.ToUpper());

                sheet.MergeCells(rowIndex, ColumnIndex, rowIndex, ColumnCurrency);
                rowIndex++;
            }

            return rowIndex;
        }

        private static IList<ColossusBetWinLoss> GetTotalCurrency(IEnumerable<ColossusBetWinLoss> dataList)
        {
            var listTotalCurrency = dataList.GroupBy(x => new { x.CurrencyName })
                    .Select(group => new ColossusBetWinLoss
                    {
                        CurrencyName = group.Key.CurrencyName,
                        TurnOver = group.Sum(a => a.TurnOver),
                        TotalTransaction = group.Sum(a => a.TotalTransaction),
                        MemberWinning = group.Sum(p => p.MemberWinning),
                        CashOut = group.Sum(p => p.CashOut),
                        CashOutShare = group.Sum(p => p.CashOutShare),
                        CommissionShare = group.Sum(p => p.CommissionShare),
                        GrandPrize = group.Sum(p => p.GrandPrize)
                    }).ToList();

            return listTotalCurrency;
        }

        private string GenerateProductReportTitle(string fromDate, string toDate, string title)
        {
            StringBuilder reportTitle = new StringBuilder(string.Empty);
            reportTitle.Append(ViewResources.ProductCB).Append(" ");
            reportTitle.Append(ViewResources.WinLoss);
            reportTitle.Append(string.Format(", {0} --> {1} - {2}", fromDate, toDate, title));
            return reportTitle.ToString();
        }
    }
}