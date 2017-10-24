namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
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

        public CustomerPendingFundsExcel(string templatePath, CustomerPendingFundsExcelModel excelModel)
            : base(templatePath)
        {
            _excelModel = excelModel;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.BetCount,
                ViewResources.PendingFunds
            };
        }

        protected override void AssignData()
        {
            var numOfDataRow = _excelModel.DataList.Count();

            ISheet sheet;

            if (numOfDataRow == 0)
            {
                sheet = Workbook.CloneSheet("Template", "Data");
                AssignTitle(sheet);
                AssignHeader(sheet);
                AssignNoInfoRow(sheet);
                ProcessUI(sheet);
            }
            else
            {
                var numOfSheet = GetNumberOfSheets(numOfDataRow, PageSize);
                for (int i = 0; i < numOfSheet; i++)
                {
                    sheet = Workbook.CloneSheet("Template", "Data" + (i + 1).ToString());
                    AssignTitle(sheet);
                    AssignHeader(sheet);
                    AssignSheetData(sheet, i);
                    ProcessUI(sheet);
                }
            }
        }

        protected void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }
        }

        protected void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected int AssignSheetData(ISheet sheet, int pageIndex)
        {
            var rowCount = Math.Min(_excelModel.DataList.Count(), checked((pageIndex + 1) * PageSize));
            var rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + 2);

                var customerPedingFunds = _excelModel.DataList.ElementAt(index);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(customerPedingFunds.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(customerPedingFunds.SystemId);
                row.Cells[ColumnBetCount].SetCellValue(customerPedingFunds.BetCounts);
                row.Cells[ColumnPendingFunds].SetCellValue(customerPedingFunds.PendingFunds);

                rowIndex++;
            }

            AssignSumData(sheet, rowIndex + 2);

            return rowIndex;
        }

        protected void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(ViewResources.CustomerPedingFundsTitle, ReportExcelHelper.GetTitleCurrencyName(_excelModel.ConvertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);

            // Freeze title and header
            sheet.CreateFreezePane(0, 2);
        }

        protected void AssignSumData(ISheet sheet, int totalRow)
        {
            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            row.Cells[ColumnBetCount].SetCellValue(_excelModel.TotalData.BetCounts);
            row.Cells[ColumnPendingFunds].SetCellValue(_excelModel.TotalData.PendingFunds);
            row.Cells[0].SetCellValue(ViewResources.Total);
            sheet.MergeCells(totalRow, 0, totalRow, 2);
        }

        protected void ProcessUI(ISheet sheet)
        {
            if (!_excelModel.SystemIdVisible)
            {
                sheet.HideColumn(ColumnSystemId);
            }
        }
    }
}