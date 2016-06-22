// -----------------------------------------------------------------------
// <copyright file="RacingBase.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    using System;
    using Fanex.Athena.Model.Core.Helper;

    /// <summary>
    /// RacingBase class.
    /// </summary>
    public class RacingBase : TotalBetsBase
    {
        private string _horseTeamName;

        public RacingBase()
        {
            RowSpan = 1;
        }

        public int TeamId { get; set; }

        public string HorseTeamName
        {
            get
            {
                return _horseTeamName ?? (_horseTeamName = CacheRef.GetHorseTeamName(TeamId));
            }
        }

        public int RowSpan { get; set; }

        public int Race { get; set; }

        public int HorseNo { get; set; }

        public string OddStatus { get; set; }

        public DateTime KickOffTime { get; set; }

        public decimal TotalWin { get; set; }

        public decimal RevenueWin { get; set; }

        public decimal TotalPlace { get; set; }

        public decimal RevenuePlace { get; set; }

        public decimal TotalWinPlace { get; set; }

        public decimal RevenueWinPlace { get; set; }
    }
}