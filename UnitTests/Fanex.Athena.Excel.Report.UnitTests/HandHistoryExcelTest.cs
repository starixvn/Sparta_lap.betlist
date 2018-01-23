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
    public class HandHistoryExcelTest
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

        private static readonly RNGCasinoResult DataItem = new RNGCasinoResult
        {
            Index = 1,
            GameId = "2",
            PlayerHistoryNo = "3",
            GameTypeName = "4",
            GameGroupName = "5",
            StartTime = "6",
            EndTime = "7",
            BeginBalance = 8,
            EndBlance = 9,
            Stake = 10,
            Bet = 11,
            WinLoss = 12,
            RollingComm = 13,
            Result = "14"
        };

        private static readonly RNGCasinoResult TotalData = new RNGCasinoResult
        {
            Index = 11,
            GameId = "22",
            PlayerHistoryNo = "33",
            GameTypeName = "44",
            GameGroupName = "55",
            StartTime = "66",
            EndTime = "77",
            BeginBalance = 88,
            EndBlance = 99,
            Stake = 100,
            Bet = 111,
            WinLoss = 122,
            RollingComm = 133,
            Result = "144"
        };

        private static readonly RNGCasinoResultMetaModel MetaModel = new RNGCasinoResultMetaModel
        {
            CurrencyName = "UUS",
            ExternalId = "abc",
            SystemId = "def",
            RefNo = 123
        };

        private static readonly RNGCasinoResultViewModel ViewModel = new RNGCasinoResultViewModel
        {
            Data = new List<RNGCasinoResult>
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
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate + 1, ReportBase.RowHeaderTemplate + 1);
                row.Cells[0].Received(1).SetCellValue("#");
                row.Cells[1].Received(1).SetCellValue(ViewResources.GameID);
                row.Cells[2].Received(1).SetCellValue(ViewResources.PlayerHistoryNo);
                row.Cells[3].Received(1).SetCellValue(ViewResources.GameType);
                row.Cells[4].Received(1).SetCellValue(ViewResources.GameGroup);
                row.Cells[5].Received(1).SetCellValue(ViewResources.StartDate);
                row.Cells[6].Received(1).SetCellValue(ViewResources.EndDate);
                row.Cells[7].Received(1).SetCellValue(ViewResources.StartBalance);
                row.Cells[8].Received(1).SetCellValue(ViewResources.EndBalance);
                row.Cells[9].Received(1).SetCellValue(ViewResources.Stake);
                row.Cells[10].Received(1).SetCellValue(ViewResources.Bet);
                row.Cells[11].Received(1).SetCellValue(ViewResources.WinLoss);
                row.Cells[12].Received(1).SetCellValue(ViewResources.RollingComm);
                row.Cells[13].Received(1).SetCellValue(ViewResources.Results);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(_sheet);

            // Assert
            _sheet.Received().SetTextRowValue(excel.TemplateSheet, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 4);
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.When(x => x.AssignGroupTotal(Arg.Any<ISheet>(), Arg.Any<RNGCasinoResult>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet);

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
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row1.Cells[0].Received(1).SetCellValue(ViewResources.CASINOHANDHISTORY);
                row2.Cells[0].Received(1).SetCellValue(expectedTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void AssignGrandTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGrandTotal(_sheet, RowIndex);

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
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGroupTotal(_sheet, TotalData, RowIndex);

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

        private static HandHistoryExcelTestable CreateExcel(int level = 0)
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