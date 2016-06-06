// -----------------------------------------------------------------------
// <copyright file="Racing.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Racing class.
    /// </summary>
    public class Racing
    {
        public IList<RacingBase> RacingDataList { get; set; }

        public IList<RacingLeague> RacingGroupByRace { get; set; }

        public decimal SumTotalWin { get; set; }

        public decimal SumRevenueWin { get; set; }

        public decimal SumTotalPlace { get; set; }

        public decimal SumRevenuePlace { get; set; }

        public decimal SumTotalWinPlace { get; set; }

        public decimal SumRevenueWinPlace { get; set; }
    }
}