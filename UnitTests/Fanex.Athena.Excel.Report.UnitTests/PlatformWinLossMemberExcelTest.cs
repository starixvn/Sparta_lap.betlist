using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.DataDomain;
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
    public class PlatformWinLossMemberExcelTest
    {
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnCurrency = 3;
        private const int ColumnTurnover = 4;
        private const int ColumnTotalTrans = 5;
        private const int ColumnCustomerWl = 6;
        private const int ColumnCustomerCommission = 7;
        private const int ColumnCustomerTotal = 8;
        private const int ColumnLicenseeWl = 9;
        private const int ColumnBaCommission = 10;
        private const int ColumnRacingRevenue = 11;
        private const int ColumnAmtDueToBa = 12;
        private const int ColumnAmtDueToRacing = 13;
        private const int ColumnTemplateCount = 14;

        private const string FormatDate = "MM/dd/yyyy";

        private const int ColumnCount = 15;

        private const string CurrencyName = "ABC";

        private static readonly PlatformWinLossMember DataItem = new PlatformWinLossMember
        {
            ExternalId = "1",
            SystemId = "2",
            Turnover = 3,
            TotalTransactions = 4,
            CustomerWinLoss = 5,
            CustomerComm = 6,
            LicenseeWL = 7,
            BACommission = 8,
            RacingRevenue = 9,
            AmtDueToBA = 10,
            AmtDueToRacing = 11
        };

        private static readonly PlatformWinLossViewModel ViewModel = new PlatformWinLossViewModel
        {
            MainResult = new[] { DataItem },
            TotalResult = DataItem,
            Param = new PlatformWinLossMetaModel
            {
                CurrencyName = CurrencyName,
                PlatformType = PlatformType.Asia,
                FromDay = 1,
                ToDay = 2,
                Year = 2018,
                Month = 1,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption { Code = "SB", Status = true },
                    new ProductOption { Code = "RB", Status = false }
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
                row1.Cells[0].Received(1).SetCellValue("#");
                row1.Cells[1].Received(1).SetCellValue(ViewResources.ExternalId);
                row1.Cells[2].Received(1).SetCellValue(ViewResources.SystemId);
                row1.Cells[3].Received(1).SetCellValue(ViewResources.Currency);
                row1.Cells[4].Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[5].Received(1).SetCellValue(ViewResources.TotalTransaction);
                row1.Cells[6].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[7].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[8].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[9].Received(1).SetCellValue(ViewResources.LicenseeW_L);
                row1.Cells[10].Received(1).SetCellValue(ViewResources.BACommission);
                row1.Cells[11].Received(1).SetCellValue(ViewResources.RacingRevenue);
                row1.Cells[12].Received(1).SetCellValue(ViewResources.AmtDueToBA);
                row1.Cells[13].Received(1).SetCellValue(ViewResources.AmtDueToRacing);

                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerCommission].Received(1).SetCellValue(ViewResources.Comm);
                row2.Cells[ColumnCustomerTotal].Received(1).SetCellValue(ViewResources.Total);
            });
        }

        [Test]
        public void AssignNoInfoRow_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
            var excel = CreateExcel();

            // Action
            excel.Public_AssignNoInfoRow(_sheet);

            // Assert
            _sheet.Received().SetTextRowValue(excel.TemplateSheet, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<int>(), ViewResources.NoInfo);
        }

        [Test]
        public void AssignSheetData_WhenCall_CallSetValueCorrectly()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 3);
            var excel = CreateExcel();
            excel.WhenForAnyArgs(x => x.AssignTotal(Arg.Any<ISheet>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnCurrency].Received(1).Received(1).SetCellValue(DataItem.CurrencyName);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnTotalTrans].Received(1).SetCellValue(DataItem.TotalTransactions);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWinLoss);
                row.Cells[ColumnCustomerCommission].Received(1).SetCellValue(DataItem.CustomerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerTotal);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(DataItem.LicenseeWL);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(DataItem.BACommission);
                row.Cells[ColumnRacingRevenue].Received(1).SetCellValue(DataItem.RacingRevenue);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(DataItem.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].Received(1).SetCellValue(DataItem.AmtDueToRacing);

                excel.Received(1).AssignTotal(Arg.Any<ISheet>());
            });
        }

        [TestCase("EUR", "View in EUR")]
        [TestCase("USD", "View in USD")]
        [TestCase("GBP", "View in GBP")]
        [TestCase("HIHI", "View in " + CurrencyName)]
        public void AssignTitle_WhenCall_CallRelevantMethods(string baseCurrency, string expectedBaseCurrencyTitle)
        {
            // Arrange
            const string FormatDate = "MM/dd/yyyy";
            ViewModel.Param.BaseCurrency = baseCurrency;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 0);
            var excel = CreateExcel();
            var platformText = ViewResources.Platform + ": " + PlatformType.GetPlatformName(ViewModel.Param.PlatformType);

            var reportTitle = string.Format(
                                         ViewResources.PlatformWinLossTitle,
                                         ViewModel.Param.FromDate.ToString(FormatDate),
                                         ViewModel.Param.ToDate.ToString(FormatDate),
                                         ViewResources.AllCurrencies,
                                         expectedBaseCurrencyTitle,
                                         platformText) + ", " + ViewResources.Product + ": SB";

            // Action
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(reportTitle);
                _sheet.Received(1).CreateFreezePane(0, 3);
                _sheet.ReceivedWithAnyArgs().DeleteRow(0);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsInvisible_CallHideColumnCorrectly()
        {
            // Arrange
            var excel = CreateExcel();

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().HideColumn(ColumnCustomerTotal, ColumnCustomerCommission);
                _sheet.Received().HideRow(ReportBase.RowHeaderTemplate + 1);
                _sheet.Received().HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
                _sheet.Received().HideColumn(ColumnRacingRevenue, ColumnAmtDueToRacing);
            });
        }

        [Test]
        public void ProcessUI_AllOptionsIsVisible_NotCallHideColumn()
        {
            // Arrange
            var showingOption = new ShowingOption
            {
                CustomerCommVisible = true,
                BAVisible = true,
                RBVisible = true
            };
            var excel = CreateExcel(showingOption);

            // Action
            excel.Public_ProcessUI(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.DidNotReceive().HideColumn(ColumnCustomerTotal, ColumnCustomerCommission);
                _sheet.DidNotReceive().HideRow(ReportBase.RowHeaderTemplate + 1);
                _sheet.DidNotReceive().HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
                _sheet.DidNotReceive().HideColumn(ColumnRacingRevenue, ColumnAmtDueToRacing);
            });
        }

        [Test]
        public void AssignTotal_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, 4);
            var excel = CreateExcel();

            // Action
            excel.AssignTotal(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(ViewResources.Total);
                _sheet.MergeCells(4, 0, 4, ColumnCurrency);
            });
        }

        private static PlatformWinLossMemberExcelTestable CreateExcel(ShowingOption showingOption = null)
        {
            var excel = Substitute.ForPartsOf<PlatformWinLossMemberExcelTestable>(
                string.Empty,
                ViewModel,
                showingOption ?? new ShowingOption());
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}