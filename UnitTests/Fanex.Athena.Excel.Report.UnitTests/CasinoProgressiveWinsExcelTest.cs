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
    public class CasinoProgressiveWinsExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnRefNo = 4;
        private const int ColumnChoice = 5;
        private const int ColumnAmount = 6;

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
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue("#");
                row.Cells[ColumnDate].Received(1).SetCellValue(ViewResources.Date);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(ViewResources.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(ViewResources.SystemId);
                row.Cells[ColumnRefNo].Received(1).SetCellValue(ViewResources.ReferenceNo);
                row.Cells[ColumnChoice].Received(1).SetCellValue(ViewResources.Choice);
                row.Cells[ColumnAmount].Received(1).SetCellValue(ViewResources.Amount);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 2);
            var excel = CreateExcel();
            excel.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(DataItem.Index);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnDate].Received(1).SetCellValue(DataItem.WinlostDate.ToShortDateString());
                row.Cells[ColumnRefNo].Received(1).SetCellValue(DataItem.RefNo);
                row.Cells[ColumnAmount].Received(1).SetCellValue(DataItem.WinAmount);
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
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
            row.Cells[0].Received(1).SetCellValue(expectedTitle);
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