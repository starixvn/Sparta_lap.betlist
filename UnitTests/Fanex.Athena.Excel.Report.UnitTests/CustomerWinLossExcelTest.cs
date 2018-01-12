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
    public class CustomerWinLossExcelTest
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
        private const int ColumnRacingRevenue = 11;
        private const int ColumnAmtDueToBa = 12;
        private const int ColumnAmtDueToRacing = 13;

        private const string FormatDate = "MM/dd/yyyy";

        private const int ColumnCount = 15;

        private static readonly CustomerWinLoss DataItem = new CustomerWinLoss
        {
            ExternalId = "1",
            SystemId = "2",
            Turnover = 3,
            TotalTransaction = 4,
            CustomerWL = 5.5m,
            PlayerComm = 6,
            BetEffective = 7,
            LicenseeWL = 8,
            BAComm = 9,
            RacingRevenue = 10,
            AmtDueToBA = 11,
            AmtDueToRacing = 12
        };

        private static readonly CustomerWinLossExcelModel ExcelModel = new CustomerWinLossExcelModel
        {
            DataList = new List<CustomerWinLoss> { DataItem },
            TotalData = DataItem,
            ConvertingOption = new ConvertingOption(),
            MetaModel = new CustomerWinLossMetaModel
            {
                Username = "Abc",
                ProductOptions = new List<ProductOption>()
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
                row1.Cells[3].Received(1).SetCellValue(ViewResources.Turnover);
                row1.Cells[4].Received(1).SetCellValue(ViewResources.TotalTransaction);
                row1.Cells[5].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[6].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[7].Received(1).SetCellValue(ViewResources.CustomerW_L);
                row1.Cells[8].Received(1).SetCellValue(ViewResources.EffectiveBettingValue);
                row1.Cells[9].Received(1).SetCellValue(ViewResources.LicenseeW_L);
                row1.Cells[10].Received(1).SetCellValue(ViewResources.BACommission);
                row1.Cells[11].Received(1).SetCellValue(ViewResources.RacingRevenue);
                row1.Cells[12].Received(1).SetCellValue(ViewResources.AmtDueToBA);
                row1.Cells[13].Received(1).SetCellValue(ViewResources.AmtDueToRacing);

                row2.Cells[ColumnCustomerWl].Received(1).SetCellValue(ViewResources.WinLoss);
                row2.Cells[ColumnCustomerComm].Received(1).SetCellValue(ViewResources.Comm);
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
            excel.WhenForAnyArgs(x => x.AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            excel.Public_AssignSheetData(_sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(1);
                row.Cells[ColumnExternalId].Received(1).SetCellValue(DataItem.ExternalId);
                row.Cells[ColumnSystemId].Received(1).SetCellValue(DataItem.SystemId);
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransaction);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWL);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.PlayerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerWL + DataItem.PlayerComm);
                row.Cells[ColumnBetEffective].Received(1).SetCellValue(DataItem.BetEffective);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(DataItem.LicenseeWL);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(DataItem.BAComm);
                row.Cells[ColumnRacingRevenue].Received(1).SetCellValue(DataItem.RacingRevenue);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(DataItem.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].Received(1).SetCellValue(DataItem.AmtDueToRacing);

                excel.Received(1).AssignSumData(Arg.Any<ISheet>(), Arg.Any<int>());
            });
        }

        [Test]
        public void AssignTitle_WhenCall_CallRelevantMethods()
        {
            // Arrange
            ExcelTestHelper.MockGetRow(_sheet, ColumnCount);
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
            excel.Public_AssignTitle(_sheet);

            // Assert
            Assert.Multiple(() =>
            {
                _sheet.Received().SetTextRowValue(
                    excel.TemplateSheet,
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    Arg.Any<int>(),
                    reportTitle);
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
                _sheet.Received().HideColumn(2);
                _sheet.Received().HideColumn(7, 6);
                _sheet.Received().HideRow(2);
                _sheet.Received().HideColumn(10, 12);
                _sheet.Received().HideColumn(11, 13);
            });
        }

        [Test]
        public void AssignSumData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            const int StartIndex = 100;
            var row = ExcelTestHelper.MockGetRow(_sheet, ColumnCount, StartIndex + 3);
            var excel = CreateExcel();

            // Action
            excel.AssignSumData(_sheet, StartIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnTurnover].Received(1).SetCellValue(DataItem.Turnover);
                row.Cells[ColumnTotalTransaction].Received(1).SetCellValue(DataItem.TotalTransaction);
                row.Cells[ColumnCustomerWl].Received(1).SetCellValue(DataItem.CustomerWL);
                row.Cells[ColumnCustomerComm].Received(1).SetCellValue(DataItem.PlayerComm);
                row.Cells[ColumnCustomerTotal].Received(1).SetCellValue(DataItem.CustomerWL + DataItem.PlayerComm);
                row.Cells[ColumnBetEffective].Received(1).SetCellValue(DataItem.BetEffective);
                row.Cells[ColumnLicenseeWl].Received(1).SetCellValue(DataItem.LicenseeWL);
                row.Cells[ColumnBaCommission].Received(1).SetCellValue(DataItem.BAComm);
                row.Cells[ColumnRacingRevenue].Received(1).SetCellValue(DataItem.RacingRevenue);
                row.Cells[ColumnAmtDueToBa].Received(1).SetCellValue(DataItem.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].Received(1).SetCellValue(DataItem.AmtDueToRacing);
                _sheet.MergeCells(StartIndex, 0, StartIndex, 2);
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
    }
}