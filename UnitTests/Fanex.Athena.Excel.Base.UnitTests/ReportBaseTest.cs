namespace Fanex.Athena.Excel.Base.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using ClassTest;
    using Entities;
    using Model.Core.MetaModel.Product;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ReportBaseTest
    {
        [Test]
        public void AssignData_ReportDoesNotHaveData_AssignNoInfoRowMethodIsCalled()
        {
            // Arrange
            var workbook = Substitute.For<IWorkbook>();
            var sheet = Substitute.For<ISheet>();

            string sourceSheetName = "Template";
            var sourceSheetIndex = 0;
            workbook.GetSheetIndex(sourceSheetName).Returns(sourceSheetIndex);
            workbook.CloneSheet(sourceSheetIndex).Returns(sheet);

            var report = Substitute.ForPartsOf<FakeReportBase>("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.Workbook = workbook;

            // Action
            report.AssignData();

            // Assert
            report.Received().AssignNoInfoRow(sheet);
        }

        [Test]
        public void AssignData_ReportHasData_AssignSheetDataMethodIsCalled()
        {
            // Arrange
            var workbook = Substitute.For<IWorkbook>();
            var sheet = Substitute.For<ISheet>();

            string sourceSheetName = "Template";
            var sourceSheetIndex = 0;
            workbook.GetSheetIndex(sourceSheetName).Returns(sourceSheetIndex);
            workbook.CloneSheet(sourceSheetIndex).Returns(sheet);

            var report = Substitute.ForPartsOf<FakeReportBase>("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.Workbook = workbook;

            report.ExportData = GetDataTable(new string[] { "Col1", "Col2" });

            // Action
            report.AssignData();

            // Assert
            report.Received().AssignSheetData(sheet);
        }

        [Test]
        public void Export_WhenCall_AssignDataMethodIsCalled()
        {
            // Arrange
            var report = Substitute.ForPartsOf<ReportBase>("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.When(x => x.AssignData()).DoNotCallBase();
            report.When(x => x.LoadTemplate()).DoNotCallBase();
            report.When(x => x.RemoveTemplate()).DoNotCallBase();

            // Action
            report.Export();

            // Assert
            report.Received().LoadTemplate();
            report.Received().AssignData();
            report.Received().RemoveTemplate();
        }

        [Test]
        public void AssignRowContent_WhenCall_SetCellValueMethodIsCalled()
        {
            // Arrange
            var columns = new string[] { "Row0", "Row1" };
            var dataRow = GetDataTable(columns).Rows[0];
            var excelRow = Substitute.For<IRow>();
            var cell = Substitute.For<ICell>();
            excelRow.Cells.Returns(new List<ICell>() { cell, cell });
            var report = new FakeReportBase("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);

            // Action
            report.AssignRowContent(excelRow, dataRow, columns, 0);

            // Assert
            cell.Received().SetCellValue("Data1");
        }

        [TestCase(0, "All Currencies")]
        [TestCase(-1, "View in EUR")]
        [TestCase(-2, "View in USD")]
        [TestCase(-3, "View in GBP")]
        public void GetExactTitleCurrencyName_CurrencyIdInRange_ReturnCorrectValue(int currencyId, string expected)
        {
            // Arrange
            var report = new FakeReportBase("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);

            // Action
            var result = report.GetExactTitleCurrencyName(currencyId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(-10, true, "ABC", "View in EUR")]
        [TestCase(10, true, "ABC", "View in EUR")]
        [TestCase(10, false, "ABC", "View in ABC")]
        public void GetExactTitleCurrencyName_CurrencyIdNotInRange_ReturnCorrectValue(int currencyId, bool convertOption, string currencyName, string expected)
        {
            // Arrange
            var convertingOption = new ConvertingOption { IsConvert = convertOption, BaseCurrency = "EUR", CurrencyName = currencyName };
            var report = new FakeReportBase("TestPath.xls", "ExportFileName", new ShowingOption(), convertingOption, 0);

            // Action
            var result = report.GetExactTitleCurrencyName(currencyId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("Caption", ", Caption (SB, CS)")]
        [TestCase(null, ", Licensee W/L (SB, CS)")]
        public void GetLicenseeWLProduct_ProductOptionListHasData_ReturnCorrectValue(string caption, string expected)
        {
            // Arrange
            var report = new FakeReportBase("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.ProductOptions = GetProductOption();

            // Action
            var result = report.GetLicenseeWLProduct(caption);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("Caption Test", ", Caption Test")]
        [TestCase(null, "")]
        public void GetLicenseeWLProduct_ProductOptionListDoesNotHaveData_ReturnCorrectValue(string caption, string expected)
        {
            // Arrange
            var report = new FakeReportBase("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.ProductOptions = new List<ProductOption>();

            // Action
            var result = report.GetLicenseeWLProduct(caption);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, 2)]
        [TestCase(2, 0)]
        public void SetTextRowValue_WhenCall_SetCellValueMethodIsCalled(int sourceRowIndex, int destRowIndex)
        {
            // Arrange
            var report = new FakeReportBase("TestPath.xls", "ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.TemplateSheet = Substitute.For<ISheet>();
            var sheet = Substitute.For<ISheet>();
            var row = Substitute.For<IRow>();

            sheet.GetRow(destRowIndex).Returns(row);

            var cell = Substitute.For<ICell>();
            row.Cells.Returns(new List<ICell>() { cell });

            // Action
            report.SetTextRowValue(sheet, sourceRowIndex, destRowIndex, 1, "Test");

            // Assert
            cell.Received().SetCellValue("Test");
        }

        private DataTable GetDataTable(string[] columnName)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(columnName[0]);
            dataTable.Columns.Add(columnName[1]);

            DataRow dr = dataTable.NewRow();
            dr[columnName[0]] = "Data1";
            dr[columnName[1]] = "Data2";
            dataTable.Rows.Add(dr);

            return dataTable;
        }

        private IList<ProductOption> GetProductOption()
        {
            return new List<ProductOption>()
            {
                new ProductOption() { Code = "SB", Name = "Sportbook", Status = true },
                new ProductOption() { Code = "CS", Name = "RNGCasino", Status = true },
                new ProductOption() { Code = "NG", Name = "Number Game", Status = false },
                new ProductOption() { Code = "BA", Name = "BA", Status = true }
            };
        }
    }
}