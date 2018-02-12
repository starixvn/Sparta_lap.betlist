using System.Data;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.MetaModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class MatchWinLossExcelTestable : MatchWinLossExcel
    {
        public MatchWinLossExcelTestable(
            string templatePath,
            ConvertingOption convertingOption,
            ShowingOption showingOption,
            DataTable exportData,
            DataTable totalData,
            MatchWinLossMetaModel model)
            : base(templatePath, convertingOption, showingOption, exportData, totalData, model)
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