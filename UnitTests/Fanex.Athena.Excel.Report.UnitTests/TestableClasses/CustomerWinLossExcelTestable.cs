using Fanex.Athena.Excel.Report.Models;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class CustomerWinLossExcelTestable : CustomerWinLossExcel
    {
        public CustomerWinLossExcelTestable(
                                 string templatePath,
                                 CustomerWinLossExcelModel excelModel)
            : base(templatePath, excelModel)
        {
        }

        public void Public_AssignHeader(ISheet sheet)
        {
            AssignHeader(sheet);
        }

        public void Public_AssignNoInfoRow(ISheet sheet)
        {
            AssignNoInfoRow(sheet);
        }

        public void Public_AssignSheetData(ISheet sheet, int pageIndex)
        {
            AssignSheetData(sheet, pageIndex);
        }

        public void Public_AssignTitle(ISheet sheet)
        {
            AssignTitle(sheet);
        }

        public void Public_ProcessUI(ISheet sheet)
        {
            ProcessUI(sheet);
        }
    }
}