// -----------------------------------------------------------------------
// <copyright file="RacingLeague.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
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