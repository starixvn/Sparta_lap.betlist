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
    public class BalanceExcelTest
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

        private static readonly BalanceDetails DataItem = new BalanceDetails
        {
            LicWinLoss = 1,
            BetTradeCommission = 2,
            BACommission = 3,
            RacingRevenue = 100,
            RBCommission = 200,
            AmtDueToBetTrade = 6,
            AmtDueToBA = 7,
            AmtDueToRacing = 8,
            CustomerCommission = 9
        };

        private static readonly Balance Balance = new Balance
        {
            BalanceList = new List<BalanceDetails>
            {
                DataItem
            }
        };

        private ISheet _sheet;

        [SetUp]
        public void Setup()
        {
            _sheet = Substitute.For<ISheet>();
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 2);
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 6);
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 7);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnDate].SetCellValue(Date);
                row.Cells[ColumnLicenseeWl].SetCellValue(DataItem.LicWinLoss);
                row.Cells[ColumnBetTradeCommission].SetCellValue(DataItem.BetTradeCommission);
                row.Cells[ColumnBaCommission].SetCellValue(DataItem.BACommission);
                row.Cells[ColumnRacingRevenue].SetCellValue(DataItem.RacingWinLoss);
                row.Cells[ColumnAmtDueToBetTrade].SetCellValue(DataItem.AmtDueToBetTrade);
                row.Cells[ColumnAmtDueToBa].SetCellValue(DataItem.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].SetCellValue(DataItem.AmtDueToRacing);
                row.Cells[ColumnCustomerCommission].SetCellValue(DataItem.CustomerCommission);
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var expectedTitle = string.Format(ViewResources.BalanceTitle, ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            _sheet.Received().SetTextRowValue(
                    excel.TemplateSheet,
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    expectedTitle);
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 1);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 6);
            var row3 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 7);
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 5);
            var excel = CreateExcel();
            var columIndex = 1;

            // Action
            excel.Public_AssignHeader(_sheet);

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
            const int ColumnBetTradeCommission = 2;
            const int ColumnBaCommission = 3;
            const int ColumnRacingRevenue = 4;
            const int ColumnAmtDueToBetTrade = 5;
            const int ColumnAmtDueToBa = 6;
            const int ColumnAmtDueToRacing = 7;
            const int ColumnCustomerCommission = 8;
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
                _sheet.Received().HideColumn(ColumnRacingRevenue, ColumnAmtDueToRacing);
                _sheet.Received().HideColumn(ColumnRacingRevenue, ColumnAmtDueToRacing);
                _sheet.Received().HideColumn(ColumnBetTradeCommission, ColumnAmtDueToBetTrade);
                _sheet.Received().HideColumn(ColumnCustomerCommission);
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
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.DidNotReceiveWithAnyArgs().HideColumn(0);
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