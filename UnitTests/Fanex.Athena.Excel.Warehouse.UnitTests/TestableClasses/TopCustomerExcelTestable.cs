using System.Data;
using Fanex.Athena.Excel.Base.Entities;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Warehouse.UnitTests.TestableClasses
{
    public class TopCustomerExcelTestable : TopCustomerExcel
    {
        public TopCustomerExcelTestable(
                                 string templatePath,
                                 ShowingOption showingOption,
                                 DataTable exportData,
                                 string reportTitle)
            : base(templatePath, showingOption, exportData, reportTitle)
        {
        }

        public void Public_AssignHeader(ISheet sheet)
        {
            AssignHeader(sheet);
        }

        public void Public_AssignSheetData(ISheet sheet)
        {
            AssignSheetData(sheet);
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