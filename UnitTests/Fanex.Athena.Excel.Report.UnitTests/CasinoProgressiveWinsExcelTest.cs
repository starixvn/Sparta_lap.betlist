using System;
using System.Collections.Generic;
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
    public class CasinoProgressiveWinsExcelTest
    {
        private const int ColumnCount = 9;

        private static readonly CasinoProgressiveWins DataItem = new CasinoProgressiveWins
        {
            Index = 1,
            ExternalId = "2",
            SystemId = "3",
            WinlostDate = new DateTime(2017, 1, 1),
            RefNo = 4,
            WinAmount = 5
        };

        private static readonly CasinoProgressiveWinsViewModel ViewModel = new CasinoProgressiveWinsViewModel
        {
            Param = new BaseReportMetaModel
            {
                FromDate = new DateTime(2017, 2, 2),
                ToDate = new DateTime(2017, 3, 3),
                CurrencyId = 99
            },
            DataList = new List<CasinoProgressiveWins>
            {
                DataItem
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
                cell.Received(1).SetCellValue("#");
                cell.Received(1).SetCellValue(ViewResources.Date);
                cell.Received(1).SetCellValue(ViewResources.ExternalId);
                cell.Received(1).SetCellValue(ViewResources.SystemId);
                cell.Received(1).SetCellValue(ViewResources.ReferenceNo);
                cell.Received(1).SetCellValue(ViewResources.Choice);
                cell.Received(1).SetCellValue(ViewResources.Amount);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            excel.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(DataItem.Index);
                cell.Received(1).SetCellValue(DataItem.ExternalId);
                cell.Received(1).SetCellValue(DataItem.SystemId);
                cell.Received(1).SetCellValue(DataItem.WinlostDate.ToShortDateString());
                cell.Received(1).SetCellValue(DataItem.RefNo);
                cell.Received(1).SetCellValue(DataItem.WinAmount);
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(
                ViewResources.CasinoProgressiveWins,
                new ConvertingOption(),
                ViewModel.Param.FromDate,
                ViewModel.Param.ToDate,
                ViewModel.Param.CurrencyId);

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            _sheet.Received().SetTextRowValue(
                    excel.TemplateSheet,
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    expectedTitle);
        }

        private static CasinoProgressiveWinsExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<CasinoProgressiveWinsExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}