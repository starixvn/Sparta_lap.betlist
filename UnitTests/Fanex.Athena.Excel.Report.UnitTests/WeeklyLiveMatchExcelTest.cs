using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ResultModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class WeeklyLiveMatchExcelTest : BaseReportExcelTest
    {
        private const int ColumnSport = 1;
        private const int NumberOfStaticColumns = 2; // Column: Index, Sport.
        private const int MaxWeekColumns = 10;
        private const int RowContentTemplate = 2;

        private const int ColumnCount = 15;
        private static readonly string[] WeekHeaders = new[] { "Week1", "Week3", "Week100" };

        private static readonly WeeklyLiveMatch DataItem = FakeHelper.Create<WeeklyLiveMatch>();
        private static readonly WeeklyLiveMatchMetaModel MetaModel = FakeHelper.Create<WeeklyLiveMatchMetaModel>();

        private static readonly WeeklyLiveMatchResultModel ResultModel = new WeeklyLiveMatchResultModel
        {
            DataList = new List<WeeklyLiveMatch> { DataItem },
            NumberOfWeeks = WeekHeaders.Length,
            TotalData = FakeHelper.CreateMany<int>().ToArray()
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnSport].Received(1).SetCellValue(ViewResources.Sport);
                row.Cells[ColumnSport + 1].Received(1).SetCellValue(WeekHeaders[0]);
                row.Cells[ColumnSport + 2].Received(1).SetCellValue(WeekHeaders[1]);
                row.Cells[ColumnSport + 3].Received(1).SetCellValue(WeekHeaders[2]);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowContentTemplate);
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
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowContentTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(DataItem.Index);
                row.Cells[ColumnSport].Received(1).SetCellValue(DataItem.SportName);
                row.Cells[ColumnSport + 1].Received(1).SetCellValue(DataItem.NumbersOfMatches.ElementAt(0));
                row.Cells[ColumnSport + 2].Received(1).SetCellValue(DataItem.NumbersOfMatches.ElementAt(1));
                row.Cells[ColumnSport + 3].Received(1).SetCellValue(DataItem.NumbersOfMatches.ElementAt(2));
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var reportTitle = string.Format(
                ViewResources.WeeklyLiveMatches + ", {0} --> {1}",
                MetaModel.FromDate.ToString(ReportBase.FormatDate),
                MetaModel.ToDate.ToString(ReportBase.FormatDate));

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 2);
            });
        }

        [Test]
        public void ProcessUI_WhenCall_CallHideColumn()
        {
            // Arrange
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotalRow(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            var hiddenColumns = Enumerable.Range(ResultModel.NumberOfWeeks + NumberOfStaticColumns, MaxWeekColumns - ResultModel.NumberOfWeeks);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received(1).AssignTotalRow(FakeSheet, 3);
                AssertHideColumns(FakeSheet, hiddenColumns.ToArray());
            });
        }

        [Test]
        public void AssignTotalRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var rowIndex = FakeHelper.Create<int>();
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, rowIndex);
            var excel = CreateExcel();
            int cellIndex = 2;

            // Action
            excel.AssignTotalRow(FakeSheet, rowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnSport].Received(1).SetCellValue(ViewResources.Total);
                for (int i = 0; i < ResultModel.NumberOfWeeks; i++)
                {
                    row.Cells[cellIndex++].SetCellValue(ResultModel.TotalData[i]);
                }
            });
        }

        private static WeeklyLiveMatchExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<WeeklyLiveMatchExcelTestable>(
                string.Empty,
                MetaModel,
                ResultModel,
                WeekHeaders);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}