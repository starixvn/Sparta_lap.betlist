using Fanex.Athena.Excel.Base.Entities;
using Fanex.Athena.Model.Core.ResultModel;
using Fanex.Athena.Model.Report.DTO;
using Fanex.Athena.Model.Report.MetaModel;
using NPOI.SS.UserModel;

namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    public class ThirdPartyWinLossExcelTestable : ThirdPartyWinLossExcel<ThirdPartyWinLoss>
    {
        public ThirdPartyWinLossExcelTestable(
                                 string templatePath,
                                 ConvertingOption convertingOption,
                                 ThirdPartyWinLossMetaModel metaModel,
                                 ReportResultModel<ThirdPartyWinLoss> resultData)
            : base(templatePath, convertingOption, metaModel, resultData)
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