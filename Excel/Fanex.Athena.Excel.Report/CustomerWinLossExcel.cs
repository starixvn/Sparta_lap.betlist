// --------------------------------------------------------------------------
// <copyright file="CustomerWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
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

        private readonly IEnumerable<CustomerWinLoss> _customerWinlossList;
        private readonly CustomerWinLoss _total;
        private readonly CustomerWinLossMetaModel _metaModel;

        public CustomerWinLossExcel(
            string templatePath,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            CustomerWinLossMetaModel metaModel,
            IEnumerable<CustomerWinLoss> dataList,
            CustomerWinLoss totalData)
            : base(templatePath, showingOption, convertingOption, metaModel.PageSize)
        {
            _metaModel = metaModel;
            _customerWinlossList = dataList;
            _total = totalData;
            HeaderTextList = new List<string>
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
        }

        protected override void AssignData()
        {
            int numOfDataRow = _customerWinlossList.Count();

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
                var numOfSheet = GetNumberOfSheets(numOfDataRow, _metaModel.PageSize);
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

        protected override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[5].SetCellValue(ViewResources.WinLoss);
            row.Cells[6].SetCellValue(ViewResources.Comm);
            row.Cells[7].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowCount = Math.Min(_customerWinlossList.Count(), checked((pageIndex + 1) * PageSize));
            int rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);

                var customerWinloss = _customerWinlossList.ElementAt(index);

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
            return rowIndex;
        }

        protected override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                                        ViewResources.CustomerWinLossTitle,
                                        _metaModel.FromDate.ToString(FormatDate),
                                        _metaModel.ToDate.ToString(FormatDate),
                                        GetTitleCurrencyName()) + GetLicenseeWLProduct(_metaModel.ProductOptions);

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                HideRow(sheet, RowTableHeaderTemplate + 1);
            }

            if (!ShowingOption.BAVisible)
            {
                HideColumn(sheet, ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RBVisible)
            {
                HideColumn(sheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
            }
        }

        protected void AssignSumData(ISheet sheet, int startIndex)
        {
            int rowTotalNumber = checked(startIndex + RowContentTemplate);

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, rowTotalNumber);

            row.Cells[ColumnTurnover].SetCellValue(_total.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(_total.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(_total.CustomerWL);
            row.Cells[ColumnCustomerComm].SetCellValue(_total.PlayerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_total.CustomerWL + _total.PlayerComm);
            row.Cells[ColumnBetEffective].SetCellValue(_total.BetEffective);
            row.Cells[ColumnLicenseeWl].SetCellValue(_total.LicenseeWL);
            row.Cells[ColumnBaCommission].SetCellValue(_total.BAComm);
            row.Cells[ColumnRacingRevenue].SetCellValue(_total.RacingRevenue);
            row.Cells[ColumnAmtDueToBa].SetCellValue(_total.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(_total.AmtDueToRacing);

            MergeTotalCell(sheet, row, rowTotalNumber);
        }

        private static void MergeTotalCell(ISheet sheet, IRow row, int rowTotalNumber)
        {
            sheet.MergeCells(rowTotalNumber, ColumnIndex, rowTotalNumber, 2);
            row.Cells[0].SetCellValue(ViewResources.Total);
        }
    }
}