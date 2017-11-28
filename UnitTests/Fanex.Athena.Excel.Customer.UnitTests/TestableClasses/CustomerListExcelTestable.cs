using System.Data;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Customer.DTO;
using Fanex.Athena.Model.Customer.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Customer.UnitTests.TestableClasses
{
    public class CustomerListExcelTestable : CustomerListExcel
    {
        public CustomerListExcelTestable(
                                 string templatePath,
                                 string reportTitle,
                                 CustomerListViewModel model)
            : base(templatePath, reportTitle, model)
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

        public void Public_AssignCommonColumns(CustomerDTO item, int numOfRow, IRow row)
        {
            AssignCommonColumns(item, numOfRow, row);
        }

        public void Public_AssignCommissionColumns(CustomerDTO item, IRow row)
        {
            AssignCommissionColumns(item, row);
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