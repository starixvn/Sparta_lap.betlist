using Fanex.Athena.Excel.Report.RNGCasino;
using Fanex.Athena.Model.Casino.DTO;
using Fanex.Athena.Model.Casino.MetaModel;
using Fanex.Athena.Model.Casino.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class HandHistoryExcelTestable : HandHistoryExcel
    {
        public HandHistoryExcelTestable(
                                string templatePath,
                                RNGCasinoResultMetaModel metaModel,
                                RNGCasinoResultViewModel viewModel,
                                RNGCasinoResult totalData)
            : base(templatePath, metaModel, viewModel, totalData)
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