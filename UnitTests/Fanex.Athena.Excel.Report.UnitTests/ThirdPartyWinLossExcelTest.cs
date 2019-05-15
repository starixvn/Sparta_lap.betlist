using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Core.ResultModel;
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
    public class ThirdPartyWinLossExcelTest : BaseReportExcelTest
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnover = 5;
        private const int ColumnTotalTransaction = 6;
        private const int ColumnCustomerWl = 7;
        private const int ColumnLicenseeWl = 8;

        private const int RowContentTemplate = 2;

        private const int ColumnCount = 9;

        private static readonly ThirdPartyWinLoss DataItem = FakeHelper.Create<ThirdPartyWinLoss>();

        private static readonly ThirdPartyWinLossMetaModel MetaModel = new ThirdPartyWinLossMetaModel
        {
            ProductName = "Si pot buc",
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
            BaseCurrency = string.Empty
        };

        private static readonly ReportResultModel<ThirdPartyWinLoss> ResultData = new ReportResultModel<ThirdPartyWinLoss>
        {
            DataList = new List<ThirdPartyWinLoss>
            {
                DataItem
            },
            TotalDataList = new List<ThirdPartyWinLoss>
            {
                DataItem
            },
            SummaryTotal = new Dictionary<string, ThirdPartyWinLoss>
            {
                ["USD"] = new ThirdPartyWinLoss()
            }
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row.Cells[0].Received(1).SetCellValue("#");
                row.Cells[ColumnExternalId].Received(1).SetCellValue(ViewResources.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(ViewResources.SystemId);
                row.Cells[ColumnDate].Received(1).SetCellValue(ViewResources.Date);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(ViewResources.Currency);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(ViewResources.Turnover);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(ViewResources.TotalTransaction);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(ViewResources.LicenseeW_L);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowContentTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [TestCase(1, "MM/dd/yyyy")]
        [TestCase(2, "MM/yyyy")]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly(byte reportBy, string dateFormat)
        {
            // Arrange
            MetaModel.ReportBy = reportBy;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 2);
            var excel = CreateExcel();
            excel.When(x => x.AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();
            excel.When(x => x.AssignGrandTotalInEurData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(DataItem.Index);
                row.Cells[ColumnDate].Received(1).SetCellValue(DataItem.Date.Value.ToString(dateFormat));
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);

                excel.Received(1).AssignTotalData(Arg.Any<ISheet>(), Arg.Any<int>());
                excel.Received(1).AssignGrandTotalInEurData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = MetaModel.ProductName + " Win Loss, 01/01/2017 --> 02/02/2017 - Base Currency: AUD";

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 2);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [TestCase(0, new[] { ColumnExternalId, ColumnSystemId })]
        [TestCase(1, new[] { ColumnExternalId, ColumnDate })]
        [TestCase(2, new[] { ColumnDate })]
        public void ProcessUI_WhenCall_CallHideColumnCorrectly(int level, int[] hiddenColumnList)
        {
            // Arrange
            var excel = CreateExcel(level);

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            FakeSheet.Received().HideColumn(hiddenColumnList);
        }

        [Test]
        public void AssignGrandTotalInEurData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            const int ExpectedColumnCurrency = 4;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignGrandTotalInEurData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Totalin + " USD");
                FakeSheet.ReceivedWithAnyArgs(1).MergeCells(RowIndex, 0, RowIndex, ExpectedColumnCurrency);
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
                row.Cells[ColumnCurrency].Received(1).SetCellValue(DataItem.CurrencyName);
                FakeSheet.MergeCells(RowIndex - 1, 0, RowIndex - 1, 3);
            });
        }

        private static ThirdPartyWinLossExcelTestable CreateExcel(int level = 0)
        {
            MetaModel.DrillDownLevel = level;
            var excel = Substitute.ForPartsOf<ThirdPartyWinLossExcelTestable>(
                string.Empty,
                MetaModel,
                ResultData);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}