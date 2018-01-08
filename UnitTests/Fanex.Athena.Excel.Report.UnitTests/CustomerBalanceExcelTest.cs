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
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignHeader(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                _sheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate + 1, ReportBase.RowHeaderTemplate + 1);
                cell.Received(1).SetCellValue("#");
                cell.Received(1).SetCellValue(ViewResources.ExternalId);
                cell.Received(1).SetCellValue(ViewResources.SystemId);
                cell.Received(1).SetCellValue(ViewResources.OpeningBalance);
                cell.Received(1).SetCellValue(ViewResources.Deposit);
                cell.Received(1).SetCellValue(ViewResources.Withdrawal);
                cell.Received(1).SetCellValue(ViewResources.Bonus);
                cell.Received(1).SetCellValue(ViewResources.Adjustment);
                cell.Received(2).SetCellValue(ViewResources.Turnover);
                cell.Received(1).SetCellValue(ViewResources.DrawGame);
                cell.Received(1).SetCellValue(ViewResources.NetTurnover);
                cell.Received(1).SetCellValue(ViewResources.PendingFunds);
                cell.Received(3).SetCellValue(ViewResources.CustomerW_L);
                cell.Received(1).SetCellValue(ViewResources.ClosingBalance);
                cell.Received(1).SetCellValue(ViewResources.AffiliateTransfer);
                cell.Received(1).SetCellValue(ViewResources.ProductBA);

                cell.Received(1).SetCellValue(ViewResources.WinLoss);
                cell.Received(2).SetCellValue(ViewResources.Comm);
                cell.Received(1).SetCellValue(ViewResources.Total);
                cell.Received(1).SetCellValue(ViewResources.AmtDueTo);
            });
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var cell = ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                cell.Received(1).SetCellValue(DataItem.Index);
                cell.Received(1).SetCellValue(DataItem.ExternalId);
                cell.Received(1).SetCellValue(DataItem.SystemId);
                cell.Received(1).SetCellValue(DataItem.OpeningBalance);
                cell.Received(1).SetCellValue(DataItem.Deposit);
                cell.Received(1).SetCellValue(DataItem.Withdrawal);
                cell.Received(1).SetCellValue(DataItem.Bonus);
                cell.Received(1).SetCellValue(DataItem.Adjustment);
                cell.Received(1).SetCellValue(DataItem.Turnover);
                cell.Received(1).SetCellValue(DataItem.DrawGame);
                cell.Received(1).SetCellValue(DataItem.NetTurnover);
                cell.Received(1).SetCellValue(DataItem.PendingFunds);
                cell.Received(1).SetCellValue(DataItem.WinLoss);
                cell.Received(1).SetCellValue(DataItem.PlayerComm);
                cell.Received(1).SetCellValue(DataItem.WinLoss + DataItem.PlayerComm);
                cell.Received(1).SetCellValue(DataItem.ClosingBalance);
                cell.Received(1).SetCellValue(DataItem.AffiliateTransfer);
                cell.Received(1).SetCellValue(DataItem.TurnoverBA);
                cell.Received(1).SetCellValue(DataItem.CommBA);
                cell.Received(1).SetCellValue(DataItem.AmountDueToBA);
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