using System.Collections.Generic;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class BalanceExcelTest : BaseReportExcelTest
    {
        private const int ColumnDate = 0;
        private const int ColumnLicenseeWl = 1;
        private const int ColumnBetTradeCommission = 2;
        private const int ColumnBaCommission = 3;
        private const int ColumnRacingRevenue = 4;
        private const int ColumnAmtDueToBetTrade = 5;
        private const int ColumnAmtDueToBa = 6;
        private const int ColumnAmtDueToRacing = 7;
        private const int ColumnCustomerCommission = 8;

        private const string Date = "abc";

        private const int ColumnCount = 9;

        private static readonly BalanceDetails DataItem = FakeHelper.Create<BalanceDetails>();

        private static readonly Balance Balance = new Balance
        {
            BalanceList = new List<BalanceDetails> { DataItem }
        };

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 2);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 6);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 7);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnDate].Received(1).SetCellValue(Date);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(DataItem.LicWinLoss);
                row.Cells[ColumnBetTradeCommission].Received(1).SetCellValue(DataItem.BetTradeCommission);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(DataItem.BACommission);
                row.Cells[ColumnRacingRevenue].Received(1).SetCellValue(DataItem.RacingWinLoss);
                row.Cells[ColumnAmtDueToBetTrade].Received(1).SetCellValue(DataItem.AmtDueToBetTrade);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(DataItem.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].Received(1).SetCellValue(DataItem.AmtDueToRacing);
                row.Cells[ColumnCustomerCommission].Received(1).SetCellValue(DataItem.CustomerCommission);
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = string.Format(ViewResources.BalanceTitle, ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(expectedTitle);
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 1);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 6);
            var row3 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 7);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 5);
            var excel = CreateExcel();
            var columIndex = 1;

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.LicenseeWinLoss);
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.BetTradeCommission);
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.BACommission);
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.RacingRevenue);
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.AmtDueToBetTrade);
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.AmtDueToBA);
                row1.Cells[columIndex++].Received(1).SetCellValue(ViewResources.AmtDueToRacing);
                row1.Cells[columIndex].Received(1).SetCellValue(ViewResources.CustomerCommission);

                row2.Cells[0].Received(1).SetCellValue(ViewResources.CustomerPedingFunds);
                row3.Cells[0].Received(1).SetCellValue(ViewResources.TotalCustomerActiveClosedSuspended);
            });
        }

        [Test]
        public void ProcessUI_WhenCall_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertHideColumns(FakeSheet, ColumnBaCommission, ColumnAmtDueToBa);
                AssertHideColumns(FakeSheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
                AssertHideColumns(FakeSheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
                AssertHideColumns(FakeSheet, ColumnBetTradeCommission, ColumnAmtDueToBetTrade);
                AssertHideColumns(FakeSheet, ColumnCustomerCommission);
            });
        }

        [Test]
        public void ProcessUI_AllProductsVisible_NotHideAnyColumn()
        {
            // Arrange
            var showingOption = new ShowingOption
            {
                BAVisible = true,
                RBVisible = true,
                BTVisible = true,
                CustomerCommVisible = true
            };

            var excel = CreateExcel(showingOption);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, false);
            });
        }

        private static BalanceExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<BalanceExcelTestable>(
                string.Empty,
                showingOption ?? new ShowingOption(),
                new ConvertingOption(),
                Balance,
                Date);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}