using System.Collections.Generic;
using Fanex.Athena.Model.Report.MetaModel;
using Fanex.Athena.Model.Report.ResultModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class WeeklyLiveMatchExcelTestable : WeeklyLiveMatchExcel
    {
        public WeeklyLiveMatchExcelTestable(
                string templatePath,
                WeeklyLiveMatchMetaModel metaModel,
                WeeklyLiveMatchResultModel resultModel,
                IEnumerable<string> weekHeaders)
            : base(templatePath, metaModel, resultModel, weekHeaders)
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

        public void Public_AssignTotalRow(ISheet sheet, int rowIndex)
        {
            AssignSheetData(sheet, rowIndex);
        }
    }
}