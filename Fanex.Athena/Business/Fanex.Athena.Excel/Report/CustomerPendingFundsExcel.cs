// --------------------------------------------------------------------------
// <copyright file="CustomerPendingFundsExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using Base;
    using Common.Extensions;
    using Entities;
    using Models.DTO.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    ///     Export excel for Customer Pending Funds.
    /// </summary>
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

        private readonly IList<CustPendingFunds> _customerPendingFundsList;
        private readonly CustPendingFunds _total;

        public CustomerPendingFundsExcel(
                                         string templatePath,
                                         string exportingFileName,
                                         ConvertingOption convertingOption,
                                         CustomerPendingFundsViewModel viewModel)
            : base(templatePath, exportingFileName, new ShowingOption(), convertingOption)
        {
            _customerPendingFundsList = viewModel.DataList;
            _total = viewModel.TotalData;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.BetCount,
                ViewResources.PendingFunds
            };
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _customerPendingFundsList.Count;

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            ColumnTemplateIndexList = GetColumnTemplateIndex();

            AssignTitle(sheet);
            AssignHeader(sheet);

            if (numOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUi(sheet);
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
        }

        /// <summary>
        /// Assign no info row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUi(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int rowCount = _customerPendingFundsList.Count;

            for (int index = 0; index < rowCount; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + 2);

                CustPendingFunds customerPedingFunds = _customerPendingFundsList[index];

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnExternalId].SetCellValue(customerPedingFunds.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(customerPedingFunds.SystemId);
                row.Cells[ColumnBetCount].SetCellValue(customerPedingFunds.BetCounts);
                row.Cells[ColumnPendingFunds].SetCellValue(customerPedingFunds.PendingFunds);
            }

            AssignSumData(sheet, rowCount + RowContentTemplate);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(ViewResources.CustomerPedingFundsTitle, GetTitleCurrencyName());

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);

            // Freeze title and header
            sheet.CreateFreezePane(0, 2);
        }

        /// <summary>
        /// Assign total row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="totalRow">The total value.</param>
        protected void AssignSumData(ISheet sheet, int totalRow)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            row.Cells[ColumnBetCount].SetCellValue(_total.BetCounts);
            row.Cells[ColumnPendingFunds].SetCellValue(_total.PendingFunds);
            row.Cells[0].SetCellValue(ViewResources.Total);
            sheet.MergeCells(totalRow, 0, totalRow, 2);
        }
    }
}