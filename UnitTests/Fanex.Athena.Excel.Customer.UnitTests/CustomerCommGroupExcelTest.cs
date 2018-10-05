using System.Data;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Customer.UnitTests.TestableClasses;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Customer.UnitTests
{
    [TestFixture]
    public class CustomerCommGroupExcelTest
    {
        private const int ColumnGroupName = 1;
        private const int ColumnDefaultGroup = 2;
        private const int ColumnNoOfCustomer = 3;
        private const int ColumnSbCommission = 4;
        private const int ColumnNgCommission = 5;
        private const int ColumnCsCommission = 6;

        private const int ColumnCount = 7;
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
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row.Cells[0].Received(1).SetCellValue("#");
                row.Cells[ColumnGroupName].Received(1).SetCellValue(ViewResources.GroupName);
                row.Cells[ColumnDefaultGroup].Received(1).SetCellValue(ViewResources.DefaultGroup);
                row.Cells[ColumnNoOfCustomer].Received(1).SetCellValue(ViewResources.NoOfCustmer);
                row.Cells[ColumnSbCommission].Received(1).SetCellValue(ViewResources.SBComm + " (%)");
                row.Cells[ColumnNgCommission].Received(1).SetCellValue(ViewResources.NGComm + " (%)");
                row.Cells[ColumnCsCommission].Received(1).SetCellValue(ViewResources.CSComm + " (%)");
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 2);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnGroupName].Received(1).SetCellValue("GroupA");
                row.Cells[ColumnDefaultGroup].Received(1).SetCellValue("X");
                row.Cells[ColumnNoOfCustomer].Received(1).SetCellValue(100);
                row.Cells[ColumnSbCommission].Received(1).SetCellValue("100.00");
                row.Cells[ColumnNgCommission].Received(1).SetCellValue("200.00");
                row.Cells[ColumnCsCommission].Received(1).SetCellValue("300.00");
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            _sheet.LastRowNum.Returns(2);
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
                    ViewResources.CommissionGroupList);
                _sheet.Received(1).CreateFreezePane(0, 2);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllProductsInvisible_CallEnoughHideColumnMethod()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.ReceivedWithAnyArgs(3).HideColumn(0);
            });
        }

        [Test]
        public void ProcessUI_AllProductsVisible_NotCallAnyHideColumnMethod()
        {
            // Arrange
            var excel = CreateExcel(new ShowingOption
            {
                SBVisible = true,
                NGVisible = true,
                CSVisible = true
            });

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.DidNotReceiveWithAnyArgs().HideColumn(0);
            });
        }

        private static CustomerCommGroupExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var table = CreateDataTable();
            var excel = Substitute.ForPartsOf<CustomerCommGroupExcelTestable>(
                string.Empty,
                table,
                showingOption ?? new ShowingOption());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }

        private static DataTable CreateDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("IsDefault", typeof(bool));
            table.Columns.Add("SBComm", typeof(decimal));
            table.Columns.Add("NGComm", typeof(decimal));
            table.Columns.Add("RNGComm", typeof(decimal));

            table.Columns.Add("GroupName", typeof(string));
            table.Columns.Add("NumberOfCustomer", typeof(int));
            table.Rows.Add(true, 1, 2, 3, "GroupA", 100);
            return table;
        }
    }
}