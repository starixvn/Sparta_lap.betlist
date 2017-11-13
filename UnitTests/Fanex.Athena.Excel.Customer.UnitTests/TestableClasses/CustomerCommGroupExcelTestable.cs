using System.Data;
using Fanex.Athena.Excel.Base.Entities;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Customer.UnitTests.TestableClasses
{
    public class CustomerCommGroupExcelTestable : CustomerCommGroupExcel
    {
        public CustomerCommGroupExcelTestable(
                                 string templatePath,
                                 DataTable exportData,
                                 ShowingOption showingOption)
            : base(templatePath, exportData, showingOption)
        {
        }

        public void Public_AssignHeader(ISheet sheet)
        {
            AssignHeader(sheet);
        }
    }
}