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
    public class CustomerPendingFundsExcelTest : BaseReportExcelTest
    {
        private const int ColumnBetCount = 3;
        private const int ColumnExternalId = 1;
        private const int ColumnPendingFunds = 4;
        private const int ColumnSystemId = 2;

        private const int ColumnCount = 5;

        private static readonly CustPendingFunds DataItem = FakeHelper.Create<CustPendingFunds>();

        private static readonly CustomerPendingFundsExcelModel ExcelModel = new CustomerPendingFundsExcelModel
        {
            DataList = new List<CustPendingFunds> { DataItem },
            TotalData = DataItem,
            ConvertingOption = new ConvertingOption()
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.BetCount,
                ViewResources.PendingFunds
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
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 2);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnBetCount].Received(1).SetCellValue(DataItem.BetCounts);
                row.Cells[ColumnPendingFunds].Received(1).SetCellValue(DataItem.PendingFunds);

                excel.Received(1).AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = string.Format(ViewResources.CustomerPedingFundsTitle, ReportExcelHelper.GetTitleCurrencyName(ExcelModel.ConvertingOption));

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 2);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_SystemIdInvisible_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, 2);
        }

        [Test]
        public void AssignSumData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int TotalRow = 100;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, TotalRow);
            var excel = CreateExcel();

            // Action
            excel.AssignSumData(FakeSheet, TotalRow);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                row.Cells[ColumnBetCount].Received(1).SetCellValue(ExcelModel.TotalData.BetCounts);
                row.Cells[ColumnPendingFunds].Received(1).SetCellValue(ExcelModel.TotalData.PendingFunds);
                FakeSheet.MergeCells(TotalRow, 0, TotalRow, 2);
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