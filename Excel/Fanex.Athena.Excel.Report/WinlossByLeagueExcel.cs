// --------------------------------------------------------------------------
// <copyright file="WinlossByLeagueExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;
    using NPOI.SS.UserModel;
    using Resources;

    public class WinlossByLeagueExcel : ReportBase
    {
        private const int ColumnBetCount = 4;
        private const int ColumnBaComm = 8;
        private const int ColumnAmtDueTo = 9;
        private const int ColumnCustomerWinLoss = 6;
        private const int ColumnLicenseeWl = 7;
        private const int ColumnMatch = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnTurnover = 5;
        private const int RowContentTemplate = 4;
        private const int RowContentTitleTemplate = 3;
        private const int RowNoInfoTemplate = 9;
        private const int RowTotalSolidBorder = 8;
        private const int NumOfColumnLevel0 = 10;
        private const int NumOfColumnLevel1 = 8;

        private readonly WinLossByLeagueMetaModel _metaModel;
        private readonly IEnumerable<WinLossByLeague> _dataList;
        private readonly WinLossByLeague _totalData;

        private readonly bool _hasAmountDueToColumn;

        public WinlossByLeagueExcel(
                                    string templatePath,
                                    ReportResultModel<WinLossByLeague> resultModel,
                                    WinLossByLeagueMetaModel metaModel,
                                    bool hasAmountDueToColumn)
            : base(templatePath, resultModel.DataList.Count())
        {
            _hasAmountDueToColumn = hasAmountDueToColumn;
            _metaModel = metaModel;
            _dataList = resultModel.DataList;
            _totalData = resultModel.TotalData;
            IsMultiSheet = true;
        }

        public int Level
        {
            get;
            set;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            var headerTextList = new[]
            {
                "#",
                ViewResources.Match,
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.BetCount,
                ViewResources.Turnover,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmountDueTo,
            };
            SetTextRowHeader(row, headerTextList);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTitleTemplate, Level == 0 ? NumOfColumnLevel0 : NumOfColumnLevel1, ViewResources.NoInfo);
        }

        protected override void AssignSheetData(ISheet sheet, int pageIndex)
        {
            if (Level == 1)
            {
                sheet.HideRow(3);
            }

            var rowCount = Math.Min(_dataList.Count(), checked((pageIndex + 1) * PageSize));
            var currentRow = 4;

            var row = sheet.CopyRow(TemplateSheet, RowContentTitleTemplate, 3);
            row.Cells[0].SetCellValue(_metaModel.LeagueName);

            for (int index = pageIndex * PageSize; index < rowCount; index++)
            {
                var dataItem = _dataList.ElementAt(index);

                var rowContent = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);

                rowContent.Cells[ColumnIndex].SetCellValue(dataItem.Index);
                if (Level == 0)
                {
                    rowContent.Cells[ColumnMatch].SetCellValue(dataItem.HomeName + Environment.NewLine + dataItem.AwayName);
                    rowContent.Cells[ColumnAmtDueTo].SetCellValue(dataItem.AmountDue);

                    rowContent.Height = (short)(rowContent.Height * 2);
                }
                else
                {
                    rowContent.Cells[ColumnExternalId].SetCellValue(dataItem.ExternalId);
                    rowContent.Cells[ColumnSystemId].SetCellValue(dataItem.SystemId);
                }

                AssignCommonData(rowContent, dataItem);

                currentRow++;
            }

            AssignTotalData(sheet, currentRow);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, Level == 0 ? NumOfColumnLevel0 : NumOfColumnLevel1, _metaModel.ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 4);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (Level == 0)
            {
                sheet.HideColumn(ColumnExternalId, ColumnSystemId);
            }

            if (Level != 0 || !_hasAmountDueToColumn)
            {
                sheet.HideColumn(ColumnMatch, ColumnAmtDueTo);
            }
        }

        private static void AssignCommonData(IRow row, WinLossByLeague dataItem)
        {
            row.Cells[ColumnTurnover].SetCellValue(dataItem.TurnOver);
            row.Cells[ColumnBetCount].SetCellValue(dataItem.BetCount);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(dataItem.CustomerWinLoss);
            row.Cells[ColumnLicenseeWl].SetCellValue(dataItem.LicenseeWL);
            row.Cells[ColumnBaComm].SetCellValue(dataItem.BACommission);
        }

        protected internal virtual int AssignTotalData(ISheet sheet, int currentRow)
        {
            sheet.MergeCells(currentRow, 0, currentRow, Level == 0 ? 1 : 3);

            var row = sheet.CopyRow(TemplateSheet, RowTotalSolidBorder, currentRow);
            AssignCommonData(row, _totalData);
            row.Cells[ColumnAmtDueTo].SetCellValue(_totalData.AmountDue);
            return currentRow;
        }
    }
}