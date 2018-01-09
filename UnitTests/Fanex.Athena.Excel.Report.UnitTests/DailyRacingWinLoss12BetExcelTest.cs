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
    public class DailyRacingWinLoss12BetExcelTest
    {
        private const string FormatDate = "MM/dd/yyyy";

        private const int ColumnCount = 15;

        private static readonly DailyRacingWinLoss DataItem = new DailyRacingWinLoss
        {
            WinTO = 1,
            PlaceTO = 2,
            WPTO = 3,
            TotalTO = 4,
            DividendWon = 5,
            CustomerCommission = 6,

            WinTO12Bet = 7,
            PlaceTO12Bet = 8,
            WPTO12Bet = 9,
            TotalTO12Bet = 10,
            WinLoss12Bet = 11,
            CommIncome = 12,
            AmountDueToRacing = 13
        };

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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                cell.Received(2).SetCellValue(ViewResources.Race);
                cell.Received(1).SetCellValue(ViewResources.CustomerOverall);
                cell.Received(1).SetCellValue(ViewResources.Licensee12Bet);
                cell.Received(2).SetCellValue(ViewResources.AmtDueTo);
                cell.Received(2).SetCellValue(ViewResources.WinTO);
                cell.Received(2).SetCellValue(ViewResources.PlaceTO);
                cell.Received(2).SetCellValue(ViewResources.WPTO);
                cell.Received(2).SetCellValue(ViewResources.TotalTO);
                cell.Received(1).SetCellValue(ViewResources.DividendWon);
                cell.Received(1).SetCellValue(ViewResources.Customer + " " + ViewResources.Comm);
                cell.Received(1).SetCellValue(ViewResources.WinLoss);
                cell.Received(1).SetCellValue(ViewResources.Comm);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(_sheet);

            // Assert
            _sheet.Received().SetTextRowValue(excel.TemplateSheet, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), ViewResources.NoInfo);
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var reportTitle = string.Format(
                                       ViewResources.DailyRacing12BetTitle,
                                       ViewModel.Param.Date.ToString(FormatDate),
                                       ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

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
                    reportTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            _sheet.Received().HideColumn(7);
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