namespace Fanex.Athena.Excel.Base.UnitTests.ClassTest
{
    using NPOI.SS.UserModel;

    public class FakeExcelBase : ExcelBase
    {
        public static new void HideColumn(ISheet sheet, params int[] columnIndex)
        {
            ExcelBase.HideColumn(sheet, columnIndex);
        }

        public override void HideRow(ISheet sheet, params int[] rowIndex)
        {
            base.HideRow(sheet, rowIndex);
        }        
    }
}