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
    public class AgCasinoBalanceExcelTest : BaseReportExcelTest
    {
        private const int ColumnCount = 13;

        private static readonly AgCasinoBalance DataItem = FakeHelper.Create<AgCasinoBalance>();

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
                },
            CurrencyName = "AUD",
            BaseCurrency = "USD"
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

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();
            var headers = new[] 
            {
                "#", ViewResources.Date, ViewResources.ExternalId, ViewResources.SystemId,
                ViewResources.Currency, ViewResources.OpeningBalance, ViewResources.TransferIn,
                ViewResources.TransferOut, ViewResources.Turnover, ViewResources.TotalTransaction,
                ViewResources.CustomerW_L, ViewResources.LicenseeW_L, ViewResources.ClosingBalance
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            AssertSetTextRowHeader(row, headers);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 2);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[1].Received(1).SetCellValue(DataItem.Date.Value.ToString("MM/dd/yyyy"));
                row.Cells[2].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[3].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[4].Received(1).SetCellValue(DataItem.CurrencyName);
                row.Cells[5].Received(1).SetCellValue(DataItem.OpeningBalance);
                row.Cells[6].Received(1).SetCellValue(DataItem.TransferIn);
                row.Cells[7].Received(1).SetCellValue(DataItem.TransferOut);
                row.Cells[8].Received(1).SetCellValue(DataItem.TurnOver);
                row.Cells[9].Received(1).SetCellValue(DataItem.TotalTransaction);
                row.Cells[10].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[11].Received(1).SetCellValue(DataItem.LicenseeWL);
                row.Cells[12].Received(1).SetCellValue(DataItem.ClosingBalance);

                excel.Received(1).AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            const string ExpectedTitle = "AG Casino Balance, 01/01/2017 --> 02/02/2017, Currency: AUD, Base Currency: USD";

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ExpectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 2);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
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
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, hiddenColumnIndexList);
        }

        [Test]
        public void AssignGrandTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            const int ColumnCurrency = 4;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowIndex);
            var excel = CreateExcel();
            ReportExcelHelper.GenerateReportTitle(ViewResources.AGCasinoBalance, new ConvertingOption(), MetaModel);

            // Action
            excel.AssignGrandTotalData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Totalin + " USD");
                FakeSheet.ReceivedWithAnyArgs(1).MergeCells(RowIndex, 0, RowIndex, ColumnCurrency);
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 100;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                row.Cells[4].Received(1).SetCellValue(DataItem.CurrencyName);
                FakeSheet.MergeCells(RowIndex - 1, 0, RowIndex - 1, 3);
            });
        }

        private static AgCasinoBalanceExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.DrillDownLevel = level;
            var excel = Substitute.ForPartsOf<AgCasinoBalanceExcelTestable>(
                string.Empty,
                MetaModel,
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}