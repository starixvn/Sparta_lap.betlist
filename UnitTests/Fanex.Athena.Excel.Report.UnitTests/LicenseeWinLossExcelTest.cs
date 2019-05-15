using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.Models;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public partial class LicenseeWinLossExcelTest : BaseReportExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;

        private const int ColumnTurnoverSportBook = 5;
        private const int ColumnTurnoverTotal = 20;

        private const int ColumnActualStake = 21;

        private const int ColumnBuyBackAmountSportsbook = 22;
        private const int ColumnBuyBackAmountBA = 23;
        private const int ColumnBuyBackAmountTotal = 24;

        private const int ColumnTotalTransaction = 25;

        private const int ColumnCustomerWl = 26;
        private const int ColumnCustomerComm = 27;
        private const int ColumnCustomerTotal = 28;

        private const int ColumnLicenseeWlSportsBook = 29;
        private const int ColumnLicenseeWlTotal = 44;

        private const int ColumnBetTradeCommission = 45;
        private const int ColumnCashOutCommission = 46;
        private const int ColumnBaCommission = 47;
        private const int ColumnAmtDueToBetTrade = 48;
        private const int ColumnAmtDueToBa = 49;

        private const int ColumnTemplateCount = 50;

        private const int RowContentTemplate = 3;

        private static readonly LicenseeWinLoss DataItem = FakeHelper.Create<LicenseeWinLoss>();

        private static readonly LicenseeWinLossMetaModel MetaModel = new LicenseeWinLossMetaModel
        {
            BaseCurrency = string.Empty,
            CurrencyName = "INH",
            FromDate = new DateTime(2018, 1, 1),
            ToDate = new DateTime(2018, 2, 2),
        };

        private static readonly LicenseeWinLossExcelModel ExcelModel = new LicenseeWinLossExcelModel
        {
            MetaModel = MetaModel,
            DataList = new List<LicenseeWinLoss> { DataItem }
        };

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignCommonData(Arg.Any<IRow>(), Arg.Any<LicenseeWinLoss>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.AssignGrandTotalInEurData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnDate].Received(1).SetCellValue(DataItem.Date.ToShortDateString());
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);

                excel.Received(1).AssignCommonData(row, DataItem);
                excel.Received(1).AssignTotalData(FakeSheet, 4);
                excel.Received(1).AssignGrandTotalInEurData(FakeSheet, Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel();
            var title = ReportExcelHelper.GetTitleBaseCurrency(
                ExcelModel.MetaModel.BaseCurrency, 
                ExcelModel.MetaModel.CurrencyName);
            var reportTitle = ReportExcelHelper.GenerateReportTitle(
                ViewResources.LicenseeWinLoss,
                ExcelModel.MetaModel.FromDate,
                ExcelModel.MetaModel.ToDate,
                title);

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void AssignGrandTotalInEurData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var item = FakeHelper.Create<LicenseeWinLoss>();
            ExcelModel.SummaryTotal = new Dictionary<string, LicenseeWinLoss>
            {
                { "USD", item }
            };
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignCommonData(Arg.Any<IRow>(), Arg.Any<LicenseeWinLoss>())).DoNotCallBase();

            // Action
            excel.AssignGrandTotalInEurData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received(1).AssignCommonData(row, item);
                row.Cells[0].Received(1).SetCellValue(ViewResources.Totalin + " USD");
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var item1 = FakeHelper.Create<LicenseeWinLoss>();
            var item2 = FakeHelper.Create<LicenseeWinLoss>();
            ExcelModel.TotalData = new List<LicenseeWinLoss> { item1, item2 };
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex + 1);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignCommonData(Arg.Any<IRow>(), Arg.Any<LicenseeWinLoss>())).DoNotCallBase();

            // Action
            excel.AssignTotalData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received(1).AssignCommonData(row1, item1);
                row1.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                row1.Cells[ColumnCurrency].SetCellValue(item1.CurrencyName);

                excel.Received(1).AssignCommonData(row2, item2);
                row2.Cells[0].DidNotReceiveWithAnyArgs().SetCellValue(0);
                row2.Cells[ColumnCurrency].SetCellValue(item2.CurrencyName);
            });
        }

        [Test]
        public void ProcessUILicenseeWinLoss_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetCell(FakeSheet, ReportBase.RowHeaderTemplate, ColumnLicenseeWlSportsBook);
            var excel = CreateExcel(productCodes: new[] { Product.SB, Product.BA });

            // Action
            excel.ProcessUILicenseeWinLoss(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(ViewResources.LicenseeW_L);
                FakeSheet.Received(1).SetColumnHidden(ColumnLicenseeWlSportsBook + 1, true);
            });
        }

        [Test]
        public void ProcessUITurnover_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetCell(FakeSheet, ReportBase.RowHeaderTemplate, ColumnTurnoverSportBook);
            var excel = CreateExcel(productCodes: new[] { Product.SB, Product.BT });

            // Action
            excel.ProcessUITurnover(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(ViewResources.Turnover);
                FakeSheet.Received(1).SetColumnHidden(ColumnTurnoverSportBook + 1, true);
            });
        }

        [TestCase(0, new[] { ColumnExternalId, ColumnSystemId })]
        [TestCase(1, new[] { ColumnExternalId, ColumnDate })]
        [TestCase(3, new[] { ColumnDate })]
        public void HideColumnsByLevel_WhenCall_HideColumns(int level, int[] hiddenColumns)
        {
            // Arrange
            var excel = CreateExcel(level);

            // Action
            excel.HideColumnsByLevel(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, hiddenColumns);
        }

        private static LicenseeWinLossExcelTestable CreateExcel(int level = 0, string[] productCodes = null)
        {
            MetaModel.DrillDownLevel = level;
            MetaModel.ProductOptions = (productCodes ?? new string[] { }).Select(x => new ProductOption
            {
                Code = x,
                Status = true,
                Name = x
            }).ToList();
            var excel = Substitute.ForPartsOf<LicenseeWinLossExcelTestable>(
                string.Empty,
                ExcelModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}