// --------------------------------------------------------------------------
// <copyright file="MatchWinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Fanex.Athena.Excel.Entities;
    using Fanex.Common.Extensions;
    using Fanex.NPOI.Base;

    using NPOI.SS.UserModel;

    using Olympus.LRF.Model.BL.Cache;

    public class MatchWinLossDetail : ExcelBase
    {
        private const int ColumnLicenseeWL = 4;
        private const int RowContentTemplate = 3;
        private const int RowMatchTemplate = 2;
        private const int RowNoInfoTemplate = 5;
        private const int RowTotalTemplate = 4;

        private readonly IList<Fanex.Athena.Models.DTO.Report.MatchWinLoss> _exportData;
        private readonly IList<Fanex.Athena.Models.DTO.Report.MatchWinLoss> _parlayData;

        public MatchWinLossDetail(string templatePath,
                                  string exportingFileName,
                                  ConvertingOption convertingOption,
                                  ShowingOption showingOption,
                                  IList<Fanex.Athena.Models.DTO.Report.MatchWinLoss> exportData,
                                  IList<Fanex.Athena.Models.DTO.Report.MatchWinLoss> parlayData)
        : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _exportData = exportData;
            _parlayData = parlayData;
        }

        public override void AssignData()
        {
            var numOfDataRow = _exportData.Count;
            var sheet = Workbook.CloneSheet("Template", "Data");

            ColumnTemplateIndexList = GetColumnTemplateIndex();

            AssignHeader(sheet);

            if (numOfDataRow == 0)
            {
                AssignEmptyRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }
        }

        public override void AssignEmptyRow(ISheet sheet)
        {
            SetReportTitle(sheet, TemplateSheet, ColumnTemplateIndexList, RowNoInfoTemplate, RowMatchTemplate, Resources.Excel.NoInfo);
        }

        public override void AssignHeader(ISheet sheet)
        {
            SetReportTitle(sheet, TemplateSheet, ColumnTemplateIndexList, RowTitleTemp, RowTitle, ReportTitle);

            var row = SetReportHeader(sheet, TemplateSheet, ColumnTemplateIndexList, RowHeaderTemp, RowHeader);

            RenderLicenseeWLHeader(row, ColumnLicenseeWL);

            // Remove content below header before insert new data
            for (int i = RowContentTemplate; i <= RowNoInfoTemplate; i++)
            {
                SetEmptyRow(sheet, TemplateSheet, i, i);
            }

            // Freeze title and header
            sheet.CreateFreezePane(0, 2);
        }

        public override void AssignSheetData(ISheet sheet)
        {
            var currentRow = 2;
            var rowCount = _exportData.Count;
            var oldLeague = 0;
            var colSpan = 8 - (ShowingOption.BaVisible ? 0 : 2) - (ShowingOption.RbVisible ? 0 : 2);

            for (var index = 0; index < rowCount; index++)
            {
                var entity = _exportData[index];

                if (entity.LeagueId != oldLeague)
                {
                    oldLeague = entity.LeagueId;
                    var leagueRow = sheet.CopyRow(TemplateSheet, RowMatchTemplate, currentRow);
                    sheet.MergeCells(currentRow, 0, currentRow, colSpan);
                    leagueRow.Cells[0].SetCellValue(CacheRef.GetLeagueName(entity.LeagueId));
                    currentRow++;
                }

                // Get betype and bettype name
                var productName = entity.ProductName;
                var liveIndicator = entity.IsLive;

                // Set content for excel
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow, ColumnTemplateIndexList, DefaultStyle);

                AssignCellData(sheet, row, entity, currentRow, productName, index);
                currentRow++;
            }

            var grandTotal = new Fanex.Athena.Models.DTO.Report.MatchWinLoss();
            // Total
            AssignTotalRow(sheet, currentRow++, grandTotal);
            // Parlay
            AssignParlayRow(sheet, ref currentRow, grandTotal);
            // Grand Total
            AssignGrandTotalRow(sheet, currentRow, grandTotal);
        }

        protected void AssignCellData(ISheet sheet, IRow row, Fanex.Athena.Models.DTO.Report.MatchWinLoss entity, int currentRow, string productName, int index)
        {
            int cellIndex = 0;
            row.Cells[cellIndex++].SetCellValue(index + 1);
            row.Cells[cellIndex++].SetCellValue(GetMatchName(entity, productName));
            row.Cells[cellIndex++].SetCellValue(entity.Turnover);
            row.Cells[cellIndex++].SetCellValue(entity.CustomerWl);
            row.Cells[cellIndex++].SetCellValue(entity.LicenseeWl);

            if (ShowingOption.BaVisible)
            {
                row.Cells[cellIndex++].SetCellValue(entity.BaCommission);
            }

            if (ShowingOption.RbVisible)
            {
                row.Cells[cellIndex++].SetCellValue(entity.RacingRevenue);
            }
            if (ShowingOption.BaVisible)
            {
                row.Cells[cellIndex++].SetCellValue(entity.AmtDueToBa);
            }

            if (ShowingOption.RbVisible)
            {
                row.Cells[cellIndex++].SetCellValue(entity.AmtDueToRacing);
            }
        }

        /// <summary>
        /// 0: # (index), 1: Bet Type, 2: Turnover, 3: CustomerWL, 4: LicenseeWL, 5: BA Commission, 6: Racing Revenue,
        /// 7: Amt Due To BA, 8: Amt Due To Racing
        /// </summary>
        /// <returns></returns>
        protected override IList<int> GetColumnTemplateIndex()
        {
            if (ShowingOption.BaVisible == false && ShowingOption.RbVisible == false)
            {
                return new List<int> { 0, 1, 2, 3, 4 };
            }
            else if (ShowingOption.BaVisible == false)
            {
                return new List<int> { 0, 1, 2, 3, 4, 6, 8 };
            }
            else if (ShowingOption.RbVisible == false)
            {
                return new List<int> { 0, 1, 2, 3, 4, 5, 7 };
            }
            else
            {
                return new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            }
        }

        protected IList<string> GetDataColumnNameList()
        {
            var currencyColumnList = new List<string> { "turnover", "memberwinlost", "licenseewl", "commba", "rbrevenue", "amtduetoba", "amtduetorb" };

            if (!ShowingOption.BaVisible)
            {
                currencyColumnList.Remove("commba");
                currencyColumnList.Remove("amtduetoba");
            }

            if (!ShowingOption.RbVisible)
            {
                currencyColumnList.Remove("rbrevenue");
                currencyColumnList.Remove("amtduetorb");
            }

            return currencyColumnList.Select(s => s.ToString() == "rbrevenue" ? "rbrevenue" : GetCurrencyColumn(s))
                   .ToList();
        }

        /// <summary>
        /// Get title for normal match
        /// </summary>
        /// <param name="exportRow"></param>
        /// <param name="currentMatchId"></param>
        /// <param name="betTypeName"></param>
        /// <returns></returns>
        protected string GetNormalMatchTitle(Fanex.Athena.Models.DTO.Report.MatchWinLoss entity)
        {
            var matchId = entity.MatchId;
            var homeName = CacheRef.GetTeamName(entity.HomeId) +
                           ((bool)entity.IsNeutral ? Fanex.Athena.Excel.Resources.Excel.IsNeutral : string.Empty);
            var awayName = CacheRef.GetTeamName(entity.AwayId);
            return homeName + "-VS-" + awayName;
        }

        protected IList<string> GetTotalColumnNameList()
        {
            var currencyColumnList = GetDataColumnNameList();

            currencyColumnList.Remove("productname");

            for (int i = 0; i < currencyColumnList.Count; i++)
            {
                currencyColumnList[i] = "sum" + currencyColumnList[i];
            }

            return currencyColumnList;
        }

        private void AssignGrandTotalRow(ISheet sheet, int currentRow, Fanex.Athena.Models.DTO.Report.MatchWinLoss grandTotal)
        {
            var totalRow = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow, ColumnTemplateIndexList, DefaultStyle);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            totalRow.Cells[0].SetCellValue("Grand Total");
            var cellIndex = 2;

            totalRow.Cells[cellIndex++].SetCellValue(grandTotal.Turnover);
            totalRow.Cells[cellIndex++].SetCellValue(grandTotal.CustomerWl);
            totalRow.Cells[cellIndex++].SetCellValue(grandTotal.LicenseeWl);

            if (ShowingOption.BaVisible)
            {
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.BaCommission);
            }

            if (ShowingOption.RbVisible)
            {
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.RacingRevenue);
            }
            if (ShowingOption.BaVisible)
            {
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.AmtDueToBa);
            }

            if (ShowingOption.RbVisible)
            {
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.AmtDueToRacing);
            }
        }

        private void AssignParlayRow(ISheet sheet,ref int currentRow,Fanex.Athena.Models.DTO.Report.MatchWinLoss grantTotal)
        {
            foreach (var entity in _parlayData)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, currentRow, ColumnTemplateIndexList, DefaultStyle);
                sheet.MergeCells(currentRow, 0, currentRow, 1);
                switch (entity.BetType)
                {
                case 9:
                    row.Cells[0].SetCellValue("Mix Parlay");
                    break;
                case 10:
                    row.Cells[0].SetCellValue("Outright");
                    break;
                default:
                    row.Cells[0].SetCellValue("System Parlay");
                    break;
                }
                var cellIndex = 2;

                grantTotal.Turnover += entity.Turnover;
                grantTotal.CustomerWl += entity.CustomerWl;
                grantTotal.LicenseeWl += entity.LicenseeWl;

                row.Cells[cellIndex++].SetCellValue(entity.Turnover);
                row.Cells[cellIndex++].SetCellValue(entity.CustomerWl);
                row.Cells[cellIndex++].SetCellValue(entity.LicenseeWl);

                if (ShowingOption.BaVisible)
                {
                    grantTotal.BaCommission += entity.BaCommission;
                    row.Cells[cellIndex++].SetCellValue(entity.BaCommission);
                }

                if (ShowingOption.RbVisible)
                {
                    grantTotal.RacingRevenue += entity.RacingRevenue;
                    row.Cells[cellIndex++].SetCellValue(entity.RacingRevenue);
                }
                if (ShowingOption.BaVisible)
                {
                    grantTotal.AmtDueToBa += entity.AmtDueToBa;
                    row.Cells[cellIndex++].SetCellValue(entity.AmtDueToBa);
                }

                if (ShowingOption.RbVisible)
                {
                    grantTotal.AmtDueToRacing += entity.AmtDueToRacing;
                    row.Cells[cellIndex++].SetCellValue(entity.AmtDueToRacing);
                }
                currentRow++;
            }
        }

        private void AssignTotalRow(ISheet sheet, int currentRow, Fanex.Athena.Models.DTO.Report.MatchWinLoss grandTotal)
        {
            var totalRow = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow, ColumnTemplateIndexList, DefaultStyle);
            sheet.MergeCells(currentRow, 0, currentRow, 1);
            totalRow.Cells[0].SetCellValue("Total");
            var cellIndex = 2;

            grandTotal.Turnover = _exportData.Sum(i => i.Turnover);
            grandTotal.CustomerWl = _exportData.Sum(i => i.CustomerWl);
            grandTotal.LicenseeWl = _exportData.Sum(i => i.LicenseeWl);

            totalRow.Cells[cellIndex++].SetCellValue(grandTotal.Turnover);
            totalRow.Cells[cellIndex++].SetCellValue(grandTotal.CustomerWl);
            totalRow.Cells[cellIndex++].SetCellValue(grandTotal.LicenseeWl);

            if (ShowingOption.BaVisible)
            {
                grandTotal.BaCommission = _exportData.Sum(i => i.BaCommission);
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.BaCommission);
            }

            if (ShowingOption.RbVisible)
            {
                grandTotal.RacingRevenue = _exportData.Sum(i => i.RacingRevenue);
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.RacingRevenue);
            }
            if (ShowingOption.BaVisible)
            {
                grandTotal.AmtDueToBa = _exportData.Sum(i => i.AmtDueToBa);
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.AmtDueToBa);
            }

            if (ShowingOption.RbVisible)
            {
                grandTotal.AmtDueToRacing = _exportData.Sum(i => i.AmtDueToRacing);
                totalRow.Cells[cellIndex++].SetCellValue(grandTotal.AmtDueToRacing);
            }
        }

        /// <summary>
        /// Get title for match
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        private string GetMatchName(Fanex.Athena.Models.DTO.Report.MatchWinLoss entity, string productName)
        {
            string title = string.Empty;

            switch (productName)
            {
            case "RB":
                title = GetRacingTitle(entity);
                break;
            case "NG":
                title = GetNumberGameTitle(entity);
                break;
            default:
                title = GetNormalMatchTitle(entity);
                break;
            }

            return title;
        }

        /// <summary>
        /// Get title for number game
        /// </summary>
        /// <param name="exportRow"></param>
        /// <param name="currentMatchId"></param>
        /// <returns></returns>
        private string GetNumberGameTitle(Fanex.Athena.Models.DTO.Report.MatchWinLoss entity)
        {
            return Resources.Excel.NumberGameNo + " " + entity.MatchCode;
        }

        /// <summary>
        /// Get title for racing
        /// </summary>
        /// <param name="exportRow"></param>
        /// <param name="currentTitle"></param>
        /// <returns></returns>
        private string GetRacingTitle(Fanex.Athena.Models.DTO.Report.MatchWinLoss entity)
        {
            return Resources.Excel.Race + entity.Race;
        }
    }
}