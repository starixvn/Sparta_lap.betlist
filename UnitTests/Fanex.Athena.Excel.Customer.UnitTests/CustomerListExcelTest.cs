using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Customer.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Customer.DTO;
using Fanex.Athena.Model.Customer.MetaModel;
using Fanex.Athena.Model.Customer.ViewModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Customer.UnitTests
{
    [TestFixture]
    public class CustomerListExcelTest
    {
        private const string ReportTitle = "Customer List 123456";
        private const int ColumnCount = 17;
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
            const int RowHeaderTemplate = 1;
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
                _sheet.Received().CopyRow(excel.TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
                cell.ReceivedWithAnyArgs(ColumnCount).SetCellValue(string.Empty);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_SetGroupCommAndBAStatusColumns()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var viewModel = new CustomerListViewModel
            {
                IsAllowedCustomerCommission = true,
                DataList = new List<CustomerDTO>
                {
                    new CustomerDTO { GroupCommId = 1, Group = "GroupA" }
                },
                Param = new CustomerListMetaModel
                {
                    GridColumns = new List<CheckedItem>
                    {
                        new CheckedItem { Name = "Group", Checked = true },
                        new CheckedItem { Name = "BAStatus", Checked = true }
                    }
                },
                ProductAccess = new ProductAccess(CreateProductList())
            };
            var excel = CreateExcel(viewModel);

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue("GroupA");
                cell.Received(1).SetCellValue(ViewResources.Off);
            });
        }

        [Test]
        public void AssignCommonColumns_WhenCall_SetCellValueCorrectly()
        {
            // Arrange
            const int NumOfRow = 100;
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var item = new CustomerDTO
            {
                ExternalId = "hello",
                SystemId = "hi"
            };

            // Action
            excel.Public_AssignCommonColumns(item, NumOfRow, cell.Row);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(NumOfRow);
                cell.Received(1).SetCellValue(item.ExternalId);
                cell.Received(1).SetCellValue(item.SystemId);
            });
        }

        [Test]
        public void AssignCommissionColumns_WhenCall_SetCellValueCorrectly()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var item = new CustomerDTO
            {
                SBComm = 1,
                ABComm = 2,
                NGComm = 3,
                RBComm = 4,
                LCComm = 5,
                CSComm = 6
            };

            // Action
            excel.Public_AssignCommissionColumns(item, cell.Row);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue("1.00");
                cell.Received(1).SetCellValue("2.00");
                cell.Received(1).SetCellValue("3.00");
                cell.Received(1).SetCellValue("4.00");
                cell.Received(1).SetCellValue("5.00");
                cell.Received(1).SetCellValue("6.00");
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().SetTextRowValue(
                    excel.TemplateSheet,
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    ReportTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void ProcessUI_WhenCall_CallHideColumnsCorrectly()
        {
            // Arrange
            var commColumns = new[] { 10, 11, 12, 13, 14, 15 };
            var otherColumns = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var viewModel = new CustomerListViewModel
            {
                DataList = new List<CustomerDTO>(),
                Param = new CustomerListMetaModel
                {
                    GridColumns = new List<CheckedItem>()
                },
                ProductAccess = new ProductAccess(new List<Product>())
            };
            var excel = CreateExcel(viewModel);

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().HideColumn(commColumns);
                _sheet.Received().HideColumn(otherColumns);
            });
        }

        private static CustomerListExcelTestable CreateExcel(CustomerListViewModel viewModel = null)
        {
            var model = viewModel ?? CreateViewModel();
            var excel = Substitute.ForPartsOf<CustomerListExcelTestable>(
                string.Empty,
                ReportTitle,
                model);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }

        private static CustomerListViewModel CreateViewModel()
        {
            return new CustomerListViewModel
            {
                IsAllowedCustomerCommission = true,
                DataList = new List<CustomerDTO>(),
                Param = new CustomerListMetaModel
                {
                    GridColumns = new List<CheckedItem>
                    {
                        new CheckedItem { Name = "Commission", Checked = true },
                        new CheckedItem { Name = "ExternalId", Checked = true },
                        new CheckedItem { Name = "SystemId", Checked = true }
                    }
                },
                ProductAccess = new ProductAccess(CreateProductList())
            };
        }

        private static IList<Product> CreateProductList()
        {
            var productCodes = new[] { "SB", "BA", "NG", "RB", "LC", "CS", "AB" };
            return productCodes.Select(x => new Product
            {
                ProductCode = x
            }).ToList();
        }
    }
}