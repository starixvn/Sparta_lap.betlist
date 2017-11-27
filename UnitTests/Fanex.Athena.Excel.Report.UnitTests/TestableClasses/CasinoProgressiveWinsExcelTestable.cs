using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class CasinoProgressiveWinsExcelTestable : CasinoProgressiveWinsExcel
    {
        public CasinoProgressiveWinsExcelTestable(
                                 string templatePath,
                                 ConvertingOption convertingOption,
                                 CasinoProgressiveWinsViewModel viewModel)
            : base(templatePath, convertingOption, viewModel)
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