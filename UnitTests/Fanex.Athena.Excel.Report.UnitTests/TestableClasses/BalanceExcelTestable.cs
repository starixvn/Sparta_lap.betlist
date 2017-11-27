using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class BalanceExcelTestable : BalanceExcel
    {
        public BalanceExcelTestable(
                                 string templatePath,
                                 ShowingOption showingOption,
                                 ConvertingOption convertingOption,
                                 Balance balance,
                                 string date)
            : base(templatePath, showingOption, convertingOption, balance, date)
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