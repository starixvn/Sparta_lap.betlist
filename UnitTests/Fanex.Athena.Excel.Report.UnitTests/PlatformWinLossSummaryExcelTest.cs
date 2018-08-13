using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
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
    public class PlatformWinLossSummaryExcelTest : BaseReportExcelTest
    {
        private const int ColumnPlatform = 0;
        private const int ColumnTurnover = 1;
        private const int ColumnTurnoverPercent = 2;
        private const int ColumnTotalTransaction = 3;
        private const int ColumnCustomerWinLoss = 4;
        private const int ColumnCustomerComm = 5;
        private const int ColumnCustomerTotal = 6;
        private const int ColumnLicenseeWinLoss = 7;

        private const int ColumnCount = 15;

        private static readonly PlatformWinLossSummary DataItem = FakeHelper.Create<PlatformWinLossSummary>();

        private static readonly PlatformWinLossSummary TotalData = FakeHelper.Create<PlatformWinLossSummary>();

        private static readonly PlatformWinLossMetaModel MetaModel = new PlatformWinLossMetaModel
        {
            Year = 2018,
            Month = 1,
            FromDay = 1,
            ToDay = 19,
            CurrencyName = "UUS",
            ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true },
                    new ProductOption { Code = "NG", Status = false }
                }
        };

        private static readonly IEnumerable<PlatformWinLossSummary> DataList = new List<PlatformWinLossSummary>
        {
           DataItem
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                ViewResources.Platform,
                ViewResources.Turnover,
                ViewResources.TurnoverPercent,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                AssertSetTextRowHeader(row1, headerTextList);

                row2.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(ViewResources.WinLoss);
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
        public void AssignSheetData_WhenCall_SetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 4);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnPlatform].Received(1).SetCellValue(DataItem.Name);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnTurnoverPercent].Received(1).SetCellValue(string.Format("{0:0.00}%", DataItem.TurnoverPercent));
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransactions);
                row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerTotal);
                row.Cells[ColumnLicenseeWinLoss].Received(1).SetCellValue(DataItem.LicenseeWL);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_SetValueToTotalRow()
        {
            // Arrange
            var totalRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 4);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                totalRow.Cells[ColumnPlatform].Received(1).SetCellValue(ViewResources.Total);
                totalRow.Cells[ColumnTurnover].Received(1).SetCellValue(TotalData.Turnover);
                totalRow.Cells[ColumnTurnoverPercent].Received(1).SetCellValue(string.Format("{0:0.00}%", TotalData.TurnoverPercent));
                totalRow.Cells[ColumnTotalTransaction].Received(1).SetCellValue(TotalData.TotalTransactions);
                totalRow.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(TotalData.CustomerWinLoss);
                totalRow.Cells[ColumnCustomerComm].Received(1).SetCellValue(TotalData.CustomerComm);
                totalRow.Cells[ColumnCustomerTotal].Received(1).SetCellValue(TotalData.CustomerTotal);
                totalRow.Cells[ColumnLicenseeWinLoss].Received(1).SetCellValue(TotalData.LicenseeWL);
            });
        }

        [TestCase(0, "", "Platform Win Loss - 01/01/2018 --> 01/19/2018 - All Currencies - View in UUS - Platform: All, Product: SB")]
        [TestCase(1, "", "Platform Win Loss - 01/01/2018 --> 01/19/2018 - Currency UUS - View in UUS - Platform: All, Product: SB")]
        [TestCase(1, "EUR", "Platform Win Loss - 01/01/2018 --> 01/19/2018 - Currency UUS - View in EUR - Platform: All, Product: SB")]
        [TestCase(1, "USD", "Platform Win Loss - 01/01/2018 --> 01/19/2018 - Currency UUS - View in USD - Platform: All, Product: SB")]
        [TestCase(1, "GBP", "Platform Win Loss - 01/01/2018 --> 01/19/2018 - Currency UUS - View in GBP - Platform: All, Product: SB")]
        public void AssignTitle_WhenCall_CallRelevantMethods(int currencyId, string baseCurrency, string expectedTitle)
        {
            // Arrange
            MetaModel.CurrencyId = currencyId;
            MetaModel.BaseCurrency = baseCurrency;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        private static PlatformWinLossSummaryExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<PlatformWinLossSummaryExcelTestable>(
                string.Empty,
                MetaModel,
                DataList,
                TotalData);

            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}