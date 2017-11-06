// --------------------------------------------------------------------------
// <copyright file="HandHistoryExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report.RNGCasino
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Base;
    using Base.Extension;
    using Model.Casino.DTO;
    using Model.Casino.MetaModel;
    using Model.Casino.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

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
                    RNGCasinoResultMetaModel metaModel,
                    RNGCasinoResultViewModel viewModel,
                    RNGCasinoResult totalData)
            : base(templatePath, viewModel.Data.Count)
        {
            _metaModel = metaModel;
            _data = viewModel.Data;
            _totalData = totalData;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        protected override void AssignData()
        {
            var numOfDataRow = _data.Count;

            var sheet = Workbook.CloneSheet("Template", "Data");

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
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            var headerTextList = new[]
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
            for (int i = 0; i < headerTextList.Length; i++)
            {
                row.Cells[i].SetCellValue(headerTextList[i]);
            }
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTitleTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            var indexReport = 1;
            var currentRow = 3;
            var rowCount = _data.Count;
            var groupId = string.Empty;
            var groupTotal = new RNGCasinoResult();
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

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ViewResources.CASINOHANDHISTORY);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            var currency = _metaModel.CurrencyId == 0 || !string.IsNullOrEmpty(_metaModel.BaseCurrency)
                    ? _metaModel.BaseCurrency
                    : _metaModel.CurrencyName;

            var title = $"{ViewResources.Username}: {_metaModel.ExternalId}({_metaModel.SystemId}) -  "
                + $"{ViewResources.RefNo}: {_metaModel.RefNo} - {string.Format(ViewResources.Viewin, currency.ToUpperInvariant())}";

            sheet.SetTextRowValue(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate, ColumnTemplateCount, title);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Assign Common Data.
        /// </summary>
        /// <param name="row">Row of sheet.</param>
        /// <param name="item">RNG Casino result.</param>
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
            row.Cells[ColumnResults].SetCellValue(RemoveHtmlTags(item.Result ?? string.Empty));
        }

        private static void AssignTotalColumns(IRow row, RNGCasinoResult item)
        {
            row.Cells[ColumnStake].SetCellValue(item.Stake);
            row.Cells[ColumnBet].SetCellValue(item.Bet);
            row.Cells[ColumnWinLoss].SetCellValue(item.WinLoss);
            row.Cells[ColumnRollingComm].SetCellValue(item.RollingComm);
        }

        private static string RemoveHtmlTags(string result)
        {
            return Regex.Replace(result.Replace("<BR>", "\n"), @"<[^>]+>|&nbsp;", string.Empty).Trim();
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
            sheet.SetTextRowValue(TemplateSheet, RowContentTitleTemplate, currentRow, ColumnTemplateCount, ViewResources.SessionID + ": " + sessionLogId);
        }
    }
}