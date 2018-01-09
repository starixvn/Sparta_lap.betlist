// --------------------------------------------------------------------------
// <copyright file="CustomerWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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

    public class CustomerWinLossExcel : ReportBase
    {
        private const int ColumnExternalId = 1;
        private const int ColumnSystemId = 2;
        private const int ColumnTurnover = 3;
        private const int ColumnTotalTransaction = 4;
        private const int ColumnCustomerWl = 5;
        private const int ColumnCustomerComm = 6;
        private const int ColumnCustomerTotal = 7;
        private const int ColumnBetEffective = 8;
        private const int ColumnLicenseeWl = 9;
        private const int ColumnBaCommission = 10;
        private const int ColumnRacingRevenue = 11;
        private const int ColumnAmtDueToBa = 12;
        private const int ColumnAmtDueToRacing = 13;

        private const int ColumnTemplateCount = 14;

        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly CustomerWinLossExcelModel _excelModel;

        public CustomerWinLossExcel(string templatePath, CustomerWinLossExcelModel excelModel)
            : base(templatePath, excelModel.DataList.Count())
        {
            _excelModel = excelModel;
            PageSize = _excelModel.MetaModel.PageSize;
            IsMultiSheet = true;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            var headerTextList = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.EffectiveBettingValue,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.RacingRevenue,
                ViewResources.AmtDueToBA,
                ViewResources.AmtDueToRacing
            };
            for (int i = 0; i < headerTextList.Length; i++)
            {
                row.Cells[i].SetCellValue(headerTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[5].SetCellValue(ViewResources.WinLoss);
            row.Cells[6].SetCellValue(ViewResources.Comm);
            row.Cells[7].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet, int pageIndex)
        {
            var rowCount = Math.Min(_excelModel.DataList.Count(), checked((pageIndex + 1) * PageSize));
            var rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);

                var customerWinloss = _excelModel.DataList.ElementAt(index);

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(customerWinloss.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(customerWinloss.SystemId);
                row.Cells[ColumnTurnover].SetCellValue(customerWinloss.Turnover);
                row.Cells[ColumnTotalTransaction].SetCellValue(customerWinloss.TotalTransaction);
                row.Cells[ColumnCustomerWl].SetCellValue(customerWinloss.CustomerWL);
                row.Cells[ColumnCustomerComm].SetCellValue(customerWinloss.PlayerComm);
                row.Cells[ColumnCustomerTotal].SetCellValue(customerWinloss.CustomerWL + customerWinloss.PlayerComm);
                row.Cells[ColumnBetEffective].SetCellValue(customerWinloss.BetEffective);
                row.Cells[ColumnLicenseeWl].SetCellValue(customerWinloss.LicenseeWL);
                row.Cells[ColumnBaCommission].SetCellValue(customerWinloss.BAComm);
                row.Cells[ColumnRacingRevenue].SetCellValue(customerWinloss.RacingRevenue);
                row.Cells[ColumnAmtDueToBa].SetCellValue(customerWinloss.AmtDueToBA);
                row.Cells[ColumnAmtDueToRacing].SetCellValue(customerWinloss.AmtDueToRacing);

                rowIndex++;
            }

            AssignSumData(sheet, rowIndex);

            ProcessUI(sheet);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var filterTitle = ReportExcelHelper.GetTitleCurrencyName(_excelModel.ConvertingOption)
                            + ReportExcelHelper.GetLicenseeWLProduct(_excelModel.MetaModel.ProductOptions)
                            + (string.IsNullOrEmpty(_excelModel.MetaModel.Username)
                                ? string.Empty
                                : ", " + ViewResources.Username + ": " + _excelModel.MetaModel.Username);

            var reportTitle = string.Format(
                                        ViewResources.CustomerWinLossTitle,
                                        _excelModel.MetaModel.FromDate.ToString(FormatDate),
                                        _excelModel.MetaModel.ToDate.ToString(FormatDate),
                                        filterTitle);

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (!_excelModel.SystemIdVisible)
            {
                sheet.HideColumn(ColumnSystemId);
            }

            if (!_excelModel.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerComm);
                sheet.HideRow(RowTableHeaderTemplate + 1);
            }

            if (!_excelModel.BAVisible)
            {
                sheet.HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!_excelModel.RBVisible)
            {
                sheet.HideColumn(ColumnRacingRevenue, ColumnAmtDueToRacing);
            }
        }

        protected internal virtual void AssignSumData(ISheet sheet, int startIndex)
        {
            var rowTotalNumber = checked(startIndex + RowContentTemplate);

            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, rowTotalNumber);

            row.Cells[ColumnTurnover].SetCellValue(_excelModel.TotalData.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(_excelModel.TotalData.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(_excelModel.TotalData.CustomerWL);
            row.Cells[ColumnCustomerComm].SetCellValue(_excelModel.TotalData.PlayerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_excelModel.TotalData.CustomerWL + _excelModel.TotalData.PlayerComm);
            row.Cells[ColumnBetEffective].SetCellValue(_excelModel.TotalData.BetEffective);
            row.Cells[ColumnLicenseeWl].SetCellValue(_excelModel.TotalData.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(_excelModel.TotalData.BAComm);
            row.Cells[ColumnRacingRevenue].SetCellValue(_excelModel.TotalData.RacingRevenue);
            row.Cells[ColumnAmtDueToBa].SetCellValue(_excelModel.TotalData.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(_excelModel.TotalData.AmtDueToRacing);

            MergeTotalCell(sheet, row, rowTotalNumber);
        }

        private static void MergeTotalCell(ISheet sheet, IRow row, int rowTotalNumber)
        {
            sheet.MergeCells(rowTotalNumber, ColumnIndex, rowTotalNumber, 2);
            row.Cells[0].SetCellValue(ViewResources.Total);
        }
    }
}