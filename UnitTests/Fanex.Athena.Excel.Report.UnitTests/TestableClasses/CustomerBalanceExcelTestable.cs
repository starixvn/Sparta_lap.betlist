using System.Collections.Generic;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class CustomerBalanceExcelTestable : CustomerBalanceExcel
    {
        public CustomerBalanceExcelTestable(
                                 string templatePath,
                                 CustomerBalanceMetaModel metaModel,
                                 IEnumerable<CustBalance> exportData,
                                 CustBalance totalData,
                                 ShowingOption showingOption,
                                 ConvertingOption convertingOption)
            : base(templatePath, metaModel, exportData, totalData, showingOption, convertingOption)
        {
        }

        public void Public_AssignData()
        {
            AssignData();
        }

        public void Public_AssignHeader(ISheet sheet)
        {
            AssignHeader(sheet);
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