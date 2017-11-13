using System.Collections.Generic;
using System.Data;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Customer.UnitTests.TestableClasses;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Customer.UnitTests
{
    [TestFixture]
    public class CustomerCommGroupExcelTest
    {
        private ISheet _sheet;
        private CustomerCommGroupExcelTestable _customerCommGroupExcel;

        [SetUp]
        public void Setup()
        {
            _sheet = Substitute.For<ISheet>();
            _customerCommGroupExcel = Substitute.ForPartsOf<CustomerCommGroupExcelTestable>(string.Empty, new DataTable(), new ShowingOption());
            _customerCommGroupExcel.TemplateSheet.Returns(Substitute.For<ISheet>());
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int HeaderLength = 10;
            var row = Substitute.For<IRow>();
            var cell = Substitute.For<ICell>();
            var cellList = new List<ICell>();
            for (int i = 0; i < HeaderLength; i++)
            {
                cellList.Add(cell);
            }

            row.Cells.ReturnsForAnyArgs(cellList);
            _sheet.GetRow(0).ReturnsForAnyArgs(row);

            // Action
            _customerCommGroupExcel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(_customerCommGroupExcel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                cell.ReceivedWithAnyArgs(10).SetCellValue(string.Empty);
            });
        }
    }
}