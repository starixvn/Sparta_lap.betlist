using System.Collections.Generic;
using System.Data;
using Fanex.Athena.Excel.Base.Extension;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Common;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public partial class MatchWinLossExcelTest : BaseReportExcelTest
    {
        [Test]
        public void AssignMatchData_MatchInfoListEmpty_ReturnCurrencyRow()
        {
            // Arrange
            const int CurrentRow = 20;
            var excel = CreateExcel();

            // Action
            var result = excel.AssignMatchData(FakeSheet, new List<DataRow>(), null, CurrentRow, 0);

            // Assert
            Assert.That(result, Is.EqualTo(CurrentRow));
        }

        [Test]
        public void AssignMatchData_WhenCall_SetIndexAndMatchName()
        {
            // Arrange
            const string MatchName = "Sample Match Name";
            const int CurrentIndex = 10;
            var currentRow = 20;
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow + 1);
            var excel = CreateExcel();
            var matchInfoList = new List<DataRow>
            {
                _contentData.Rows[0]
            };

            // Action
            excel.AssignMatchData(FakeSheet, matchInfoList, MatchName, currentRow, CurrentIndex);

            // Assert
            Assert.Multiple(() =>
            {
                row.Cells[0].Received(1).SetCellValue(CurrentIndex);
                row.Cells[1].Received(1).SetCellValue(MatchName);
            });
        }

        [Test]
        public void AssignMatchData_WhenCall_SetMatchData()
        {
            // Arrange
            const string MatchName = "Sample Match Name";
            const int CurrentIndex = 10;
            var currentRow = 20;
            var columns = new[] 
            {
                "turnover", "betcount", "memberwinlost", "playercomm", "totalwl", "licenseewl",
                "commba", "rbrevenue", "amtduetoba", "amtduetorb"
            };
            var row = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow + 1);
            var excel = CreateExcel();
            var matchInfoList = new List<DataRow>
            {
                 _contentData.Rows[0]
            };

            // Action
            excel.AssignMatchData(FakeSheet, matchInfoList, MatchName, currentRow, CurrentIndex);

            // Assert
            Assert.Multiple(() =>
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    var columnName = columns[i];
                    row.Cells[i + 2].Received(1).SetCellValue(Converter.ToDecimal(matchInfoList[0][columnName]));
                }
            });
        }

        [Test]
        public void AssignMatchData_WhenCall_SetBettypeRow()
        {
            // Arrange
            const string MatchName = "Sample\n Match Name";
            const int CurrentIndex = 10;
            var currentRow = 20;
            var columns = new[] 
            {
                "turnover", "betcount", "memberwinlost", "playercomm", "totalwl", "licenseewl",
                "commba", "rbrevenue", "amtduetoba", "amtduetorb"
            };
            ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow);
            var bettypeRow = ExcelTestHelper.MockGetRow(FakeSheet, ColumnTemplateCount, currentRow + 1);
            var excel = CreateExcel();
            var matchInfoList = new List<DataRow>
            {
                 _contentData.Rows[0]
            };

            // Action
            excel.AssignMatchData(FakeSheet, matchInfoList, MatchName, currentRow, CurrentIndex);

            // Assert
            Assert.Multiple(() =>
            {
                var columnStart = 2;
                foreach (string columnName in columns)
                {
                    bettypeRow.Cells[columnStart++].Received(1).SetCellValue(matchInfoList[0][columnName]);
                }
            });
        }
    }
}