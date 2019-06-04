using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
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
    public class TurnoverExcelTest : BaseReportExcelTest
    {
        private const int ColumnDay = 1;
        private const int ColumnOpeningBalance = 2;
        private const int ColumnDeposit = 3;
        private const int ColumnWithdrawal = 4;
        private const int ColumnBonus = 5;
        private const int ColumnAdjustment = 6;
        private const int ColumnTurnOver = 7;
        private const int ColumnDrawGame = 8;
        private const int ColumnNetTurnOver = 9;
        private const int ColumnPendingFunds = 10;
        private const int ColumnCustomerWinLoss = 11;
        private const int ColumnCustomerComm = 12;
        private const int ColumnCustomerTotal = 13;
        private const int ColumnClosingBalance = 14;
        private const int ColumnAffiliateTransfer = 15;
        private const int ColumnCasinoTransfer = 16;
        private const int ColumnTurnoverBa = 17;
        private const int ColumnCommBa = 18;
        private const int ColumnAmountDueToBa = 19;

        private const int ColumnCount = 20;

        private static readonly TurnoverDTO DataItem = FakeHelper.Create<TurnoverDTO>();

        private static readonly TurnoverViewModel ViewModel = new TurnoverViewModel
        {
            Param = new TurnoverMetaModel
            {
                FromDay = 1,
                ToDay = 2,
                Month = 2,
                Year = 2018,
                CurrencyId = 1,
                CurrencyName = "UUS",
                BaseCurrency = "USD"
            },
            DataList = new List<TurnoverDTO> { DataItem },
            TotalData = FakeHelper.Create<TurnoverDTO>()
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
                "#",
                ViewResources.Date,
                ViewResources.OpeningBalance,
                ViewResources.Deposit,
                ViewResources.Withdrawal,
                ViewResources.Bonus,
                ViewResources.Adjustment,
                ViewResources.Turnover,
                ViewResources.DrawGame,
                ViewResources.NetTurnover,
                ViewResources.PendingFunds,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.ClosingBalance,
                ViewResources.AffiliateTransfer,
                ViewResources.CasinoTransfer,
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
            excel.WhenForAnyArgs(x => x.AssignTotalCellData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(FakeSheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnDay].Received(1).SetCellValue(DataItem.Date);
                row.Cells[ColumnOpeningBalance].Received(1).SetCellValue(DataItem.OpeningBalance);
                row.Cells[ColumnClosingBalance].Received(1).SetCellValue(DataItem.ClosingBalance);

                AssertAssignCommonData(row, DataItem);

                excel.Received(1).AssignTotalCellData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [TestCase("", "Turnover, 02/01/2018 --> 02/02/2018, Currency: UUS, Base Currency: USD")]
        [TestCase("ABC", "Turnover, 02/01/2018 --> 02/02/2018 - Customer ABC (USD)")]
        public void AssignTitle_WhenCall_CallRelevantMethods(string userName, string expectedTitle)
        {
            // Arrange
            ViewModel.Param.Username = userName;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, 0);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignTitle(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(expectedTitle);
                FakeSheet.Received(1).CreateFreezePane(0, 3);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumn()
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
                AssertHideColumns(FakeSheet, ColumnCasinoTransfer);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_NotHideColumn()
        {
            // Arrange
            var excel = CreateExcel(new ShowingOption
            {
                CustomerCommVisible = true,
                BAVisible = true
            });
            excel.IsShowCasinoTransfer = true;

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.DidNotReceiveWithAnyArgs().SetColumnHidden(0, true);
            });
        }

        [Test]
        public void AssignTotalCellData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int RowIndex = 10;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, RowIndex);
            var excel = CreateExcel();

            // Action
            excel.AssignTotalCellData(FakeSheet, RowIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue("Total");
                row.Cells[ColumnOpeningBalance].Received(1).SetCellValue("-");
                row.Cells[ColumnClosingBalance].Received(1).SetCellValue("-");

                AssertAssignCommonData(row, ViewModel.TotalData);
            });
        }

        private static TurnoverExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<TurnoverExcelTestable>(
                string.Empty,
                showingOption ?? new ShowingOption(),
                ViewModel);
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }

        private static void AssertAssignCommonData(IRow row, TurnoverDTO item)
        {
            row.Cells[ColumnDeposit].Received(1).SetCellValue(item.Deposit);
            row.Cells[ColumnWithdrawal].Received(1).SetCellValue(item.Withdrawal);
            row.Cells[ColumnBonus].Received(1).SetCellValue(item.Bonus);
            row.Cells[ColumnAdjustment].Received(1).SetCellValue(item.Adjustment);
            row.Cells[ColumnTurnOver].Received(1).SetCellValue(item.Turnover);
            row.Cells[ColumnDrawGame].Received(1).SetCellValue(item.DrawGame);
            row.Cells[ColumnNetTurnOver].Received(1).SetCellValue(item.NetTurnover);
            row.Cells[ColumnPendingFunds].Received(1).SetCellValue(item.PendingFunds);
            row.Cells[ColumnCustomerWinLoss].Received(1).SetCellValue(item.WinLoss);
            row.Cells[ColumnCustomerComm].Received(1).SetCellValue(item.PlayerComm);
            row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(item.WinLoss + item.PlayerComm);
            row.Cells[ColumnAffiliateTransfer].Received(1).SetCellValue(item.AffiliateTransfer);
            row.Cells[ColumnCasinoTransfer].Received(1).SetCellValue(item.Transfer3rd);
            row.Cells[ColumnTurnoverBa].Received(1).SetCellValue(item.TurnoverBA);
            row.Cells[ColumnCommBa].Received(1).SetCellValue(item.CommBA);
            row.Cells[ColumnAmountDueToBa].Received(1).SetCellValue(item.AmountDueToBA);
        }
    }
}