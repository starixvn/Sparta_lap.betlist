namespace Fanex.Athena.Excel.Report
{
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using Resources;

    public class GoldDeluxeWinLossExcel : ThirdPartyWinLossExcel<GoldDeluxeWinLoss>
    {
        private const int ColumnTipsAmount = 9;
        private const int ColumnTipsSharing = 10;
        private const int ColumnAmountDue = 11;

        public GoldDeluxeWinLossExcel(
                string templatePath,
                ThirdPartyWinLossMetaModel metaModel,
                ReportResultModel<GoldDeluxeWinLoss> data)
            : base(templatePath, metaModel, data)
        {
        }

        protected override void PostAssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);
            row.Cells[ColumnTipsAmount].SetCellValue(ViewResources.TipAmount);
            row.Cells[ColumnTipsSharing].SetCellValue(ViewResources.TipSharing);
            row.Cells[ColumnAmountDue].SetCellValue(ViewResources.AmountDue);
        }

        protected override void AssignCommonData(IRow row, GoldDeluxeWinLoss data)
        {
            base.AssignCommonData(row, data);

            row.Cells[ColumnTipsAmount].SetCellValue(data.TipAmount);
            row.Cells[ColumnTipsSharing].SetCellValue(data.TipSharing);
            row.Cells[ColumnAmountDue].SetCellValue(data.AmountDue);
        }
    }
}