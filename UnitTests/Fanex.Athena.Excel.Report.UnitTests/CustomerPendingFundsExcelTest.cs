using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.Models;
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
    public class CustomerPendingFundsExcelTest
    {
        private const int ColumnCount = 5;

        private static readonly CustPendingFunds DataItem = new CustPendingFunds
        {
            ExternalId = "1",
            SystemId = "2",
            BetCounts = 3,
            PendingFunds = 4
        };

        private static readonly CustomerPendingFundsExcelModel ExcelModel = new CustomerPendingFundsExcelModel
        {
            DataList = new List<CustPendingFunds> { DataItem },
            TotalData = DataItem,
            ConvertingOption = new ConvertingOption()
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
                cell.Received(1).SetCellValue(ViewResources.ExternalId);
                cell.Received(1).SetCellValue(ViewResources.SystemId);
                cell.Received(1).SetCellValue(ViewResources.BetCount);
                cell.Received(1).SetCellValue(ViewResources.PendingFunds);
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
            excel.WhenForAnyArgs(x => x.AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(DataItem.ExternalId);
                cell.Received(1).SetCellValue(DataItem.SystemId);
                cell.Received(1).SetCellValue(DataItem.BetCounts);
                cell.Received(1).SetCellValue(DataItem.PendingFunds);

                excel.Received(1).AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var expectedTitle = string.Format(ViewResources.CustomerPedingFundsTitle, ReportExcelHelper.GetTitleCurrencyName(ExcelModel.ConvertingOption));

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
                    expectedTitle);
                _sheet.Received(1).CreateFreezePane(0, 2);
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
            _sheet.Received().HideColumn(2);
        }

        [Test]
        public void AssignSumData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int TotalRow = 100;
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.AssignSumData(_sheet, TotalRow);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(ViewResources.Total);
                cell.Received(1).SetCellValue(ExcelModel.TotalData.BetCounts);
                cell.Received(1).SetCellValue(ExcelModel.TotalData.PendingFunds);
                _sheet.MergeCells(TotalRow, 0, TotalRow, 2);
            });
        }

        private static CustomerPendingFundsExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<CustomerPendingFundsExcelTestable>(
                string.Empty,
                ExcelModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}