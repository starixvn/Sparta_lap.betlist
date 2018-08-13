using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
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
    public class WinLossByBetTypeExcelTest : BaseReportExcelTest
    {
        private const int ColumnAmtDueToBa = 8;
        private const int ColumnBaCommission = 7;
        private const int ColumnCustomerComm = 4;
        private const int ColumnCustomerTotal = 5;
        private const int ColumnCustomerWinLoss = 3;
        private const int ColumnGameTypeName = 1;
        private const int ColumnLicenseeWl = 6;
        private const int ColumnTemplateCount = 9;
        private const int ColumnTurnover = 2;
        private const int RowContentTemplate = 3;

        private static readonly WinLossByBetType DataItem = FakeHelper.Create<WinLossByBetType>();

        private static readonly WinLossByBetTypeViewModel ViewModel = new WinLossByBetTypeViewModel
        {
            Param = FakeHelper.Create<WinLossByBetTypeMetaModel>(),
            MainResult = new List<WinLossByBetType> { DataItem },
            Total = FakeHelper.Create<WinLossByBetType>()
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
                ViewResources.GameTypeName,
                ViewResources.Turnover,
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
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignSumData(Arg.Any<ISheet>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnGameTypeName].Received(1).SetCellValue(DataItem.BetTypeName);

                AssertAssignCommonData(row, DataItem);

                excel.Received(1).AssignSumData(FakeSheet);
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel();
            var reportTitle = string.Format(
                                       ViewResources.WinLossByBetTypeTitle,
                                       ViewModel.Param.FromDate.ToString(ReportBase.FormatDate),
                                       ViewModel.Param.ToDate.ToString(ReportBase.FormatDate),
                                       "View in ABC");

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumns()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(
                FakeSheet,
                ColumnCustomerTotal,
                ColumnCustomerComm,
                ColumnBaCommission,
                ColumnAmtDueToBa);
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_NotHideColumn()
        {
            // Arrange
            var excel = CreateExcel(new ShowingOption
            {
                CustomerCommVisible = true,
                BAVisible = true
            });

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, false);
        }

        [Test]
        public void AssignSumData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate + 1);
            var excel = CreateExcel();
            var totalData = ViewModel.Total;

            // Action
            excel.AssignSumData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(totalData.Turnover);
                row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(totalData.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(totalData.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(totalData.CustomerTotal);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(totalData.LicenseeWL);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(totalData.BACommission);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(totalData.AmtDueToBA);
            });
        }

        private static void AssertAssignCommonData(IRow row, WinLossByBetType dataItem)
        {
            row.Cells[ColumnTurnover].Received(1).SetCellValue(dataItem.Turnover);
            row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(dataItem.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(dataItem.CustomerComm);
            row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(dataItem.CustomerTotal);
            row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(dataItem.LicenseeWL);
            row.Cells[ColumnBaCommission].Received(1).SetCellValue(dataItem.BACommission);
            row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(dataItem.AmtDueToBA);
        }

        private static WinLossByBetTypeExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<WinLossByBetTypeExcelTestable>(
                string.Empty,
                new ConvertingOption { BaseCurrency = "ABC" },
                showingOption ?? new ShowingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}