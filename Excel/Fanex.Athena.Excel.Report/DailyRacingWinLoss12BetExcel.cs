// --------------------------------------------------------------------------
// <copyright file="DailyRacingWinLoss12BetExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Core.Helper;
    using Model.Report.DTO;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

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

        public DailyRacingWinLoss12BetExcel(
                                            string templatePath,
                                            string exportingFileName,
                                            ConvertingOption convertingOption,
                                            ShowingOption showingOption,
                                            DailyRacingWinLossViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            ViewModel = viewModel;
            DailyRacingWinLossData = viewModel.DailyRacingWinLossData;
            HeaderTextList = new List<string>
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
        }

        protected int ColumnCount
        {
            get
            {
                return ShowingOption.CustomerCommVisible ? ColumnTemplateCount : ColumnTemplateCount - 1;
            }
        }

        protected DailyRacingWinLossData DailyRacingWinLossData
        {
            get;
            set;
        }

        protected DailyRacingWinLossViewModel ViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        protected override void AssignData()
        {
            int numOfDataRow = DailyRacingWinLossData.ContentData.Count;
            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignTitle(sheet);
            AssignHeader(sheet);

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
        protected override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[0].SetCellValue(ViewResources.Race);
            row.Cells[2].SetCellValue(ViewResources.CustomerOverall);
            row.Cells[8].SetCellValue(ViewResources.Licensee12Bet);
            row.Cells[14].SetCellValue(ViewResources.AmtDueTo);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i + 1].SetCellValue(HeaderTextList[i]);
            }
        }

        /// <summary>
        /// Assign "no information" string to sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentStart, ColumnCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            // Get current row depend licensee id
            IDictionary<string, List<DailyRacingWinLoss>> contentData = DailyRacingWinLossData.ContentData;
            IDictionary<string, DailyRacingWinLoss> contentTotalData = DailyRacingWinLossData.ContentTotalData;

            int currentRow = RowContentStart;

            foreach (string leagueCurrency in contentData.Keys)
            {
                List<DailyRacingWinLoss> leagueCurrencyData = contentData[leagueCurrency];
                DailyRacingWinLoss firstRowData = leagueCurrencyData[0];
                string name = GetMatchName(firstRowData);

                // League - Currency
                IRow leagueCurrencyRow = sheet.CopyRow(TemplateSheet, RowLeagueTemplate, currentRow);

                leagueCurrencyRow.Cells[0].SetCellValue(name);
                sheet.MergeCells(currentRow, 0, currentRow, ColumnCount - 1);
                currentRow++;

                // RaceNo
                foreach (DailyRacingWinLoss dailyRacingWinLoss in leagueCurrencyData)
                {
                    IRow row = sheet.CopyRow(TemplateSheet, RowRaceTemplate, currentRow);

                    AssignDataRow(row, dailyRacingWinLoss, dailyRacingWinLoss.RaceNo.ToString(CultureInfo.InvariantCulture), string.Empty);
                    sheet.MergeCells(currentRow, 0, currentRow, 1);

                    currentRow++;
                }

                // Total common
                DailyRacingWinLoss leagueCurrencyTotalData = contentTotalData[leagueCurrency];
                IRow currencyRow = sheet.CopyRow(TemplateSheet, RowTotalRaceTemplate, currentRow);
                currentRow++;
                AssignDataRow(currencyRow, leagueCurrencyTotalData, ViewResources.Total, leagueCurrencyTotalData.CurrencyName);

                IRow currencyMyrRow = sheet.CopyRow(TemplateSheet, RowTotalRaceTemplate, currentRow);
                AssignDataMyr(currencyMyrRow, leagueCurrencyTotalData, ViewResources.Total, "RM");
                currentRow++;

                // Merge
                sheet.MergeCells(currentRow - 2, 0, currentRow - 1, 0);
            }

            AssignTotalData(sheet, currentRow);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                                        ViewResources.DailyRacing12BetTitle,
                                        ViewModel.Param.Date.ToString(FormatDate),
                                        GetTitleCurrencyName());

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnCount, ReportTitle);

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
                HideColumn(sheet, ColumnCustomerComm);
            }
        }

        /// <summary>
        /// Assign data to EUR row.
        /// </summary>
        /// <param name="row">The current row.</param>
        /// <param name="dailyRacingWinLoss">The daily racing win-loss.</param>
        /// <param name="cell1">The value for first cell.</param>
        /// <param name="cell2">The value for second cell.</param>
        protected void AssignDataEur(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
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

        /// <summary>
        /// Assign data to MYR row.
        /// </summary>
        /// <param name="row">The current row.</param>
        /// <param name="dailyRacingWinLoss">The daily racing win-loss.</param>
        /// <param name="cell1">The string value for first cell.</param>
        /// <param name="cell2">The string value for second cell.</param>
        protected void AssignDataMyr(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
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

        /// <summary>
        /// Assign data to row.
        /// </summary>
        /// <param name="row">The current row.</param>
        /// <param name="dailyRacingWinLoss">The daily racing win-loss.</param>
        /// <param name="cell1">The value for first cell.</param>
        /// <param name="cell2">The value for second row.</param>
        protected void AssignDataRow(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
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

        /// <summary>
        /// Assign data to USD row.
        /// </summary>
        /// <param name="row">The current row.</param>
        /// <param name="dailyRacingWinLoss">The daily racing win-loss.</param>
        /// <param name="cell1">The value  for first cell.</param>
        /// <param name="cell2">The value for second cell.</param>
        protected void AssignDataUsd(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
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

        /// <summary>
        /// Assign data to GBP row.
        /// </summary>
        /// <param name="row">The current row.</param>
        /// <param name="dailyRacingWinLoss">The daily racing win-loss.</param>
        /// <param name="cell1">The value  for first cell.</param>
        /// <param name="cell2">The value for second cell.</param>
        protected void AssignDataGbp(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
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
        private static string GetMatchName(DailyRacingWinLoss dailyRacingWinLoss)
        {
            int leagueId = dailyRacingWinLoss.LeagueId;
            string curCurrency = dailyRacingWinLoss.CurrencyName;
            return CacheRef.GetLeagueName(leagueId) + " - " + curCurrency;
        }

        /// <summary>
        /// Assign data to total row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">The current row index.</param>
        private void AssignTotalData(ISheet sheet, int currentRow)
        {
            // Total currency
            var currencyTotalData = DailyRacingWinLossData.CurrencyTotalData;
            foreach (string currencyName in currencyTotalData.Keys)
            {
                IRow currencyRow = sheet.CopyRow(TemplateSheet, RowTotalCurrencyTemplate, currentRow);
                AssignDataRow(currencyRow, currencyTotalData[currencyName], ViewResources.Total, currencyName);
                currentRow++;
            }

            // Merge total currency all
            sheet.MergeCells(currentRow - currencyTotalData.Count, 0, currentRow - 1, 0);

            // Grand Total
            IRow grandTotalMyrRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate, currentRow);
            AssignDataMyr(grandTotalMyrRow, DailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.MYR, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            currentRow++;

            IRow grandTotalEurRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + 1, currentRow);
            AssignDataEur(grandTotalEurRow, DailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.EUR, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            currentRow++;

            IRow grandTotalUsdRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + 1, currentRow);
            AssignDataUsd(grandTotalUsdRow, DailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.USD, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            currentRow++;

            IRow grandTotalGbpRow = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate + 1, currentRow);
            AssignDataGbp(grandTotalGbpRow, DailyRacingWinLossData.GrandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.GBP, string.Empty);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
        }
    }
}