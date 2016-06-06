// -----------------------------------------------------------------------
// <copyright file="RacingLeague.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// RacingLeague class.
    /// </summary>
    public class RacingLeague
    {
        public string LeagueName { get; set; }

        public IList<RacingRace> RaceList { get; set; }
    }
}