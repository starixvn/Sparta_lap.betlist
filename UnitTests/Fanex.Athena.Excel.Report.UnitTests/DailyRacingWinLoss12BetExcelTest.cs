using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
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
    public class DailyRacingWinLoss12BetExcelTest : BaseReportExcelTest
    {
        private const string FormatDate = "MM/dd/yyyy";

        private const int ColumnCount = 15;

        private static readonly DailyRacingWinLossViewModel ViewModel = new DailyRacingWinLossViewModel
        {
            DailyRacingWinLossData = new DailyRacingWinLossData
            {
                ContentData = new Dictionary<string, List<DailyRacingWinLoss>>()
            },
            Param = new DailyRacingWinLossMetaModel
            {
                Date = new DateTime(2018, 2, 3)
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
                ViewResources.Race, ViewResources.WinTO, ViewResources.PlaceTO,
                ViewResources.WPTO, ViewResources.TotalTO, ViewResources.DividendWon,
                ViewResources.Customer + " " + ViewResources.Comm, ViewResources.WinTO,
                ViewResources.PlaceTO, ViewResources.WPTO, ViewResources.TotalTO,
                ViewResources.WinLoss, ViewResources.Comm, ViewResources.AmtDueTo
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row1.Cells[0].Received(1).SetCellValue(ViewResources.Race);
                row1.Cells[2].Received(1).SetCellValue(ViewResources.CustomerOverall);
                row1.Cells[8].Received(1).SetCellValue(ViewResources.Licensee12Bet);
                row1.Cells[14].Received(1).SetCellValue(ViewResources.AmtDueTo);

                AssertSetTextRowHeader(row2, headerTextList, 1);
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
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var reportTitle = string.Format(
                                       ViewResources.DailyRacing12BetTitle,
                                       ViewModel.Param.Date.ToString(FormatDate),
                                       ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

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
            AssertHideColumns(FakeSheet, 7);
        }

        private static DailyRacingWinLoss12BetExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<DailyRacingWinLoss12BetExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                new ShowingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}