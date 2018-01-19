using System;
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
    public class RacingCommissionExcelTest
    {
        private const int ColumnCustomerCommission = 7;
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 8;
        private const int ColumnTotePlace = 4;
        private const int ColumnToteTotal = 6;
        private const int ColumnToteWin = 3;
        private const int ColumnToteWinPlace = 5;

        private const int ColumnCount = 15;

        private static readonly RacingCommission DataItem = new RacingCommission
        {
            ExternalID = "1",
            SystemID = "2",
            ToteWin = 3,
            TotePlace = 4,
            CustomerCommission = 5
        };

        private static readonly RacingCommissionViewModel ViewModel = new RacingCommissionViewModel
        {
            Param = new RacingCommissionMetaModel
            {
                FromDate = new DateTime(2018, 2, 3),
                ToDate = new DateTime(2018, 5, 6)
            },
            RacingCommissionList = new List<RacingCommission> { DataItem },
            Total = new RacingCommission
            {
                ExternalID = "11",
                SystemID = "22",
                ToteWin = 33,
                TotePlace = 44,
                CustomerCommission = 55
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
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row1.Cells[0].Received(1).SetCellValue("#");
                row1.Cells[ColumnExternalId].Received(1).SetCellValue(ViewResources.ExternalId);
                row1.Cells[ColumnSystemId].Received(1).SetCellValue(ViewResources.SystemId);
                row1.Cells[ColumnToteWin].Received(1).SetCellValue(ViewResources.Tote);
                row1.Cells[ColumnCustomerCommission].Received(1).SetCellValue(ViewResources.Customer + " " + ViewResources.Comm);

                row2.Cells[ColumnToteWin].Received(1).SetCellValue(ViewResources.Win);
                row2.Cells[ColumnTotePlace].Received(1).SetCellValue(ViewResources.Place);
                row2.Cells[ColumnToteWinPlace].Received(1).SetCellValue(ViewResources.Win + " " + ViewResources.Place);
                row2.Cells[ColumnToteTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(_sheet);

            // Assert
            _sheet.Received().SetTextRowValue(excel.TemplateSheet, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalID);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemID);
                row.Cells[ColumnToteWin].Received(1).SetCellValue(DataItem.ToteWin);
                row.Cells[ColumnTotePlace].Received(1).SetCellValue(DataItem.TotePlace);
                row.Cells[ColumnToteWinPlace].Received(1).SetCellValue(DataItem.ToteWinPlace);
                row.Cells[ColumnToteTotal].Received(1).SetCellValue(DataItem.ToteTotal);
                row.Cells[ColumnCustomerCommission].Received(1).SetCellValue(DataItem.CustomerCommission);

                excel.Received(1).AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
            var excel = CreateExcel();
            const string ExpectedTitle = "Racing Commission, 02/03/2018 --> 05/06/2018 - View in ABC";

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ExpectedTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumn()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().HideColumn(ColumnCustomerCommission);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_NotHideColumn()
        {
            // Arrange
            var excel = CreateExcel(new ShowingOption
            {
                CustomerCommVisible = true
            });

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.DidNotReceiveWithAnyArgs().HideColumn(0);
            });
        }

        [Test]
        public void AssignSumData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowIndex + 3);
            var excel = CreateExcel();
            var totalData = ViewModel.Total;

            // Action
            excel.AssignSumData(_sheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue("Total in ABC");
                row.Cells[ColumnToteWin].Received(1).SetCellValue(totalData.ToteWin);
                row.Cells[ColumnTotePlace].Received(1).SetCellValue(totalData.TotePlace);
                row.Cells[ColumnToteWinPlace].Received(1).SetCellValue(totalData.ToteWinPlace);
                row.Cells[ColumnToteTotal].Received(1).SetCellValue(totalData.ToteTotal);
                row.Cells[ColumnCustomerCommission].Received(1).SetCellValue(totalData.CustomerCommission);
            });
        }

        private static RacingCommissionExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<RacingCommissionExcelTestable>(
                string.Empty,
                new ConvertingOption { BaseCurrency = "ABC" },
                showingOption ?? new ShowingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}