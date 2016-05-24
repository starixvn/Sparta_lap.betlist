// --------------------------------------------------------------------------
// <copyright file="ResultBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Base
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Model.Core.Helper;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// The result base.
    /// </summary>
    public abstract class ResultBase : ReportBase
    {
        protected const int ColumnKickOffTime = 0;
        protected const string Na = "N/A";
        protected const int RowLeagueTemplate = 2;
        protected const int RowNoInfoTemplate = 5;

        static ResultBase()
        {
            ResultStatusMap = new Dictionary<string, string>
            {
                { "running", ViewResources.Running },
                { "completed", ViewResources.Completed },
                { "closed", ViewResources.Closed },
                { "postponed", ViewResources.Postponed },
                { "internal", ViewResources.Internal },
                { "refund", ViewResources.Refund },
                { "refund ft", ViewResources.RefundFT },
                { "refund ht", ViewResources.RefundHT },
                { "won", ViewResources.Won },
            };
        }

        protected ResultBase(string templatePath, string exportingFileName, ShowingOption showingOption, ConvertingOption convertingOption)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
        }

        public static IDictionary<string, string> ResultStatusMap
        {
            get;
            set;
        }

        public abstract int ColumnTemplateCount
        {
            get;
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowLeagueTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            sheet.GetCell(0, 0).SetCellValue(ViewResources.Results);

            ClearTemplate(sheet, 2);

            sheet.CreateFreezePane(0, 2);
        }

        /// <summary>
        ///     Get the result status.
        /// </summary>
        /// <param name="dr">Data table row.</param>
        /// <returns>The result status.</returns>
        protected static string GetResultStatus(DataRow dr)
        {
            bool isPending = dr.Table.Columns.Contains("ispending") && Convert.ToBoolean(dr["ispending"]);
            string eventStatus = Convert.ToString(dr["eventstatus"]).ToLower();

            if (isPending)
            {
                return ViewResources.Pending;
            }

            return ResultStatusMap.ContainsKey(eventStatus)
                   ? ResultStatusMap[eventStatus]
                   : Convert.ToString(dr["eventstatus"]);
        }

        /// <summary>
        ///     Assign array string to row.
        /// </summary>
        /// <param name="row">Row in sheet export excel.</param>
        /// <param name="cellIndex">Position cell will insert.</param>
        /// <param name="values">Array string values.</param>
        /// <returns>Cell position after insert.</returns>
        protected virtual int AssignStringData(IRow row, int cellIndex, params string[] values)
        {
            foreach (string t in values)
            {
                row.Cells[cellIndex].SetCellValue(t);
                cellIndex++;
            }

            return cellIndex;
        }

        /// <summary>
        /// Get result of match.
        /// </summary>
        /// <param name="exportRow">The export row.</param>
        /// <param name="enableHalfTime">Enable haft time.</param>
        /// <returns>The match result.</returns>
        protected virtual Dictionary<string, string> GetMatchResult(DataRow exportRow, bool enableHalfTime)
        {
            var homeTeam = CacheRef.GetTeamName(exportRow["homeid"]) +
                           ((bool)exportRow["isNeutral"] ? ViewResources.IsNeutral : string.Empty);
            var getResultMatch = new Dictionary<string, string>
            {
                { "finalHomeScore", exportRow["finalhomescore"].ToString() },
                { "finalAwayScore", exportRow["finalawayscore"].ToString() },
                { "homeTeam", homeTeam },
                { "awayTeam", CacheRef.GetTeamName(exportRow["awayid"]) }
            };

            if (enableHalfTime)
            {
                var homeScore = Convert.IsDBNull(exportRow["hthomescore"])
                    ? string.Empty
                    : exportRow["hthomescore"].ToString();
                var awayScore = Convert.IsDBNull(exportRow["hthomescore"])
                    ? string.Empty
                    : exportRow["htawayscore"].ToString();

                getResultMatch.Add("htHomeScore", homeScore);
                getResultMatch.Add("htAwayScore", awayScore);
            }

            return getResultMatch;
        }

        /// <summary>
        /// Get match round.
        /// </summary>
        /// <param name="exportRow">The export row.</param>
        /// <param name="homeSet">The home set.</param>
        /// <param name="awaySet">The away set.</param>
        /// <param name="homeOT">The home OT.</param>
        /// <param name="awayOT">The away OT.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        protected virtual void GetMatchRound(DataRow exportRow, ref string[] homeSet, ref string[] awaySet, ref int homeOT, ref int awayOT)
        {
            // Bet status
            string eventStatus = exportRow["eventstatus"].ToString().Trim();

            if (eventStatus != "Refund")
            {
                string betStatus = Convert.IsDBNull(exportRow["betstatus"])
                                   ? string.Empty
                                   : exportRow["betstatus"].ToString();
                if (betStatus.IndexOf(",", StringComparison.Ordinal) >= 0)
                {
                    string[] betStatusList = betStatus.Split(',');
                    for (int j = 0; j < betStatusList.Length && j < 5; j++)
                    {
                        homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                        awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();

                        if (j != 4)
                        {
                            continue;
                        }

                        homeOT += Convert.ToInt32(homeSet[j]);
                        awayOT += Convert.ToInt32(awaySet[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Get win team row.
        /// </summary>
        /// <param name="finalHomeScore">The final home score.</param>
        /// <param name="finalAwayScore">The final away score.</param>
        /// <returns>A integer array contain row index.</returns>
        protected virtual int[] GetRowTeamWin(string finalHomeScore, string finalAwayScore)
        {
            var arrRowTeam = new int[2];

            if (string.IsNullOrEmpty(finalAwayScore))
            {
                arrRowTeam[0] = 4;
                arrRowTeam[1] = 4;
            }
            else
            {
                int homeScore = Converter.ToInt(finalHomeScore);
                int awayScore = Converter.ToInt(finalAwayScore);
                /*-----------------------------------------------------
                 * Row 3 is row in excel template formated bold
                 * Row 4 is row in excel template formated no bold
                 -----------------------------------------------------*/
                if (homeScore > awayScore)
                {
                    arrRowTeam[0] = 3;
                    arrRowTeam[1] = 4;
                }
                else if (homeScore < awayScore)
                {
                    arrRowTeam[0] = 4;
                    arrRowTeam[1] = 3;
                }
                else if (homeScore == awayScore)
                {
                    arrRowTeam[0] = 4;
                    arrRowTeam[1] = 4;
                }
            }

            return arrRowTeam;
        }

        protected virtual void MergeResultCells(ISheet sheet, int currentRow, int rowSpan, int colSpanKichOffTime, int colSpanStatus)
        {
            sheet.MergeCells(currentRow, colSpanKichOffTime, rowSpan, colSpanKichOffTime);
            sheet.MergeCells(currentRow, colSpanStatus, rowSpan, colSpanStatus);
        }

        /// <summary>
        /// Set league name.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="exportRow">The export row.</param>
        /// <param name="colLeagueName">The column league name.</param>
        /// <param name="currentLeagueId">The currency league id.</param>
        /// <param name="currentRow">The current row.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        protected virtual void SetLeague(ISheet sheet, DataRow exportRow, string colLeagueName, ref int currentLeagueId, ref int currentRow)
        {
            int leagueId = Convert.ToInt32(exportRow[colLeagueName]);

            if (currentLeagueId == 0 || currentLeagueId != leagueId)
            {
                currentLeagueId = leagueId;
                string leagueName = CacheRef.GetLeagueName(leagueId);
                SetTextRowValue(sheet, 2, currentRow, ColumnTemplateCount, leagueName.ToUpper());
                currentRow++;
            }
        }
    }
}