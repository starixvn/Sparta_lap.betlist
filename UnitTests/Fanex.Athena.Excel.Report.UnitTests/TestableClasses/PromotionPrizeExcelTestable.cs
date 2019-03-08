namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    using Model.Core.ResultModel;
    using Model.Report;
    using NPOI.SS.UserModel;

    public class PromotionPrizeExcelTestable : PromotionPrizeExcel
    {
        public PromotionPrizeExcelTestable(string templatePath, string title, ReportResultModel<PromotionPrize> data)
            : base(templatePath, title, data)
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

        public void Public_AssignGrandTotal(ISheet sheet, int rowIndex)
        {
            AssignGrandTotal(sheet, rowIndex);
        }

        public void Public_ProcessUI(ISheet sheet)
        {
            ProcessUI(sheet);
        }
    }
}