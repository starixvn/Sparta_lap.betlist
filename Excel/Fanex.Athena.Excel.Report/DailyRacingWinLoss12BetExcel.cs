// --------------------------------------------------------------------------
// <copyright file="DailyRacingWinLoss12BetExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
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
    /// Daily racing win-loss.
    /// only used for 12bet licensee.
    /// </summary>
    public class DailyRacingWinLoss12BetExcel : ReportBase
    {
        private const int ColumnAmountDueTo = 14;
        private const int ColumnCommIncome12Bet = 13;
        private const int ColumnCustomerComm = 7;
        private const int ColumnDividenWon = 6;
        private const int ColumnPlaceTo = 3;
        private const int ColumnPlaceTo12Bet = 9;
        private const int ColumnTemplateCount = 15;
        private const int ColumnTotalTo = 5;
        private const int ColumnTotalTo12Bet = 11;
        private const int ColumnWinLoss12Bet = 12;
        private const int ColumnWinTo = 2;
        private const int ColumnWinTo12Bet = 8;
        private const int ColumnWpto = 4;
        private const int ColumnWpto12Bet = 10;
        private const int RowContentStart = 3;
        private const int RowGrandTotalTemplate = 9;
        private const int RowLeagueTemplate = 3;
        private const int RowNoInfoTemplate = 11;
        private const int RowRaceTemplate = 4;
        private const int RowTotalCurrencyTemplate = 7;
        private const int RowTotalRaceTemplate = 5;

        private readonly ConvertingOption _convertingOption;

        private readonly DailyRacingWinLossViewModel _viewModel;

        private readonly DailyRacingWinLossData _dailyRacingWinLossData;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        protected internal DailyRacingWinLoss12BetExcel()
            : base(string.Empty, 0)
        {
        }

        public DailyRacingWinLoss12BetExcel(
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

        protected int ColumnCount
        {
            get
            {
                return ShowingOption.CustomerCommVisible ? ColumnTemplateCount : ColumnTemplateCount - 1;
            }
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[0].SetCellValue(ViewResources.Race);
            row.Cells[2].SetCellValue(ViewResources.CustomerOverall);
            row.Cells[8].SetCellValue(ViewResources.Licensee12Bet);
            row.Cells[14].SetCellValue(ViewResources.AmtDueTo);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            var headerTextList = new[]
            {
                ViewResources.Race,
                ViewResources.WinTO,
                ViewResources.PlaceTO,
                ViewResources.WPTO,
                ViewResources.TotalTO,
                ViewResources.DividendWon,
                ViewResources.Customer + " " + ViewResources.Comm,
                ViewResources.WinTO,
                ViewResources.PlaceTO,
                ViewResources.WPTO,
                ViewResources.TotalTO,
                ViewResources.WinLoss,
                ViewResources.Comm,
                ViewResources.AmtDueTo
            };
            for (int i = 0; i < headerTextList.Length; i++)
            {
                row.Cells[i + 1].SetCellValue(headerTextList[i]);
            }
        }

        /// <summary>
        /// Assign "no information" string to sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentStart, ColumnCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            // Get current row depend licensee id
            var contentData = _dailyRacingWinLossData.ContentData;
            var contentTotalData = _dailyRacingWinLossData.ContentTotalData;

            var currentRow = RowContentStart;

            foreach (string leagueCurrency in contentData.Keys)
            {
                var leagueCurrencyData = contentData[leagueCurrency];
                var firstRowData = leagueCurrencyData[0];
                var name = GetMatchName(firstRowData);

                // League - Currency
                var leagueCurrencyRow = sheet.CopyRow(TemplateSheet, RowLeagueTemplate, currentRow);

                leagueCurrencyRow.Cells[0].SetCellValue(name);
                sheet.MergeCells(currentRow, 0, currentRow, ColumnCount - 1);
                currentRow++;

                // RaceNo
                foreach (DailyRacingWinLoss dailyRacingWinLoss in leagueCurrencyData)
                {
                    var row = sheet.CopyRow(TemplateSheet, RowRaceTemplate, currentRow);

                    AssignDataRow(row, dailyRacingWinLoss, dailyRacingWinLoss.RaceNo.ToString(CultureInfo.InvariantCulture), string.Empty);
                    sheet.MergeCells(currentRow, 0, currentRow, 1);

                    currentRow++;
                }

                // Total common
                var leagueCurrencyTotalData = contentTotalData[leagueCurrency];
                var currencyRow = sheet.CopyRow(TemplateSheet, RowTotalRaceTemplate, currentRow);
                currentRow++;
                AssignDataRow(currencyRow, leagueCurrencyTotalData, ViewResources.Total, leagueCurrencyTotalData.CurrencyName);

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
                                        ViewResources.DailyRacing12BetTitle,
                                        _viewModel.Param.Date.ToString(FormatDate),
                                        ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentStart);
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

        private static void AssignDataEur(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].SetCellValue(cell1);
            row.Cells[1].SetCellValue(cell2);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOEUR);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOEUR);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOEUR);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOEUR);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonEUR);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionEUR);

            row.Cells[ColumnWinTo12Bet].SetCellValue(dailyRacingWinLoss.WinTO12BetEUR);
            row.Cells[ColumnPlaceTo12Bet].SetCellValue(dailyRacingWinLoss.PlaceTO12BetEUR);
            row.Cells[ColumnWpto12Bet].SetCellValue(dailyRacingWinLoss.WPTO12BetEUR);
            row.Cells[ColumnTotalTo12Bet].SetCellValue(dailyRacingWinLoss.TotalTO12BetEUR);
            row.Cells[ColumnWinLoss12Bet].SetCellValue(dailyRacingWinLoss.WinLoss12BetEUR);
            row.Cells[ColumnCommIncome12Bet].SetCellValue(dailyRacingWinLoss.CommIncomeEUR);

            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingEUR);
        }

        private static void AssignDataMyr(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].SetCellValue(cell1);
            row.Cells[1].SetCellValue(cell2);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOMYR);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOMYR);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOMYR);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOMYR);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonMYR);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionMYR);

            row.Cells[ColumnWinTo12Bet].SetCellValue(dailyRacingWinLoss.WinTO12BetMYR);
            row.Cells[ColumnPlaceTo12Bet].SetCellValue(dailyRacingWinLoss.PlaceTO12BetMYR);
            row.Cells[ColumnWpto12Bet].SetCellValue(dailyRacingWinLoss.WPTO12BetMYR);
            row.Cells[ColumnTotalTo12Bet].SetCellValue(dailyRacingWinLoss.TotalTO12BetMYR);
            row.Cells[ColumnWinLoss12Bet].SetCellValue(dailyRacingWinLoss.WinLoss12BetMYR);
            row.Cells[ColumnCommIncome12Bet].SetCellValue(dailyRacingWinLoss.CommIncomeMYR);

            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingMYR);
        }

        private static void AssignDataRow(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].SetCellValue(cell1);
            row.Cells[1].SetCellValue(cell2);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTO);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTO);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTO);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTO);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWon);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommission);

            row.Cells[ColumnWinTo12Bet].SetCellValue(dailyRacingWinLoss.WinTO12Bet);
            row.Cells[ColumnPlaceTo12Bet].SetCellValue(dailyRacingWinLoss.PlaceTO12Bet);
            row.Cells[ColumnWpto12Bet].SetCellValue(dailyRacingWinLoss.WPTO12Bet);
            row.Cells[ColumnTotalTo12Bet].SetCellValue(dailyRacingWinLoss.TotalTO12Bet);
            row.Cells[ColumnWinLoss12Bet].SetCellValue(dailyRacingWinLoss.WinLoss12Bet);
            row.Cells[ColumnCommIncome12Bet].SetCellValue(dailyRacingWinLoss.CommIncome);

            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacing);
        }

        private static void AssignDataUsd(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].SetCellValue(cell1);
            row.Cells[1].SetCellValue(cell2);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOUSD);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOUSD);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOUSD);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOUSD);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonUSD);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionUSD);

            row.Cells[ColumnWinTo12Bet].SetCellValue(dailyRacingWinLoss.WinTO12BetUSD);
            row.Cells[ColumnPlaceTo12Bet].SetCellValue(dailyRacingWinLoss.PlaceTO12BetUSD);
            row.Cells[ColumnWpto12Bet].SetCellValue(dailyRacingWinLoss.WPTO12BetUSD);
            row.Cells[ColumnTotalTo12Bet].SetCellValue(dailyRacingWinLoss.TotalTO12BetUSD);
            row.Cells[ColumnWinLoss12Bet].SetCellValue(dailyRacingWinLoss.WinLoss12BetUSD);
            row.Cells[ColumnCommIncome12Bet].SetCellValue(dailyRacingWinLoss.CommIncomeUSD);

            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingUSD);
        }

        private static void AssignDataGbp(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].SetCellValue(cell1);
            row.Cells[1].SetCellValue(cell2);

            row.Cells[ColumnWinTo].SetCellValue(dailyRacingWinLoss.WinTOGBP);
            row.Cells[ColumnPlaceTo].SetCellValue(dailyRacingWinLoss.PlaceTOGBP);
            row.Cells[ColumnWpto].SetCellValue(dailyRacingWinLoss.WPTOGBP);
            row.Cells[ColumnTotalTo].SetCellValue(dailyRacingWinLoss.TotalTOGBP);
            row.Cells[ColumnDividenWon].SetCellValue(dailyRacingWinLoss.DividendWonGBP);
            row.Cells[ColumnCustomerComm].SetCellValue(dailyRacingWinLoss.CustomerCommissionGBP);

            row.Cells[ColumnWinTo12Bet].SetCellValue(dailyRacingWinLoss.WinTO12BetGBP);
            row.Cells[ColumnPlaceTo12Bet].SetCellValue(dailyRacingWinLoss.PlaceTO12BetGBP);
            row.Cells[ColumnWpto12Bet].SetCellValue(dailyRacingWinLoss.WPTO12BetGBP);
            row.Cells[ColumnTotalTo12Bet].SetCellValue(dailyRacingWinLoss.TotalTO12BetGBP);
            row.Cells[ColumnWinLoss12Bet].SetCellValue(dailyRacingWinLoss.WinLoss12BetGBP);
            row.Cells[ColumnCommIncome12Bet].SetCellValue(dailyRacingWinLoss.CommIncomeGBP);

            row.Cells[ColumnAmountDueTo].SetCellValue(dailyRacingWinLoss.AmountDueToRacingGBP);
        }

        /// <summary>
        /// Get match name from remoted cache.
        /// </summary>
        /// <param name="dailyRacingWinLoss">The daily racing win-loss.</param>
        /// <returns>Return the match name.</returns>
        private string GetMatchName(DailyRacingWinLoss dailyRacingWinLoss)
        {
            var leagueId = dailyRacingWinLoss.LeagueId;
            var curCurrency = dailyRacingWinLoss.CurrencyName;
            return _cacheRefAdapter.GetLeagueName(leagueId) + " - " + curCurrency;
        }

        protected internal virtual void AssignTotalData(ISheet sheet, int currentRow)
        {
            // Total currency
            var currencyTotalData = _dailyRacingWinLossData.CurrencyTotalData;
            foreach (var currencyData in currencyTotalData)
            {
                var currencyRow = sheet.CopyRow(TemplateSheet, RowTotalCurrencyTemplate, currentRow);
                AssignDataRow(currencyRow, currencyData.Value, ViewResources.Total, currencyData.Key);
                currentRow++;
            }

            // Merge total currency all
            sheet.MergeCells(currentRow - currencyTotalData.Count, 0, currentRow - 1, 0);

            // Grand Total
            var currencies = new[] { ViewResources.MYR, ViewResources.EUR, ViewResources.USD, ViewResources.GBP };
            var actions = new Action<IRow, DailyRacingWinLoss, string>[] 
            {
                 (row, item, cell1) => AssignDataMyr(row, item, cell1, string.Empty),
                 (row, item, cell1) => AssignDataEur(row, item, cell1, string.Empty),
                 (row, item, cell1) => AssignDataUsd(row, item, cell1, string.Empty),
                 (row, item, cell1) => AssignDataGbp(row, item, cell1, string.Empty)
            };
            for (int i = 0; i < currencies.Length; i++)
            {
                var totalRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + (i == 0 ? 0 : 1), currentRow);
                actions[i](totalRow, _dailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + currencies[i]);
                sheet.MergeCells(currentRow, 0, currentRow, 1);
                currentRow++;
            }
        }
    }
}