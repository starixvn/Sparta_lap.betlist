using System.Collections.Generic;
using Fanex.Athena.Excel.Base;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.Excel.Report.UnitTests.TestableClasses;
using Fanex.Athena.Model.Core.ResultModel;
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
    public class GoldDeluxeWinLossExcelTest : BaseReportExcelTest
    {
        private const int ColumnCount = 12;

        private const int ColumnTipsAmount = 9;
        private const int ColumnTipsSharing = 10;
        private const int ColumnAmountDue = 11;

        [Test]
        public void PostAssignHeader_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();

            // Action
            excel.Public_PostAssignHeader(FakeSheet);

            // Assert
            Assert.Multiple(() =>
            {
                FakeSheet.Received().CopyRow(excel.TemplateSheet, ReportBase.RowHeaderTemplate, ReportBase.RowHeaderTemplate);
                row.Cells[ColumnTipsAmount].Received(1).SetCellValue(ViewResources.TipAmount);
                row.Cells[ColumnTipsSharing].Received(1).SetCellValue(ViewResources.TipSharing);
                row.Cells[ColumnAmountDue].Received(1).SetCellValue(ViewResources.AmountDue);
            });
        }

        [Test]
        public void AssignCommonData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnCount, ReportBase.RowHeaderTemplate);
            var excel = CreateExcel();
            var data = new GoldDeluxeWinLoss
            {
                TipAmount = 12.45m,
                TipSharing = 232.34m,
                AmountDue = 4324
            };

            // Action
            excel.Public_AssignCommonData(row, data);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[ColumnTipsAmount].Received(1).SetCellValue(data.TipAmount);
                row.Cells[ColumnTipsSharing].Received(1).SetCellValue(data.TipSharing);
                row.Cells[ColumnAmountDue].Received(1).SetCellValue(data.AmountDue);
            });
        }

        private static GoldDeluxeWinLossExcelTestable CreateExcel()
        {
            var excel = Substitute.ForPartsOf<GoldDeluxeWinLossExcelTestable>(
                string.Empty,
                new ThirdPartyWinLossMetaModel(),
                new ReportResultModel<GoldDeluxeWinLoss>
                {
                    DataList = new List<GoldDeluxeWinLoss>
                    {
                        new GoldDeluxeWinLoss()
                    }
                });
            excel.TemplateSheet.Returns(Substitute.For<ISheet>());
            return excel;
        }
    }
}