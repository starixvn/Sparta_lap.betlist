using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.MetaModel.Product;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class ActiveCustomerSummaryExcelTest : BaseReportExcelTest
    {
        private const int ColumnTotalTransaction = 18;
        private const int ColumnCount = 19;

        private static readonly ActiveCustomer DataItem = new ActiveCustomer
        {
            TransactionDate = new DateTime(2017, 1, 1),
            TotalCustomer = 100,
            CustomerCount = new Dictionary<string, int>
                        {
                            { "SB", 200 }
                        }
        };

        private static readonly ActiveCustomerMetaModel MetaModel = new ActiveCustomerMetaModel
        {
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2),
            ProductOptions = new List<ProductOption>
                {
                    new ProductOption
                    {
                        Code = "SB",
                        Status = true
                    }
                }
        };

        private static readonly ActiveCustomerViewModel ViewModel = new ActiveCustomerViewModel
        {
            TotalCustomer = 30000,
            MainResult = new List<ActiveCustomer> { DataItem }
        };

        [Test]
        public void ProcessUI_AllProductsInvisible_CallEnoughHideColumnMethod()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, 2);
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var offeredProducts = new[]
            {
                Product.SB, Product.RB, Product.NG, Product.BA, Product.VS, Product.CS, Product.KN, Product.PTCS,
                Product.AGC, Product.MGCLC, Product.CB, Product.MGCRNG, Product.AB, Product.VB, Product.FW, Product.VS2, Product.LT
            };
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(true, Is.True);
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row1.Cells[0].Received(1).SetCellValue(ViewResources.Month);
                row1.Cells[1].Received(1).SetCellValue(ViewResources.Product);
                row1.Cells[ColumnTotalTransaction].Received(1).SetCellValue(ViewResources.Total);

                for (int i = 0; i < offeredProducts.Length; i++)
                {
                    row2.Cells[i + 1].Received(1).SetCellValue(ViewResources.ResourceManager.GetString($"Product{offeredProducts[i]}"));
                }
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var totalRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 4);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(DataItem.TransactionDate.ToString("MM/yyyy"));
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalCustomer);
                row.Cells[1].Received(1).SetCellValue(DataItem.CustomerCount[MetaModel.ProductOptions[0].Code]);
                totalRow.Cells[0].Received(1).SetCellValue(ViewResources.TotalActiveCustomer + " = " + ViewModel.TotalCustomer.ToString("#,##0"));
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            FakeSheet.LastRowNum.Returns(2);
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = ReportExcelHelper.GenerateReportTitle(
                ViewResources.ActiveCustomerSummary,
                new ConvertingOption(),
                MetaModel.FromDate,
                MetaModel.ToDate,
                MetaModel.CurrencyId);

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        private static ActiveCustomerSummaryExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<ActiveCustomerSummaryExcelTestable>(
                string.Empty,
                new ConvertingOption(),
                new ShowingOption(),
                MetaModel,
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}