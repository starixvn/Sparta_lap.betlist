// -----------------------------------------------------------------------
// <copyright file="Match.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    public class Match
    {
        public int MatchId { get; set; }

        public int LeagueId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string MatchName { get; set; }

        public bool IsOutRight { get; set; }

        public bool IsNeutral { get; set; }
    }
}