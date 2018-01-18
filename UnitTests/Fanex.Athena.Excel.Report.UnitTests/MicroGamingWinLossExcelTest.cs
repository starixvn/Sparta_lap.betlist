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
    public class MicroGamingWinLossExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnCurrency = 2;
        private const int ColumnTurnover = 3;
        private const int ColumnTotalTransaction = 4;
        private const int ColumnCustomerWl = 5;
        private const int ColumnProgressiveContribution = 6;
        private const int ColumnProgressivePayout = 7;
        private const int ColumnLicenseeWl = 8;
        private const int ColumnLicenseeNetWin = 9;
        private const int ColumnAmountDueToMGC = 10;
        private const int ColumnProgressiveFee = 11;

        private const int ColumnCount = 21;

        private static readonly MicroGamingWinLoss DataItem = new MicroGamingWinLoss
        {
            TypeID = 1,
            TypeName = "2",
            Date = new DateTime(2018, 2, 3),
            CurrencyName = "3",
            TurnOver = 4,
            TotalTransaction = 5,
            CustomerWinLoss = 6,
            ProgressiveContribution = 7,
            ProgressivePayout = 8,
            LicenseeWL = 9,
            LicenseeNetWin = 10,
            AmountDueToMGC = 11,
            ProgressiveFee = 12
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

        private static readonly MicroGamingWinLossViewModel ViewModel = new MicroGamingWinLossViewModel
        {
            MainResult = new List<MicroGamingWinLoss>
            {
                DataItem
            },
            SummaryTotal = new Dictionary<string, MicroGamingWinLoss>
            {
                ["USD"] = new MicroGamingWinLoss()
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
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate + 1, ReportBase.RowHeaderTemplate + 1);
                row.Cells[0].Received(1).SetCellValue("#");
                row.Cells[ColumnDate].Received(1).SetCellValue(ViewResources.Date);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(ViewResources.Currency);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(ViewResources.Turnover);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(ViewResources.TotalTransaction);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row.Cells[ColumnProgressiveContribution].Received(1).SetCellValue(ViewResources.ProgressiveContribution);
                row.Cells[ColumnProgressivePayout].Received(1).SetCellValue(ViewResources.ProgressivePayout);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(ViewResources.LicenseeW_L);
                row.Cells[ColumnLicenseeNetWin].Received(1).SetCellValue(ViewResources.LicenseeNetWin);
                row.Cells[ColumnAmountDueToMGC].Received(1).SetCellValue(ViewResources.AmountDueToMGC);
                row.Cells[ColumnProgressiveFee].Received(1).SetCellValue(ViewResources.ProgressiveFee);
            });
        }

        [TestCase(1, "MM/dd/yyyy")]
        [TestCase(2, "MM/yyyy")]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly(byte reportBy, string dateFormat)
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 2);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotalInEurData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            MetaModel.ReportBy = reportBy;

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row1.Cells[0].Received(1).SetCellValue(DataItem.TypeName);

                row2.Cells[0].Received(1).SetCellValue(1);
                row2.Cells[ColumnDate].Received(1).SetCellValue(DataItem.Date.ToString(dateFormat));
                row2.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);

                row2.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.TurnOver);
                row2.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransaction);
                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row2.Cells[ColumnProgressiveContribution].Received(1).SetCellValue(DataItem.ProgressiveContribution);
                row2.Cells[ColumnProgressivePayout].Received(1).SetCellValue(DataItem.ProgressivePayout);
                row2.Cells[ColumnLicenseeWl].Received(1).SetCellValue(DataItem.LicenseeWL);
                row2.Cells[ColumnLicenseeNetWin].Received(1).SetCellValue(DataItem.LicenseeNetWin);
                row2.Cells[ColumnAmountDueToMGC].Received(1).SetCellValue(DataItem.AmountDueToMGC);
                row2.Cells[ColumnProgressiveFee].Received(1).SetCellValue(DataItem.ProgressiveFee);

                excel.Received(1).AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotalInEurData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(
                ViewResources.Microgaming + " " + ViewResources.WinLoss,
                new ConvertingOption(),
                MetaModel.FromDate,
                MetaModel.ToDate,
                MetaModel.CurrencyId);

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                _sheet.Received(1).CreateFreezePane(0, 2);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void AssignGrandTotalInEurData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            const int ColumnCurrency = 4;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGrandTotalInEurData(_sheet, RowIndex);

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

        private static MicroGamingWinLossExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.DrillDownLevel = level;
            var excel = Substitute.ForPartsOf<MicroGamingWinLossExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                MetaModel,
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}