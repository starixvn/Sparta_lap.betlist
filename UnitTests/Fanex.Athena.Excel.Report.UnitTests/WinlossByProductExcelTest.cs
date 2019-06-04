using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Models;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.MetaModel.Product;
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
        private const int RowContentTitleTemplate = 3;

        private static readonly WinLossByProduct DataItem = FakeHelper.Create<WinLossByProduct>();

        private static readonly WinLossByProductMetaModel MetaModel = new WinLossByProductMetaModel
        {
            FromDate = new DateTime(2018, 1, 1),
            ToDate = new DateTime(2018, 2, 2),
            DrillDownFromDate = new DateTime(2018, 3, 3),
            DrillDownToDate = new DateTime(2018, 4, 4),
            CurrencyId = 1,
            DrillDownCurrencyId = 2,
            ProductOptions = new List<ProductOption>(),
            BaseCurrency = "USD",
            CurrencyName = "AUD"
        };

        private static readonly WinLossByProductExcelModel ExcelModel = new WinLossByProductExcelModel
        {
            DataList = new[] { DataItem },
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
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 3);
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 4);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.AssignSummaryTotal(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);
                row.Cells[ColumnProduct].Received(1).SetCellValue(DataItem.ProductName);

                AssertAssignCommonData(row, DataItem);

                excel.Received(1).AssignTotalData(FakeSheet, Arg.Any<int>());
                excel.Received(1).AssignSummaryTotal(FakeSheet, Arg.Any<int>());
            });
        }

        [TestCase(0, "Win Loss By Products, 01/01/2018 --> 02/02/2018, Currency: AUD, Base Currency: USD")]
        [TestCase(1, "Win Loss By Products, 03/03/2018 --> 04/04/2018, Currency: AUD, Base Currency: USD")]
        public void AssignTitle_WhenCall_CallRelevantMethods(int level, string expectedTitle)
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel(level);

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void ProcessUI_Level0_CallHideColumnsCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnExternalId, ColumnSystemId);
        }

        [Test]
        public void ProcessUI_Level1_CallHideColumnsCorrectly()
        {
            // Arrange
            var excel = CreateExcel(1);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnExternalId);
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumnsCorrectly()
        {
            // Arrange
            ExcelModel.SystemIdVisible = false;
            ExcelModel.CustomerCommVisible = false;
            ExcelModel.SystemIdVisible = false;
            ExcelModel.BAVisible = false;
            var excel = CreateExcel(2);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(
                FakeSheet,
                ColumnSystemId,
                ColumnCustomerTotal,
                ColumnCustomerComm,
                ColumnComm);
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_NotHideAnyColumns()
        {
            // Arrange
            ExcelModel.SystemIdVisible = true;
            ExcelModel.CustomerCommVisible = true;
            ExcelModel.SystemIdVisible = true;
            ExcelModel.BAVisible = true;
            ExcelModel.HasAmountDueToColumn = true;
            var excel = CreateExcel(2);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, false);
        }

        [Test]
        public void AssignSummaryTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var item = FakeHelper.Create<WinLossByProduct>();
            ExcelModel.SummaryTotal = new Dictionary<string, IEnumerable<WinLossByProduct>>
            {
                ["EUR"] = new List<WinLossByProduct> { item }
            };
            const int RowIndex = 10;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignSummaryTotal(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                AssertAssignCommonData(row, item);
                row.Cells[ColumnProduct].Received(1).SetCellValue(item.ProductName);
                row.Cells[0].Received(1).SetCellValue(ViewResources.Totalin + " EUR");
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var totalData = FakeHelper.Create<WinLossByProduct>();
            ExcelModel.TotalData = new List<WinLossByProduct> { totalData };
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                AssertAssignCommonData(row, totalData);
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(totalData.CurrencyName);
                row.Cells[ColumnCurrency + 1].Received(1).SetCellValue(totalData.ProductName);
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

        private static WinlossByProductExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.Level = level;
            var excel = Substitute.ForPartsOf<WinlossByProductExcelTestable>(
                string.Empty,
                ExcelModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}