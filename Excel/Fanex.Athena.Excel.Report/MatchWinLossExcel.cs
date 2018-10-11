// --------------------------------------------------------------------------
// <copyright file="MatchWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Base.Helpers;
    using Common;
    using Constant.DataDomain;
    using Extension.Helper;
    using Fanex.Athena.Excel.Base.Extension;
    using Helpers;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using Resources;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class MatchWinLossExcel : ReportBase
    {
        private const int ColumnCustomerWl = 4;
        private const int ColumnCustomerComm = 5;
        private const int ColumnCustomerTotal = 6;
        private const int ColumnBaCommission = 8;
        private const int ColumnAmtDueToBa = 9;
        private const int ColumnTemplateCount = 10;

        private const int RowTableHeaderTemplate = 1;
        private const int RowMatchTemplate = 3;
        private const int RowContentTemplate = 4;
        private const int RowBetTypeContentTemplate = 5;
        private const int RowTotalTemplate = 6;
        private const int RowSpecialBetTypeTemplate = 7;
        private const int RowGrandTotalTemplate = 8;
        private const int RowNoInfoTemplate = 9;

        private static readonly IEnumerable<int> SpecialBettypes = new[]
        {
            (int)BetType.MixParlay, (int)BetType.Outright, (int)BetType.CombinationMixParlay, (int)BetType.VirtualSports2MixParlay
        };

        private readonly MatchWinLossMetaModel _model;
        private readonly DataTable _totalData;

        private readonly DataTable _exportData;

        private readonly ConvertingOption _convertingOption;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        protected internal MatchWinLossExcel()
            : base(string.Empty, 0)
        {
        }

        public MatchWinLossExcel(
                                string templatePath,
                                ConvertingOption convertingOption,
                                ShowingOption showingOption,
                                DataTable exportData,
                                DataTable totalData,
                                MatchWinLossMetaModel model,
                                ICacheRefAdapter cacheRefAdapter = null)
            : base(templatePath, exportData.Rows.Count, showingOption)
        {
            _cacheRefAdapter = cacheRefAdapter ?? new CacheRefAdapter();
            _convertingOption = convertingOption;
            _model = model;
            _exportData = exportData;
            _totalData = totalData;
            AddExtraColumns();
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            var headerTextList = new[]
            {
                "#",
                ViewResources.Match,
                ViewResources.Turnover,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
                ViewResources.BACommission,
                ViewResources.AmtDueToBA
            };
            SetTextRowHeader(row, headerTextList);

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowMatchTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        [SuppressMessage(
               "StyleCop.CSharp.StylecopPlus.Rules",
               "SP2101:MethodMustNotContainMoreLinesThan",
               Justification = "Reviewed. Suppression is OK here.")]
        protected override void AssignSheetData(ISheet sheet)
        {
            var currentRowIndex = 3;
            var leagueName = string.Empty;
            var matchId = 0;
            var matchName = "Default";
            var formatMatchName = string.Empty;
            var columnNameList = GetColumnNameList();
            var rowCount = _exportData.Rows.Count;
            var matchInfoList = new List<DataRow>();
            var currentIndex = 1;
            var specialBetType = new List<DataRow>();

            for (int index = 0; index < rowCount; index++)
            {
                var exportRow = _exportData.Rows[index];
                var betType = Converter.ToInt(exportRow["bettype"]);
                if (SpecialBettypes.Contains(betType))
                {
                    specialBetType.Add(exportRow);
                    continue;
                }

                var currentLeagueName = Converter.ToString2(exportRow["leaguename"]);
                if (leagueName != currentLeagueName)
                {
                    currentRowIndex = AssignMatchData(sheet, matchInfoList, formatMatchName, currentRowIndex, currentIndex);
                    currentIndex += matchInfoList.Count > 0 ? 1 : 0;
                    matchInfoList = new List<DataRow>();
                    leagueName = currentLeagueName;
                    sheet.SetTextRowValue(TemplateSheet, RowMatchTemplate, currentRowIndex, ColumnTemplateCount, leagueName);
                    currentRowIndex++;
                }

                var currentMatchId = Converter.ToInt(exportRow["matchid"]);
                var currentMatchName = Converter.ToString2(exportRow["matchname"]);

                if (matchId != currentMatchId || matchName != currentMatchName)
                {
                    currentRowIndex = AssignMatchData(sheet, matchInfoList, formatMatchName, currentRowIndex, currentIndex);
                    currentIndex += matchInfoList.Count > 0 ? 1 : 0;
                    matchId = currentMatchId;
                    matchName = currentMatchName;
                    formatMatchName = GetFormatMatchName(exportRow, matchName);
                    matchInfoList = new List<DataRow>();
                }

                matchInfoList.Add(exportRow);
            }

            currentRowIndex = AssignMatchData(sheet, matchInfoList, formatMatchName, currentRowIndex, currentIndex);

            _exportData.DefaultView.RowFilter = "bettype <> '9' and bettype <> '10' and bettype <> '29' and bettype <> '2799'";
            var sumTable = _exportData.DefaultView.ToTable();

            if (_exportData.Rows.Count > sumTable.Rows.Count)
            {
                currentRowIndex = AssignTotalSummary(sheet, columnNameList, currentRowIndex, sumTable);
            }

            currentRowIndex = AssignSpecialBetType(sheet, specialBetType, currentRowIndex);
            AssignGrandTotalSummary(sheet, currentRowIndex);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var reportTitle = string.Format(
                ViewResources.MatchWinLossTitle,
                _model.Date.ToString(FormatDate),
                ReportExcelHelper.GetTitleCurrencyName(_convertingOption)) + ReportExcelHelper.GetLicenseeWLProduct(_model.ProductOptions);

            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                sheet.HideColumn(ColumnCustomerTotal, ColumnCustomerComm);
                sheet.HideRow(RowTableHeaderTemplate + 1);
            }

            if (!ShowingOption.BAVisible)
            {
                sheet.HideColumn(ColumnBaCommission, ColumnAmtDueToBa);
            }
        }

        private static IList<string> GetCommonColumnNameList()
        {
            return new List<string>
            {
                "turnover",
                "betcount",
                "memberwinlost",
                "playercomm",
                "totalwl",
                "licenseewl",
                "commba",
                "amtduetoba"
            };
        }

        private static int AssignDecimalData(IRow row, int cellIndex, params decimal[] decimalValues)
        {
            for (int i = 0, j = decimalValues.Length; i < j && cellIndex < row.Cells.Count; i++)
            {
                row.Cells[cellIndex].SetCellValue(decimalValues[i]);
                cellIndex++;
            }

            return cellIndex;
        }

        private string GetFormatMatchName(DataRow exportRow, string matchName)
        {
            string formatMatchName;

            switch ((int)exportRow["newproductid"])
            {
                case ProductTypes.NumberGame:
                case ProductTypes.VirtualSports:
                    formatMatchName = matchName;
                    break;

                default:
                    var homeName = _cacheRefAdapter.GetTeamName(exportRow["homeid"]) +
                                      (Converter.ToBool(exportRow["isneutral"]) ? " " + "(N)" : string.Empty);
                    var awayName = _cacheRefAdapter.GetTeamName(exportRow["awayid"]);
                    formatMatchName = homeName + "\n" + awayName;
                    break;
            }

            return formatMatchName;
        }

        private void AddExtraColumns()
        {
            var totalExpression = string.Format("{0} + {1}", GetCurrencyColumn("memberwinlost"), GetCurrencyColumn("playercomm"));
            var sumtotal = string.Format("{0} + {1}", GetCurrencyColumn("summemberwinlost"), GetCurrencyColumn("sumplayercomm"));

            _exportData.Columns.Add(GetCurrencyColumn("totalwl"), typeof(decimal), totalExpression);
            _totalData.Columns.Add(GetCurrencyColumn("sumtotalwl"), typeof(decimal), sumtotal);
        }

        protected internal virtual void AssignGrandTotalSummary(ISheet sheet, int currentRow)
        {
            var cellIndex = 2;

            var row = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate, currentRow);

            row.Cells[0].SetCellValue(ViewResources.GrandTotal);
            sheet.MergeCells(currentRow, 0, currentRow, 1);

            var columnNameList = GetTotalColumnNameList();

            foreach (string columnName in columnNameList)
            {
                cellIndex = AssignDecimalData(row, cellIndex, Converter.ToDecimal(_totalData.Rows[0][columnName]));
            }

            sheet.MergeCells(currentRow, ColumnIndex, currentRow, 1);
        }

        [SuppressMessage(
        "StyleCop.CSharp.StylecopPlus.Rules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        protected internal virtual int AssignMatchData(ISheet sheet, IList<DataRow> matchInfoList, string matchName, int currentRow, int currentIndex)
        {
            if (matchInfoList.Count == 0)
            {
                return currentRow;
            }

            var rowStartIndex = currentRow;
            var columnNameList = GetColumnNameList();
            var rowCount = matchInfoList.Count;

            // Assign match data
            var matchRow = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow);
            matchRow.Cells[0].SetCellValue(currentIndex);
            matchRow.Cells[1].SetCellValue(matchName);

            // Calculate match win loss
            for (int i = 0; i < columnNameList.Count; i++)
            {
                var columnName = columnNameList[i];
                decimal columnValue = 0;
                for (int j = 0; j < rowCount; j++)
                {
                    var matchWinloss = matchInfoList[j];
                    columnValue += Converter.ToDecimal(matchWinloss[columnName]);
                }

                matchRow.Cells[i + 2].SetCellValue(columnValue);
            }

            currentRow++;
            if (matchName.Contains('\n'))
            {
                matchRow.Height = (short)(matchRow.Height * 2);
            }

            // Assign bettype
            for (int i = 0; i < rowCount; i++)
            {
                var matchWinloss = matchInfoList[i];
                var betTypeRow = sheet.CopyRow(TemplateSheet, RowBetTypeContentTemplate, currentRow);
                var betType = Converter.ToInt(matchWinloss["bettype"]);
                var liveIndicator = Converter.To<bool>(matchWinloss["liveindicator"]);
                var betTypeName = "         " + _cacheRefAdapter.GetBettype(betType, liveIndicator);

                betTypeRow.Cells[1].SetCellValue(betTypeName);
                ExcelHelper.AssignRowContent(betTypeRow, matchWinloss, columnNameList, 2);
                currentRow++;
            }

            sheet.MergeCells(rowStartIndex, 0, rowStartIndex + rowCount, 0);

            return currentRow;
        }

        /// <summary>
        ///     Assign special bet type such as: outright, mix parlay, system mix parlay.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="exportData">Export data.</param>
        /// <param name="currentRowIndex">Current row index.</param>
        /// <returns>
        ///     Returns the excel row index, after assigning special bet type.
        /// </returns>
        private int AssignSpecialBetType(ISheet sheet, IList<DataRow> exportData, int currentRowIndex)
        {
            var rowCount = exportData.Count;
            var columnNameList = GetColumnNameList();

            // Assign bettype
            for (int i = 0; i < rowCount; i++)
            {
                var dataRow = exportData[i];
                var betTypeRow = sheet.CopyRow(TemplateSheet, RowSpecialBetTypeTemplate, currentRowIndex);
                var betType = Converter.ToInt(dataRow["bettype"]);
                var betTypeName = betType == (int)BetType.VirtualSports2MixParlay ?
                    _cacheRefAdapter.GetBettypeWithProductName(betType) :
                    _cacheRefAdapter.GetBettype(betType, false);

                betTypeRow.Cells[0].SetCellValue(betTypeName);
                ExcelHelper.AssignRowContent(betTypeRow, dataRow, columnNameList, 2);
                sheet.MergeCells(currentRowIndex, 0, currentRowIndex, 1);
                currentRowIndex++;
            }

            return currentRowIndex;
        }

        protected internal virtual int AssignTotalSummary(ISheet sheet, IEnumerable<string> columnNameList, int currentRow, DataTable sumTable)
        {
            var cellIndex = 2;

            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);

            row.Cells[0].SetCellValue(ViewResources.Total);
            sheet.MergeCells(currentRow, 0, currentRow, 1);

            foreach (string columnName in columnNameList)
            {
                var sumData = sumTable.Compute("Sum(" + columnName + ")", string.Empty);
                cellIndex = AssignDecimalData(
                                    row,
                                    cellIndex,
                                    Converter.To<decimal>(sumData));
            }

            sheet.MergeCells(currentRow, ColumnIndex, currentRow, 1);

            return currentRow + 1;
        }

        private IList<string> GetColumnNameList()
        {
            var currencyColumnList = GetCommonColumnNameList();

            return currencyColumnList.Select(name => name == "betcount" ? name : GetCurrencyColumn(name))
                   .ToList();
        }

        private IList<string> GetTotalColumnNameList()
        {
            var currencyColumnList = GetCommonColumnNameList();

            for (int i = 0; i < currencyColumnList.Count; i++)
            {
                currencyColumnList[i] = "sum" + currencyColumnList[i];
            }

            return currencyColumnList.Select(item => item.Equals("sumbetcount", StringComparison.OrdinalIgnoreCase) ? item : GetCurrencyColumn(item)).ToList();
        }

        private string GetCurrencyColumn(string columnName)
        {
            return DataHelper.GetCurrencyColumnName(columnName, _convertingOption.IsConvert, _convertingOption.BaseCurrency);
        }
    }
}