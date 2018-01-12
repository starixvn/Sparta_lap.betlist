using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
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
    public class CustomerTurnoverExcelTest
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

        private static readonly CustTurnover DataItem = new CustTurnover
        {
            Index = 1,
            ReportBy = new DateTime(2018, 1, 1),
            UserName = "3",
            Currency = "4",
            TotalTurnover = 5,
            Bettype = "6",
            EffectiveBettingValue = 7,
            TotalTransactions = 8,
            Turnover = 9,
            CustomerWinLoss = 10,
            CustomerComm = 11,
            RowSpan = 1,
            RowSpan2 = 1
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
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row1.Cells[0].Received(1).Received(1).SetCellValue("#");
                row1.Cells[1].Received(1).Received(1).SetCellValue(ViewResources.Date);
                row1.Cells[2].Received(1).Received(1).SetCellValue(ViewResources.Username);
                row1.Cells[3].Received(1).Received(1).SetCellValue(ViewResources.Currency);
                row1.Cells[4].Received(1).Received(1).SetCellValue(ViewResources.Sport);
                row1.Cells[5].Received(1).Received(1).SetCellValue(ViewResources.BetType);
                row1.Cells[6].Received(1).Received(1).SetCellValue(ViewResources.EffectiveBettingValue);
                row1.Cells[7].Received(1).Received(1).SetCellValue(ViewResources.TotalTransaction);
                row1.Cells[8].Received(1).Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[9].Received(1).Received(1).SetCellValue(ViewResources.Total + " " + ViewResources.Turnover);
                row1.Cells[10].Received(1).Received(1).SetCellValue(ViewResources.CustomerW_L);

                row2.Cells[ColumnCustomerWl].Received(1).Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).Received(1).SetCellValue(ViewResources.Total);
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
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalRow(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).Received(1).SetCellValue(DataItem.Index);
                row.Cells[ColumnDate].Received(1).Received(1).SetCellValue(DataItem.ReportBy.ToString(DateTimeFormat));
                row.Cells[ColumnUserName].Received(1).Received(1).SetCellValue(DataItem.UserName);
                row.Cells[ColumnCurrency].Received(1).Received(1).SetCellValue(DataItem.Currency);
                row.Cells[ColumnTotalTurnover].Received(1).Received(1).SetCellValue(DataItem.TotalTurnover);
                row.Cells[ColumnSport].Received(1).Received(1).SetCellValue(DataItem.SportType);

                row.Cells[ColumnBetType].Received(1).Received(1).SetCellValue(DataItem.Bettype);
                row.Cells[ColumnEffectiveBettingValue].Received(1).Received(1).SetCellValue(DataItem.EffectiveBettingValue);
                row.Cells[ColumnTotalTransaction].Received(1).Received(1).SetCellValue(DataItem.TotalTransactions);
                row.Cells[ColumnTurnover].Received(1).Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnCustomerWl].Received(1).Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].Received(1).Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).Received(1).SetCellValue(DataItem.CustomerWinLoss + DataItem.CustomerComm);

                excel.Received(1).AssignTotalRow(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().SetTextRowValue(
                    excel.TemplateSheet,
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    ReportTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_WhenCall_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().HideColumn(12, 11);
                _sheet.Received().HideRow(2);
            });
        }

        [Test]
        public void AssignTotalRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int TotalRow = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, TotalRow);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalRow(_sheet, TotalRow);

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
                _sheet.MergeCells(TotalRow, 0, TotalRow, 1);
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