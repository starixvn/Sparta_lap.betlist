using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
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
    public class CustomerTurnoverExcelTest : BaseReportExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnUserName = 2;
        private const int ColumnCurrency = 3;
        private const int ColumnSport = 4;
        private const int ColumnBetType = 5;
        private const int ColumnEffectiveBettingValue = 6;
        private const int ColumnTotalTransaction = 7;
        private const int ColumnTurnover = 8;
        private const int ColumnTotalTurnover = 9;
        private const int ColumnCustomerWl = 10;
        private const int ColumnCustomerComm = 11;
        private const int ColumnCustomerTotal = 12;

        private const string DateTimeFormat = "yyyy/mm/dd";
        private const string ReportTitle = "haaaaaaaaaaaaaaaaaa";
        private const int ColumnCount = 20;

        private static readonly CustTurnover DataItem = FakeHelper.Create<CustTurnover>();

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#", ViewResources.Date, ViewResources.Username,
                ViewResources.Currency, ViewResources.Sport, ViewResources.BetType,
                ViewResources.EffectiveBettingValue, ViewResources.TotalTransaction, ViewResources.Turnover,
                ViewResources.Total + " " + ViewResources.Turnover, ViewResources.CustomerW_L,
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertSetTextRowHeader(row1, headerTextList);

                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);
            });
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
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalRow(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(DataItem.Index);
                row.Cells[ColumnDate].Received(1).SetCellValue(DataItem.ReportBy.ToString(DateTimeFormat));
                row.Cells[ColumnUserName].Received(1).SetCellValue(DataItem.UserName);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.Currency);
                row.Cells[ColumnTotalTurnover].Received(1).SetCellValue(DataItem.TotalTurnover);
                row.Cells[ColumnSport].Received(1).SetCellValue(DataItem.SportType);

                row.Cells[ColumnBetType].Received(1).SetCellValue(DataItem.Bettype);
                row.Cells[ColumnEffectiveBettingValue].Received(1).SetCellValue(DataItem.EffectiveBettingValue);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransactions);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerWinLoss + DataItem.CustomerComm);

                excel.Received(1).AssignTotalRow(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ReportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
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
            AssertHideColumns(FakeSheet, 12, 11);
        }

        [Test]
        public void AssignTotalRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int TotalRow = 100;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, TotalRow);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalRow(FakeSheet, TotalRow);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnCurrency].Received(1).SetCellValue(string.Empty);
                row.Cells[ColumnBetType].Received(1).SetCellValue(string.Empty);
                row.Cells[ColumnSport].Received(1).SetCellValue(string.Empty);
                row.Cells[ColumnUserName].Received(1).SetCellValue(string.Empty);
                row.Cells[ColumnEffectiveBettingValue].Received(1).SetCellValue(DataItem.EffectiveBettingValue);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransactions);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnTotalTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerWinLoss + DataItem.CustomerComm);
                FakeSheet.MergeCells(TotalRow, 0, TotalRow, 1);
            });
        }

        private static CustomerTurnoverExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<CustomerTurnoverExcelTestable>(
                string.Empty,
                new ShowingOption(),
                new List<CustTurnover> { DataItem },
                DataItem,
                DateTimeFormat,
                ReportTitle);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}