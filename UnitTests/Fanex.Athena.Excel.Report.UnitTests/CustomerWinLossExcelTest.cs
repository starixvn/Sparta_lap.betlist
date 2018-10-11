using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
using Fanex.Athena.Excel.Report.Models;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.MetaModel.Product;
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
    public class CustomerWinLossExcelTest : BaseReportExcelTest
    {
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnTurnover = 3;
        private const int ColumnTotalTransaction = 4;
        private const int ColumnCustomerWl = 5;
        private const int ColumnCustomerComm = 6;
        private const int ColumnCustomerTotal = 7;
        private const int ColumnBetEffective = 8;
        private const int ColumnLicenseeWl = 9;
        private const int ColumnBaCommission = 10;
        private const int ColumnAmtDueToBa = 11;

        private const string FormatDate = "MM/dd/yyyy";

        private const int ColumnCount = 12;

        private static readonly CustomerWinLoss DataItem = FakeHelper.Create<CustomerWinLoss>();

        private static readonly CustomerWinLossExcelModel ExcelModel = new CustomerWinLossExcelModel
        {
            DataList = new List<CustomerWinLoss> { DataItem },
            TotalData = FakeHelper.Create<CustomerWinLoss>(),
            ConvertingOption = new ConvertingOption(),
            MetaModel = new CustomerWinLossMetaModel
            {
                Username = "Abc",
                ProductOptions = new List<ProductOption>()
            }
        };

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#", ViewResources.ExternalId, ViewResources.SystemId, ViewResources.Turnover,
                ViewResources.TotalTransaction, ViewResources.CustomerW_L, ViewResources.CustomerW_L,
                ViewResources.CustomerW_L, ViewResources.EffectiveBettingValue, ViewResources.LicenseeW_L,
                ViewResources.BACommission, ViewResources.AmtDueToBA
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
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(FakeSheet);

            // Assert
            FakeSheet.Received().SetTextRowValue(excel.TemplateSheet, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                AssertAssignCommonData(row, DataItem);

                excel.Received(1).AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var filterTitle = ReportExcelHelper.GetTitleCurrencyName(ExcelModel.ConvertingOption)
                            + ReportExcelHelper.GetLicenseeWLProduct(ExcelModel.MetaModel.ProductOptions)
                            + ", " + ViewResources.Username + ": " + ExcelModel.MetaModel.Username;

            var reportTitle = string.Format(
                                        ViewResources.CustomerWinLossTitle,
                                        ExcelModel.MetaModel.FromDate.ToString(FormatDate),
                                        ExcelModel.MetaModel.ToDate.ToString(FormatDate),
                                        filterTitle);

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnSystemId, ColumnCustomerTotal, ColumnCustomerComm, ColumnBaCommission, ColumnAmtDueToBa);
        }

        [Test]
        public void AssignSumData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int StartIndex = 100;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, StartIndex + 3);
            var excel = CreateExcel();

            // Action
            excel.AssignSumData(FakeSheet, StartIndex);

            // Assert
            Assert.Multiple(() =>
            {
                AssertAssignCommonData(row, ExcelModel.TotalData);

                FakeSheet.MergeCells(StartIndex, 0, StartIndex, 2);
            });
        }

        private static CustomerWinLossExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<CustomerWinLossExcelTestable>(
                string.Empty,
                ExcelModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }

        private static void AssertAssignCommonData(IRow row, CustomerWinLoss item)
        {
            row.Cells[ColumnTurnover].Received(1).SetCellValue(item.Turnover);
            row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(item.TotalTransaction);
            row.Cells[ColumnCustomerWl].Received(1).SetCellValue(item.CustomerWL);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(item.PlayerComm);
            row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(item.CustomerWL + item.PlayerComm);
            row.Cells[ColumnBetEffective].Received(1).SetCellValue(item.BetEffective);
            row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(item.LicenseeWL);
            row.Cells[ColumnBaCommission].Received(1).SetCellValue(item.BAComm);
            row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(item.AmtDueToBA);
        }
    }
}