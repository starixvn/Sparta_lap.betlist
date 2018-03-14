using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class DailyRacingWinLossExcelTestable : DailyRacingWinLossExcel
    {
        public DailyRacingWinLossExcelTestable(
                                 string templatePath,
                                 ConvertingOption convertingOption,
                                 ShowingOption showingOption,
                                 DailyRacingWinLossViewModel viewModel)
            : base(templatePath, convertingOption, showingOption, viewModel)
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