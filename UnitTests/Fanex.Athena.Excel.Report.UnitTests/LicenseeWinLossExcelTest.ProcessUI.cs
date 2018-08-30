using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public partial class LicenseeWinLossExcelTest : BaseReportExcelTest
    {
        [Test]
        public void ProcessUI_AllOptionsIsInvisible_HideColumnsCorrectly()
        {
            // Arrange
            ExcelModel.CustomerCommVisible = false;
            ExcelModel.COVisible = false;
            ExcelModel.BTVisible = false;
            ExcelModel.BAVisible = false;
            ExcelModel.RBVisible = false;
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.ProcessUITurnover(Arg.Any<ISheet>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.ProcessUILicenseeWinLoss(Arg.Any<ISheet>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.HideColumnsByLevel(Arg.Any<ISheet>())).DoNotCallBase();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(
                FakeSheet,
                ColumnCustomerTotal,
                ColumnCustomerComm,
                ColumnCashOutCommission,
                ColumnBuyBackAmountTotal,
                ColumnBuyBackAmountBA,
                ColumnBetTradeCommission,
                ColumnBuyBackAmountSportsbook,
                ColumnBuyBackAmountBA,
                ColumnBuyBackAmountTotal,
                ColumnActualStake,
                ColumnAmtDueToBetTrade,
                ColumnBaCommission,
                ColumnAmtDueToBa,
                ColumnRacingRevenue,
                ColumnAmtDueToRacing);
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_HideColumnsCorrectly()
        {
            // Arrange
            ExcelModel.SBVisible = true;
            ExcelModel.CustomerCommVisible = true;
            ExcelModel.COVisible = true;
            ExcelModel.BTVisible = true;
            ExcelModel.BAVisible = true;
            ExcelModel.RBVisible = true;
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.ProcessUITurnover(Arg.Any<ISheet>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.ProcessUILicenseeWinLoss(Arg.Any<ISheet>())).DoNotCallBase();
            excel.WhenForAnyArgs(x => x.HideColumnsByLevel(Arg.Any<ISheet>())).DoNotCallBase();

            // Action
            excel.Public_ProcessUI(FakeSheet);

            // Assert
            AssertHideColumns(FakeSheet, ColumnBuyBackAmountTotal, ColumnBuyBackAmountBA);
        }
    }
}