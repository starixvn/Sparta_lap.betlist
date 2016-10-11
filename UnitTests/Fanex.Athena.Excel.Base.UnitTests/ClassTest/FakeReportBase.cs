namespace Fanex.Athena.Excel.Base.UnitTests.ClassTest
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Excel.Base.Entities;
    using NPOI.SS.UserModel;

    public class FakeReportBase : ReportBase
    {
        public FakeReportBase(
            string templatePath,
            string exportingFileName,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            int pageSize = 0)
            : base(templatePath, exportingFileName, showingOption, convertingOption, pageSize)
        {
        }

        public new int AssignRowContent(IRow row, DataRow data, IList<string> columnNameList, int columnStart = 0)
        {
            return base.AssignRowContent(row, data, columnNameList, columnStart);
        }

        public new string GetExactTitleCurrencyName(int currencyId)
        {
            return base.GetExactTitleCurrencyName(currencyId);
        }

        public new string GetLicenseeWLProduct(string caption = null)
        {
            return base.GetLicenseeWLProduct(caption);
        }

        public new void SetTextRowValue(ISheet sheet, int sourceRowIndex, int destRowIndex, int columnCount, string value)
        {
            base.SetTextRowValue(sheet, sourceRowIndex, destRowIndex, columnCount, value);
        }
    }
}