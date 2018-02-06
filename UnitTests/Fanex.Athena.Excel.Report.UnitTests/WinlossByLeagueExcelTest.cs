using System;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.ResultModel;
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
    public class WinlossByLeagueExcelTest : BaseReportExcelTest
    {
        private const int ColumnBetCount = 4;
        private const int ColumnBaComm = 8;
        private const int ColumnAmtDueTo = 9;
        private const int ColumnCustomerWinLoss = 6;
        private const int ColumnLicenseeWl = 7;
        private const int ColumnMatch = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnTurnover = 5;
        private const int RowContentTitleTemplate = 3;
        private const int NumOfRowLevel0 = 10;

        private static readonly WinLossByLeague DataItem = FakeHelper.Create<WinLossByLeague>();

        private static readonly WinLossByLeague TotalData = FakeHelper.Create<WinLossByLeague>();
        private static readonly WinLossByLeagueMetaModel MetaModel = FakeHelper.Create<WinLossByLeagueMetaModel>();

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#",
                ViewResources.Match,
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.BetCount,
                ViewResources.Turnover,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmountDueTo,
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertSetTextRowHeader(row1, headerTextList);
                row2.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(ViewResources.WinLoss);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, RowContentTitleTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_Level0_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, 3);
            var rowContent = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, 4);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(MetaModel.LeagueName);
                rowContent.Cells[0].Received(1).SetCellValue(DataItem.Index);
                rowContent.Cells[ColumnMatch].Received(1).SetCellValue(DataItem.HomeName + Environment.NewLine + DataItem.AwayName);
                rowContent.Cells[ColumnAmtDueTo].Received(1).SetCellValue(DataItem.AmountDue);

                AssertAssignCommonData(rowContent, DataItem);

                excel.Received(1).AssignTotalData(FakeSheet, Arg.Any<int>());
            });
        }

        [Test]
        public void AssignSheetData_Level1_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, 3);
            var rowContent = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, 4);
            var excel = CreateExcel(1);
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(row.ZeroHeight, Is.True);
                row.Cells[0].Received(1).SetCellValue(MetaModel.LeagueName);
                rowContent.Cells[0].Received(1).SetCellValue(DataItem.Index);
                rowContent.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                rowContent.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);

                AssertAssignCommonData(rowContent, DataItem);

                excel.Received(1).AssignTotalData(FakeSheet, Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, 0);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(MetaModel.ReportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 4);
            });
        }

        [Test]
        public void ProcessUI_Level0_CallHideColumns()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnExternalId, ColumnSystemId);
        }

        [Test]
        public void ProcessUI_Level1_CallHideColumns()
        {
            // Arrange
            var excel = CreateExcel(1);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnMatch, ColumnAmtDueTo);
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, NumOfRowLevel0, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                AssertAssignCommonData(row, TotalData);
                row.Cells[ColumnAmtDueTo].SetCellValue(TotalData.AmountDue);
                row.Cells[ColumnBaComm].SetCellValue(TotalData.BACommission);
            });
        }

        private static void AssertAssignCommonData(IRow row, WinLossByLeague dataItem)
        {
            row.Cells[ColumnTurnover].Received(1).SetCellValue(dataItem.TurnOver);
            row.Cells[ColumnBetCount].Received(1).SetCellValue(dataItem.BetCount);
            row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(dataItem.CustomerWinLoss);
            row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(dataItem.LicenseeWL);
            row.Cells[ColumnBaComm].Received(1).SetCellValue(dataItem.BACommission);
        }

        private static WinlossByLeagueExcelTestable CreateExcel(int level = 0)
        {
            var excel = Substitute.ForPartsOf<WinlossByLeagueExcelTestable>(
                string.Empty,
                new ReportResultModel<WinLossByLeague>
                {
                    DataList = new[] { DataItem },
                    TotalData = TotalData
                },
                MetaModel);
            excel.Level = level;
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}