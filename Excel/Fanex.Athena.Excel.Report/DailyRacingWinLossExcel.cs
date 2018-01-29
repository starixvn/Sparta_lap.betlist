// --------------------------------------------------------------------------
// <copyright file="DailyRacingWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Globalization;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Helpers;
    using Model.Report.DTO;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    /// <summary>
    /// Daily racing win loss - Export excel.
    /// </summary>
    public class DailyRacingWinLossExcel : ReportBase
    {
        private const int ColumnAmountDueTo = 9;
        private const int ColumnCommIncome = 8;
        private const int ColumnCustomerComm = 7;
        private const int ColumnDividenWon = 6;
        private const int ColumnMeetingCurrency = 0;
        private const int ColumnPlaceTo = 3;
        private const int ColumnRace = 1;
        private const int ColumnTemplateCount = 10;
        private const int ColumnTotalTo = 5;
        private const int ColumnWinTo = 2;
        private const int ColumnWpto = 4;
        private const int RowContentStart = 2;
        private const int RowContentTemplate = 2;
        private const int RowGrandTotalTemplate = 9;
        private const int RowNoInfoTemplate = 11;
        private const int RowTotalCurrencyTemplate = 7;
        private const int RowTotalRaceTemplate = 5;

        private readonly ConvertingOption _convertingOption;

        private readonly DailyRacingWinLossViewModel _viewModel;

        private readonly DailyRacingWinLossData _dailyRacingWinLossData;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        protected internal DailyRacingWinLossExcel()
            : base(string.Empty, 0)
        {
        }

        public DailyRacingWinLossExcel(
                                       string templatePath,
                                       ConvertingOption convertingOption,
                                       ShowingOption showingOption,
                                       DailyRacingWinLossViewModel viewModel)
            : base(templatePath, viewModel.DailyRacingWinLossData.ContentData.Count, showingOption)
        {
            _cacheRefAdapter = new CacheRefAdapter();
            _convertingOption = convertingOption;
            _viewModel = viewModel;
            _dailyRacingWinLossData = viewModel.DailyRacingWinLossData;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var headerTextList = new[]
            {
                ViewResources.MeettingCurrency,
                ViewResources.Race,
                ViewResources.WinTO,
                ViewResources.PlaceTO,
                ViewResources.WPTO,
                ViewResources.TotalTO,
                ViewResources.DividendWon,
                ViewResources.CustomerCommission,
                ViewResources.Comm,
                ViewResources.AmtDueToRacing
            };
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            SetTextRowHeader(row, headerTextList);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentStart, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            var contentData = _dailyRacingWinLossData.ContentData;
            var contentTotalData = _dailyRacingWinLossData.ContentTotalData;

            var currentRow = RowContentStart;

            foreach (string leagueCurrency in contentData.Keys)
            {
                var leagueCurrencyData = contentData[leagueCurrency];
                foreach (DailyRacingWinLoss dailyRacingWinLoss in leagueCurrencyData)
                {
                    var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);
                    var name = GetMatchName(dailyRacingWinLoss);

                    AssignDataRow(row, dailyRacingWinLoss, name, dailyRacingWinLoss.RaceNo.ToString(CultureInfo.InvariantCulture));
                    currentRow++;
                }

                // Merge meeting - currency
                sheet.MergeCells(currentRow - leagueCurrencyData.Count, 0, currentRow - 1, 0);

                // Total common
                var leagueCurrencyTotalData = contentTotalData[leagueCurrency];
                var currencyRow = sheet.CopyRow(TemplateSheet, RowTotalRaceTemplate, currentRow);
                AssignDataRow(currencyRow, leagueCurrencyTotalData, ViewResources.Total, leagueCurrencyTotalData.CurrencyName);
                currentRow++;

                var currencyMyrRow = sheet.CopyRow(TemplateSheet, RowTotalRaceTemplate, currentRow);
                AssignDataMyr(currencyMyrRow, leagueCurrencyTotalData, ViewResources.Total, "RM");
                currentRow++;

                // Merge
                sheet.MergeCells(currentRow - 2, 0, currentRow - 1, 0);
            }

            AssignTotalData(sheet, currentRow);
            ProcessUI(sheet);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(
                                        ViewResources.DailyRacingTitle,
                                        string.Empty,
                                        _viewModel.Param.Date.ToString(FormatDate),
                                        ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerComm);
            }
        }

        /// <summary>
        /// Assign data to euro section.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="dailyRacingWinLoss">Daily Racing win loss data.</param>
        /// <param name="leagueName">League name.</param>
        /// <param name="raceName">Race name.</param>
        private static void AssignDataEur(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].SetCellValue(leagueName);
            row.Cells[ColumnRace].SetCellValue(raceName);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOEUR);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOEUR);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOEUR);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOEUR);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonEUR);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionEUR);
            row.Cells[ColumnCommIncome].SetCellValue(dailyRacingWinLoss.CommIncomeEUR);
            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingEUR);
        }

        private static void AssignDataMyr(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].SetCellValue(leagueName);
            row.Cells[ColumnRace].SetCellValue(raceName);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOMYR);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOMYR);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOMYR);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOMYR);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonMYR);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionMYR);
            row.Cells[ColumnCommIncome].SetCellValue(dailyRacingWinLoss.CommIncomeMYR);
            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingMYR);
        }

        private static void AssignDataRow(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].SetCellValue(leagueName);
            row.Cells[ColumnRace].SetCellValue(raceName);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTO);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTO);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTO);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTO);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWon);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommission);
            row.Cells[ColumnCommIncome].SetCellValue(dailyRacingWinLoss.CommIncome);
            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacing);
        }

        /// <summary>
        /// Assign data to USD section.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="dailyRacingWinLoss">Daily Racing win loss data.</param>
        /// <param name="leagueName">League name.</param>
        /// <param name="raceName">Race name.</param>
        private static void AssignDataUsd(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].SetCellValue(leagueName);
            row.Cells[ColumnRace].SetCellValue(raceName);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOUSD);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOUSD);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOUSD);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOUSD);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonUSD);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionUSD);
            row.Cells[ColumnCommIncome].SetCellValue(dailyRacingWinLoss.CommIncomeUSD);
            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingUSD);
        }

        private static void AssignDataGbp(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].SetCellValue(leagueName);
            row.Cells[ColumnRace].SetCellValue(raceName);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOGBP);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOGBP);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOGBP);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOGBP);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonGBP);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionGBP);
            row.Cells[ColumnCommIncome].SetCellValue(dailyRacingWinLoss.CommIncomeGBP);
            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingGBP);
        }

        // DataRow exportRow
        private string GetMatchName(DailyRacingWinLoss dailyRacingWinLoss)
        {
            var leageId = dailyRacingWinLoss.LeagueId;
            var curCurrency = dailyRacingWinLoss.CurrencyName.Trim(); // Save the current Currency
            return _cacheRefAdapter.GetLeagueName(leageId) + " - " + curCurrency;
        }

        /// <summary>
        /// Assign data to total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">The row index of total section.</param>
        private void AssignTotalData(ISheet sheet, int currentRow)
        {
            // Total currency
            var currencyTotalData = _dailyRacingWinLossData.CurrencyTotalData;
            foreach (string currencyName in currencyTotalData.Keys)
            {
                var currencyRow = sheet.CopyRow(TemplateSheet, RowTotalCurrencyTemplate, currentRow);
                AssignDataRow(currencyRow, currencyTotalData[currencyName], ViewResources.Total, currencyName);
                currentRow++;
            }

            // Merge total currency all
            sheet.MergeCells(currentRow - currencyTotalData.Count, 0, currentRow - 1, 0);

            // Grand Total
            var grandTotalMyrRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate, currentRow);
            AssignDataMyr(grandTotalMyrRow, _dailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.MYR, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            currentRow++;

            var grandTotalEurRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + 1, currentRow);
            AssignDataEur(grandTotalEurRow, _dailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.EUR, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            currentRow++;

            var grandTotalUsdRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + 1, currentRow);
            AssignDataUsd(grandTotalUsdRow, _dailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.USD, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            currentRow++;

            var grandTotalGbpRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + 1, currentRow);
            AssignDataGbp(grandTotalGbpRow, _dailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.GBP, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
        }
    }
}