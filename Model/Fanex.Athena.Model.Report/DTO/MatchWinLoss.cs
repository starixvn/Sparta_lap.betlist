// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class MatchWinLoss
    {
        public DateTime KickOffTime { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public short SportType { get; set; }

        public int MatchId { get; set; }

        public string ShowTime { get; set; }

        public string MatchName { get; set; }

        public string MatchCode { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public string ProductName { get; set; }

        public string RaceNo { get; set; }

        public bool IsLive { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int BetType { get; set; }

        public string BettypeName { get; set; }

        public bool IsNeutral { get; set; }

        public decimal Turnover { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal CustomerWL { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal BACommission { get; set; }

        public decimal AmtDueToBA { get; set; }
    }
}