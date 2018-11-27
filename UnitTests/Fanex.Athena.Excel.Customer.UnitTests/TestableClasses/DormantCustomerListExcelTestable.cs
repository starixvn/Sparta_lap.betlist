using System.Collections.Generic;
using Fanex.Athena.Model.Customer.DTO;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Customer.UnitTests.TestableClasses
{
    public class DormantCustomerListExcelTestable : DormantCustomerListExcel
    {
        public DormantCustomerListExcelTestable(
                                 string templatePath,
                                 string reportTitle,
                                 IEnumerable<DormantCustomer> dataList)
            : base(templatePath, reportTitle, dataList)
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
    }
}