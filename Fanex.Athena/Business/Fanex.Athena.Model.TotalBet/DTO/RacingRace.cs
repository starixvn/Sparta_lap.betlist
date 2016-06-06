// -----------------------------------------------------------------------
// <copyright file="RacingRace.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// RacingRace class.
    /// </summary>
    public class RacingRace
    {
        public int Race { get; set; }

        public DateTime KickOffTime { get; set; }

        public IList<RacingBase> MatchInRace { get; set; }
    }
}