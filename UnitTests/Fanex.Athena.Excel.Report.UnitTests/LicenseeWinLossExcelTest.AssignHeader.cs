using System.Linq;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public partial class LicenseeWinLossExcelTest : BaseReportExcelTest
    {
        [Test]
        public void AssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var productCodes = new[] { Product.SB, Product.NG };
            var row1 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate);
            var row2 = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, ReportBase.RowHeaderTemplate + 1);
            var excel = CreateExcel(productCodes: productCodes);

            // Action
            excel.Public_AssignHeader(FakeSheet);

            // Assert
            AssertAssignHeader(productCodes, row1, row2);
        }

        private static void AssertAssignHeader(string[] productCodes, IRow row1, IRow row2)
        {
            Assert.Multiple(() =>
            {
                row1.Cells[0].Received(1).SetCellValue("#");
                row1.Cells[ColumnDate].Received(1).SetCellValue(ViewResources.Date);
                row1.Cells[ColumnExternalId].Received(1).SetCellValue(ViewResources.ExternalId);
                row1.Cells[ColumnSystemId].Received(1).SetCellValue(ViewResources.SystemId);
                row1.Cells[ColumnCurrency].Received(1).SetCellValue(ViewResources.Currency);
                row1.Cells[ColumnTurnoverSportBook].Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[ColumnTurnoverTotal].Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[ColumnActualStake].Received(1).SetCellValue(ViewResources.ActualStake);
                row1.Cells[ColumnBuyBackAmountSportsbook].Received(1).SetCellValue(ViewResources.BuyBackAmount);
                row1.Cells[ColumnBuyBackAmountBA].Received(1).SetCellValue(ViewResources.BuyBackAmount);
                row1.Cells[ColumnBuyBackAmountTotal].Received(1).SetCellValue(ViewResources.BuyBackAmount);
                row1.Cells[ColumnTotalTransaction].Received(1).SetCellValue(ViewResources.TotalTransaction);
                row1.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[ColumnLicenseeWlSportsBook].Received(1).SetCellValue(ViewResources.LicenseeW_L);
                row1.Cells[ColumnLicenseeWlTotal].Received(1).SetCellValue(ViewResources.LicenseeW_L);
                row1.Cells[ColumnCashOutCommission].Received(1).SetCellValue(ViewResources.CashOutCommission);
                row1.Cells[ColumnBetTradeCommission].Received(1).SetCellValue(ViewResources.BetTradeCommission);
                row1.Cells[ColumnBaCommission].Received(1).SetCellValue(ViewResources.BACommission);
                row1.Cells[ColumnRacingRevenue].Received(1).SetCellValue(ViewResources.RacingRevenue);
                row1.Cells[ColumnAmtDueToBetTrade].Received(1).SetCellValue(ViewResources.AmtDueToBetTrade);
                row1.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(ViewResources.AmtDueToBA);
                row1.Cells[ColumnAmtDueToRacing].Received(1).SetCellValue(ViewResources.AmtDueToRacing);

                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);

                row2.Cells[ColumnBuyBackAmountSportsbook].Received(1).SetCellValue(ViewResources.Sportsbook);
                row2.Cells[ColumnBuyBackAmountBA].Received(1).SetCellValue(ViewResources.ProductBA);
                row2.Cells[ColumnBuyBackAmountTotal].Received(1).SetCellValue(ViewResources.Total);

                var resourceManager = ViewResources.ResourceManager;
                for (int i = 0; i < productCodes.Count(); i++)
                {
                    var text = resourceManager.GetString("Product" + productCodes[i]);
                    row2.Cells[ColumnTurnoverSportBook + i].Received(1).SetCellValue(text);
                    row2.Cells[ColumnLicenseeWlSportsBook + i].Received(1).SetCellValue(text);
                }

                row2.Cells[ColumnTurnoverTotal].Received(1).SetCellValue(ViewResources.Total);
                row2.Cells[ColumnLicenseeWlTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }
    }
}