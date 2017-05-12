// --------------------------------------------------------------------------
// <copyright file="MatchWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
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
    using Common;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.Helper;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Match Win Loss report - Export excel.
    /// </summary>
    public class MatchWinLossExcel : ReportBase
    {
        private const int ColumnCustomerWl = 4;
        private const int ColumnCustomerComm = 5;
        private const int ColumnCustomerTotal = 6;
        private const int ColumnBaCommission = 8;
        private const int ColumnRacingRevenue = 9;
        private const int ColumnAmtDueToBa = 10;
        private const int ColumnAmtDueToRacing = 11;
        private const int ColumnTemplateCount = 11;

        private const int RowTableHeaderTemplate = 1;
        private const int RowMatchTemplate = 3;
        private const int RowContentTemplate = 4;
        private const int RowBetTypeContentTemplate = 5;
        private const int RowTotalTemplate = 6;
        private const int RowSpecialBetTypeTemplate = 7;
        private const int RowGrandTotalTemplate = 8;
        private const int RowNoInfoTemplate = 9;

        private readonly MatchWinLossMetaModel _model;
        private readonly DataTable _totalData;

        public MatchWinLossExcel(
                                string templatePath,
                                ConvertingOption convertingOption,
                                ShowingOption showingOption,
                                DataTable exportData,
                                DataTable totalData,
                                MatchWinLossMetaModel model)
            : base(templatePath, showingOption, convertingOption)
        {
            _model = model;
            ExportData = exportData;
            _totalData = totalData;
            AddExtraColumns();

            HeaderTextList = new List<string>
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
                ViewResources.RacingRevenue,
                ViewResources.AmtDueToBA,
                ViewResources.AmtDueToRacing
            };
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowMatchTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        [SuppressMessage(
               "StyleCop.CSharp.StylecopPlus.Rules",
               "SP2101:MethodMustNotContainMoreLinesThan",
               Justification = "Reviewed. Suppression is OK here.")]
        protected override void AssignSheetData(ISheet sheet)
        {
            int currentRowIndex = 3;
            var leagueName = string.Empty;
            var matchId = 0;
            var matchName = "Default";
            var formatMatchName = string.Empty;
            var columnNameList = GetColumnNameList();
            var rowCount = ExportData.Rows.Count;
            var matchInfoList = new List<DataRow>();
            var currentIndex = 1;
            var specialBetType = new List<DataRow>();

            for (int index = 0; index < rowCount; index++)
            {
                var exportRow = ExportData.Rows[index];
                var betType = Converter.ToInt(exportRow["bettype"]);
                if (betType == (int)BetType.MixParlay || betType == (int)BetType.Outright || betType == (int)BetType.CombinationMixParlay)
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
                    SetTextRowValue(sheet, RowMatchTemplate, currentRowIndex, ColumnTemplateCount, leagueName);
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

            ExportData.DefaultView.RowFilter = "bettype <> '9' and bettype <> '10' and bettype <> '29'";
            var sumTable = ExportData.DefaultView.ToTable();

            if (ExportData.Rows.Count > sumTable.Rows.Count)
            {
                currentRowIndex = AssignTotalSummary(sheet, columnNameList, currentRowIndex, sumTable);
            }

            currentRowIndex = AssignSpecialBetType(sheet, specialBetType, currentRowIndex);
            AssignGrandTotalSummary(sheet, currentRowIndex);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignTitle(ISheet sheet)
        {
            ReportTitle = string.Format(
                ViewResources.MatchWinLossTitle,
                _model.Date.ToString(FormatDate),
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

        /// <summary>
        /// Get column name in data table.
        /// </summary>
        /// <returns>The list of column name in data table.</returns>
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
                "rbrevenue",
                "amtduetoba",
                "amtduetorb"
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

        /// <summary>
        /// Sum column: race book revenue, total win loss, sum total win loss, sum race book revenue.
        /// </summary>
        private void AddExtraColumns()
        {
            string racebookRevenueExpression = string.Format("{0} + {1}", GetCurrencyColumn("commrb"), GetCurrencyColumn("winlossrb"));
            string revenueExpression = string.Format("{0} + {1}", GetCurrencyColumn("sumcommrb"), GetCurrencyColumn("sumwinlossrb"));
            string totalExpression = string.Format("{0} + {1}", GetCurrencyColumn("memberwinlost"), GetCurrencyColumn("playercomm"));
            string sumtotal = string.Format("{0} + {1}", GetCurrencyColumn("summemberwinlost"), GetCurrencyColumn("sumplayercomm"));

            ExportData.Columns.Add("rbrevenue", typeof(decimal), racebookRevenueExpression);
            ExportData.Columns.Add(GetCurrencyColumn("totalwl"), typeof(decimal), totalExpression);
            _totalData.Columns.Add(GetCurrencyColumn("sumtotalwl"), typeof(decimal), sumtotal);
            _totalData.Columns.Add(GetCurrencyColumn("sumrbrevenue"), typeof(decimal), revenueExpression);
        }

        /// <summary>
        /// Assign data to grand total summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="currentRow">Excel row index.</param>
        private void AssignGrandTotalSummary(ISheet sheet, int currentRow)
        {
            int cellIndex = 2;

            IRow row = sheet.CopyRow(TemplateSheet, RowGrandTotalTemplate, currentRow);

            row.Cells[0].SetCellValue(ViewResources.GrandTotal);
            sheet.MergeCells(currentRow, 0, currentRow, 1);

            IList<string> columnNameList = GetTotalColumnNameList();

            foreach (string columnName in columnNameList)
            {
                cellIndex = AssignDecimalData(row, cellIndex, Converter.ToDecimal(_totalData.Rows[0][columnName]));
            }

            sheet.MergeCells(currentRow, ColumnIndex, currentRow, 1);
        }

        /// <summary>
        /// Assign match data.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="matchInfoList">Match list.</param>
        /// <param name="matchName">Match name.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <param name="currentIndex">Index of match.</param>
        /// <returns>The row index after assign data.</returns>
        private int AssignMatchData(ISheet sheet, IList<DataRow> matchInfoList, string matchName, int currentRow, int currentIndex)
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
                string columnName = columnNameList[i];
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
                DataRow matchWinloss = matchInfoList[i];
                IRow betTypeRow = sheet.CopyRow(TemplateSheet, RowBetTypeContentTemplate, currentRow);
                int betType = Converter.ToInt(matchWinloss["bettype"]);
                var liveIndicator = Converter.To<bool>(matchWinloss["liveindicator"]);
                string betTypeName = "         " + CacheRef.GetBettype(betType, liveIndicator);

                betTypeRow.Cells[1].SetCellValue(betTypeName);
                AssignRowContent(betTypeRow, matchWinloss, columnNameList, 2);
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
            int rowCount = exportData.Count;
            IList<string> columnNameList = GetColumnNameList();

            // Assign bettype
            for (int i = 0; i < rowCount; i++)
            {
                DataRow dataRow = exportData[i];
                IRow betTypeRow = sheet.CopyRow(TemplateSheet, RowSpecialBetTypeTemplate, currentRowIndex);
                int betType = Converter.ToInt(dataRow["bettype"]);
                string betTypeName = CacheRef.GetBettype(betType, false);

                betTypeRow.Cells[0].SetCellValue(betTypeName);
                AssignRowContent(betTypeRow, dataRow, columnNameList, 2);
                sheet.MergeCells(currentRowIndex, 0, currentRowIndex, 1);
                currentRowIndex++;
            }

            return currentRowIndex;
        }

        /// <summary>
        /// Assign data to total summary section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="columnNameList">Column name list.</param>
        /// <param name="currentRow">Excel row index.</param>
        /// <param name="sumTable">Summary table.</param>
        /// <returns>The row index after assign data.</returns>
        private int AssignTotalSummary(ISheet sheet, IList<string> columnNameList, int currentRow, DataTable sumTable)
        {
            int cellIndex = 2;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow);

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
            IList<string> currencyColumnList = GetCommonColumnNameList();

            return currencyColumnList.Select(name => (name == "rbrevenue" || name == "betcount") ? name : GetCurrencyColumn(name))
                   .ToList();
        }

        /// <summary>
        /// Get column name in data table for getting the sum value.
        /// </summary>
        /// <returns>The list of column name in data table.</returns>
        private IList<string> GetTotalColumnNameList()
        {
            IList<string> currencyColumnList = GetCommonColumnNameList();

            currencyColumnList.Remove("productname");

            for (int i = 0; i < currencyColumnList.Count; i++)
            {
                currencyColumnList[i] = "sum" + currencyColumnList[i];
            }

            return currencyColumnList.Select(item => item.Equals("sumbetcount", StringComparison.OrdinalIgnoreCase) ? item : GetCurrencyColumn(item)).ToList();
        }

        /// <summary>
        /// Get format of match name, based on product.
        /// </summary>
        /// <param name="exportRow">Data table row.</param>
        /// <param name="matchName">Match name.</param>
        /// <returns>The real match name.</returns>
        private string GetFormatMatchName(DataRow exportRow, string matchName)
        {
            string formatMatchName;

            // Match name
            switch (exportRow["productname"].ToString())
            {
                case Product.RB:
                    formatMatchName = matchName.Replace("Race", "Race ");
                    break;

                case Product.NG:
                case Product.LC:
                case Product.VS:
                    formatMatchName = matchName;
                    break;

                default:
                    string homeName = CacheRef.GetTeamName(exportRow["homeid"]) +
                                      (Converter.ToBool(exportRow["isneutral"]) ? " " + "(N)" : string.Empty);
                    string awayName = CacheRef.GetTeamName(exportRow["awayid"]);
                    formatMatchName = homeName + "\n" + awayName;
                    break;
            }

            return formatMatchName;
        }
    }
}