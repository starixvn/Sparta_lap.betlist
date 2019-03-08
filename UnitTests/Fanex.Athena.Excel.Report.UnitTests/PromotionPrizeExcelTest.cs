namespace Fanex.Athena.Excel.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Base.Extension;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using TestableClasses;
    using UnitTest.Common.Helpers;

    public class PromotionPrizeExcelTest : BaseReportExcelTest
    {
        private const int ColumnIndex = 0;
        private const int ColumnDate = 1;
        private const int ColumnCurrency = 2;
        private const int ColumnExternalId = 3;
        private const int ColumnSystemId = 4;
        private const int ColumnAmount = 5;

        private const int ColumnTemplateCount = 6;

        private const int RowContentTemplate = 2;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var data = new ReportResultModel<PromotionPrize>();

            var excel = CreateExcel(data);

            var header = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 1);

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                header.Cells[ColumnIndex].Received().SetCellValue("#");
                header.Cells[ColumnDate].Received().SetCellValue(ViewResources.Date);
                header.Cells[ColumnCurrency].Received().SetCellValue(ViewResources.Currency);
                header.Cells[ColumnExternalId].Received().SetCellValue(ViewResources.ExternalId);
                header.Cells[ColumnSystemId].Received().SetCellValue(ViewResources.SystemId);
                header.Cells[ColumnAmount].Received().SetCellValue(ViewResources.Amount);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var data = new ReportResultModel<PromotionPrize>();

            var excel = CreateExcel(data);

            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var data = new ReportResultModel<PromotionPrize>
            {
                DataList = FakeHelper.CreateMany<PromotionPrize>(1),
                TotalRows = 1,
                SummaryTotal = new Dictionary<string, PromotionPrize>()
            };

            var excel = CreateExcel(data);

            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);

            // Action
            excel.Public_AssignSheetData(FakeSheet);
            var firstItem = data.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnIndex].Received(1).SetCellValue(1);
                row.Cells[ColumnDate].Received(1).SetCellValue(firstItem.Date.ToString(FormatKey.DateTimeFormat));
                row.Cells[ColumnCurrency].Received(1).SetCellValue(firstItem.CurrencyName);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(firstItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(firstItem.SystemId);
                row.Cells[ColumnAmount].Received(1).SetCellValue(firstItem.Amount);
            });
        }

        [Test]
        public void AssignGrandTotal_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var sumKey1 = FakeHelper.Create<string>();
            var sumKey2 = FakeHelper.Create<string>();

            var data = new ReportResultModel<PromotionPrize>
            {
                DataList = FakeHelper.CreateMany<PromotionPrize>(1),
                TotalRows = 1,
                SummaryTotal = new Dictionary<string, PromotionPrize>
                {
                    { sumKey1, FakeHelper.Create<PromotionPrize>() },
                    { sumKey2, FakeHelper.Create<PromotionPrize>() }
                }
            };

            var excel = CreateExcel(data);

            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate);
            var sum1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate + 1);
            var sum2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, RowContentTemplate + 2);

            // Action
            excel.Public_AssignGrandTotal(FakeSheet, RowContentTemplate + 1);

            // Assert
            Assert.Multiple(() =>
            {
                sum1.Cells[ColumnAmount].SetCellValue(data.SummaryTotal[sumKey2].Amount);
                sum1.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + sumKey2.ToUpperInvariant());

                sum2.Cells[ColumnAmount].SetCellValue(data.SummaryTotal[sumKey1].Amount);
                sum2.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + sumKey1.ToUpperInvariant());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_AssignCorrectTitle()
        {
            // Arrange
            var data = new ReportResultModel<PromotionPrize>();
            var title = FakeHelper.Create<string>();

            var excel = CreateExcel(data, title);

            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            row.Cells[0].Received(1).SetCellValue(title);
        }

        private static PromotionPrizeExcelTestable CreateExcel(ReportResultModel<PromotionPrize> data, string title = "")
        {
            var excel = Substitute.ForPartsOf<PromotionPrizeExcelTestable>(
                string.Empty,
                title,
                data);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());

            return excel;
        }
    }
}