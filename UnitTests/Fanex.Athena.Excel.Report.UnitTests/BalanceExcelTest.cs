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
        private const string Date = "abc";

        private const int ColumnCount = 21;

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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(Date);
                cell.Received(1).SetCellValue(DataItem.LicWinLoss);
                cell.Received(1).SetCellValue(DataItem.BetTradeCommission);
                cell.Received(1).SetCellValue(DataItem.BACommission);
                cell.Received(1).SetCellValue(DataItem.RacingWinLoss);
                cell.Received(1).SetCellValue(DataItem.AmtDueToBetTrade);
                cell.Received(1).SetCellValue(DataItem.AmtDueToBA);
                cell.Received(1).SetCellValue(DataItem.AmtDueToRacing);
                cell.Received(1).SetCellValue(DataItem.CustomerCommission);
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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(ViewResources.LicenseeWinLoss);
                cell.Received(1).SetCellValue(ViewResources.BetTradeCommission);
                cell.Received(1).SetCellValue(ViewResources.BACommission);
                cell.Received(1).SetCellValue(ViewResources.RacingRevenue);
                cell.Received(1).SetCellValue(ViewResources.AmtDueToBetTrade);
                cell.Received(1).SetCellValue(ViewResources.AmtDueToBA);
                cell.Received(1).SetCellValue(ViewResources.AmtDueToRacing);
                cell.Received(1).SetCellValue(ViewResources.CustomerCommission);

                cell.Received(1).SetCellValue(ViewResources.CustomerPedingFunds);
                cell.Received(1).SetCellValue(ViewResources.TotalCustomerActiveClosedSuspended);
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