using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
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
    public class ColossusBetWinLossExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnover = 5;
        private const int ColumnTotalTransaction = 6;
        private const int ColumnMemberWinning = 7;
        private const int ColumnCashOutAmt = 8;
        private const int ColumnCashOutShare = 9;
        private const int ColumnCommissionShare = 10;
        private const int ColumnAmountDueToCB = 11;
        private const int ColumnGrandPrize = 12;

        private const int ColumnCount = 21;

        private static readonly ColossusBetWinLoss DataItem = new ColossusBetWinLoss
        {
            Index = 1,
            Date = new DateTime(2017, 1, 1),
            ExternalId = "1",
            SystemId = "2",
            CurrencyName = "UUS",
            RowSpan = 2,
            TurnOver = 3,
            TotalTransaction = 4,
            MemberWinning = 5,
            CashOut = 6,
            CashOutShare = 7,
            CommissionShare = 8,
            GrandPrize = 10
        };

        private static readonly ThirdPartyWinLossMetaModel MetaModel = new ThirdPartyWinLossMetaModel
        {
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2),
            ProductOptions = new List<ProductOption>
                {
                    new ProductOption
                    {
                        Code = "SB",
                        Status = true
                    }
                }
        };

        private static readonly ColossusBetWinLossViewModel ViewModel = new ColossusBetWinLossViewModel
        {
            MainResult = new List<ColossusBetWinLoss>
            {
                DataItem
            },
            SummaryTotal = new Dictionary<string, ColossusBetWinLoss>
            {
                ["USD"] = new ColossusBetWinLoss()
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
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate + 1, ReportBase.RowHeaderTemplate + 1);
                row1.Cells[0].Received(1).SetCellValue("#");
                row1.Cells[ColumnExternalId].Received(1).SetCellValue(ViewResources.ExternalId);
                row1.Cells[ColumnSystemId].Received(1).SetCellValue(ViewResources.SystemId);
                row1.Cells[ColumnDate].Received(1).SetCellValue(ViewResources.Date);
                row1.Cells[ColumnCurrency].Received(1).SetCellValue(ViewResources.Currency);
                row1.Cells[ColumnTurnover].Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[ColumnTotalTransaction].Received(1).SetCellValue(ViewResources.TotalTransaction);
                row1.Cells[ColumnMemberWinning].Received(1).SetCellValue(ViewResources.AmtLessThan30kGBP);
                row1.Cells[ColumnCashOutShare].Received(1).SetCellValue(ViewResources.CashOutShare);
                row1.Cells[ColumnCommissionShare].Received(1).SetCellValue(ViewResources.CommissionShare);
                row1.Cells[ColumnAmountDueToCB].Received(1).SetCellValue(ViewResources.AmountDueToCB);
                row1.Cells[ColumnGrandPrize].Received(1).SetCellValue(ViewResources.AmtGreaterThan30kGBP);

                row2.Cells[ColumnMemberWinning].Received(1).SetCellValue(ViewResources.CustomerWinnings);
                row2.Cells[ColumnCashOutAmt].Received(1).SetCellValue(ViewResources.CashOutAmt);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(DataItem.Index);
                row.Cells[ColumnDate].Received(1).SetCellValue(DataItem.Date.ToString("MM/yyyy"));
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);

                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.TurnOver);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransaction);
                row.Cells[ColumnMemberWinning].Received(1).SetCellValue(DataItem.MemberWinning);
                row.Cells[ColumnCashOutAmt].Received(1).SetCellValue(DataItem.CashOut);
                row.Cells[ColumnCashOutShare].Received(1).SetCellValue(DataItem.CashOutShare);
                row.Cells[ColumnCommissionShare].Received(1).SetCellValue(DataItem.CommissionShare);
                row.Cells[ColumnAmountDueToCB].Received(1).SetCellValue(DataItem.AmountDueToCB);
                row.Cells[ColumnGrandPrize].Received(1).SetCellValue(DataItem.GrandPrize);

                excel.Received(1).AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(
                ViewResources.ProductCB + " " + ViewResources.WinLoss,
                new ConvertingOption(),
                MetaModel);

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [TestCase(0, new[] { 2, 3 })]
        [TestCase(1, new[] { 1 })]
        public void ProcessUI_WhenCall_CallHideColumnCorrectly(int level, int[] hiddenColumnList)
        {
            // Arrange
            var excel = CreateExcel(level);

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            _sheet.Received().HideColumn(hiddenColumnList);
        }

        [Test]
        public void AssignGrandTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 99;
            const int ColumnCurrency = 4;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGrandTotal(_sheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Totalin + " USD");
                _sheet.ReceivedWithAnyArgs(1).MergeCells(RowIndex, 0, RowIndex, ColumnCurrency);
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(_sheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);
                _sheet.MergeCells(RowIndex - 1, 0, RowIndex - 1, 3);
            });
        }

        private static ColossusBetsWinLossExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.DrillDownLevel = level;
            var excel = Substitute.ForPartsOf<ColossusBetsWinLossExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                MetaModel,
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}