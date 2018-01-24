using System.Collections.Generic;
using System.Linq;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.Report.UnitTests
{
    [TestFixture]
    public class BaseReportExcelTest
    {
        protected ISheet FakeSheet { get; set; }

        [SetUp]
        public void Setup()
        {
            FakeSheet = Substitute.For<ISheet>();
        }

        protected static void AssertSetTextRowHeader(IRow row, IEnumerable<string> headers)
        {
            for (int i = 0; i < headers.Count(); i++)
            {
                row.Cells[i].Received(1).SetCellValue(headers.ElementAt(i));
            }
        }

        protected static void AssertHideColumns(ISheet sheet, params int[] columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                sheet.Received(1).SetColumnHidden(columns[i], true);
            }
        }
    }
}