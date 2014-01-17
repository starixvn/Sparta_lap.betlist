// -----------------------------------------------------------------------
// <copyright file="BetListMatch.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetList
{
    /// <summary>
    /// BetListMatch class.
    /// </summary>
    public class BetListMatch
    {
        public byte SportType { get; set; }

        public string Status { get; set; }

        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public string MatchCode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string Race { get; set; }
    }
}