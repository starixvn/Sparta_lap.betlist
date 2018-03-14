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
    public class DailyRacingWinLoss12BetExcelTest : BaseReportExcelTest
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
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                ViewResources.Race, ViewResources.WinTO, ViewResources.PlaceTO,
                ViewResources.WPTO, ViewResources.TotalTO, ViewResources.DividendWon,
                ViewResources.Customer + " " + ViewResources.Comm, ViewResources.WinTO,
                ViewResources.PlaceTO, ViewResources.WPTO, ViewResources.TotalTO,
                ViewResources.WinLoss, ViewResources.Comm, ViewResources.AmtDueTo
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row1.Cells[0].Received(1).SetCellValue(ViewResources.Race);
                row1.Cells[2].Received(1).SetCellValue(ViewResources.CustomerOverall);
                row1.Cells[8].Received(1).SetCellValue(ViewResources.Licensee12Bet);
                row1.Cells[14].Received(1).SetCellValue(ViewResources.AmtDueTo);

                AssertSetTextRowHeader(row2, headerTextList, 1);
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
                                       ViewResources.DailyRacing12BetTitle,
                                       ViewModel.Param.Date.ToString(ReportBase.FormatDate),
                                       ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
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
            var childRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart + 1);
            var totalRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart + 2);
            var myrRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentStart + 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).Received(1).SetCellValue(" - " + item.CurrencyName);

                AssertAssignDataRow(childRow, item, item.RaceNo.ToString(CultureInfo.InvariantCulture), string.Empty);
                AssertAssignDataRow(totalRow, totalItem, ViewResources.Total, totalItem.CurrencyName);
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

        private static void AssertAssignDataRow(IRow row, DailyRacingWinLoss dataItem, string cell1, string cell2)
        {
            row.Cells[0].Received(1).SetCellValue(cell1);
            row.Cells[1].Received(1).SetCellValue(cell2);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dataItem.WinTO);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dataItem.PlaceTO);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dataItem.WPTO);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dataItem.TotalTO);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dataItem.DividendWon);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dataItem.CustomerCommission);

            row.Cells[ColumnWinTo12Bet].Received(1).SetCellValue(dataItem.WinTO12Bet);
            row.Cells[ColumnPlaceTo12Bet].Received(1).SetCellValue(dataItem.PlaceTO12Bet);
            row.Cells[ColumnWpto12Bet].Received(1).SetCellValue(dataItem.WPTO12Bet);
            row.Cells[ColumnTotalTo12Bet].Received(1).SetCellValue(dataItem.TotalTO12Bet);
            row.Cells[ColumnWinLoss12Bet].Received(1).SetCellValue(dataItem.WinLoss12Bet);
            row.Cells[ColumnCommIncome12Bet].Received(1).SetCellValue(dataItem.CommIncome);

            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dataItem.AmountDueToRacing);
        }

        private static void AssertAssignDataMyr(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].Received(1).SetCellValue(cell1);
            row.Cells[1].Received(1).SetCellValue(cell2);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOMYR);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOMYR);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOMYR);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOMYR);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonMYR);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionMYR);

            row.Cells[ColumnWinTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinTO12BetMYR);
            row.Cells[ColumnPlaceTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTO12BetMYR);
            row.Cells[ColumnWpto12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WPTO12BetMYR);
            row.Cells[ColumnTotalTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.TotalTO12BetMYR);
            row.Cells[ColumnWinLoss12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinLoss12BetMYR);
            row.Cells[ColumnCommIncome12Bet].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeMYR);

            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingMYR);
        }

        private static void AssertAssignDataEur(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].Received(1).SetCellValue(cell1);
            row.Cells[1].Received(1).SetCellValue(cell2);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOEUR);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOEUR);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOEUR);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOEUR);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonEUR);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionEUR);

            row.Cells[ColumnWinTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinTO12BetEUR);
            row.Cells[ColumnPlaceTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTO12BetEUR);
            row.Cells[ColumnWpto12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WPTO12BetEUR);
            row.Cells[ColumnTotalTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.TotalTO12BetEUR);
            row.Cells[ColumnWinLoss12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinLoss12BetEUR);
            row.Cells[ColumnCommIncome12Bet].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeEUR);

            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingEUR);
        }

        private static void AssertAssignDataUsd(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].Received(1).SetCellValue(cell1);
            row.Cells[1].Received(1).SetCellValue(cell2);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOUSD);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOUSD);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOUSD);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOUSD);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonUSD);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionUSD);

            row.Cells[ColumnWinTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinTO12BetUSD);
            row.Cells[ColumnPlaceTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTO12BetUSD);
            row.Cells[ColumnWpto12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WPTO12BetUSD);
            row.Cells[ColumnTotalTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.TotalTO12BetUSD);
            row.Cells[ColumnWinLoss12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinLoss12BetUSD);
            row.Cells[ColumnCommIncome12Bet].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeUSD);

            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingUSD);
        }

        private static void AssertAssignDataGbp(IRow row, DailyRacingWinLoss dailyRacingWinLoss, string cell1, string cell2)
        {
            row.Cells[0].Received(1).SetCellValue(cell1);
            row.Cells[1].Received(1).SetCellValue(cell2);

            row.Cells[ColumnWinTo].Received(1).SetCellValue(dailyRacingWinLoss.WinTOGBP);
            row.Cells[ColumnPlaceTo].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTOGBP);
            row.Cells[ColumnWpto].Received(1).SetCellValue(dailyRacingWinLoss.WPTOGBP);
            row.Cells[ColumnTotalTo].Received(1).SetCellValue(dailyRacingWinLoss.TotalTOGBP);
            row.Cells[ColumnDividenWon].Received(1).SetCellValue(dailyRacingWinLoss.DividendWonGBP);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dailyRacingWinLoss.CustomerCommissionGBP);

            row.Cells[ColumnWinTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinTO12BetGBP);
            row.Cells[ColumnPlaceTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.PlaceTO12BetGBP);
            row.Cells[ColumnWpto12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WPTO12BetGBP);
            row.Cells[ColumnTotalTo12Bet].Received(1).SetCellValue(dailyRacingWinLoss.TotalTO12BetGBP);
            row.Cells[ColumnWinLoss12Bet].Received(1).SetCellValue(dailyRacingWinLoss.WinLoss12BetGBP);
            row.Cells[ColumnCommIncome12Bet].Received(1).SetCellValue(dailyRacingWinLoss.CommIncomeGBP);

            row.Cells[ColumnAmountDueTo].Received(1).SetCellValue(dailyRacingWinLoss.AmountDueToRacingGBP);
        }

        private static DailyRacingWinLoss12BetExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<DailyRacingWinLoss12BetExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                new ShowingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}