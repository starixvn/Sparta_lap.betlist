using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Customer.UnitTests.TestableClasses;
using Fanex.Athena.Model.Customer.DTO;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Customer.UnitTests
{
    [TestFixture]
    public class DormantCustomerListExcelTest
    {
        private const int ExternalIDColumn = 0;
        private const int SystemIDColumn = 1;
        private const int BalanceColumn = 2;
        private const int ArchivedDateColumn = 3;

        private const int ColumnCount = 4;

        private const string ReportTitle = "Dormant Customer List 123456";
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
            const int RowHeaderTemplate = 1;
            var headerTexts = new List<string>
            {
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Balance,
                ViewResources.ArchivedDate
            };
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);

            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
                for (int i = 0; i < headerTexts.Count; i++)
                {
                    row.Cells[i].Received(1).SetCellValue(headerTexts[i]);
                }
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_SetGroupCommAndBAStatusColumns()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var item = FakeHelper.Create<DormantCustomer>();
            var dataList = new List<DormantCustomer>
                {
                    item
                };
            var excel = CreateExcel(dataList);

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ExternalIDColumn].Received(1).SetCellValue(item.ExternalId);
                row.Cells[SystemIDColumn].Received(1).SetCellValue(item.SystemId);
                row.Cells[BalanceColumn].Received(1).SetCellValue(item.Balance.ToString("#,##0.00"));
                row.Cells[ArchivedDateColumn].Received(1).SetCellValue(item.ArchivedDate.ToString("MM/dd/yyyy"));
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
            });
        }

        private static DormantCustomerListExcelTestable CreateExcel(IEnumerable<DormantCustomer> dataList = null)
        {
            var excel = Substitute.ForPartsOf<DormantCustomerListExcelTestable>(
                string.Empty,
                ReportTitle,
                dataList ?? new List<DormantCustomer>());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}