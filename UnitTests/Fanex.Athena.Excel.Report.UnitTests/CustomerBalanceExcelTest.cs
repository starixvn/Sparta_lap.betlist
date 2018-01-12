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
    public class CustomerBalanceExcelTest
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
        private const int ColumnTemplateCount = 20;
        private const int ColumnTurnOver = 8;
        private const int ColumnTurnoverBa = 17;
        private const int ColumnWithdrawal = 5;

        private const int ColumnCount = 21;

        private static readonly CustBalance DataItem = new CustBalance
        {
            Index = 1,
            ExternalId = "2",
            SystemId = "3",
            OpeningBalance = 4,
            Deposit = 5,
            Withdrawal = 6,
            Bonus = 7,
            Adjustment = 8,
            Turnover = 9,
            DrawGame = 10,
            NetTurnover = 11,
            PendingFunds = 12,
            WinLoss = 13,
            PlayerComm = 14,
            ClosingBalance = 15,
            AffiliateTransfer = 16,
            TurnoverBA = 17,
            CommBA = 18,
            AmountDueToBA = 19
        };

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
            var row1 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate + 1, ReportBase.RowHeaderTemplate + 1);
                row1.Cells[0].Received(1).SetCellValue("#");
                row1.Cells[1].Received(1).SetCellValue(ViewResources.ExternalId);
                row1.Cells[2].Received(1).SetCellValue(ViewResources.SystemId);
                row1.Cells[3].Received(1).SetCellValue(ViewResources.OpeningBalance);
                row1.Cells[4].Received(1).SetCellValue(ViewResources.Deposit);
                row1.Cells[5].Received(1).SetCellValue(ViewResources.Withdrawal);
                row1.Cells[6].Received(1).SetCellValue(ViewResources.Bonus);
                row1.Cells[7].Received(1).SetCellValue(ViewResources.Adjustment);
                row1.Cells[8].Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[9].Received(1).SetCellValue(ViewResources.DrawGame);
                row1.Cells[10].Received(1).SetCellValue(ViewResources.NetTurnover);
                row1.Cells[11].Received(1).SetCellValue(ViewResources.PendingFunds);
                row1.Cells[12].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[13].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[14].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[15].Received(1).SetCellValue(ViewResources.ClosingBalance);
                row1.Cells[16].Received(1).SetCellValue(ViewResources.AffiliateTransfer);
                row1.Cells[17].Received(1).SetCellValue(ViewResources.ProductBA);

                row2.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);

                row2.Cells[ColumnTurnoverBa].Received(1).SetCellValue(ViewResources.Turnover);
                row2.Cells[ColumnCommBa].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnAmountDueToBa].Received(1).SetCellValue(ViewResources.AmtDueTo);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
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

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();
            var expectedTitle = string.Format(
                                        ViewResources.CustomerBalance + ", {0} - {1}, {2}",
                                        MetaModel.FromDate.ToString("MM/dd/yyyy"),
                                        MetaModel.ToDate.ToString("MM/dd/yyyy"),
                                        ReportExcelHelper.GetTitleCurrencyName(new ConvertingOption()));

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
                    expectedTitle);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionIsInvisible_HideRelevantColumns()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().HideColumn(14, 13);
                _sheet.Received().HideColumn(17, 18, 19);
                _sheet.Received().HideColumn(2);
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
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.DidNotReceiveWithAnyArgs().HideColumn(0);
            });
        }

        [Test]
        public void AssignTotalData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            const int RowTotal = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, RowTotal);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalData(_sheet, RowTotal);

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

        private static CustomerBalanceExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<CustomerBalanceExcelTestable>(
                string.Empty,
                MetaModel,
                new List<CustBalance> { DataItem },
                DataItem,
                showingOption ?? new ShowingOption(),
                new ConvertingOption());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}