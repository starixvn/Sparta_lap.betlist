namespace Fanex.Athena.Excel.Base.UnitTests.Helpers
{
    using System.Collections.Generic;
    using System.Data;
    using Base.Helpers;
    using Model.Core.MetaModel.Product;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report.Helpers;

    [TestFixture]
    public class ExcelHelperTest
    {
        [Test]
        public void AssignRowContent_WhenCall_SetCellValueMethodIsCalled()
        {
            // Arrange
            var columns = new string[] { "Row0", "Row1" };
            var dataRow = GetDataTable(columns).Rows[0];
            var excelRow = Substitute.For<IRow>();
            var cell = Substitute.For<ICell>();
            excelRow.Cells.Returns(new List<ICell>() { cell, cell });

            // Action
            ExcelHelper.AssignRowContent(excelRow, dataRow, columns, 0);

            // Assert
            cell.Received().SetCellValue("Data1");
        }

        [TestCase("Caption", ", Caption (SB, CS, BA)")]
        [TestCase(null, ", Licensee W/L (SB, CS, BA)")]
        public void GetLicenseeWLProduct_ProductOptionListHasData_ReturnCorrectValue(string caption, string expected)
        {
            // Action
            var result = ReportExcelHelper.GetLicenseeWLProduct(GetProductOption(), caption);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("Caption Test", ", Caption Test")]
        [TestCase(null, "")]
        public void GetLicenseeWLProduct_ProductOptionListDoesNotHaveData_ReturnCorrectValue(string caption, string expected)
        {
            // Action
            var result = ReportExcelHelper.GetLicenseeWLProduct(new List<ProductOption>(), caption);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
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