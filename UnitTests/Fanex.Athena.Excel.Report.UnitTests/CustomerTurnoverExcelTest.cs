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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                cell.Received(1).SetCellValue("#");
                cell.Received(1).SetCellValue(ViewResources.Date);
                cell.Received(1).SetCellValue(ViewResources.Username);
                cell.Received(1).SetCellValue(ViewResources.Currency);
                cell.Received(1).SetCellValue(ViewResources.Sport);
                cell.Received(1).SetCellValue(ViewResources.BetType);
                cell.Received(1).SetCellValue(ViewResources.EffectiveBettingValue);
                cell.Received(1).SetCellValue(ViewResources.TotalTransaction);
                cell.Received(1).SetCellValue(ViewResources.Turnover);
                cell.Received(1).SetCellValue(ViewResources.Total + " " + ViewResources.Turnover);
                cell.Received(1).SetCellValue(ViewResources.CustomerW_L);

                cell.Received(1).SetCellValue(ViewResources.WinLoss);
                cell.Received(1).SetCellValue(ViewResources.Comm);
                cell.Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalRow(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(DataItem.Index);
                cell.Received(1).SetCellValue(DataItem.ReportBy.ToString(DateTimeFormat));
                cell.Received(1).SetCellValue(DataItem.UserName);
                cell.Received(1).SetCellValue(DataItem.Currency);
                cell.Received(1).SetCellValue(DataItem.TotalTurnover);
                cell.Received(1).SetCellValue(DataItem.SportType);

                cell.Received(1).SetCellValue(DataItem.Bettype);
                cell.Received(1).SetCellValue(DataItem.EffectiveBettingValue);
                cell.Received(1).SetCellValue(DataItem.TotalTransactions);
                cell.Received(1).SetCellValue(DataItem.Turnover);
                cell.Received(1).SetCellValue(DataItem.CustomerWinLoss);
                cell.Received(1).SetCellValue(DataItem.CustomerComm);
                cell.Received(1).SetCellValue(DataItem.CustomerWinLoss + DataItem.CustomerComm);

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
        public void ProcessUI_SystemIdInvisible_CallHideColumnCorrectly()
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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalRow(_sheet, TotalRow);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(4).SetCellValue(string.Empty);
                cell.Received(1).SetCellValue(DataItem.EffectiveBettingValue);
                cell.Received(1).SetCellValue(DataItem.TotalTransactions);
                cell.Received(2).SetCellValue(DataItem.Turnover);
                cell.Received(1).SetCellValue(DataItem.CustomerWinLoss);
                cell.Received(1).SetCellValue(DataItem.CustomerComm);
                cell.Received(1).SetCellValue(DataItem.CustomerWinLoss + DataItem.CustomerComm);
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