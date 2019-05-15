using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class TurnoverExcelTestable : TurnoverExcel
    {
        public TurnoverExcelTestable(
                                 string templatePath,
                                 ShowingOption showingOption,
                                 TurnoverViewModel viewModel)
            : base(templatePath, showingOption, viewModel)
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