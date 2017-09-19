// --------------------------------------------------------------------------
// <copyright file="CustomerPendingFundsExcel.cs" company="">
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
    using Helpers;
    using Model.Report.DTO;
    using NPOI.SS.UserModel;
    using Resources;

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

        private readonly IEnumerable<CustPendingFunds> _customerPendingFundsList;
        private readonly CustPendingFunds _total;

        private readonly ConvertingOption _convertingOption;

        public CustomerPendingFundsExcel(
                                         string templatePath,
                                         ConvertingOption convertingOption,
                                         IEnumerable<CustPendingFunds> dataList,
                                         CustPendingFunds totalData)
            : base(templatePath, new ShowingOption())
        {
            _convertingOption = convertingOption;
            _customerPendingFundsList = dataList;
            _total = totalData;
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
        protected override void AssignData()
        {
            var numOfDataRow = _customerPendingFundsList.Count();

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

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }
        }

        /// <summary>
        /// Assign no info row.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="pageIndex">Start index.</param>
        /// <returns>Row index.</returns>
        protected override int AssignSheetData(ISheet sheet, int pageIndex)
        {
            var rowCount = Math.Min(_customerPendingFundsList.Count(), checked((pageIndex + 1) * PageSize));
            var rowIndex = 0;

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex + 2);

                var customerPedingFunds = _customerPendingFundsList.ElementAt(index);

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

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(ViewResources.CustomerPedingFundsTitle, ReportExcelHelper.GetTitleCurrencyName(_convertingOption));

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

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
            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, totalRow);

            row.Cells[ColumnBetCount].SetCellValue(_total.BetCounts);
            row.Cells[ColumnPendingFunds].SetCellValue(_total.PendingFunds);
            row.Cells[0].SetCellValue(ViewResources.Total);
            sheet.MergeCells(totalRow, 0, totalRow, 2);
        }
    }
}