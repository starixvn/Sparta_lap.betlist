using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Core.ResultModel;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class GoldDeluxeWinLossExcelTestable : GoldDeluxeWinLossExcel
    {
        public GoldDeluxeWinLossExcelTestable(
                                 string templatePath,
                                 ThirdPartyWinLossMetaModel metaModel,
                                 ReportResultModel<GoldDeluxeWinLoss> resultData)
            : base(templatePath, metaModel, resultData)
        {
        }

        public void Public_PostAssignHeader(ISheet sheet)
        {
            PostAssignHeader(sheet);
        }

        public void Public_AssignCommonData(IRow row, GoldDeluxeWinLoss data)
        {
            AssignCommonData(row, data);
        }
    }
}