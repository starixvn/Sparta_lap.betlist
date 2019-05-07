namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Linq;
    using Base;
    using Fanex.Athena.Excel.Base.Extension;
    using Helpers;
    using Models;
    using NPOI.SS.UserModel;
    using Resources;

    public class CustomerPendingFundsExcel : ReportBase
    {
        private const int ColumnBetCount = 3;
        private const int ColumnExternalId = 1;
        private const int ColumnPendingFunds = 4;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 5;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 4;
        private const int RowTotalTemplate = 3;

        private readonly CustomerPendingFundsExcelModel _excelModel;

        protected internal CustomerPendingFundsExcel() : base(string.Empty, 0)
        {
        }

        public CustomerPendingFundsExcel(string templatePath, CustomerPendingFundsExcelModel excelModel)
            : base(templatePath, excelModel.DataList.Count())
        {
            _excelModel = excelModel;
            IsMultiSheet = true;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);
            var headerTextList = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.BetCount,
                ViewResources.PendingFunds
            };
            SetTextRowHeader(row, headerTextList);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet, int pageIndex)
        {
            var rowCount = Math.Min(_excelModel.DataList.Count(), checked((pageIndex + 1) * PageSize));
            var rowIndex = RowContentTemplate;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex);

                var customerPedingFunds = _excelModel.DataList.ElementAt(index);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(customerPedingFunds.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(customerPedingFunds.SystemId);
                row.Cells[ColumnBetCount].SetCellValue(customerPedingFunds.BetCounts);
                row.Cells[ColumnPendingFunds].SetCellValue(customerPedingFunds.PendingFunds);

                rowIndex++;
            }

            AssignSumData(sheet, rowIndex);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(ViewResources.CustomerPedingFundsTitle, ReportExcelHelper.GetTitleCurrencyName(_excelModel.ConvertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);

            // Freeze title and header
            sheet.CreateFreezePane(0, RowContentTemplate);
        }

        protected internal virtual void AssignSumData(ISheet sheet, int totalRow)
        {
            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            row.Cells[ColumnBetCount].SetCellValue(_excelModel.TotalData.BetCounts);
            row.Cells[ColumnPendingFunds].SetCellValue(_excelModel.TotalData.PendingFunds);
            row.Cells[0].SetCellValue(ViewResources.Total);
            sheet.MergeCols(ColumnIndex, ColumnSystemId, totalRow);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (!_excelModel.SystemIdVisible)
            {
                sheet.HideColumn(ColumnSystemId);
            }
        }
    }
}