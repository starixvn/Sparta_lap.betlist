namespace Fanex.Athena.Excel.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Base.Extension;
    using Model.Report.DTO.LiveReports;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel.LiveReports;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using TestableClasses;
    using UnitTest.Common.Helpers;

    public class LiveReportExcelTest : BaseReportExcelTest
    {
        private const int ColumnAmountDue = 8;
        private const int ColumnBetCount = 5;
        private const int ColumnCustomerComm = 2;
        private const int ColumnCustomerTotal = 3;
        private const int ColumnCustomerWl = 1;
        private const int ColumnLicenseeComm = 7;
        private const int ColumnLicenseeWl = 6;
        private const int ColumnProduct = 0;
        private const int ColumnTemplateCount = 9;
        private const int ColumnTurnover = 4;
        private const int RowContentTemplate = 4;
        private const int RowSummaryHeaderTemplate = 1;
        private const int SummarySectionRowIndex = RowSummaryHeaderTemplate;

        private static readonly ConvertingOption ConvertingOpt = new ConvertingOption
        {
            CurrencyName = "USD"
        };

        private readonly ShowingOption _showingOpt = new ShowingOption();

        private readonly IList<LiveReport> _liveReportList = new List<LiveReport>
        {
            FakeHelper.Create<LiveReport>()
        };

        private LiveReportDetailViewModel _excelModel;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _excelModel = new LiveReportDetailViewModel();
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var excel = CreateExcel();

            var headers = SetUpForAssignCommonHeader(RowSummaryHeaderTemplate);
            var groupRow = headers.ElementAt(0);
            var headerRow1 = headers.ElementAt(1);
            var headerRow2 = headers.ElementAt(2);

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                groupRow.Cells[ColumnProduct].Received().SetCellValue(ViewResources.Summary);
                headerRow1.Cells[ColumnProduct].Received().SetCellValue(ViewResources.Product);
                headerRow1.Cells[ColumnCustomerWl].Received().SetCellValue(ViewResources.CustomerW_L);
                headerRow1.Cells[ColumnTurnover].Received().SetCellValue(ViewResources.Turnover);
                headerRow1.Cells[ColumnTurnover + 1].Received().SetCellValue(ViewResources.BetCount);
                headerRow1.Cells[ColumnTurnover + 2].Received().SetCellValue(ViewResources.LicenseeW_L);
                headerRow1.Cells[ColumnTurnover + 3].Received().SetCellValue(ViewResources.Comm);
                headerRow1.Cells[ColumnTurnover + 4].Received().SetCellValue(ViewResources.AmountDueTo);
                headerRow2.Cells[ColumnCustomerWl].Received().SetCellValue(ViewResources.WinLoss);
                headerRow2.Cells[ColumnCustomerComm].Received().SetCellValue(ViewResources.Comm);
                headerRow2.Cells[ColumnCustomerTotal].Received().SetCellValue(ViewResources.Total);
            });
        }

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
            var excel = CreateExcel();

            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);
            var totalRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate + 1);

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            var data = _liveReportList[0];

            // Assert
            Assert.Multiple(() =>
            {
                // main data
                row.Cells[ColumnProduct].Received().SetCellValue(data.ProductName);
                AssertCommonCells(row, data);

                // total summary
                totalRow.Cells[0].Received().SetCellValue(ViewResources.Total);
                AssertCommonCells(totalRow, data);
            });
        }

        [Test]
        public void AssignSheetData_DataBySportsType_AddRowsForSportsTypeData()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            var sportsTypeRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow + 3);

            _excelModel.BySports = new List<LiveReport>
            {
                new LiveReport
                {
                    ProductName = "Product",
                    CustomerWinLoss = 10,
                    CustomerComm = 20,
                    TurnOver = 30,
                    BetCount = 40,
                    LicenseeComm = 50,
                    LicenseeWinLoss = 60,
                    AmountDue = 70
                }
            };

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            var data = _excelModel.BySports.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                sportsTypeRow.Cells[ColumnProduct].Received().SetCellValue(data.ProductName);
                AssertCommonCells(sportsTypeRow, data);
            });
        }

        [Test]
        public void AssignSheetData_DataBySportsType_DataIsEmpty_DoNotAddRowsForSportsTypeData()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            var headers = SetUpForAssignCommonHeader(currentRow + 3);
            var groupRow = headers.ElementAt(0);

            _excelModel.BySports = new List<LiveReport>();

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            groupRow.Cells[ColumnProduct].DidNotReceive().SetCellValue(ViewResources.BySportsType);
        }

        [Test]
        public void AssignSheetData_DataBySportsType_CustomerCommVisibleIsFalse_MergeCellCustomerWL()
        {
            _excelModel.BySports = new List<LiveReport>
            {
                new LiveReport()
            };

            AssignSheetData_CustomerCommVisibleIsFalse_MergeCellCustomerWL();
        }

        [Test]
        public void AssignSheetData_DataBySportsType_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL()
        {
            _excelModel.BySports = new List<LiveReport>
            {
                new LiveReport()
            };

            AssignSheetData_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL();
        }

        [Test]
        public void AssignSheetData_DataByCurrency_AddRowsForCurrencyData()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            var currencyRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow + 3);

            _excelModel.ByCurrencies = new List<LiveReport>
            {
                new LiveReport
                {
                    ProductName = "Product",
                    CustomerWinLoss = 10,
                    CustomerComm = 20,
                    TurnOver = 30,
                    BetCount = 40,
                    LicenseeComm = 50,
                    LicenseeWinLoss = 60,
                    AmountDue = 70
                }
            };

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            var data = _excelModel.ByCurrencies.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                currencyRow.Cells[ColumnProduct].Received().SetCellValue(data.ProductName);
                AssertCommonCells(currencyRow, data);
            });
        }

        [Test]
        public void AssignSheetData_DataByCurrency_DataIsEmpty_DoNotAddRowsForCurrencyData()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            var headers = SetUpForAssignCommonHeader(currentRow + 3);
            var groupRow = headers.ElementAt(0);

            _excelModel.ByCurrencies = new List<LiveReport>();

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            groupRow.Cells[ColumnProduct].DidNotReceive().SetCellValue(ViewResources.ByCurrency);
        }

        [Test]
        public void AssignSheetData_DataByCurrency_CustomerCommVisibleIsFalse_MergeCellCustomerWL()
        {
            _excelModel.ByCurrencies = new List<LiveReport>
            {
                new LiveReport()
            };

            AssignSheetData_CustomerCommVisibleIsFalse_MergeCellCustomerWL();
        }

        [Test]
        public void AssignSheetData_DataByCurrency_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL()
        {
            _excelModel.ByCurrencies = new List<LiveReport>
            {
                new LiveReport()
            };

            AssignSheetData_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL();
        }

        [Test]
        public void AssignSheetData_DataByBetType_AddRowsForBetTypeData()
        {
            // Arrange
            var currentRow = RowContentTemplate;
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            currentRow += 3;
            var betTypeRow = ExcelTestHelper.MockGetRow(FakeSheet, 1, currentRow);
            var betTypeDetailRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            _excelModel.ByBetType = new List<LiveReportByBetType>
            {
                new LiveReportByBetType
                {
                    ProductName = "Product",
                    LiveReports = new List<LiveReport>
                    {
                        new LiveReport
                        {
                            ProductName = "Product",
                            CustomerWinLoss = 10,
                            CustomerComm = 20,
                            TurnOver = 30,
                            BetCount = 40,
                            LicenseeComm = 50,
                            LicenseeWinLoss = 60,
                            AmountDue = 70
                        }
                    }
                }
            };

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            var data = _excelModel.ByBetType.ElementAt(0).LiveReports.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                betTypeRow.Cells[ColumnProduct].Received().SetCellValue(_excelModel.ByBetType.ElementAt(0).ProductName);
                betTypeDetailRow.Cells[ColumnProduct].Received().SetCellValue(data.ProductName);
                AssertCommonCells(betTypeDetailRow, data);
            });
        }

        [Test]
        public void AssignSheetData_DataByBetType_ProductSB_AssignCorrectCellName()
        {
            // Arrange
            var expected = ViewResources.Sportsbook;
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            currentRow += 3;
            var betTypeRow = ExcelTestHelper.MockGetRow(FakeSheet, 1, currentRow);

            _excelModel.ByBetType = new List<LiveReportByBetType>
            {
                new LiveReportByBetType
                {
                    ProductName = ViewResources.SB,
                    LiveReports = new List<LiveReport>()
                }
            };

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            betTypeRow.Cells[ColumnProduct].Received().SetCellValue(expected);
        }

        [Test]
        public void AssignSheetData_DataByBetType_ProductIsNotSB_AssignCorrectCellName()
        {
            // Arrange
            var expected = "Not SB";
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            currentRow += 3;
            var betTypeRow = ExcelTestHelper.MockGetRow(FakeSheet, 1, currentRow);

            _excelModel.ByBetType = new List<LiveReportByBetType>
            {
                new LiveReportByBetType
                {
                    ProductName = expected,
                    LiveReports = new List<LiveReport>()
                }
            };

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            betTypeRow.Cells[ColumnProduct].Received().SetCellValue(expected);
        }

        [Test]
        public void AssignSheetData_DataByBetType_DataIsEmpty_DoNotAddRowsForBetTypeData()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            var headers = SetUpForAssignCommonHeader(currentRow + 3);
            var groupRow = headers.ElementAt(0);

            _excelModel.ByBetType = new List<LiveReportByBetType>();

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            groupRow.Cells[ColumnProduct].DidNotReceive().SetCellValue(ViewResources.ByBetType);
        }

        [Test]
        public void AssignSheetData_DataByBetType_CustomerCommVisibleIsFalse_MergeCellCustomerWL()
        {
            _excelModel.ByBetType = new List<LiveReportByBetType>
            {
                new LiveReportByBetType
                {
                    ProductName = "Product",
                    LiveReports = new List<LiveReport>()
                }
            };

            AssignSheetData_CustomerCommVisibleIsFalse_MergeCellCustomerWL();
        }

        [Test]
        public void AssignSheetData_DataByBetType_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL()
        {
            _excelModel.ByBetType = new List<LiveReportByBetType>
            {
                new LiveReportByBetType
                {
                    ProductName = "Product",
                    LiveReports = new List<LiveReport>()
                }
            };

            AssignSheetData_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL();
        }

        [TestCase(0, nameof(ViewResources.NoneLive))]
        [TestCase(1, nameof(ViewResources.Live))]
        [TestCase(2, nameof(ViewResources.LiveAndNonLive))]
        [TestCase(3, "")]
        public void AssignTitle_IsNonLive_AssignCorrectTitle(int liveIndicator, string expectedTitleKey)
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowTitleTemplate);

            _excelModel.Param = new LiveReportMetaModel
            {
                LiveIndicator = liveIndicator,
                FromDate = new DateTime(2018, 1, 1),
                ToDate = new DateTime(2018, 2, 2)
            };

            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue("Live Report - " +
                ViewResources.ResourceManager.GetString(expectedTitleKey) +
                " data, 01/01/2018 --> 02/02/2018 - View in EUR");
        }

        [Test]
        public void ProcessUI_CustomerCommVisibleIsFalse_HideCustomerColumn()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var hiddenRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, SummarySectionRowIndex + 2);

            FakeSheet.FirstRowNum.Returns(0);
            FakeSheet.LastRowNum.Returns(0);
            _showingOpt.CustomerCommVisible = false;
            _showingOpt.BAVisible = true;

            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertHideColumns(FakeSheet, ColumnCustomerTotal, ColumnCustomerComm);

                Assert.That(hiddenRow.ZeroHeight, Is.True);
            });
        }

        [Test]
        public void ProcessUI_BAVisibleAndRBVisibleAreFalse_HideLicenseeColumn()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);

            FakeSheet.FirstRowNum.Returns(0);
            FakeSheet.LastRowNum.Returns(0);
            _showingOpt.CustomerCommVisible = true;
            _showingOpt.BAVisible = false;

            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnLicenseeComm);
        }

        private static void AssertCommonCells(IRow row, LiveReport data)
        {
            row.Cells[ColumnCustomerWl].Received(1).SetCellValue(data.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(data.CustomerComm);
            row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(data.CustomerTotal);
            row.Cells[ColumnTurnover].Received(1).SetCellValue(data.TurnOver);
            row.Cells[ColumnBetCount].Received(1).SetCellValue(data.BetCount);
            row.Cells[ColumnLicenseeComm].Received(1).SetCellValue(data.LicenseeComm);
            row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(data.LicenseeWinLoss);
            row.Cells[ColumnAmountDue].Received(1).SetCellValue(data.AmountDue);
        }

        private void AssignSheetData_CustomerCommVisibleIsFalse_MergeCellCustomerWL()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            // main data
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            currentRow += 3;
            //// addition data (data by sport type/bet type/ currency)
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);

            var rowHeaderMerge = currentRow - 2;

            _showingOpt.CustomerCommVisible = false;

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            FakeSheet.Received().AddMergedRegion(Arg.Is<CellRangeAddress>(x => x.FirstRow == rowHeaderMerge
                && x.FirstColumn == ColumnCustomerWl
                && x.LastRow == rowHeaderMerge + 1
                && x.LastColumn == ColumnCustomerWl));
        }

        private void AssignSheetData_CustomerCommVisibleIsTrue_DoNotMergeCellCustomerWL()
        {
            // Arrange
            var currentRow = RowContentTemplate;

            // main data
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ++currentRow);

            currentRow += 3;
            SetUpForAssignCommonHeader(currentRow);

            currentRow += 3;

            // Addition data (data by sport type/bet type/ currency)
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);

            var rowHeaderMerge = currentRow - 2;

            _showingOpt.CustomerCommVisible = true;

            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            FakeSheet.DidNotReceive().AddMergedRegion(Arg.Is<CellRangeAddress>(x => x.FirstRow == rowHeaderMerge
                && x.FirstColumn == ColumnCustomerWl
                && x.LastRow == rowHeaderMerge + 1
                && x.LastColumn == ColumnCustomerWl));
        }

        private LiveReportExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<LiveReportExcelTestable>(
                string.Empty,
                ConvertingOpt,
                _showingOpt,
                _liveReportList,
                _excelModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());

            return excel;
        }

        private IEnumerable<IRow> SetUpForAssignCommonHeader(int rowIndex)
        {
            return new List<IRow>
            {
                ExcelTestHelper.MockGetRow(FakeSheet, 1, rowIndex),
                ExcelTestHelper.MockGetRow(FakeSheet, 10, rowIndex + 1),
                ExcelTestHelper.MockGetRow(FakeSheet, 4, rowIndex + 2)
            };
        }
    }
}