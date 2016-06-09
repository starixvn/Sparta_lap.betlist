// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLossExcel.cs" company="Microsoft">
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
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    ///  Third party Win Loss - Export excel.
    /// </summary>
    public class MicroGamingWinLossExcel : ReportBase
    {
        private const int ColumnTemplateCount = 12;

        private const int ColumnDate = 1;
        private const int ColumnCurrency = 2;
        private const int ColumnTurnover = 3;
        private const int ColumnTotalTransaction = 4;
        private const int ColumnCustomerWl = 5;
        private const int ColumnProgressiveContribution = 6;
        private const int ColumnProgressivePayout = 7;
        private const int ColumnLicenseeWl = 8;
        private const int ColumnLicenseeNetWin = 9;
        private const int ColumnAmountDueToMGC = 10;
        private const int ColumnProgressiveFee = 11;

        private const int RowContentTitleTemplate = 2;
        private const int RowContentTemplate = 3;
        private const int RowTotalSolidBorder = 4;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;

        private readonly IList<MicroGamingWinLoss> _winLossList;
        private readonly MicroGamingWinLossMetaModel _metaModel;
        private readonly IDictionary<string, MicroGamingWinLoss> _summaryTotal;

        public MicroGamingWinLossExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                MicroGamingWinLossMetaModel metaModel,
                MicroGamingWinLossViewModel viewModel)
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
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnover].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            row.Cells[ColumnProgressiveContribution].SetCellValue(ViewResources.ProgressiveContribution);
            row.Cells[ColumnProgressivePayout].SetCellValue(ViewResources.ProgressivePayout);
            row.Cells[ColumnLicenseeWl].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnLicenseeNetWin].SetCellValue(ViewResources.LicenseeNetWin);
            row.Cells[ColumnAmountDueToMGC].SetCellValue(ViewResources.AmountDueToMGC);
            row.Cells[ColumnProgressiveFee].SetCellValue(ViewResources.ProgressiveFee);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTitleTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int indexReport = 1;
            int index;
            var rowCount = _winLossList.Count;
            int endRowSpan = 1;
            var currentRow = 2;
            var typeId = 0;

            for (index = 0; index < rowCount; index++)
            {
                var winloss = _winLossList[index];

                if (typeId != winloss.TypeID)
                {
                    SetTextRowValue(sheet, RowContentTitleTemplate, currentRow, ColumnTemplateCount, winloss.TypeName);
                    typeId = winloss.TypeID;
                    currentRow++;
                }

                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

                // Report Date
                var dateStringFormat = _metaModel.ReportBy == 1 ? "MM/dd/yyyy" : "MM/yyyy";
                var reportDateString = winloss.Date.ToString(dateStringFormat);

                row.Cells[ColumnIndex].SetCellValue(indexReport);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                row.Cells[ColumnCurrency].SetCellValue(winloss.CurrencyName);
                AssignCommonData(row, winloss);

                var rowSpan = CalculateReportRowSpan(winloss);

                if (currentRow == 2 || endRowSpan < currentRow)
                {
                    endRowSpan = currentRow + rowSpan - 1;

                    sheet.MergeCells(currentRow, ColumnIndex, endRowSpan, ColumnIndex);
                    sheet.MergeCells(currentRow, ColumnDate, endRowSpan, ColumnDate);
                    indexReport++;
                }

                currentRow++;
            }

            currentRow = AssignTotalData(sheet, currentRow);
            AssignGrandTotalInEurData(sheet, currentRow);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = GetExactTitleCurrencyName(_metaModel.CurrencyId);

            ReportTitle = GenerateProductReportTitle(fromdate, todate, title);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Assign common data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="data">Licensee win loss data.</param>
        protected void AssignCommonData(IRow row, MicroGamingWinLoss data)
        {
            row.Cells[ColumnTurnover].SetCellValue(data.TurnOver);
            row.Cells[ColumnTotalTransaction].SetCellValue(data.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(data.CustomerWinLoss);
            row.Cells[ColumnProgressiveContribution].SetCellValue(data.ProgressiveContribution);
            row.Cells[ColumnProgressivePayout].SetCellValue(data.ProgressivePayout);
            row.Cells[ColumnLicenseeWl].SetCellValue(data.LicenseeWL);
            row.Cells[ColumnLicenseeNetWin].SetCellValue(data.LicenseeNetWin);
            row.Cells[ColumnAmountDueToMGC].SetCellValue(data.AmountDueToMGC);
            row.Cells[ColumnProgressiveFee].SetCellValue(data.ProgressiveFee);
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
            List<MicroGamingWinLoss> totalCurrencyList = GetTotalCurrency(_winLossList);

            foreach (MicroGamingWinLoss totalCurrency in totalCurrencyList)
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
            sheet.MergeCells(rowIndex - totalCurrencyList.Count, 0, rowIndex - 1, ColumnDate);

            return rowIndex;
        }

        /// <summary>
        /// Get total currency data.
        /// </summary>
        /// <param name="dataList">Report data.</param>
        /// <returns>List of third party WinLoss entity.</returns>
        private static List<MicroGamingWinLoss> GetTotalCurrency(IEnumerable<MicroGamingWinLoss> dataList)
        {
            var listTotalCurrency = dataList.GroupBy(x => new { x.CurrencyName })
                    .Select(group => new MicroGamingWinLoss
                    {
                        CurrencyName = group.Key.CurrencyName,
                        TurnOver = group.Sum(a => a.TurnOver),
                        TotalTransaction = group.Sum(a => a.TotalTransaction),
                        CustomerWinLoss = group.Sum(a => a.CustomerWinLoss),
                        ProgressiveContribution = group.Sum(a => a.ProgressiveContribution),
                        ProgressivePayout = group.Sum(a => a.ProgressivePayout),
                        LicenseeWL = group.Sum(a => a.LicenseeWL),
                        LicenseeNetWin = group.Sum(a => a.LicenseeNetWin),
                        AmountDueToMGC = group.Sum(a => a.AmountDueToMGC),
                        ProgressiveFee = group.Sum(a => a.ProgressiveFee)
                    }).ToList();

            return listTotalCurrency;
        }

        /// <summary>
        /// Generates the product report's title.
        /// </summary>
        /// <param name="fromDate">The fromDate.</param>
        /// <param name="toDate">The toDate.</param>
        /// <param name="title">The title.</param>
        /// <returns>String of Report Title.</returns>
        private string GenerateProductReportTitle(string fromDate, string toDate, string title)
        {
            StringBuilder reportTitle = new StringBuilder(string.Empty);
            reportTitle.Append(ViewResources.Microgaming).Append(" ");
            reportTitle.Append(ViewResources.WinLoss);
            reportTitle.Append(string.Format(", {0} --> {1} - {2}", fromDate, toDate, title));
            return reportTitle.ToString();
        }

        private int CalculateReportRowSpan(MicroGamingWinLoss winloss)
        {
            return _winLossList.Count(x => x.TypeID == winloss.TypeID && x.Date.Date == winloss.Date.Date);
        }
    }
}