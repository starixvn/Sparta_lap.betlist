using System.Collections.Generic;
using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ViewModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class PlatformWinLossSummaryExcelTestable : PlatformWinLossSummaryExcel
    {
        public PlatformWinLossSummaryExcelTestable(
                                 string templatePath,
                                 PlatformWinLossMetaModel metaModel,
                                 IEnumerable<PlatformWinLossSummary> dataList,
                                 PlatformWinLossSummary total)
            : base(templatePath, metaModel, dataList, total)
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