// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoExcel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using Common.Extensions;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlaytechCasinoExcel : ReportBase
    {
        private const int ColumnCount = 9;
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnover = 5;
        private const int ColumnTotalTrans = 6;
        private const int ColumnCustomerWL = 7;
        private const int ColumnLicenseeWL = 8;

        private const int RowTitle = 0;
        private const int RowHeader = 1;
        private const int RowContent = 2;
        private const int RowTotalTop = 3;
        private const int RowTotal = 4;
        private const int RowGrantTotal = 5;
        private const int RowNoInfo = 6;

        private PlaytechCasinoViewModel _viewModel;

        public PlaytechCasinoExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                PlaytechCasinoViewModel viewModel)
            : base(templatePath, exportingFileName, new ShowingOption(), convertingOption)
        {
            _viewModel = viewModel;

            NumOfDataRow = viewModel.MainData.Count;

            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.Date,
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Currency,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L
            };
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeader, RowHeader);
            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfo, RowContent, ColumnCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            var metaModel = _viewModel.Param;
            var fromdate = metaModel.FromDate.ToString(FormatDate);
            var todate = metaModel.ToDate.ToString(FormatDate);
            var title = metaModel.Level > 0 && metaModel.CurrencyId == 0
                ? GetExactTitleCurrencyName(metaModel.DrillDownCurrency)
                : GetExactTitleCurrencyName(metaModel.CurrencyId);

            ReportTitle = string.Format(ViewResources.PlaytechCasino + " " + ViewResources.WinLoss + ", {0} --> {1} - {2}", fromdate, todate, title);
            SetTextRowValue(sheet, RowTitle, RowTitle, ColumnCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, RowHeader);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContent);
        }

        /// <summary>
        /// Assign Sheet Data.
        /// </summary>
        /// <param name="sheet">Sheet value.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int rowIndex;

            for (rowIndex = 0; rowIndex < _viewModel.MainData.Count; rowIndex++)
            {
                var item = _viewModel.MainData[rowIndex];
                var row = sheet.CopyRow(TemplateSheet, RowContent, RowContent + rowIndex);
                row.Cells[ColumnIndex].SetCellValue(rowIndex + 1);
                row.Cells[ColumnDate].SetCellValue(item.WinlossDate);
                row.Cells[ColumnExternalId].SetCellValue(item.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(item.SystemId);
                row.Cells[ColumnCurrency].SetCellValue(item.CurrencyName);
                row.Cells[ColumnTurnover].SetCellValue(item.Turnover);
                row.Cells[ColumnTotalTrans].SetCellValue(item.TotalTrans);
                row.Cells[ColumnCustomerWL].SetCellValue(item.CustomerWinLoss);
                row.Cells[ColumnLicenseeWL].SetCellValue(item.LicenseeWinLoss);
            }

            AssignTotalData(sheet, RowContent + rowIndex);
        }

        /// <summary>
        /// Assign Total Data.
        /// </summary>
        /// <param name="sheet">Sheet value.</param>
        /// <param name="rowIndex">Row index.</param>
        public void AssignTotalData(ISheet sheet, int rowIndex)
        {
            for (int i = 0; i < _viewModel.TotalData.Count; i++)
            {
                var item = _viewModel.TotalData[i];
                var row = sheet.CopyRow(TemplateSheet, i == 0 ? RowTotalTop : RowTotal, checked(rowIndex + i));
                row.Cells[ColumnIndex].SetCellValue(ViewResources.Total);
                row.Cells[ColumnCurrency].SetCellValue(item.CurrencyName);
                row.Cells[ColumnTurnover].SetCellValue(item.Turnover);
                row.Cells[ColumnTotalTrans].SetCellValue(item.TotalTrans);
                row.Cells[ColumnCustomerWL].SetCellValue(item.CustomerWinLoss);
                row.Cells[ColumnLicenseeWL].SetCellValue(item.LicenseeWinLoss);
            }

            sheet.MergeCells(rowIndex, ColumnIndex, rowIndex + _viewModel.TotalData.Count - 1, ColumnSystemId);

            rowIndex += _viewModel.TotalData.Count;
            var grandTotalRow = sheet.CopyRow(TemplateSheet, RowGrantTotal, rowIndex);
            grandTotalRow.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + ViewResources.USD);
            grandTotalRow.Cells[ColumnTurnover].SetCellValue(_viewModel.TotalInUsd.Turnover);
            grandTotalRow.Cells[ColumnTotalTrans].SetCellValue(_viewModel.TotalInUsd.TotalTrans);
            grandTotalRow.Cells[ColumnCustomerWL].SetCellValue(_viewModel.TotalInUsd.CustomerWinLoss);
            grandTotalRow.Cells[ColumnLicenseeWL].SetCellValue(_viewModel.TotalInUsd.LicenseeWinLoss);
            sheet.MergeCells(rowIndex, ColumnIndex, rowIndex, ColumnCurrency);

            var nextIndex = checked(rowIndex + 1);
            grandTotalRow = sheet.CopyRow(TemplateSheet, RowGrantTotal, nextIndex);

            grandTotalRow.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + ViewResources.EUR);
            grandTotalRow.Cells[ColumnTurnover].SetCellValue(_viewModel.TotalInEur.Turnover);
            grandTotalRow.Cells[ColumnTotalTrans].SetCellValue(_viewModel.TotalInEur.TotalTrans);
            grandTotalRow.Cells[ColumnCustomerWL].SetCellValue(_viewModel.TotalInEur.CustomerWinLoss);
            grandTotalRow.Cells[ColumnLicenseeWL].SetCellValue(_viewModel.TotalInEur.LicenseeWinLoss);
            sheet.MergeCells(nextIndex, ColumnIndex, nextIndex, ColumnCurrency);
        }

        /// <summary>
        /// Process UI.
        /// </summary>
        /// <param name="sheet">Sheet value.</param>
        public override void ProcessUi(ISheet sheet)
        {
            switch (_viewModel.Param.Level)
            {
                case 0:
                    HideColumn(sheet, ColumnExternalId);
                    HideColumn(sheet, ColumnSystemId);
                    break;

                case 1:
                    HideColumn(sheet, ColumnExternalId);
                    HideColumn(sheet, ColumnDate);
                    break;

                default:
                    HideColumn(sheet, ColumnDate);
                    break;
            }
        }
    }
}
