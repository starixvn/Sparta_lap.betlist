// --------------------------------------------------------------------------
// <copyright file="LiveReportExcel.cs" company="Nexcel Solutions Vietnam">
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
    using Common.Extensions;
    using Entities;
    using Models.DTO.Report.LiveReports;
    using Models.ViewModel;
    using Models.ViewModel.Report.LiveReports;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Live report - Export excel.
    /// </summary>
    public class LiveReportExcel : ReportBase
    {
        private const int ColumnAmountDue = 8;
        private const int ColumnBetCount = 5;
        private const int ColumnCustomerComm = 2;
        private const int ColumnCustomerTotal = 3;
        private const int ColumnCustomerWl = 1;
        private const int ColumnLicenseeComm = 7;
        private const int ColumnLicenseeWl = 6;
        private const int ColumnProduct = 0;
        private const int ColumnTemplateCount = 9;
        private const int ColumnTurnover = 4;
        private const int RowByBetTypeHeaderTemplate = 22;
        private const int RowByBetTypeProductTitle = 25;
        private const int RowByCurrencyHeaderTemplate = 16;
        private const int RowBySportsTypeContentTemplate = 13;
        private const int RowBySportsTypeHeaderTemplate = 10;
        private const int RowContentDetailTemplate = 19;
        private const int RowContentTemplate = 4;
        private const int RowNoInfoTemplate = 7;
        private const int RowSummaryHeaderTemplate = 1;
        private const int RowTotalSummaryTemplate = 6;
        private const int SummarySectionRowIndex = RowSummaryHeaderTemplate;

        private readonly LiveReportDetailViewModel _liveReportDetailList;
        private readonly IList<LiveReport> _liveReportList;

        public LiveReportExcel(
                               string templatePath,
                               string exportingFileName,
                               ConvertingOption convertingOption,
                               ShowingOption showingOption,
                               IList<LiveReport> liveReportList,
                               LiveReportDetailViewModel liveReportDetailList)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _liveReportList = liveReportList;
            _liveReportDetailList = liveReportDetailList;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _liveReportList.Count;

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
            // Summary
            AssignCommonHeader(sheet, ViewResources.Summary, ViewResources.Product, RowSummaryHeaderTemplate, RowSummaryHeaderTemplate);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUi(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int rowCount = _liveReportList.Count;

            for (int index = 0; index < rowCount; index++)
            {
                LiveReport liveReport = _liveReportList[index];
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                row.Cells[ColumnProduct].SetCellValue(liveReport.ProductName);
                row.Cells[ColumnCustomerWl].SetCellValue(liveReport.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].SetCellValue(liveReport.CustomerComm);
                row.Cells[ColumnCustomerTotal].SetCellValue(liveReport.CustomerTotal);
                row.Cells[ColumnTurnover].SetCellValue(liveReport.TurnOver);
                row.Cells[ColumnBetCount].SetCellValue(liveReport.BetCount);
                row.Cells[ColumnLicenseeComm].SetCellValue(liveReport.LicenseeComm);
                row.Cells[ColumnLicenseeWl].SetCellValue(liveReport.LicenseeWinLoss);
                row.Cells[ColumnAmountDue].SetCellValue(liveReport.AmountDue);
            }

            int currentRow = AssignTotalSummary(sheet);

            if (_liveReportDetailList.BySports != null)
            {
                // 2 rows empty
                currentRow = AssignBySportsTypeData(sheet, currentRow + 2);
            }

            if (_liveReportDetailList.ByCurrencies != null)
            {
                // 2 rows empty
                currentRow = AssignByCurrencyData(sheet, currentRow + 2);
            }

            if (_liveReportDetailList.ByBetType != null)
            {
                // 2 rows empty
                AssignByBetTypeData(sheet, currentRow + 2);
            }
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                                        ViewResources.LiveReportTitle,
                                        _liveReportDetailList.Param.FromDate.ToString("d MMMM yy"),
                                        _liveReportDetailList.Param.ToDate.ToString("d MMMM yy"),
                                        GetTitleCurrencyName());

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 4);

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
                HideRow(sheet, SummarySectionRowIndex + 2);
            }

            if (!ShowingOption.LicenseeCommVisible)
            {
                HideColumn(sheet, ColumnLicenseeComm);
            }
        }

        /// <summary>
        /// Assign common data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="liveReport">Live report data.</param>
        protected static void AssignCommonData(IRow row, LiveReport liveReport)
        {
            row.Cells[ColumnCustomerWl].SetCellValue(liveReport.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(liveReport.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(liveReport.CustomerTotal);
            row.Cells[ColumnTurnover].SetCellValue(liveReport.TurnOver);
            row.Cells[ColumnBetCount].SetCellValue(liveReport.BetCount);
            row.Cells[ColumnLicenseeComm].SetCellValue(liveReport.LicenseeComm);
            row.Cells[ColumnLicenseeWl].SetCellValue(liveReport.LicenseeWinLoss);
            row.Cells[ColumnAmountDue].SetCellValue(liveReport.AmountDue);
        }

        /// <summary>
        /// Assign data to bet type section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        protected void AssignByBetTypeData(ISheet sheet, int currentRow)
        {
            IList<LiveReportByBetType> liveReportSportList = _liveReportDetailList.ByBetType;

            if (liveReportSportList.Count > 0)
            {
                AssignCommonHeader(sheet, ViewResources.ByBetType, ViewResources.BetType, RowByBetTypeHeaderTemplate, currentRow);
                currentRow += 3;

                if (!ShowingOption.CustomerCommVisible)
                {
                    // If row isn't hide (in case BA visible), then merge cell and set value for header
                    int rowHeaderMerge = currentRow - 2;
                    sheet.MergeCells(rowHeaderMerge, ColumnCustomerWl, rowHeaderMerge + 1, ColumnCustomerWl);
                }

                foreach (var liveReportBetType in liveReportSportList)
                {
                    var liveReportBetTypeList = liveReportBetType.LiveReports;

                    var row = sheet.CopyRow(TemplateSheet, RowByBetTypeProductTitle, currentRow);
                    sheet.MergeCells(currentRow, ColumnProduct, currentRow, ColumnAmountDue);
                    checked
                    {
                        currentRow++;
                    }

                    string productName = liveReportBetType.ProductName.Equals(ViewResources.SB)
                        ? ViewResources.Sportsbook
                        : liveReportBetType.ProductName;

                    row.Cells[ColumnProduct].SetCellValue(productName);

                    foreach (LiveReport liveReportSport in liveReportBetTypeList)
                    {
                        row = sheet.CopyRow(TemplateSheet, RowContentDetailTemplate, currentRow);
                        row.Cells[ColumnProduct].SetCellValue(liveReportSport.ProductName);
                        AssignCommonData(row, liveReportSport);
                        currentRow++;
                    }
                }
            }
        }

        /// <summary>
        /// Assign data to currency section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <returns>The row index after assign data.</returns>
        protected int AssignByCurrencyData(ISheet sheet, int currentRow)
        {
            IList<LiveReport> liveReportSportList = _liveReportDetailList.ByCurrencies;

            if (liveReportSportList.Count > 0)
            {
                AssignCommonHeader(sheet, ViewResources.ByCurrency, ViewResources.Currency, RowByCurrencyHeaderTemplate, currentRow);
                currentRow += 3;

                if (!ShowingOption.CustomerCommVisible)
                {
                    // If row isn't hide (in case BA visible), then merge cell and set value for header
                    int rowHeaderMerge = currentRow - 2;
                    sheet.MergeCells(rowHeaderMerge, ColumnCustomerWl, rowHeaderMerge + 1, ColumnCustomerWl);
                }

                for (int i = 0; i < liveReportSportList.Count; i++)
                {
                    LiveReport liveReportSport = liveReportSportList[i];

                    IRow row = sheet.CopyRow(TemplateSheet, RowContentDetailTemplate, currentRow + i);

                    row.Cells[ColumnProduct].SetCellValue(liveReportSport.ProductName);

                    AssignCommonData(row, liveReportSport);
                }
            }

            return currentRow + liveReportSportList.Count;
        }

        /// <summary>
        /// Assign data to sport type section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <returns>The row index after assign data.</returns>
        protected int AssignBySportsTypeData(ISheet sheet, int currentRow)
        {
            IList<LiveReport> liveReportSportList = _liveReportDetailList.BySports;

            if (liveReportSportList.Count > 0)
            {
                AssignCommonHeader(sheet, ViewResources.BySportsType, ViewResources.Sport, RowBySportsTypeHeaderTemplate, currentRow);
                currentRow += 3;

                if (!ShowingOption.CustomerCommVisible)
                {
                    // If row isn't hide (in case BA visible), then merge cell and set value for header
                    int rowHeaderMerge = currentRow - 2;
                    sheet.MergeCells(rowHeaderMerge, ColumnCustomerWl, rowHeaderMerge + 1, ColumnCustomerWl);
                }

                for (int i = 0; i < liveReportSportList.Count; i++)
                {
                    LiveReport liveReportSport = liveReportSportList[i];

                    IRow row = sheet.CopyRow(TemplateSheet, RowBySportsTypeContentTemplate, currentRow + i);

                    row.Cells[ColumnProduct].SetCellValue(liveReportSport.ProductName);

                    AssignCommonData(row, liveReportSport);
                }
            }

            return currentRow + liveReportSportList.Count;
        }

        /// <summary>
        /// Assign data to summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <returns>The row index after assign data.</returns>
        protected int AssignTotalSummary(ISheet sheet)
        {
            int currentRow = RowContentTemplate + _liveReportList.Count;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalSummaryTemplate, currentRow);

            row.Cells[ColumnIndex].SetCellValue(ViewResources.Total);
            row.Cells[ColumnCustomerWl].SetCellValue(_liveReportList.Sum(a => a.CustomerWinLoss));
            row.Cells[ColumnCustomerComm].SetCellValue(_liveReportList.Sum(a => a.CustomerComm));
            row.Cells[ColumnCustomerTotal].SetCellValue(_liveReportList.Sum(a => a.CustomerTotal));
            row.Cells[ColumnTurnover].SetCellValue(_liveReportList.Sum(a => a.TurnOver));
            row.Cells[ColumnBetCount].SetCellValue(_liveReportList.Sum(a => a.BetCount));
            row.Cells[ColumnLicenseeWl].SetCellValue(_liveReportList.Sum(a => a.LicenseeWinLoss));
            row.Cells[ColumnLicenseeComm].SetCellValue(_liveReportList.Sum(a => a.LicenseeComm));
            row.Cells[ColumnAmountDue].SetCellValue(_liveReportList.Sum(a => a.AmountDue));

            return currentRow + 1;
        }

        /// <summary>
        /// Assign title for header.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="groupName">Group name.</param>
        /// <param name="firstHeader">Title of the first header.</param>
        /// <param name="rowTplIdx">Row template index.</param>
        /// <param name="rowIndex">Excel row index.</param>
        private void AssignCommonHeader(ISheet sheet, string groupName, string firstHeader, int rowTplIdx, int rowIndex)
        {
            var row1Header = new List<string>
            {
                ViewResources.Turnover,
                ViewResources.BetCount,
                ViewResources.LicenseeWinLoss,
                ViewResources.Comm,
                ViewResources.AmountDue
            };

            // group name
            IRow row = sheet.CopyRow(TemplateSheet, rowTplIdx++, rowIndex++);
            row.Cells[ColumnProduct].SetCellValue(groupName);

            // header row 1
            row = sheet.CopyRow(TemplateSheet, rowTplIdx++, rowIndex++);
            row.Cells[ColumnProduct].SetCellValue(firstHeader);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            sheet.MergeCells(rowIndex - 1, ColumnProduct, rowIndex, ColumnProduct);
            sheet.MergeCells(rowIndex - 1, ColumnCustomerWl, rowIndex - 1, ColumnCustomerTotal);

            int colIndex = ColumnTurnover;
            foreach (string item in row1Header)
            {
                sheet.MergeCells(rowIndex - 1, colIndex, rowIndex, colIndex);
                row.Cells[colIndex++].SetCellValue(item);
            }

            // header row 2
            row = sheet.CopyRow(TemplateSheet, rowTplIdx, rowIndex);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }
    }
}