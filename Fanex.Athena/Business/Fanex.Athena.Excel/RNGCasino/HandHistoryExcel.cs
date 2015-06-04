// --------------------------------------------------------------------------
// <copyright file="HandHistoryExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Fanex.Athena.Models.DTO.Casino;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.Casino;
    using Models.DTO.Report;
    using Models.ViewModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win loss by product - Export excel.
    /// </summary>
    public class HandHistoryExcel : ReportBase
    {
        private const int ColumnGameId = 1;
        private const int ColumnPlayerHistoryNo = 2;
        private const int ColumnGameType = 3;
        private const int ColumnGameGroup = 4;
        private const int ColumnStartDate = 5;
        private const int ColumnEndDate = 6;
        private const int ColumnStartBalance = 7;
        private const int ColumnEndBalance = 8;
        private const int ColumnStake = 9;
        private const int ColumnBet = 10;
        private const int ColumnWinLoss = 11;
        private const int ColumnRollingComm = 12;
        private const int ColumnResults = 13;

        private const int RowTableHeaderTemplate = 1;
        private const int RowContentTitleTemplate = 3;
        private const int RowContentTemplate = 4;
        private const int RowGroupTotal = 8;
        private const int RowGrandTotal = 9;
        private const int RowNoInfoTemplate = 10;
        private const int ColumnTemplateCount = 14;

        private readonly RNGCasinoResultMetaModel _metaModel;
        private readonly RNGCasinoResult _totalData;
        private readonly IList<RNGCasinoResult> _data;

        public HandHistoryExcel(
                    string templatePath,
                    string exportingFileName,
                    RNGCasinoResultMetaModel metaModel,
                    RNGCasinoResultViewModel viewModel,
                    RNGCasinoResult totalData)
            : base(templatePath, exportingFileName, null, null)
        {
            _metaModel = metaModel;
            _data = viewModel.Data;
            _totalData = totalData;

            HeaderTextList = new string[]
            {
                "#",
                ViewResources.GameID,
                ViewResources.PlayerHistoryNo,
                ViewResources.GameType,
                ViewResources.GameGroup,
                ViewResources.StartDate,
                ViewResources.EndDate,
                ViewResources.StartBalance,
                ViewResources.EndBalance,
                ViewResources.Stake,
                ViewResources.Bet,
                ViewResources.WinLoss,
                ViewResources.RollingComm,
                ViewResources.Results
            };
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _data.Count;

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (numOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTitleTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int indexReport = 1;
            int currentRow = 3;
            int rowCount = _data.Count;

            var groupId = string.Empty;
            RNGCasinoResult groupTotal = null;
            for (int index = 0; index < rowCount; index++)
            {
                var item = _data[index];
                if (groupId != item.SessionLogId)
                {
                    if (index > 0)
                    {
                        AssignGroupTotal(sheet, groupTotal, currentRow);
                        currentRow++;
                    }

                    AssignGroupHeader(sheet, item.SessionLogId, currentRow);
                    groupId = item.SessionLogId;
                    currentRow++;
                    groupTotal = new RNGCasinoResult();
                }

                groupTotal.Bet += item.Bet;
                groupTotal.RollingComm += item.RollingComm;
                groupTotal.Stake += item.Stake;
                groupTotal.WinLoss += item.WinLoss;

                var rowContent = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

                rowContent.Cells[ColumnIndex].SetCellValue(indexReport);

                AssignCommonData(rowContent, item);

                currentRow++;
            }

            AssignGroupTotal(sheet, groupTotal, currentRow);

            AssignGrandTotal(sheet, ++currentRow);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = ViewResources.CASINOHANDHISTORY;

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            var currency = _metaModel.CurrencyId == 0 || !string.IsNullOrEmpty(_metaModel.BaseCurrency)
                    ? _metaModel.BaseCurrency
                    : _metaModel.CurrencyName;

            var title = string.Format(
                "{0}: {1}({2}) -  {3}: {4} - {5}",
                ViewResources.Username,
                _metaModel.ExternalId,
                _metaModel.SystemId,
                ViewResources.RefNo,
                _metaModel.RefNo,
                string.Format(ViewResources.Viewin, currency.ToUpper()));

            SetTextRowValue(sheet, RowHeaderTemplate, RowHeaderTemplate, ColumnTemplateCount, title);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        private static void AssignCommonData(IRow row, RNGCasinoResult item)
        {
            row.Cells[ColumnIndex].SetCellValue(item.Index);
            row.Cells[ColumnGameId].SetCellValue(item.GameId);
            row.Cells[ColumnPlayerHistoryNo].SetCellValue(item.PlayerHistoryNo);
            row.Cells[ColumnGameType].SetCellValue(item.GameTypeName);
            row.Cells[ColumnGameGroup].SetCellValue(item.GameGroupName);
            row.Cells[ColumnStartDate].SetCellValue(item.StartTime);
            row.Cells[ColumnEndDate].SetCellValue(item.EndTime);
            row.Cells[ColumnStartBalance].SetCellValue(item.BeginBalance);
            row.Cells[ColumnEndBalance].SetCellValue(item.EndBlance);
            row.Cells[ColumnStake].SetCellValue(item.Stake);
            row.Cells[ColumnBet].SetCellValue(item.Bet);
            row.Cells[ColumnWinLoss].SetCellValue(item.WinLoss);
            row.Cells[ColumnRollingComm].SetCellValue(item.RollingComm);
            row.Cells[ColumnResults].SetCellValue(item.Result);
        }

        private static void AssignTotalColumns(IRow row, RNGCasinoResult item)
        {
            row.Cells[ColumnStake].SetCellValue(item.Stake);
            row.Cells[ColumnBet].SetCellValue(item.Bet);
            row.Cells[ColumnWinLoss].SetCellValue(item.WinLoss);
            row.Cells[ColumnRollingComm].SetCellValue(item.RollingComm);
        }

        private void AssignGroupTotal(ISheet sheet, RNGCasinoResult groupTotal, int currentRow)
        {
            var row = sheet.CopyRow(TemplateSheet, RowGroupTotal, currentRow);
            row.Cells[ColumnEndBalance].SetCellValue(ViewResources.Total);

            AssignTotalColumns(row, groupTotal);
        }

        private void AssignGrandTotal(ISheet sheet, int currentRow)
        {
            var row = sheet.CopyRow(TemplateSheet, RowGrandTotal, currentRow);
            row.Cells[ColumnEndBalance].SetCellValue(ViewResources.GrandTotal);

            AssignTotalColumns(row, _totalData);
        }

        private void AssignGroupHeader(ISheet sheet, string sessionLogId, int currentRow)
        {
            SetTextRowValue(sheet, RowContentTitleTemplate, currentRow, ColumnTemplateCount, ViewResources.SessionID + ": " + sessionLogId);
        }
    }
}