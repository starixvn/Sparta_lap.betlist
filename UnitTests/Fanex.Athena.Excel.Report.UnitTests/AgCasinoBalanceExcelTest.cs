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
    public class AgCasinoBalanceExcelTest
    {
        private const int ColumnCount = 13;

        private static readonly AgCasinoBalance DataItem = new AgCasinoBalance
        {
            Date = new DateTime(2017, 1, 1),
            ExternalId = "1",
            SystemId = "2",
            CurrencyName = "UUS",
            TransferIn = 3,
            TransferOut = 4,
            TurnOver = 5,
            TotalTransaction = 6,
            CustomerWinLoss = 7,
            LicenseeWL = 8,
            OpeningBalance = 9,
            ClosingBalance = 10,
            RowSpan = 2
        };

        private static readonly AgCasinoBalanceMetaModel MetaModel = new AgCasinoBalanceMetaModel
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

        private static readonly AgCasinoBalanceViewModel ViewModel = new AgCasinoBalanceViewModel
        {
            MainResult = new List<AgCasinoBalance>
            {
                DataItem
            },
            SummaryTotal = new Dictionary<string, AgCasinoBalance>
            {
                ["USD"] = new AgCasinoBalance()
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
                cell.Received(1).SetCellValue("#");
                cell.Received(1).SetCellValue(ViewResources.ExternalId);
                cell.Received(1).SetCellValue(ViewResources.SystemId);
                cell.Received(1).SetCellValue(ViewResources.Date);
                cell.Received(1).SetCellValue(ViewResources.Currency);
                cell.Received(1).SetCellValue(ViewResources.OpeningBalance);
                cell.Received(1).SetCellValue(ViewResources.TransferIn);
                cell.Received(1).SetCellValue(ViewResources.TransferOut);
                cell.Received(1).SetCellValue(ViewResources.Turnover);
                cell.Received(1).SetCellValue(ViewResources.TotalTransaction);
                cell.Received(1).SetCellValue(ViewResources.CustomerW_L);
                cell.Received(1).SetCellValue(ViewResources.LicenseeW_L);
                cell.Received(1).SetCellValue(ViewResources.ClosingBalance);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(DataItem.Date.Value.ToString("MM/dd/yyyy"));
                cell.Received(1).SetCellValue(DataItem.ExternalId);
                cell.Received(1).SetCellValue(DataItem.SystemId);
                cell.Received(1).SetCellValue(DataItem.CurrencyName);
                cell.Received(1).SetCellValue(DataItem.TransferIn);
                cell.Received(1).SetCellValue(DataItem.TransferOut);
                cell.Received(1).SetCellValue(DataItem.TurnOver);
                cell.Received(1).SetCellValue(DataItem.TotalTransaction);
                cell.Received(1).SetCellValue(DataItem.CustomerWinLoss);
                cell.Received(1).SetCellValue(DataItem.LicenseeWL);
                cell.Received(1).SetCellValue(DataItem.OpeningBalance);
                cell.Received(1).SetCellValue(DataItem.ClosingBalance);
                cell.Received(1).SetCellValue(DataItem.ExternalId);

                excel.Received(1).AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(ViewResources.AGCasinoBalance, new ConvertingOption(), MetaModel);

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
                _sheet.Received(1).CreateFreezePane(0, 2);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [TestCase(0, new[] { 2, 3 })]
        [TestCase(1, new[] { 2, 1 })]
        [TestCase(2, new[] { 1 })]
        public void ProcessUI_WhenCall_CallHideColumnCorrectly(int level, int[] hiddenColumnIndexList)
        {
            // Arrange
            var excel = CreateExcel(level);

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            _sheet.ReceivedWithAnyArgs(1).HideColumn(hiddenColumnIndexList);
        }

        [Test]
        public void AssignGrandTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 1;
            const int ColumnCurrency = 4;
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(ViewResources.AGCasinoBalance, new ConvertingOption(), MetaModel);

            // Action
            excel.AssignGrandTotalData(_sheet, RowIndex);

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
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(ViewResources.AGCasinoBalance, new ConvertingOption(), MetaModel);

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

        private static AgCasinoBalanceExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.DrillDownLevel = level;
            var excel = Substitute.ForPartsOf<AgCasinoBalanceExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                MetaModel,
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}