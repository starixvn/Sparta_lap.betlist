// --------------------------------------------------------------------------
// <copyright file="DailyRacingWinLossExcel.cs" company="">
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

        public DailyRacingWinLossExcel(
                                       string templatePath,
                                       ConvertingOption convertingOption,
                                       ShowingOption showingOption,
                                       DailyRacingWinLossViewModel viewModel)
            : base(templatePath, showingOption, convertingOption)
        {
            ViewModel = viewModel;
            DailyRacingWinLossData = viewModel.DailyRacingWinLossData;
            HeaderTextList = new List<string>
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
            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentStart, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            var contentData = DailyRacingWinLossData.ContentData;
            var contentTotalData = DailyRacingWinLossData.ContentTotalData;

            int currentRow = RowContentStart;

            foreach (string leagueCurrency in contentData.Keys)
            {
                List<DailyRacingWinLoss> leagueCurrencyData = contentData[leagueCurrency];
                foreach (DailyRacingWinLoss dailyRacingWinLoss in leagueCurrencyData)
                {
                    IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);
                    string name = GetMatchName(dailyRacingWinLoss);

                    AssignDataRow(row, dailyRacingWinLoss, name, dailyRacingWinLoss.RaceNo.ToString(CultureInfo.InvariantCulture));
                    currentRow++;
                }

                // Merge meeting - currency
                sheet.MergeCells(currentRow - leagueCurrencyData.Count, 0, currentRow - 1, 0);

                // Total common
                DailyRacingWinLoss leagueCurrencyTotalData = contentTotalData[leagueCurrency];
                IRow currencyRow = sheet.CopyRow(TemplateSheet, RowTotalRaceTemplate, currentRow);
                AssignDataRow(currencyRow, leagueCurrencyTotalData, ViewResources.Total, leagueCurrencyTotalData.CurrencyName);
                currentRow++;

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
                                        ViewResources.DailyRacingTitle,
                                        string.Empty,
                                        ViewModel.Param.Date.ToString(FormatDate),
                                        GetTitleCurrencyName());

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
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerComm);
            }
        }

        /// <summary>
        /// Assign data to euro section.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="dailyRacingWinLoss">Daily Racing win loss data.</param>
        /// <param name="leagueName">League name.</param>
        /// <param name="raceName">Race name.</param>
        protected void AssignDataEur(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
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

        /// <summary>
        /// Assign data to myanmar section.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="dailyRacingWinLoss">Daily Racing win loss data.</param>
        /// <param name="leagueName">League name.</param>
        /// <param name="raceName">Race name.</param>
        protected void AssignDataMyr(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
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

        /// <summary>
        /// Assign data to a row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="dailyRacingWinLoss">Daily Racing win loss data.</param>
        /// <param name="leagueName">League name.</param>
        /// <param name="raceName">Race name.</param>
        protected void AssignDataRow(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
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
        protected void AssignDataUsd(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
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

        /// <summary>
        /// Assign data to GBP section.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="dailyRacingWinLoss">Daily Racing win loss data.</param>
        /// <param name="leagueName">League name.</param>
        /// <param name="raceName">Race name.</param>
        protected void AssignDataGbp(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
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
        private static string GetMatchName(DailyRacingWinLoss dailyRacingWinLoss)
        {
            int leageId = dailyRacingWinLoss.LeagueId;
            string curCurrency = dailyRacingWinLoss.CurrencyName.Trim(); // Save the current Currency
            return CacheRef.GetLeagueName(leageId) + " - " + curCurrency;
        }

        /// <summary>
        /// Assign data to total section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">The row index of total section.</param>
        private void AssignTotalData(ISheet sheet, int currentRow)
        {
            // Total currency
            IDictionary<string, DailyRacingWinLoss> currencyTotalData = DailyRacingWinLossData.CurrencyTotalData;
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