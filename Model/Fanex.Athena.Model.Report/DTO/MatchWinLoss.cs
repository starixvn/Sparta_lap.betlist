// -----------------------------------------------------------------------
// <copyright file="MatchWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;
    using Fanex.Athena.Model.Core.Helper;

    /// <summary>
    /// MatchWinLoss class.
    /// </summary>
    public class MatchWinLoss
    {
        private int _betType;
        private bool _isLive;

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

        public bool IsLive
        {
            get { return _isLive; }
            set { _isLive = value; }
        }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int BetType
        {
            get { return _betType; }
            set { _betType = value; }
        }

        public string BettypeName
        {
            get
            {
                return CacheRef.GetBettype(_betType, _isLive);
            }
        }

        public bool IsNeutral { get; set; }

        public decimal Turnover { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal CustomerWL { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal BACommission { get; set; }

        public decimal RacingRevenue { get; set; }

        public decimal AmtDueToBA { get; set; }

        public decimal AmtDueToRacing { get; set; }
    }
}