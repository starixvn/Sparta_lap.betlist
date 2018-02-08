using System;
using System.Collections.Generic;
using System.Globalization;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class DailyRacingWinLossExcelTest : BaseReportExcelTest
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

        private static readonly DailyRacingWinLossViewModel ViewModel = new DailyRacingWinLossViewModel
        {
            DailyRacingWinLossData = new DailyRacingWinLossData
            {
                ContentData = new Dictionary<string, List<DailyRacingWinLoss>>(),
                ContentTotalData = new Dictionary<string, DailyRacingWinLoss>(),
            },
            Param = new DailyRacingWinLossMetaModel
            {
                Date = new DateTime(2018, 2, 3)
            }
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();
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

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);

                AssertSetTextRowHeader(row, headerTextList);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel();
            var reportTitle = string.Format(
                                       ViewResources.DailyRacingTitle,
                                       string.Empty,
                                       ViewModel.Param.Date.ToString(ReportBase.FormatDate),
                                       ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 2);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnCustomerComm);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var item = FakeHelper.Create<DailyRacingWinLoss>();
            var totalItem = FakeHelper.Create<DailyRacingWinLoss>();
            ViewModel.DailyRacingWinLossData.ContentData.Add("EUR", new List<DailyRacingWinLoss> { item });
            ViewModel.DailyRacingWinLossData.ContentTotalData.Add("EUR", totalItem);
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart);
            var currencyRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart + 1);
            var myrRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart + 2);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertAssignDataRow(row, item, " - " + item.CurrencyName, item.RaceNo.ToString(CultureInfo.InvariantCulture));
                AssertAssignDataRow(currencyRow, totalItem, ViewResources.Total, totalItem.CurrencyName);
                AssertAssignDataMyr(myrRow, totalItem, ViewResources.Total, "RM");
                excel.Received(1).AssignTotalData(FakeSheet, Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const string CurrencyName = "USD";
            const int RowIndex = 10;
            var totalData = FakeHelper.Create<DailyRacingWinLoss>();
            var grandTotalData = FakeHelper.Create<DailyRacingWinLoss>();
            ViewModel.DailyRacingWinLossData.CurrencyTotalData = new Dictionary<string, DailyRacingWinLoss>
            {
                [CurrencyName] = totalData
            };
            ViewModel.DailyRacingWinLossData.ContentTotalData.Add(CurrencyName, totalData);
            ViewModel.DailyRacingWinLossData.GrandTotalData = grandTotalData;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex);
            var myrRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex + 1);
            var eurRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex + 2);
            var usdRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex + 3);
            var gbpRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex + 4);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                AssertAssignDataRow(row, totalData, ViewResources.Total, CurrencyName);
                AssertAssignDataMyr(myrRow, grandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.MYR, string.Empty);
                AssertAssignDataEur(eurRow, grandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.EUR, string.Empty);
                AssertAssignDataUsd(usdRow, grandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.USD, string.Empty);
                AssertAssignDataGbp(gbpRow, grandTotalData, ViewResources.GrandTotalIn + " " + ViewResources.GBP, string.Empty);
            });
        }

        private static void AssertAssignDataEur(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].Received(1).SetCellValue(leagueName);
            row.Cells[ColumnRace].Received(1).SetCellValue(raceName);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOEUR);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOEUR);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOEUR);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOEUR);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonEUR);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionEUR);
            row.Cells[ColumnCommIncome].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeEUR);
            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingEUR);
        }

        private static void AssertAssignDataMyr(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].Received(1).SetCellValue(leagueName);
            row.Cells[ColumnRace].Received(1).SetCellValue(raceName);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOMYR);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOMYR);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOMYR);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOMYR);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonMYR);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionMYR);
            row.Cells[ColumnCommIncome].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeMYR);
            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingMYR);
        }

        private static void AssertAssignDataRow(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].Received(1).SetCellValue(leagueName);
            row.Cells[ColumnRace].Received(1).SetCellValue(raceName);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTO);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTO);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTO);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTO);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWon);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommission);
            row.Cells[ColumnCommIncome].Received(1).SetCellValue(dailyRacingWinLoss.CommIncome);
            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacing);
        }

        private static void AssertAssignDataUsd(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].Received(1).SetCellValue(leagueName);
            row.Cells[ColumnRace].Received(1).SetCellValue(raceName);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOUSD);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOUSD);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOUSD);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOUSD);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonUSD);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionUSD);
            row.Cells[ColumnCommIncome].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeUSD);
            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingUSD);
        }

        private static void AssertAssignDataGbp(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string leagueName, string raceName)
        {
            row.Cells[ColumnMeetingCurrency].Received(1).SetCellValue(leagueName);
            row.Cells[ColumnRace].Received(1).SetCellValue(raceName);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOGBP);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOGBP);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOGBP);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOGBP);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonGBP);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionGBP);
            row.Cells[ColumnCommIncome].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeGBP);
            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingGBP);
        }

        private static DailyRacingWinLossExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<DailyRacingWinLossExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                new ShowingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}