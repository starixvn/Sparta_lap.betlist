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
    public class CashOutWinLossExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;

        private const int ColumnCashOutTimes = 5;
        private const int ColumnCashOutTickets = 6;
        private const int ColumnCashOutValue = 7;

        private const int ColumnBuyBackAmountSportsbook = 8;
        private const int ColumnBuyBackAmountBA = 9;
        private const int ColumnBuyBackAmountTotal = 10;

        private const int ColumnTicketPayout = 11;
        private const int ColumnCashOutCommmission = 12;

        private const int ColumnSettleAmountTicketPayout = 13;
        private const int ColumnSettleAmountBBAmtComm = 14;
        private const int ColumnSettleAmountTotal = 15;

        private const int ColumnCount = 21;

        private static readonly CashOutWinLoss DataItem = new CashOutWinLoss
        {
            Date = new DateTime(2017, 1, 1),
            ExternalId = "1",
            SystemId = "2",
            CurrencyName = "UUS",
            RowSpan = 2
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

        private static readonly CashOutWinLossViewModel ViewModel = new CashOutWinLossViewModel
        {
            MainResult = new List<CashOutWinLoss>
            {
                DataItem
            },
            SummaryTotal = new Dictionary<string, CashOutWinLoss>
            {
                ["USD"] = new CashOutWinLoss()
            },
            TotalData = new List<CashOutWinLoss>
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
                row1.Cells[ColumnCashOutTimes].Received(1).SetCellValue(ViewResources.CashOutTimes);
                row1.Cells[ColumnCashOutTickets].Received(1).SetCellValue(ViewResources.CashOutTickets);
                row1.Cells[ColumnCashOutValue].Received(1).SetCellValue(ViewResources.CashOutValue);
                row1.Cells[ColumnBuyBackAmountSportsbook].Received(1).SetCellValue(ViewResources.BuyBackAmount);
                row1.Cells[ColumnTicketPayout].Received(1).SetCellValue(ViewResources.TicketPayout);
                row1.Cells[ColumnCashOutCommmission].Received(1).SetCellValue(ViewResources.CashOutCommission);
                row1.Cells[ColumnSettleAmountTicketPayout].Received(1).SetCellValue(ViewResources.SettleAmount);

                row2.Cells[ColumnBuyBackAmountSportsbook].Received(1).SetCellValue(ViewResources.ProductSB);
                row2.Cells[ColumnBuyBackAmountBA].Received(1).SetCellValue(ViewResources.ProductBA);
                row2.Cells[ColumnBuyBackAmountTotal].Received(1).SetCellValue(ViewResources.Total);
                row2.Cells[ColumnSettleAmountTicketPayout].Received(1).SetCellValue(ViewResources.TicketPayout);
                row2.Cells[ColumnSettleAmountBBAmtComm].Received(1).SetCellValue(ViewResources.BBAmtComm);
                row2.Cells[ColumnSettleAmountTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotalInEurData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnDate].Received(1).SetCellValue(DataItem.Date.ToShortDateString());
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);

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
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(ViewResources.CashOutWinLoss, new ConvertingOption(), MetaModel);

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

        [Test]
        public void ProcessUI_NotAllowBA_CallHideColumnCorrectly()
        {
            // Arrange
            const int ColumnBuyBackAmountBA = 9;
            const int ColumnBuyBackAmountTotal = 10;
            ViewModel.IsLicenseeOfferBA = false;
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            _sheet.Received().HideColumn(ColumnBuyBackAmountBA, ColumnBuyBackAmountTotal);
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

        private static CashOutWinLossExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.DrillDownLevel = level;
            var excel = Substitute.ForPartsOf<CashOutWinLossExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                MetaModel,
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}