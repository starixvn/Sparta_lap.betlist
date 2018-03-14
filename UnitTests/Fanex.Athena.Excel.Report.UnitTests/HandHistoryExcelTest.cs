using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Casino.DTO;
using Fanex.Athena.Model.Casino.MetaModel;
using Fanex.Athena.Model.Casino.ViewModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class HandHistoryExcelTest : BaseReportExcelTest
    {
        private const int ColumnGameId = 1;
        private const int ColumnPlayerHistoryNo = 2;
        private const int ColumnGameType = 3;
        private const int ColumnGameGroup = 4;
        private const int ColumnStartDate = 5;
        private const int ColumnEndDate = 6;
        private const int ColumnStartBalance = 7;
        private const int ColumnEndBalance = 8;
        private const int ColumnStake = 9;
        private const int ColumnBet = 10;
        private const int ColumnWinLoss = 11;
        private const int ColumnRollingComm = 12;
        private const int ColumnResults = 13;

        private const int ColumnCount = 14;

        private static readonly RNGCasinoResult DataItem = FakeHelper.Create<RNGCasinoResult>();

        private static readonly RNGCasinoResult TotalData = FakeHelper.Create<RNGCasinoResult>();

        private static readonly RNGCasinoResultMetaModel MetaModel = new RNGCasinoResultMetaModel
        {
            CurrencyName = "UUS",
            ExternalId = "abc",
            SystemId = "def",
            RefNo = 123
        };

        private static readonly RNGCasinoResultViewModel ViewModel = new RNGCasinoResultViewModel
        {
            Data = new List<RNGCasinoResult> { DataItem }
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#", ViewResources.GameID, ViewResources.PlayerHistoryNo,
                ViewResources.GameType, ViewResources.GameGroup, ViewResources.StartDate,
                ViewResources.EndDate, ViewResources.StartBalance, ViewResources.EndBalance,
                ViewResources.Stake, ViewResources.Bet, ViewResources.WinLoss,
                ViewResources.RollingComm, ViewResources.Results
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            AssertSetTextRowHeader(row, headerTextList);
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            FakeSheet.Received().SetTextRowValue(excel.TemplateSheet, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 4);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.When(x => x.AssignGroupTotal(Arg.Any<ISheet>(), Arg.Any<RNGCasinoResult>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnGameId].Received(1).SetCellValue(DataItem.GameId);
                row.Cells[ColumnPlayerHistoryNo].Received(1).SetCellValue(DataItem.PlayerHistoryNo);
                row.Cells[ColumnGameType].Received(1).SetCellValue(DataItem.GameTypeName);
                row.Cells[ColumnGameGroup].Received(1).SetCellValue(DataItem.GameGroupName);
                row.Cells[ColumnStartDate].Received(1).SetCellValue(DataItem.StartTime);
                row.Cells[ColumnEndDate].Received(1).SetCellValue(DataItem.EndTime);
                row.Cells[ColumnStartBalance].Received(1).SetCellValue(DataItem.BeginBalance);
                row.Cells[ColumnEndBalance].Received(1).SetCellValue(DataItem.EndBlance);
                row.Cells[ColumnStake].Received(1).SetCellValue(DataItem.Stake);
                row.Cells[ColumnBet].Received(1).SetCellValue(DataItem.Bet);
                row.Cells[ColumnWinLoss].Received(1).SetCellValue(DataItem.WinLoss);
                row.Cells[ColumnRollingComm].Received(1).SetCellValue(DataItem.RollingComm);
                row.Cells[ColumnResults].Received(1).SetCellValue(DataItem.Result);

                excel.Received(1).AssignGroupTotal(Arg.Any<ISheet>(), Arg.Any<RNGCasinoResult>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [TestCase(0, "USD", "Username: abc(def) -  Ref No: 123 - View in USD")]
        [TestCase(1, "USD", "Username: abc(def) -  Ref No: 123 - View in USD")]
        [TestCase(1, "", "Username: abc(def) -  Ref No: 123 - View in UUS")]
        public void AssignTitle_WhenCall_CallRelevantMethods(int currencyId, string baseCurrency, string expectedTitle)
        {
            // Arrange
            MetaModel.CurrencyId = currencyId;
            MetaModel.BaseCurrency = baseCurrency;
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row1.Cells[0].Received(1).SetCellValue(ViewResources.CASINOHANDHISTORY);
                row2.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void AssignGrandTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGrandTotal(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnEndBalance].Received(1).SetCellValue(ViewResources.GrandTotal);
                row.Cells[ColumnStake].Received(1).SetCellValue(TotalData.Stake);
                row.Cells[ColumnBet].Received(1).SetCellValue(TotalData.Bet);
                row.Cells[ColumnWinLoss].Received(1).SetCellValue(TotalData.WinLoss);
                row.Cells[ColumnRollingComm].Received(1).SetCellValue(TotalData.RollingComm);
            });
        }

        [Test]
        public void AssignGroupTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGroupTotal(FakeSheet, TotalData, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnEndBalance].Received(1).SetCellValue(ViewResources.Total);
                row.Cells[ColumnStake].Received(1).SetCellValue(TotalData.Stake);
                row.Cells[ColumnBet].Received(1).SetCellValue(TotalData.Bet);
                row.Cells[ColumnWinLoss].Received(1).SetCellValue(TotalData.WinLoss);
                row.Cells[ColumnRollingComm].Received(1).SetCellValue(TotalData.RollingComm);
            });
        }

        private static HandHistoryExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<HandHistoryExcelTestable>(
                string.Empty,
                MetaModel,
                ViewModel,
                TotalData);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}