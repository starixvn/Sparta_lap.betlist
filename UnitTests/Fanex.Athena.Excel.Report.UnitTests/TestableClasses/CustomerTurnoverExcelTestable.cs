using System.Collections.Generic;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Excel.Report.Models;
using Fanex.Athena.Model.Report.DTO;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class CustomerTurnoverExcelTestable : CustomerTurnoverExcel
    {
        public CustomerTurnoverExcelTestable(
                                 string templatePath,
                                 ShowingOption showingOption,
                                 IEnumerable<CustTurnover> customerTurnoverList,
                                 CustTurnover total,
                                 string formatDateTime,
                                 string reportTitle)
            : base(templatePath, showingOption, customerTurnoverList, total, formatDateTime, reportTitle)
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