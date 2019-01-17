using System;
using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.Helpers;
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
    public class CustomerBalanceExcelTest : BaseReportExcelTest
    {
        private const int ColumnAdjustment = 7;
        private const int ColumnAffiliateTransfer = 16;
        private const int ColumnAmountDueToBa = 19;
        private const int ColumnBonus = 6;
        private const int ColumnClosingBalance = 15;
        private const int ColumnCommBa = 18;
        private const int ColumnCustomerComm = 13;
        private const int ColumnCustomerTotal = 14;
        private const int ColumnCustomerWinLoss = 12;
        private const int ColumnDeposit = 4;
        private const int ColumnDrawGame = 9;
        private const int ColumnExternalId = 1;
        private const int ColumnNetTurnOver = 10;
        private const int ColumnOpeningBalance = 3;
        private const int ColumnPendingFunds = 11;
        private const int ColumnSystemId = 2;
        private const int ColumnTurnOver = 8;
        private const int ColumnTurnoverBa = 17;
        private const int ColumnWithdrawal = 5;

        private const int ColumnCount = 21;

        private static readonly CustBalance DataItem = FakeHelper.Create<CustBalance>();

        private static readonly CustomerBalanceMetaModel MetaModel = new CustomerBalanceMetaModel
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

        [Test]
        public void AssignData_NoData_SetNoinfoText()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount);
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel(hasData: false);
            excel.Workbook.Returns(Substitute.For<IWorkbook>());
            excel.Workbook.CloneSheet(0).Returns(FakeSheet);

            // Action
            excel.Public_AssignData();

            // Assert
            Assert.Multiple(() =>
            {
                excel.Workbook.Received(1).SetSheetName(0, "Data");
                row.Cells[0].Received(1).SetCellValue(ViewResources.NoInfo);
            });
        }

        [Test]
        public void AssignData_HasData_SetNoinfoText()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount);
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.Workbook.Returns(Substitute.For<IWorkbook>());
            excel.Workbook.CloneSheet(0).Returns(FakeSheet);

            // Action
            excel.Public_AssignData();

            // Assert
            Assert.Multiple(() =>
            {
                excel.Workbook.Received(1).SetSheetName(0, "Customer Balance1");
                AssertAssignSheetData(row);
            });
        }

        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();
            var headerTextList = new[]
            {
                "#", ViewResources.ExternalId, ViewResources.SystemId, ViewResources.OpeningBalance, ViewResources.Deposit,
                ViewResources.Withdrawal, ViewResources.Bonus, ViewResources.Adjustment, ViewResources.Turnover,
                ViewResources.DrawGame, ViewResources.NetTurnover, ViewResources.PendingFunds, ViewResources.CustomerW_L,
                ViewResources.CustomerW_L, ViewResources.CustomerW_L, ViewResources.ClosingBalance, ViewResources.AffiliateTransfer,
                ViewResources.ProductBA
            };

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertSetTextRowHeader(row1, headerTextList);

                row2.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);

                row2.Cells[ColumnTurnoverBa].Received(1).SetCellValue(ViewResources.Turnover);
                row2.Cells[ColumnCommBa].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnAmountDueToBa].Received(1).SetCellValue(ViewResources.AmountDueTo);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 3);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            AssertAssignSheetData(row);
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();
            var expectedTitle = string.Format(
                                        ViewResources.CustomerBalance + ", {0} - {1}, {2}",
                                        MetaModel.FromDate.ToString("MM/dd/yyyy"),
                                        MetaModel.ToDate.ToString("MM/dd/yyyy"),
                                        ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionIsInvisible_HideRelevantColumns()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                AssertHideColumns(FakeSheet, ColumnCustomerTotal, ColumnCustomerComm);
                AssertHideColumns(FakeSheet, ColumnTurnoverBa, ColumnCommBa, ColumnAmountDueToBa);
            });
        }

        [Test]
        public void ProcessUI_AllOptionIsVisible_NotHideColumns()
        {
            // Arrange
            var excel = CreateExcel(new ShowingOption
            {
                CustomerCommVisible = true,
                BAVisible = true
            });

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, false);
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            const int RowTotal = 100;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowTotal);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(FakeSheet, RowTotal);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnOpeningBalance].Received(1).SetCellValue(DataItem.OpeningBalance);
                row.Cells[ColumnDeposit].Received(1).SetCellValue(DataItem.Deposit);
                row.Cells[ColumnWithdrawal].Received(1).SetCellValue(DataItem.Withdrawal);
                row.Cells[ColumnBonus].Received(1).SetCellValue(DataItem.Bonus);
                row.Cells[ColumnAdjustment].Received(1).SetCellValue(DataItem.Adjustment);
                row.Cells[ColumnTurnOver].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnDrawGame].Received(1).SetCellValue(DataItem.DrawGame);
                row.Cells[ColumnNetTurnOver].Received(1).SetCellValue(DataItem.NetTurnover);
                row.Cells[ColumnPendingFunds].Received(1).SetCellValue(DataItem.PendingFunds);
                row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(DataItem.WinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.PlayerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.WinLoss + DataItem.PlayerComm);
                row.Cells[ColumnClosingBalance].Received(1).SetCellValue(DataItem.ClosingBalance);
                row.Cells[ColumnAffiliateTransfer].Received(1).SetCellValue(DataItem.AffiliateTransfer);
                row.Cells[ColumnTurnoverBa].Received(1).SetCellValue(DataItem.TurnoverBA);
                row.Cells[ColumnCommBa].Received(1).SetCellValue(DataItem.CommBA);
                row.Cells[ColumnAmountDueToBa].Received(1).SetCellValue(DataItem.AmountDueToBA);
            });
        }

        private static void AssertAssignSheetData(IRow row)
        {
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnOpeningBalance].Received(1).SetCellValue(DataItem.OpeningBalance);
                row.Cells[ColumnDeposit].Received(1).SetCellValue(DataItem.Deposit);
                row.Cells[ColumnWithdrawal].Received(1).SetCellValue(DataItem.Withdrawal);
                row.Cells[ColumnBonus].Received(1).SetCellValue(DataItem.Bonus);
                row.Cells[ColumnAdjustment].Received(1).SetCellValue(DataItem.Adjustment);
                row.Cells[ColumnTurnOver].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnDrawGame].Received(1).SetCellValue(DataItem.DrawGame);
                row.Cells[ColumnNetTurnOver].Received(1).SetCellValue(DataItem.NetTurnover);
                row.Cells[ColumnPendingFunds].Received(1).SetCellValue(DataItem.PendingFunds);
                row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(DataItem.WinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.PlayerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.WinLoss + DataItem.PlayerComm);
                row.Cells[ColumnClosingBalance].Received(1).SetCellValue(DataItem.ClosingBalance);
                row.Cells[ColumnAffiliateTransfer].Received(1).SetCellValue(DataItem.AffiliateTransfer);
                row.Cells[ColumnTurnoverBa].Received(1).SetCellValue(DataItem.TurnoverBA);
                row.Cells[ColumnCommBa].Received(1).SetCellValue(DataItem.CommBA);
                row.Cells[ColumnAmountDueToBa].Received(1).SetCellValue(DataItem.AmountDueToBA);
            });
        }

        private static CustomerBalanceExcelTestable CreateExcel(ShowingOption showingOption = null, bool hasData = true)
        {
            var excel = Substitute.ForPartsOf<CustomerBalanceExcelTestable>(
                string.Empty,
                MetaModel,
                hasData ? new List<CustBalance> { DataItem } : new List<CustBalance>(),
                DataItem,
                showingOption ?? new ShowingOption(),
                new ConvertingOption());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}