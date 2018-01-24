using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Customer.UnitTests.TestableClasses;
using Fanex.Athena.Extension.Helper;
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
        private const int GroupColumn = 1;
        private const int ExternalIDColumn = 2;
        private const int SystemIDColumn = 3;
        private const int BAStatusColumn = 7;
        private const int SBCommColumn = 10;
        private const int RCCommColumn = 11;
        private const int NGCommColumn = 12;
        private const int LCCommColumn = 13;
        private const int CSCommColumn = 14;

        private const int ABCommColumn = 15;

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
            var headerTexts = new List<string>
            {
                "#",
                ViewResources.Group,
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.AvailableFunds,
                ViewResources.PendingFunds,
                ViewResources.Status,
                ViewResources.BAStatus,
                ViewResources.SpecialLevel,
                ViewResources.OddsGroup,
                ViewResources.Commission + " (%)",
            };
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
                _sheet.Received().CopyRow(excel.TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
                for (int i = 0; i < headerTexts.Count; i++)
                {
                    row1.Cells[i].Received(1).SetCellValue(headerTexts[i]);
                }

                row2.Cells[SBCommColumn].Received(1).SetCellValue(ViewResources.SB);
                row2.Cells[RCCommColumn].Received(1).SetCellValue(ViewResources.RB);
                row2.Cells[NGCommColumn].Received(1).SetCellValue(ViewResources.NG);
                row2.Cells[LCCommColumn].Received(1).SetCellValue(ViewResources.LC);
                row2.Cells[CSCommColumn].Received(1).SetCellValue(ViewResources.CS);
                row2.Cells[ABCommColumn].Received(1).SetCellValue(ViewResources.ProductAB);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_SetGroupCommAndBAStatusColumns()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
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
                row.Cells[GroupColumn].Received(1).SetCellValue("GroupA");
                row.Cells[BAStatusColumn].Received(1).SetCellValue(ViewResources.Off);
            });
        }

        [Test]
        public void AssignCommonColumns_WhenCall_SetCellValueCorrectly()
        {
            // Arrange
            const int NumOfRow = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
            var excel = CreateExcel();
            var item = new CustomerDTO
            {
                ExternalId = "hello",
                SystemId = "hi"
            };

            // Action
            excel.Public_AssignCommonColumns(item, NumOfRow, row);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(NumOfRow);
                row.Cells[ExternalIDColumn].Received(1).SetCellValue(item.ExternalId);
                row.Cells[SystemIDColumn].Received(1).SetCellValue(item.SystemId);
            });
        }

        [Test]
        public void AssignCommissionColumns_WhenCall_SetCellValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
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
            excel.Public_AssignCommissionColumns(item, row);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[SBCommColumn].Received(1).SetCellValue(item.SBComm.ToStringExt(2));
                row.Cells[NGCommColumn].Received(1).SetCellValue(item.NGComm.ToStringExt(2));
                row.Cells[RCCommColumn].Received(1).SetCellValue(item.RBComm.ToStringExt(2));
                row.Cells[LCCommColumn].Received(1).SetCellValue(item.LCComm.ToStringExt(2));
                row.Cells[CSCommColumn].Received(1).SetCellValue(item.CSComm.ToStringExt(2));
                row.Cells[ABCommColumn].Received(1).SetCellValue(item.ABComm.ToStringExt(2));
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