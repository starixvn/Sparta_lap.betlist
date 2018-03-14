using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.UnitTest.Common.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public partial class LicenseeWinLossExcelTest : BaseReportExcelTest
    {
        [Test]
        public void AssignCommonData_WhenCall_AssignValueToTurnoverAndLicenseeWLColumns()
        {
            // Arrange
            var item = new LicenseeWinLoss
            {
                TurnOver = new Dictionary<string, decimal>
                {
                    { Product.SB, 1 }, { Product.RB, 2 }, { Product.BA, 3 }
                },
                LicenseeWL = new Dictionary<string, decimal>
                {
                    { Product.SB, 4 }, { Product.BT, 5 }
                }
            };

            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel(productCodes: new[] { Product.SB, Product.BT, Product.RB, Product.BA });

            // Action
            excel.AssignCommonData(row, item);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnTurnoverSportBook].Received(1).SetCellValue(item.TurnOver[Product.SB]);
                row.Cells[ColumnTurnoverSportBook + 1].DidNotReceiveWithAnyArgs().SetCellValue(0);
                row.Cells[ColumnTurnoverSportBook + 2].Received(1).SetCellValue(item.TurnOver[Product.RB]);
                row.Cells[ColumnTurnoverSportBook + 3].Received(1).SetCellValue(item.TurnOver[Product.BA]);

                row.Cells[ColumnLicenseeWlSportsBook].Received(1).SetCellValue(item.LicenseeWL[Product.SB]);
                row.Cells[ColumnLicenseeWlSportsBook + 1].Received(1).SetCellValue(item.LicenseeWL[Product.BT]);
                row.Cells[ColumnLicenseeWlSportsBook + 2].DidNotReceiveWithAnyArgs().SetCellValue(0);
                row.Cells[ColumnLicenseeWlSportsBook + 3].DidNotReceiveWithAnyArgs().SetCellValue(0);
            });
        }

        [Test]
        public void AssignCommonData_WhenCall_SetValueToColumns()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, 0);
            var excel = CreateExcel();

            // Action
            excel.AssignCommonData(row, DataItem);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnTurnoverTotal].Received(1).SetCellValue(DataItem.TurnOver.Sum(a => a.Value));

                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransaction);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerTotal);

                row.Cells[ColumnActualStake].Received(1).SetCellValue(DataItem.ActualStake);

                row.Cells[ColumnBuyBackAmountSportsbook].Received(1).SetCellValue(DataItem.BuyBackAmountSB);
                row.Cells[ColumnBuyBackAmountBA].Received(1).SetCellValue(DataItem.BuyBackAmountBA);
                row.Cells[ColumnBuyBackAmountTotal].Received(1).SetCellValue(DataItem.TotalBuyBackAmount);

                row.Cells[ColumnLicenseeWlTotal].Received(1).SetCellValue(DataItem.LicenseeWL.Sum(a => a.Value));
                row.Cells[ColumnCashOutCommission].Received(1).SetCellValue(DataItem.CashOutCommission);
                row.Cells[ColumnBetTradeCommission].Received(1).SetCellValue(DataItem.BetTradeCommission);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(DataItem.BACommission);
                row.Cells[ColumnRacingRevenue].Received(1).SetCellValue(DataItem.RacingRevenue);
                row.Cells[ColumnAmtDueToBetTrade].Received(1).SetCellValue(DataItem.AmtDueToBetTrade);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(DataItem.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].Received(1).SetCellValue(DataItem.AmtDueToRacing);
            });
        }
    }
}