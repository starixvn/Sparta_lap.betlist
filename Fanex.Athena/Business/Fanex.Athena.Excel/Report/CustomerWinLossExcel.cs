// --------------------------------------------------------------------------
// <copyright file="CustomerWinLossExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Models.DTO.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Customer win-loss class.
    /// </summary>
    public class CustomerWinLossExcel : ReportBase
    {
        private const int ColumnAmtDueToBa = 11;
        private const int ColumnAmtDueToRacing = 12;
        private const int ColumnBaCommission = 9;
        private const int ColumnCustomerComm = 6;
        private const int ColumnCustomerTotal = 7;
        private const int ColumnCustomerWl = 5;
        private const int ColumnExternalId = 1;
        private const int ColumnLicenseeWl = 8;
        private const int ColumnRacingRevenue = 10;
        private const int ColumnSystemId = 2;
        private const int ColumnTemplateCount = 13;
        private const int ColumnTurnover = 3;
        private const int ColumnTotalTransaction = 4;
        private const int RowContentTemplate = 3;
        private const int RowNoInfoTemplate = 5;
        private const int RowTableHeaderTemplate = 1;
        private const int RowTotalTemplate = 4;

        private readonly IList<CustomerWinLoss> _customerWinlossList;
        private readonly CustomerWinLoss _total;
        private readonly CustomerWinLossViewModel _viewModel;

        public CustomerWinLossExcel(
                                    string templatePath,
                                    string exportingFileName,
                                    ShowingOption showingOption,
                                    ConvertingOption convertingOption,
                                    CustomerWinLossViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _viewModel = viewModel;
            _customerWinlossList = viewModel.DataList;
            _total = viewModel.TotalData;
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
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.RacingRevenue,
                ViewResources.AmtDueToBA,
                ViewResources.AmtDueToRacing
            };
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _customerWinlossList.Count;

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
                PageSize = _viewModel.IsHistory ? _viewModel.HistoryParam.PageSize : _viewModel.Param.PageSize;
                var numOfSheet = GetNumberOfSheets(_customerWinlossList.Count, PageSize);
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

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
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

        /// <summary>
        /// Assign "no information" to sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="pageIndex">Start index.</param>
        /// <returns>Row index.</returns>
        public override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            int rowCount = Math.Min(_customerWinlossList.Count, checked((pageIndex + 1) * PageSize));
            int rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + RowContentTemplate);

                CustomerWinLoss customerWinloss = _customerWinlossList[index];

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(customerWinloss.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(customerWinloss.SystemId);
                row.Cells[ColumnTurnover].SetCellValue(customerWinloss.Turnover);
                row.Cells[ColumnTotalTransaction].SetCellValue(customerWinloss.TotalTransaction);
                row.Cells[ColumnCustomerWl].SetCellValue(customerWinloss.MemberWl);
                row.Cells[ColumnCustomerComm].SetCellValue(customerWinloss.PlayerComm);
                row.Cells[ColumnCustomerTotal].SetCellValue(customerWinloss.MemberWl + customerWinloss.PlayerComm);
                row.Cells[ColumnLicenseeWl].SetCellValue(customerWinloss.LicenseeWl);
                row.Cells[ColumnBaCommission].SetCellValue(customerWinloss.BaComm);
                row.Cells[ColumnRacingRevenue].SetCellValue(customerWinloss.RacingRevenue);
                row.Cells[ColumnAmtDueToBa].SetCellValue(customerWinloss.AmtDueToBa);
                row.Cells[ColumnAmtDueToRacing].SetCellValue(customerWinloss.AmtDueToRacing);

                rowIndex++;
            }

            AssignSumData(sheet, rowIndex);

            ProcessUI(sheet);
            return rowIndex;
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                                        ViewResources.CustomerWinLossTitle,
                                        _viewModel.Param.FromDate.ToString(FormatDate),
                                        _viewModel.Param.ToDate.ToString(FormatDate),
                                        GetTitleCurrencyName()) + GetLicenseeWLProduct();

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                HideRow(sheet, RowTableHeaderTemplate + 1);
            }

            if (!ShowingOption.BaVisible)
            {
                HideColumn(sheet, ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RbVisible)
            {
                HideColumn(sheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
            }
        }

        /// <summary>
        /// Merge cells in total rows.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="row">The first total row.</param>
        /// <param name="rowTotalNumber">The last total row index.</param>
        protected static void MergeTotalCell(ISheet sheet, IRow row, int rowTotalNumber)
        {
            sheet.MergeCells(rowTotalNumber, ColumnIndex, rowTotalNumber, 2);
            row.Cells[0].SetCellValue(ViewResources.Total);
        }

        /// <summary>
        /// Assign total data to summary row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="startIndex">Start index.</param>
        protected void AssignSumData(ISheet sheet, int startIndex)
        {
            int rowTotalNumber = checked(startIndex + RowContentTemplate);

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, rowTotalNumber);

            row.Cells[ColumnTurnover].SetCellValue(_total.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(_total.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(_total.MemberWl);
            row.Cells[ColumnCustomerComm].SetCellValue(_total.PlayerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(_total.MemberWl + _total.PlayerComm);
            row.Cells[ColumnLicenseeWl].SetCellValue(_total.LicenseeWl);
            row.Cells[ColumnBaCommission].SetCellValue(_total.BaComm);
            row.Cells[ColumnRacingRevenue].SetCellValue(_total.RacingRevenue);
            row.Cells[ColumnAmtDueToBa].SetCellValue(_total.AmtDueToBa);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(_total.AmtDueToRacing);

            MergeTotalCell(sheet, row, rowTotalNumber);
        }
    }
}