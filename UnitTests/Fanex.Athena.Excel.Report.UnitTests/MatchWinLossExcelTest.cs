using System;
using System.Data;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.VendorAdapter.Interfaces;
using Fanex.Common;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public partial class MatchWinLossExcelTest : BaseReportExcelTest
    {
        private const int ColumnCustomerWl = 4;
        private const int ColumnCustomerComm = 5;
        private const int ColumnCustomerTotal = 6;
        private const int ColumnBaCommission = 8;
        private const int ColumnAmtDueToBa = 9;
        private const int ColumnTemplateCount = 10;

        private const int RowMatchTemplate = 3;

        private static readonly MatchWinLossMetaModel MetaModel = new MatchWinLossMetaModel
        {
            Date = new DateTime(2018, 1, 1)
        };

        private DataTable _contentData;
        private DataTable _totalData;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _contentData = new DataTable();
            _totalData = new DataTable();
            _contentData.Columns.Add("bettype", typeof(int));
            _contentData.Columns.Add("liveindicator", typeof(int));
            _contentData.Columns.Add("turnover", typeof(decimal));
            _contentData.Columns.Add("betcount", typeof(int));
            _contentData.Columns.Add("memberwinlost", typeof(decimal));
            _contentData.Columns.Add("licenseewl", typeof(decimal));
            _contentData.Columns.Add("commba", typeof(decimal));
            _contentData.Columns.Add("amtduetoba", typeof(decimal));
            _contentData.Columns.Add("amtduetorb", typeof(decimal));
            _contentData.Columns.Add("commrb", typeof(decimal));
            _contentData.Columns.Add("winlossrb", typeof(decimal));
            _contentData.Columns.Add("playercomm", typeof(decimal));

            _totalData.Columns.Add("sumturnover", typeof(decimal));
            _totalData.Columns.Add("sumbetcount", typeof(int));
            _totalData.Columns.Add("summemberwinlost", typeof(decimal));
            _totalData.Columns.Add("sumlicenseewl", typeof(decimal));
            _totalData.Columns.Add("sumcommba", typeof(decimal));
            _totalData.Columns.Add("sumamtduetoba", typeof(decimal));
            _totalData.Columns.Add("sumamtduetorb", typeof(decimal));

            _totalData.Columns.Add("sumcommrb", typeof(decimal));
            _totalData.Columns.Add("sumwinlossrb", typeof(decimal));
            _totalData.Columns.Add("sumplayercomm", typeof(decimal));

            _contentData.Rows.Add(100, 1, 3, 4, 5, 6, 7, 8, 9, 10);
            _totalData.Rows.Add(11, 12, 13, 14, 15, 16, 17, 18, 19, 20);
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#",
                ViewResources.Match,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmtDueToBA
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertSetTextRowHeader(row1, headerTextList);
                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowMatchTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel();
            const string ExpectedTitle = "Match Win Loss, 01/01/2018 - View in ";

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ExpectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsInvisible_CallHideColumnsCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(
                FakeSheet,
                ColumnCustomerTotal,
                ColumnCustomerComm,
                ColumnBaCommission,
                ColumnAmtDueToBa);
        }

        [Test]
        public void ProcessUI_AllOptionsVisible_NotHideAnyColumns()
        {
            // Arrange
            var excel = CreateExcel(new ShowingOption
            {
                CustomerCommVisible = true,
                BAVisible = true,
                RBVisible = true
            });

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, false);
        }

        [Test]
        public void AssignGrandTotalSummary_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowIndex);
            var excel = CreateExcel();
            var columns = new[]
            {
               "sumturnover", "sumbetcount", "summemberwinlost", "sumplayercomm", "sumtotalwl", "sumlicenseewl",
               "sumcommba", "sumamtduetoba"
            };
            var cellIndex = 2;

            // Action
            excel.AssignGrandTotalSummary(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                foreach (string columnName in columns)
                {
                    row.Cells[cellIndex].Received(1).SetCellValue(Converter.ToDecimal(_totalData.Rows[0][columnName]));
                    cellIndex++;
                }
            });
        }

        [Test]
        public void AssignTotalSummary_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int CurrentRow = 10;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, CurrentRow);
            var excel = CreateExcel();
            var columns = new[] { "column1" };
            var sumTable = new DataTable();
            sumTable.Columns.Add("column1", typeof(decimal));
            sumTable.Rows.Add(1);
            sumTable.Rows.Add(2);

            // Action
            excel.AssignTotalSummary(FakeSheet, columns, CurrentRow, sumTable);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[2].Received(1).SetCellValue(3);
            });
        }

        private MatchWinLossExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<MatchWinLossExcelTestable>(
                string.Empty,
                new ConvertingOption { IsConvert = false },
                showingOption ?? new ShowingOption(),
                _contentData,
                _totalData,
                MetaModel,
                Substitute.For<ICacheRefAdapter>());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}