using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class PlatformWinLossMemberExcelTest : BaseReportExcelTest
    {
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnCurrency = 3;
        private const int ColumnTurnover = 4;
        private const int ColumnTotalTrans = 5;
        private const int ColumnCustomerWl = 6;
        private const int ColumnCustomerCommission = 7;
        private const int ColumnCustomerTotal = 8;
        private const int ColumnLicenseeWl = 9;
        private const int ColumnBaCommission = 10;
        private const int ColumnAmtDueToBa = 11;

        private const int ColumnCount = 15;

        private const string CurrencyName = "ABC";

        private static readonly PlatformWinLossMember DataItem = FakeHelper.Create<PlatformWinLossMember>();

        private static readonly PlatformWinLossViewModel ViewModel = new PlatformWinLossViewModel
        {
            MainResult = new[] { DataItem },
            TotalResult = DataItem,
            Param = new PlatformWinLossMetaModel
            {
                CurrencyName = CurrencyName,
                PlatformType = PlatformType.Asia,
                FromDay = 1,
                ToDay = 2,
                Year = 2018,
                Month = 1,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true }
                }
            }
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
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Currency,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmtDueToBA
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                AssertSetTextRowHeader(row1, headerTextList);

                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerCommission].Received(1).SetCellValue(ViewResources.Comm);
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
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotal(Arg.Any<ISheet>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnTotalTrans].Received(1).SetCellValue(DataItem.TotalTransactions);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerCommission].Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerTotal);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(DataItem.LicenseeWL);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(DataItem.BACommission);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(DataItem.AmtDueToBA);

                excel.Received(1).AssignTotal(Arg.Any<ISheet>());
            });
        }

        [TestCase("EUR", "View in EUR")]
        [TestCase("USD", "View in USD")]
        [TestCase("GBP", "View in GBP")]
        [TestCase("HIHI", "View in " + CurrencyName)]
        public void AssignTitle_WhenCall_CallRelevantMethods(string baseCurrency, string expectedBaseCurrencyTitle)
        {
            // Arrange
            const string FormatDate = "MM/dd/yyyy";
            ViewModel.Param.BaseCurrency = baseCurrency;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var platformText = ViewResources.Platform + ": " + PlatformType.GetPlatformName(ViewModel.Param.PlatformType);

            var reportTitle = string.Format(
                                         ViewResources.PlatformWinLossTitle,
                                         ViewModel.Param.FromDate.ToString(FormatDate),
                                         ViewModel.Param.ToDate.ToString(FormatDate),
                                         ViewResources.AllCurrencies,
                                         expectedBaseCurrencyTitle,
                                         platformText) + ", " + ViewResources.Product + ": SB";

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(
                FakeSheet,
                ColumnCustomerTotal,
                ColumnCustomerCommission,
                ColumnBaCommission,
                ColumnAmtDueToBa);
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_NotCallHideColumn()
        {
            // Arrange
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = true,
                BAVisible = true
            };
            var excel = CreateExcel(showingOption);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, false);
        }

        [Test]
        public void AssignTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 4);
            var excel = CreateExcel();

            // Action
            excel.AssignTotal(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                FakeSheet.MergeCells(4, 0, 4, ColumnCurrency);
            });
        }

        private static PlatformWinLossMemberExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcelTestable>(
                string.Empty,
                ViewModel,
                showingOption ?? new ShowingOption());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}