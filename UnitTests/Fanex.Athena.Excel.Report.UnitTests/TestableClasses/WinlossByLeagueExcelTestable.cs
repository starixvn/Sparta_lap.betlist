using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Core.ResultModel;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class WinlossByLeagueExcelTestable : WinlossByLeagueExcel
    {
        public WinlossByLeagueExcelTestable(
                                 string templatePath,
                                 ReportResultModel<WinLossByLeague> resultModel,
                                 WinLossByLeagueMetaModel metaModel)
            : base(templatePath, resultModel, metaModel)
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