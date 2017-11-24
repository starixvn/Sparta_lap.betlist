using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class AgCasinoBalanceExcelTestable : AgCasinoBalanceExcel
    {
        public AgCasinoBalanceExcelTestable(
                                 string templatePath,
                                 ConvertingOption convertingOption,
                                 AgCasinoBalanceMetaModel metaModel,
                                 AgCasinoBalanceViewModel viewModel)
            : base(templatePath, convertingOption, metaModel, viewModel)
        {
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