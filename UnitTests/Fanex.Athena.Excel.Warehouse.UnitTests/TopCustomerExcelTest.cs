using System.Data;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Report.UnitTests;
using Fanex.Athena.Excel.Warehouse.UnitTests.TestableClasses;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Warehouse.UnitTests
{
    [TestFixture]
    public class TopCustomerExcelTest : BaseReportExcelTest
    {
        private const string ReportTitle = "WWWWWWWWWWWWWWWWWw";

        private const int ColumnCustomerComm = 6;
        private const int ColumnCustomerTotal = 7;
        private const int ColumnCustomerWl = 5;
        private const int ColumnTemplateCount = 12;

        private static readonly DataTable Data = new DataTable();

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#",
                ViewResources.Customer,
                ViewResources.Currency,
                ViewResources.Turnover,
                ViewResources.BetCount,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.LastActivityDate,
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertSetTextRowHeader(row1, headerTextList);
                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Commission);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ReportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void ProcessUI_WhenCall_CallHideColumnsCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnCustomerTotal, ColumnCustomerComm);
        }

        private static TopCustomerExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<TopCustomerExcelTestable>(
                string.Empty,
                new ShowingOption(),
                Data,
                ReportTitle);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}