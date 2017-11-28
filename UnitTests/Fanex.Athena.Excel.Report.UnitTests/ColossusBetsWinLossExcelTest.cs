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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate + 1, ReportBase.RowHeaderTemplate + 1);
                cell.Received(1).SetCellValue("#");
                cell.Received(1).SetCellValue(ViewResources.ExternalId);
                cell.Received(1).SetCellValue(ViewResources.SystemId);
                cell.Received(1).SetCellValue(ViewResources.Date);
                cell.Received(1).SetCellValue(ViewResources.Currency);
                cell.Received(1).SetCellValue(ViewResources.Turnover);
                cell.Received(1).SetCellValue(ViewResources.TotalTransaction);
                cell.Received(1).SetCellValue(ViewResources.AmtLessThan30kGBP);
                cell.Received(1).SetCellValue(ViewResources.CashOutShare);
                cell.Received(1).SetCellValue(ViewResources.CommissionShare);
                cell.Received(1).SetCellValue(ViewResources.AmountDueToCB);
                cell.Received(1).SetCellValue(ViewResources.AmtGreaterThan30kGBP);

                cell.Received(1).SetCellValue(ViewResources.CustomerWinnings);
                cell.Received(1).SetCellValue(ViewResources.CashOutAmt);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(DataItem.Index);
                cell.Received(1).SetCellValue(DataItem.Date.ToString("MM/yyyy"));
                cell.Received(1).SetCellValue(DataItem.ExternalId);
                cell.Received(1).SetCellValue(DataItem.SystemId);
                cell.Received(1).SetCellValue(DataItem.CurrencyName);

                cell.Received(1).SetCellValue(DataItem.TurnOver);
                cell.Received(1).SetCellValue(DataItem.TotalTransaction);
                cell.Received(1).SetCellValue(DataItem.MemberWinning);
                cell.Received(1).SetCellValue(DataItem.CashOut);
                cell.Received(1).SetCellValue(DataItem.CashOutShare);
                cell.Received(1).SetCellValue(DataItem.CommissionShare);
                cell.Received(1).SetCellValue(DataItem.AmountDueToCB);
                cell.Received(1).SetCellValue(DataItem.GrandPrize);

                excel.Received(1).AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotal(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
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
                _sheet.Received().SetTextRowValue(
                    excel.TemplateSheet,
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    expectedTitle);
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
            const int RowIndex = 1;
            const int ColumnCurrency = 4;
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.AssignGrandTotal(_sheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(ViewResources.Totalin + " USD");
                _sheet.ReceivedWithAnyArgs(1).MergeCells(RowIndex, 0, RowIndex, ColumnCurrency);
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(_sheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(ViewResources.Total);
                cell.Received(1).SetCellValue(DataItem.CurrencyName);
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