using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Models;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class WinlossByProductExcelTest : BaseReportExcelTest
    {
        private const int ColumnAmtDueTo = 12;
        private const int ColumnComm = 11;
        private const int ColumnCurrency = 3;
        private const int ColumnCustomerComm = 8;
        private const int ColumnCustomerTotal = 9;
        private const int ColumnCustomerWinLoss = 7;
        private const int ColumnExternalId = 1;
        private const int ColumnLicenseeWl = 10;
        private const int ColumnProduct = 4;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 13;
        private const int ColumnTotalTransaction = 6;
        private const int ColumnTurnover = 5;
        private const int RowContentTemplate = 4;
        private const int RowContentTitleTemplate = 3;
        private const int RowGrandTotal = 10;
        private const int RowNoInfoTemplate = 12;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalNoSolidBorder = 9;
        private const int RowTotalSolidBorder = 8;

        private static readonly WinLossByProduct DataItem = FakeHelper.Create<WinLossByProduct>();

        private static readonly WinLossByProductMetaModel MetaModel = new WinLossByProductMetaModel
        {
        };

        private static readonly WinLossByProductExcelModel ExcelModel = new WinLossByProductExcelModel
        {
            DataList = new[] { DataItem },
            SummaryTotal = new Dictionary<string, IEnumerable<WinLossByProduct>>(),
            MetaModel = MetaModel
        };

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
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Currency,
                ViewResources.Product,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.Comm,
                ViewResources.AmountDue,
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertSetTextRowHeader(row1, headerTextList);
                row2.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [TestCase(null, "No information is available")]
        [TestCase("Hello Baby", "Hello Baby")]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods(string errorMessage, string expectedCellValue)
        {
            // Arrange
            ExcelModel.ErrorMessage = errorMessage;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTitleTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(expectedCellValue);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            ExcelModel.MetaModel.Level = 1;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.AssignSummaryTotal(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].SetCellValue(0);
                row.Cells[ColumnExternalId].SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(DataItem.SystemId);
                row.Cells[ColumnCurrency].SetCellValue(DataItem.CurrencyName);
                row.Cells[ColumnProduct].SetCellValue(DataItem.ProductName);

                AssertAssignCommonData(row, DataItem);

                excel.Received(1).AssignTotalData(FakeSheet, Arg.Any<int>());
                excel.Received(1).AssignSummaryTotal(FakeSheet, Arg.Any<int>());
            });
        }

        private static void AssertAssignCommonData(IRow row, WinLossByProduct dataItem)
        {
            row.Cells[ColumnTurnover].Received(1).SetCellValue(dataItem.TurnOver);
            row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(dataItem.TotalTransaction);
            row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(dataItem.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dataItem.CustomerComm);
            row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(dataItem.CustomerTotal);
            row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(dataItem.LicenseeWL);
            row.Cells[ColumnComm].Received(1).SetCellValue(dataItem.LicenseeWLComm);
            row.Cells[ColumnAmtDueTo].Received(1).SetCellValue(dataItem.AmountDue);
        }

        private static WinlossByProductExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<WinlossByProductExcelTestable>(
                string.Empty,
                ExcelModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}